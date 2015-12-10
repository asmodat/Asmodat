using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

using System.Speech;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Speech.Recognition.SrgsGrammar;
using System.Speech.Synthesis.TtsEngine;

using NAudio.Wave;
using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;
using Asmodat.Debugging;

namespace Asmodat.Audio
{
    public partial class Converter
    {
        
        public static Stream Mp3RemoveSilence(Stream source, double fftdb_silence_treshold = 0, int silence_span = 200)
        {
            if (source == null) return null;
            if (silence_span < 200) silence_span = 200;

            int frame_span_ms = silence_span;

            try
            {
                source.Position = 0;
                Mp3FileReader reader = new Mp3FileReader(source);

                int duration = Converter.Mp3Duration(reader);

                Mp3Frame frame = reader.ReadNextFrame();
                Dictionary<double, double> milisecond_average = new Dictionary<double, double>();

                Stream destination = new MemoryStream();

                byte[] previous_buffer = null;
                bool started = false;
                while (frame != null)
                {

                    Stream stream = new MemoryStream();
                    double start = (reader.CurrentTime).TotalMilliseconds;
                    double span = 0;

                    while (frame != null && (span < frame_span_ms || (duration - start) <= frame_span_ms * 2))
                    {
                        stream.Write(frame.RawData, 0, frame.RawData.Length);
                        frame = reader.ReadNextFrame();

                        if (frame == null)
                            break;

                        span = ((reader.CurrentTime).TotalMilliseconds - start);
                    }


                    WaveStream pcm = Converter.Mp3ToWAV(stream);

                    byte[] data = WaveStreamToArray(pcm);
                    double[] ffr_result;
                    Converter.Process16bitWAV(ref data, out ffr_result, true);
                    double ave = ffr_result.Average();

                    if (ave > fftdb_silence_treshold)
                    {
                        byte[] buf = Streams.ToArray(stream);

                        if (previous_buffer != null)
                            destination.Write(previous_buffer, 0, previous_buffer.Length);

                        destination.Write(buf, 0, buf.Length);
                        previous_buffer = null;
                        started = true;
                    }
                    else
                    {
                        if (started)
                        {
                            byte[] buf = Streams.ToArray(stream);
                            destination.Write(buf, 0, buf.Length);
                            started = false;
                        }
                        else
                        {
                            previous_buffer = Streams.ToArray(stream);
                        }
                    }

                    stream.Close();
                }

                reader.Close();

                return destination;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return null;
            }
        }

        public static void Mp3RemoveSilence(string mp3_source, double fftdb_silence_treshold = 0, int silence_span = 200)
        {
            if (silence_span < 200)  silence_span = 200;

            int frame_span_ms = silence_span;
            
            Mp3FileReader reader = new Mp3FileReader(mp3_source);
            int duration = Converter.Mp3Duration(reader);

            Mp3Frame frame = reader.ReadNextFrame();
            Dictionary<double, double> milisecond_average = new Dictionary<double, double>();

            string temp_destination = Converter.GetTemporaryFilePath;
            if (File.Exists(temp_destination))
                File.Delete(temp_destination);

            FileStream stream_destination = new FileStream(temp_destination, FileMode.Create, FileAccess.Write);

            byte[] previous_buffer = null;
            bool started = false;
            while (frame != null)
            {

                Stream stream = new MemoryStream();
                double start = (reader.CurrentTime).TotalMilliseconds;
                double span = 0;

                while (frame != null && (span < frame_span_ms || (duration - start) <= frame_span_ms*2))
                {
                    // currentTime = (reader.CurrentTime).TotalMilliseconds;
                    stream.Write(frame.RawData, 0, frame.RawData.Length);
                    frame = reader.ReadNextFrame();

                    if (frame == null)
                        break;

                    span = ((reader.CurrentTime).TotalMilliseconds - start);
                }
                

                WaveStream pcm = Converter.Mp3ToWAV(stream);
                byte[] data = WaveStreamToArray(pcm);
                double[] ffr_result;

                //silence = 0
                //whisper = 15
                //conversation = 60
                Converter.Process16bitWAV(ref data, out ffr_result, true);



                double ave = ffr_result.Average();

                if (ave > fftdb_silence_treshold)
                {
                    byte[] buf = Streams.ToArray(stream);

                    if (previous_buffer != null)
                        stream_destination.Write(previous_buffer, 0, previous_buffer.Length);

                    stream_destination.Write(buf, 0, buf.Length);
                    previous_buffer = null;
                    started = true;
                }
                else
                {
                    if (started)
                    {
                        byte[] buf = Streams.ToArray(stream);
                        stream_destination.Write(buf, 0, buf.Length);
                        started = false;
                    }
                    else
                    {
                        previous_buffer = Streams.ToArray(stream);
                    }
                }

                stream.Close();
            }

            stream_destination.Close();
            reader.Close();

            File.Delete(mp3_source);
            File.Copy(temp_destination, mp3_source);
            File.Delete(temp_destination);
        }

        


