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
using AsmodatMath;
using Asmodat.Debugging;

namespace Asmodat.Audio
{
    public partial class Converter
    {

        public static void Mp3Split(string mp3_source, int milisecond, string destination1 = null, string destination2 = null)
        {
            string name = Path.GetFileNameWithoutExtension(mp3_source);

            if (destination1 == null || destination2 == null)
            {
                string directory = Path.GetDirectoryName(mp3_source) + "\\";
                destination1 = directory + name + "-part1.mp3";
                destination2 = directory + name + "-part2.mp3";
            }

            Converter.Mp3Split(mp3_source, new int[] { milisecond }, new string[] { destination1, destination2 });
        }


        

        public static void Mp3Split(string mp3_source, int[] miliseconds, string[] mp3_destinations, bool removeSilence = false)
        {
            string name = Path.GetFileNameWithoutExtension(mp3_source);
            Mp3FileReader reader = new Mp3FileReader(mp3_source);
            //double duration = (reader.TotalTime).TotalMilliseconds;
            //int parts = (int)Math.Ceiling((double)duration / miliseconds);

            if (reader == null || miliseconds.Length + 1 != mp3_destinations.Length)
            {
                Output.WriteLine("Mp3 Output destinations missing !");
                return;
            }
                


            double currentTime;
            string destination;
            Mp3Frame frame;
            for (int i = 0; i < mp3_destinations.Length; i++)
            {
                currentTime = 0;
                destination = mp3_destinations[i];
                frame = reader.ReadNextFrame();


                Stream memory = new MemoryStream();

                while (frame != null && (i >= miliseconds.Length || currentTime < miliseconds[i]))
                {
                    currentTime = (reader.CurrentTime).TotalMilliseconds;

                    memory.Write(frame.RawData, 0, frame.RawData.Length);
                    frame = reader.ReadNextFrame();
                }



                byte[] result;

                if (removeSilence)
                    result = Streams.ToArray(Converter.Mp3RemoveSilence(memory));
                else
                    result = Streams.ToArray(memory);

                if (File.Exists(destination))
                    File.Delete(destination);

                if (Path.GetExtension(destination).ToLower().Contains("wav"))
                {
                    Stream temp = Streams.ToMemory(result);
                    WaveStream pcm = Converter.Mp3ToWAV(temp);
                    if (pcm == null)
                        continue;


                    //pcm = WavLevel16bit(pcm, 50, short.MaxValue);
                    //WaveStream pcm2 = pcm;// 
                    pcm = Converter.WavExtrapolate16bit(pcm, SampleRates.f44kHz);


                    //pcm = WavRemoveSilence2(pcm, 15, 50);

                    byte[] ba = Converter.WaveStreamToArray(pcm);
                    //double[] da = Converter.WaveStreamToDoubleArray(pcm, true);
                    double[] da = Converter.WavToDoubleArray(ba, true);

                    /// Fundamentals -  85   Hz -  250 Hz
                    /// Vowels -        350  Hz -  2k  Hz
                    /// Consonants -    1.5k Hz -  4k  Hz

                    da = AMath.FilterBandPassRLC(da, pcm.WaveFormat.SampleRate, 85, 4000, 1);
                    //da = AMath.FilterBandPassRLC(da, pcm.WaveFormat.SampleRate, 85, 250, 1);

                    byte[] ba2 = Converter.WavToByteArray(da, true);

                    pcm = Converter.ArrayToWaveStream(ba2, pcm.WaveFormat);

                    pcm = WavLevel16bit(pcm, 100, short.MaxValue);

                    /*double ave = Converter.WavGetAbsAverage16bit(pcm, false);
                    double max = Converter.WavGetAbsMax16bit(pcm);
                    long countNonZero = WavGetSamplesCount16bit(pcm, 1, -1);
                    long countAbsAverage = WavGetSamplesCount16bit(pcm, (int)ave, -(int)ave);

                   */


                    if (pcm != null)
                    {
                        WaveFileWriter.CreateWaveFile(destination, pcm);
                        temp.Close();
                        pcm.Close();
                    }
                }
                else
                {
                    FileStream stream = new FileStream(destination, FileMode.Create, FileAccess.Write);
                    stream.Write(result, 0, result.Length);
                    stream.Close();
                }
                memory.Close();
            }

            reader.Close();
        }

    }
}


/*
byte[] data = Converter.WaveStreamToArray(pcm);
                    
                    short[] data2 =  Converter.WavToShortArray(data);

                    for(int x = 0; x < data2.Length; x++)
                    {
                        int val = data2[x] * 8;
                        data2[x] = (short)val;
                    }

                    byte[] data3 = Converter.WavToByteArray(data2);

                    for (int x = 0; x < data3.Length; x++)
                    {
                        if (data3[x] != data[x])
                            break;
                    }


                    WaveStream pcm2 = Converter.ArrayToWaveStream(data3, pcm.WaveFormat);

    public static void Mp3Split(string mp3_source, int[] miliseconds, string[] mp3_destinations, bool removeSilence = false)
        {
            string name = Path.GetFileNameWithoutExtension(mp3_source);
            Mp3FileReader reader = new Mp3FileReader(mp3_source);
            //double duration = (reader.TotalTime).TotalMilliseconds;
            //int parts = (int)Math.Ceiling((double)duration / miliseconds);

            if (miliseconds.Length + 1 != mp3_destinations.Length)
                throw new Exception("Mp3 Output destinations missing !");


            double currentTime;
            string destination;
            Mp3Frame frame;
            for (int i = 0; i < mp3_destinations.Length; i++)
            {
                currentTime = 0;
                destination = mp3_destinations[i];
                frame = reader.ReadNextFrame();

                if (File.Exists(destination)) File.Delete(destination);
                FileStream stream = new FileStream(destination, FileMode.Create, FileAccess.Write);

                while (frame != null && (i >= miliseconds.Length || currentTime < miliseconds[i]))
                {
                    currentTime = (reader.CurrentTime).TotalMilliseconds;

                    stream.Write(frame.RawData, 0, frame.RawData.Length);
                    frame = reader.ReadNextFrame();
                }
                h
                stream.Close();
            }

            reader.Close();
        }

                buffer = new byte[1024 * 1024];
                WaveFormat format = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2, frame.FrameLength, frame.BitRate);
                AcmMp3FrameDecompressor decompressor = new AcmMp3FrameDecompressor(format);
                BufferedWaveProvider provider = new BufferedWaveProvider(decompressor.OutputFormat);
                int length = decompressor.DecompressFrame(frame, buffer, 0);
              
                

                buffer_decompressed = buffer.Take(length).ToArray();
*/
