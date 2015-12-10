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

namespace Asmodat.Audio
{
    public partial class Converter
    {

        //double min_speech_duration = 500,
        public static int[] GetMp3SilenceSplit(string mp3_source, double fftdb_silence_treshold = 5,  int parts_max = int.MaxValue)
        {
            //string name = Path.GetFileNameWithoutExtension(mp3_source);
            int frame_span_ms = 200;
            Mp3FileReader reader = new Mp3FileReader(mp3_source);
            int duration = Converter.Mp3Duration(reader);

            Mp3Frame frame = reader.ReadNextFrame();
            Dictionary<double, double> milisecond_average = new Dictionary<double, double>();

            while (frame != null)
            {

                Stream stream = new MemoryStream();
                double start = (reader.CurrentTime).TotalMilliseconds;
                double span = 0;

                while (frame != null && (span < frame_span_ms || (duration - start) <= frame_span_ms * 2))
                {
                    // currentTime = (reader.CurrentTime).TotalMilliseconds;
                    stream.Write(frame.RawData, 0, frame.RawData.Length);
                    frame = reader.ReadNextFrame();

                    if (frame == null)
                        break;

                    span = ((reader.CurrentTime).TotalMilliseconds - start);
                }

                stream.Position = 0;

                WaveStream pcm = Converter.Mp3ToWAV(stream);
                byte[] data = WaveStreamToArray(pcm);
                double[] left, right;

                //silence = 0
                //whisper = 15
                //conversation = 60
                Converter.Process16bitWAV(ref data, out left, out right, true);

                double aveL = left.Average();
                double aveR = right.Average();


                milisecond_average.Add(start + frame_span_ms, (double)(aveL + aveR) / 2);
                stream.Close();
            }




            var kvp_array = milisecond_average.ToArray();
            int counter = 0;
            List<double> counter_list = new List<double>();
            Dictionary<double, int> milisecond_counter = new Dictionary<double, int>();
            int counter_parts = 0;
            for (int i = 0; i < kvp_array.Length; i++)
            {
                double key = kvp_array[i].Key;
                double value = kvp_array[i].Value;

                if (value > fftdb_silence_treshold)
                {
                    ++counter;
                    counter_list.Add(key);
                }

                if (counter_list.Count > 0)
                {
                    foreach (double k in counter_list)
                    {
                        if (milisecond_counter.ContainsKey(k))
                            milisecond_counter[k] = counter;
                        else
                            milisecond_counter.Add(k, counter);
                    }
                }


                if (value <= fftdb_silence_treshold)
                {
                    if (counter > 0)
                        ++counter_parts;
                    

                    counter = 0;
                    counter_list.Clear();
                    milisecond_counter.Add(key, int.MaxValue);
                }
                else if (i == kvp_array.Length - 1 && counter > 0)
                    ++counter_parts;
                
            }

            while (counter_parts > parts_max)
            {
                var kvp = milisecond_counter.ToArray();
                bool found = false;
                int min = milisecond_counter.Values.Min();
                for (int i = 0; i < kvp.Length; i++)
                {
                    double key = kvp[i].Key;
                    double value = kvp[i].Value;

                    if (found && value != min)
                        break;

                    if (!found && value == min)
                        found = true;

                    if (!found && value != min)
                        continue;

                    milisecond_average.Remove(key);
                    milisecond_counter.Remove(key);
                }

                --counter_parts;
            }



            List<int> list = new List<int>();
            bool started = false;
            foreach (var kvp in milisecond_average)
            {
                if (kvp.Value > fftdb_silence_treshold)
                    started = true;

                if (!started)
                    continue;

                if (kvp.Value <= fftdb_silence_treshold)
                {
                    list.Add((int)(kvp.Key + ((double)frame_span_ms / 2)));
                    started = false;
                }
            }



            reader.Close();


            if (!started && list.Count <= 1)
                return new int[0];
            else if (!started)
                list.RemoveAt(list.Count - 1);

            if(list.Count > parts_max)//remove smallest parts
            {

            }

            return list.ToArray();
        }


        public static int[] GetMp3SilenceSplit(string mp3_source, int parts = 1)
        {
            if (parts < 1)
                parts = 1;

            int frame_span_ms = 200;
            Mp3FileReader reader = new Mp3FileReader(mp3_source);
            int duration = Converter.Mp3Duration(reader);

            Mp3Frame frame = reader.ReadNextFrame();
            Dictionary<double, double> milisecond_average = new Dictionary<double, double>();

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

                stream.Position = 0;

                WaveStream pcm = Converter.Mp3ToWAV(stream);
                byte[] data = WaveStreamToArray(pcm);
                double[] fft_result;

                Converter.Process16bitWAV(ref data, out fft_result, true);

                double ave = fft_result.Average();

                milisecond_average.Add(start + frame_span_ms, ave);
                stream.Close();
            }

           /* double[] keys = milisecond_average.Keys.ToArray();
            for (int i = 0; i < keys.Length;i++)
                if (milisecond_average[keys[i]] <= 0)
                    milisecond_average.Remove(keys[i]);*/
            


            int[] indexesOfMinimaParts = AsmodatMath.AMath.GetSplitIndexesMinima(milisecond_average.Values.ToArray(), parts);

            if (indexesOfMinimaParts == null)
                return null;

            List<int> results = new List<int>();
            foreach(int i in indexesOfMinimaParts)
            {
                double rezult = milisecond_average.Keys.ToArray()[i];
                results.Add((int)(rezult + ((double)frame_span_ms / 2)));
            }

            return results.ToArray();

        }


        public static int[] GetMp3WordsSplit(string mp3_source, int parts = 1)
        {
            if (parts < 1)
                parts = 1;

            int frame_span_ms = 200;
            Mp3FileReader reader = new Mp3FileReader(mp3_source);
            int duration = Converter.Mp3Duration(reader);

            Mp3Frame frame = reader.ReadNextFrame();
            Dictionary<double, double> milisecond_average = new Dictionary<double, double>();

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

                stream.Position = 0;

                WaveStream pcm = Converter.Mp3ToWAV(stream);
                byte[] data = WaveStreamToArray(pcm);
                double[] fft_result;

                Converter.Process16bitWAV(ref data, out fft_result, true);

                double ave = fft_result.Average();

                milisecond_average.Add(start + frame_span_ms, ave);
                stream.Close();
            }

            /* double[] keys = milisecond_average.Keys.ToArray();
             for (int i = 0; i < keys.Length;i++)
                 if (milisecond_average[keys[i]] <= 0)
                     milisecond_average.Remove(keys[i]);*/



            int[] indexesOfMinimaParts = AsmodatMath.AMath.GetSplitIndexesMinima(milisecond_average.Values.ToArray(), parts);

            if (indexesOfMinimaParts == null)
                return null;

            List<int> results = new List<int>();

            for(int i = 0; i < indexesOfMinimaParts.Length;i++)
            {
                int idx = indexesOfMinimaParts[i];
                double rezult = milisecond_average.Keys.ToArray()[idx];

                if(i % 2 == 0)  results.Add((int)rezult);
                else results.Add((int)(rezult + (double)((double)frame_span_ms/2)));
            }

            return results.ToArray();

        }

    }
}
/*

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
