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
using Asmodat.Types;
using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

namespace Asmodat.Audio
{
    public partial class Converter
    {
        /// <summary>
        /// processing works better with byte swap
        /// </summary>
        /// <param name="wav_array"></param>
        /// <param name="swap"></param>
        /// <returns></returns>
        public static short[] WavToShortArray(byte[] wav_array, bool swap = true)
        {
            if (wav_array == null)
                return null;

            if (wav_array.Length <= 1)
                return new short[0];

            short[] result = new short[wav_array.Length / 2];
            for (int i = 0, h = 0; i < wav_array.Length; i += 2)
                result[h++] = Abbreviate.Convert.ToShort(wav_array, i, swap);


            return result;
        }

        public static double[] WavToDoubleArray(byte[] wav_array, bool swap = true)
        {
            if (wav_array == null)
                return null;

            if (wav_array.Length <= 1)
                return new double[0];

            double[] result = new double[wav_array.Length / 2];
            for (int i = 0, h = 0; i < wav_array.Length; i += 2)
                result[h++] = (double)Abbreviate.Convert.ToShort(wav_array, i, swap);

            return result;
        }


        /// <summary>
        /// processing works better with byte swap
        /// </summary>
        /// <param name="wav_array"></param>
        /// <param name="swapped"></param>
        /// <returns></returns>
        public static byte[] WavToByteArray(short[] wav_array, bool swap = true)
        {
            if (wav_array == null)
                return null;

            if (wav_array.Length <= 0)
                return new byte[0];


            byte[] result = new byte[wav_array.Length * 2];

            for (int i = 0, h = 0; i < result.Length; i += 2)
            {
                byte[] buf = Abbreviate.Convert.ToBytes(wav_array[h++], swap);
                result[i] =  buf[0];
                result[i + 1] = buf[1];
            }

            return result;
        }

        public static byte[] WavToByteArray(double[] wav_array, bool swap = true)
        {
            if (wav_array == null)
                return null;

            if (wav_array.Length <= 0)
                return new byte[0];


            byte[] result = new byte[wav_array.Length * 2];

            for (int i = 0, h = 0; i < result.Length; i += 2)
            {
                short sample = (short)wav_array[h++];
                byte[] buf = Abbreviate.Convert.ToBytes(sample, swap);
                result[i] = buf[0];
                result[i + 1] = buf[1];
            }

            return result;
        }


        public enum SampleRates : int
        {
            f11kHz = 11025,
            f22kHz = 22050,
            f44kHz = 44100,
            f88kHz = 88200,
            f176kHz = 176400
        }


        /// <summary>
        /// wave linear extrapolation into specified bitrate
        /// </summary>
        /// <param name="pcm"></param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static WaveStream WavExtrapolate16bit(WaveStream pcm, SampleRates rate)
        {
            int desiretrate = (int)rate;
            int surrentrate = pcm.WaveFormat.SampleRate;
            int multiplayer = (int)desiretrate / surrentrate;

            if (multiplayer <= 1)
                return pcm;

            bool swap = true;
            byte[] data = Converter.WaveStreamToArray(pcm);
            short[] dataSha = Converter.WavToShortArray(data, swap);
            short[] outputSha = new short[dataSha.Length * multiplayer];


            int first = 0, second = 0;
            int i = 0, h = 0;
            for (; i < dataSha.Length - 1; i++, h += multiplayer)
            {
                first = dataSha[i];
                second = dataSha[i + 1];
                double y = second - first;
                double x = multiplayer;
                double a = (double)y / x;

                outputSha[h] = (short)first;
                for (int i2 = 1; i2 < multiplayer; i2++)
                    outputSha[h + i2] = (short)((double)first + (double)(a * i2));
            }

            outputSha[outputSha.Length - 1] = (short)second;

            byte[] outputdata = Converter.WavToByteArray(outputSha, swap);

            WaveFormat format = pcm.WaveFormat;
            WaveFormat outputformat = new WaveFormat(format.SampleRate * multiplayer, format.Channels);//format.BitsPerSample, 
            WaveStream outputstream = Converter.ArrayToWaveStream(outputdata, outputformat);
            return outputstream;
        }


        /// <summary>
        /// amplifies audio by lebel in %, if multiplayer is NaN, algorithm fixes it to max-optimum value
        /// </summary>
        /// <param name="pcm"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static WaveStream WavVolume16bit(WaveStream pcm, double multiplayer = 1)
        {
            if (Doubles.IsInfinityOrNaN(multiplayer) || pcm == null)
                return null;

            bool swap = true;
            byte[] data = Converter.WaveStreamToArray(pcm);
            short[] dataSha = Converter.WavToShortArray(data, swap);

            int i = 0, val = 0;
            short max = short.MaxValue;
            short min = short.MinValue;
            for (; i < dataSha.Length - 1; i++)
            {
                val = (int)(multiplayer * ((int)dataSha[i]));
                if (val > max)  val = max;
                else if (val < min) val = min;

                dataSha[i] = (short)val;
            }

            byte[] outputdata = Converter.WavToByteArray(dataSha, swap);
            WaveStream outputstream = Converter.ArrayToWaveStream(outputdata, pcm.WaveFormat);
            return outputstream;
        }