        public static WaveStream WavRemoveSilence2(WaveStream stream, double percentage_treshold = 0, int silence_span = 200)
        {

            double ave = Converter.WavGetAbsAverage16bit(stream, false);
            double max = Converter.WavGetAbsMax16bit(stream);
            long countNonZero = WavGetSamplesCount16bit(stream, 1, -1);
            long countAbsAverage = WavGetSamplesCount16bit(stream, (int)ave, -(int)ave);


            Debugging.Throw.Exception("not done");
            return null;
        }

    }
}
/*
//silence removal
if (stream == null) return null;
            if (silence_span < 1) silence_span = 200;


            //byte[] data = Converter.WaveStreamToArray(stream);
            double[] fftdb = Converter.WaveStreamToFFTDb(stream, true, true);


            double ave = fftdb.Average();
            double min = fftdb.Min();
            double max = fftdb.Max();

            double[] data = Converter.WaveStreamToDoubleArray(stream, true);
            List<double> oputut = new List<double>();

            for(int i = 0; i < data.Length && i < fftdb.Length; i++)
            {
                if (fftdb[i] > fftdb_silence_treshold)
                    oputut.Add(data[i]);
            }

            return Converter.ArrayToWaveStream(oputut.ToArray(), stream.WaveFormat);

            /* int frame_span_ms = silence_span;

             source.Position = 0;
             Mp3FileReader reader = new Mp3FileReader(source);
             int duration = Converter.Mp3Duration(reader);

             Mp3Frame frame = reader.ReadNextFrame();
             Dictionary<double, double> milisecond_average = new Dictionary<double, double>();

             Stream destination = new MemoryStream();

             byte[] previous_buffer = null;
             bool started = false;
             while (frame != null)
             {

                 Stream stream = new MemoryStream();
                 double start = (reader.CurrentTime).TotalMilliseconds;
                 double span = 0;

                 while (frame != null && (span < frame_span_ms || (duration - start) <= frame_span_ms * 2))
                 {
                     stream.Write(frame.RawData, 0, frame.RawData.Length);
                     frame = reader.ReadNextFrame();

                     if (frame == null)
                         break;

                     span = ((reader.CurrentTime).TotalMilliseconds - start);
                 }


                 WaveStream pcm = Converter.Mp3ToWAV(stream);
                 byte[] data = WaveStreamToArray(pcm);
                 double[] ffr_result;
                 Converter.Process16bitWAV(ref data, out ffr_result, true);
                 double ave = ffr_result.Average();

                 if (ave > fftdb_silence_treshold)
                 {
                     byte[] buf = Streams.ToArray(stream);

                     if (previous_buffer != null)
                         destination.Write(previous_buffer, 0, previous_buffer.Length);

                     destination.Write(buf, 0, buf.Length);
                     previous_buffer = null;
                     started = true;
                 }
                 else
                 {
                     if (started)
                     {
                         byte[] buf = Streams.ToArray(stream);
                         destination.Write(buf, 0, buf.Length);
                         started = false;
                     }
                     else
                     {
                         previous_buffer = Streams.ToArray(stream);
                     }
                 }

                 stream.Close();
             }

             reader.Close();

             return destination;

            return null;
        }
//---------------------------------------
var kvp_array = milisecond_average.ToArray();
            int duration_counter = 0;
            List<double> duration_list = new List<double>();
            List<double> removal_list = new List<double>();
            for (int i = 0; i < kvp_array.Length; i++)
            {
                double key = kvp_array[i].Key;
                double value = kvp_array[i].Value;
                if (value > fftdb_silence_treshold)
                {
                    ++duration_counter;
                    duration_list.Add(key);
                }
                else if((value <= fftdb_silence_treshold) || i == (kvp_array.Length - 1))
                {
                    if (frame_span_ms * duration_counter <= min_speech_duration)
                        removal_list.AddRange(duration_list);

                    duration_counter = 0;
                    duration_list.Clear();
                }
            }

            foreach (double key in removal_list)
                milisecond_average[key] = -1;
*/

/*

                buffer = new byte[1024 * 1024];
                WaveFormat format = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2, frame.FrameLength, frame.BitRate);
                AcmMp3FrameDecompressor decompressor = new AcmMp3FrameDecompressor(format);
                BufferedWaveProvider provider = new BufferedWaveProvider(decompressor.OutputFormat);
                int length = decompressor.DecompressFrame(frame, buffer, 0);
              
                

                buffer_decompressed = buffer.Take(length).ToArray();
*/