        /// <summary>
        /// level - x% 
        /// max - 100%
        /// x - result
        /// </summary>
        /// <param name="pcm"></param>
        /// <param name="level"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static WaveStream WavLevel16bit(WaveStream pcm, double level = 50, short max = short.MaxValue)
        {
            if (Doubles.IsInfinityOrNaN(level) || pcm == null)
                return null;

            bool swap = true;
            byte[] data = Converter.WaveStreamToArray(pcm);
            short[] dataSha = Converter.WavToShortArray(data, swap);

            double dmin = dataSha.Min();
            double dmax = dataSha.Min();
            double absolute = WavGetAbsMax16bit(pcm);// Math.Max(Math.Abs(dmax), Math.Abs(dmin));

            
            double current = (double)(absolute * 100) / max; //x%

            double  multiplayer = (level / current); //use level%

            return Converter.WavVolume16bit(pcm, multiplayer);
        }


        public static short WavGetAbsMax16bit(WaveStream pcm)
        {
            if (pcm == null)
                return 0;

            bool swap = true;
            byte[] data = Converter.WaveStreamToArray(pcm);
            short[] dataSha = Converter.WavToShortArray(data, swap);

            double dmin = dataSha.Min();
            double dmax = dataSha.Min();
            return (short)Math.Max(Math.Abs(dmax), Math.Abs(dmin));
        }

        public static double WavGetAbsAverage16bit(WaveStream pcm, bool CountZeros = false)
        {
            if (pcm == null)
                return 0;

            bool swap = true;
            byte[] data = Converter.WaveStreamToArray(pcm);
            short[] dataSha = Converter.WavToShortArray(data, swap);

            double sum = 0;
            int count = 0;
            short s;
            for(int i = 0; i < dataSha.Length; i++)
            {
                s = dataSha[i];
                if(!CountZeros || s != 0)
                {
                    ++count;
                    sum += Math.Abs(s);
                }
            }

            return (double)sum / count;
        }




        public static long WavGetSamplesCount16bit(WaveStream pcm, int AboweOrEqual = 1, int BelowOrEqual = -1)
        {
            if (pcm == null)
                return 0;

            bool swap = true;
            byte[] data = Converter.WaveStreamToArray(pcm);
            short[] dataSha = Converter.WavToShortArray(data, swap);

            long count = 0;
            short s;
            for (int i = 0; i < dataSha.Length; i++)
            {
                s = dataSha[i];
                if (s >= AboweOrEqual || s <= BelowOrEqual)
                    ++count;
            }

            return count;
        }


        public static WaveStream WavRemoveSamples16bit(WaveStream pcm, short MinRange = short.MinValue, short MaxRange= short.MaxValue, double continuous_ms = double.MaxValue)
        {
            if (pcm == null)
                return null;

            
            byte[] data = Converter.WaveStreamToArray(pcm);

            if (data.Length <= 1)
                return pcm;


            int rate = pcm.WaveFormat.SampleRate;
            int level_sapn = (int)Math.Ceiling(((double)rate / 1000) * continuous_ms); 

            bool swap = true;
            short[] dataSha = Converter.WavToShortArray(data, swap);

            short s;
            int current_span = 0;
            int index = -1; //start index
            for (int i = 0; i < dataSha.Length; i++)
            {
                s = dataSha[i];
                if (s >= MinRange && s <= MaxRange)
                {
                    ++current_span;
                    if (index < 0)
                        index = i;
                }
                else
                {
                    current_span = 0;
                    index = -1;
                }

                if(current_span > level_sapn)
                {
                    for (; index <= i; index++)
                        dataSha[index] = 0;
                }

            }


            return Converter.ArrayToWaveStream(dataSha, pcm.WaveFormat, swap);

        }
    }
}


/*
ublic static short[] WavToShortArray(byte[] wav_array)
        {
            if (wav_array == null)
                return null;

            if (wav_array.Length <= 1)
                return new short[0];


            short[] result = new short[wav_array.Length / 2];
            for (int i = 0, h = 0; i < wav_array.Length; i += 2)
            {
                byte[] temp = new byte[2] { wav_array[i + 1], wav_array[i] };
                short pack = BitConverter.ToInt16(temp, 0);
                result[h] = pack;
                h++;
            }

            return result;
        }



        public static byte[] WavToByteArray(short[] wav_array)
        {
            if (wav_array == null)
                return null;

            if (wav_array.Length <= 0)
                return new byte[0];


            byte[] result = new byte[wav_array.Length * 2];

            for (int i = 0, h = 0; i < wav_array.Length; i += 2)
            {
                //short s = System.Convert.ToInt16(wav_array[h]); - for double
                byte[] buf = BitConverter.GetBytes(wav_array[h]);
                result[i] = buf[1];
                result[i + 1] = buf[0];
                h++;
            }

            return result;
        }
*/
