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
using AsmodatMath;
using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

namespace Asmodat.Audio
{
    public partial class Converter
    {
        public static void Process16bitWAV(ref byte[] sample, out double[] fft_left, out double[] fft_right, bool corrections)
        {
            if (sample == null || sample.Length <= 1)
            {
                fft_left = null;
                fft_right = null;
                return;
            }

            int size = (int)(Math.Log(sample.Length) / Math.Log(2));

            byte[] data = sample.Take((int)Math.Pow(2, size)).ToArray();

            fft_left = new double[data.Length / 4];
            fft_right = new double[data.Length / 4];
            for(int i = 0, h = 0; i < data.Length; i += 4)
            {
                fft_left[h] = (double)BitConverter.ToInt16(data, i);
                fft_right[h] = (double)BitConverter.ToInt16(data, i + 2);
                h++;
            }

            fft_left = AMath.FourierTransform.FFTDb(ref fft_left);
            fft_right = AMath.FourierTransform.FFTDb(ref fft_right);

            if(corrections)
            {
                fft_left = Doubles.Replace(fft_left, double.NaN, 0);
                fft_left = Doubles.Replace(fft_left, double.NegativeInfinity, 0);
                fft_left = Doubles.Replace(fft_left, double.PositiveInfinity, 140);
                fft_right = Doubles.Replace(fft_right, double.NaN, 0);
                fft_right = Doubles.Replace(fft_right, double.NegativeInfinity, 0);
                fft_right = Doubles.Replace(fft_right, double.PositiveInfinity, 140);
            }
        }

        public static void Process16bitWAV(ref byte[] sample, out double[] fft_result, bool corrections)
        {
            if (sample == null || sample.Length <= 1)
            {
                fft_result = null;
                return;
            }

            int size = (int)(Math.Log(sample.Length) / Math.Log(2));

            byte[] data = sample.Take((int)Math.Pow(2, size)).ToArray();

            fft_result = new double[data.Length / 2];
            for (int i = 0, h = 0; i < data.Length; i += 2)
            {
                fft_result[h] = (double)BitConverter.ToInt16(data, i);
                h++;
            }

            fft_result = AMath.FourierTransform.FFTDb(ref fft_result);

            if (corrections)
            {
                fft_result = Doubles.Replace(fft_result, double.NaN, 0);
                fft_result = Doubles.Replace(fft_result, double.NegativeInfinity, 0);
                fft_result = Doubles.Replace(fft_result, double.PositiveInfinity, 140);
            }
        }


        
    }
}

/*
public static double[] WaveStreamToFFTDb(WaveStream stream, bool corrections = true, bool swap = true)
        {
            

            byte[] samples = WaveStreamToArray(stream);

            if (samples == null || samples.Length <= 1)
                return null;

      
            double[] result = new double[samples.Length / 2];

            int size = AMath.RoundToPow(samples.Length, 2);// (int)Math.Pow(2, (int)(Math.Log() / Math.Log(2)));//.ToArray( ;

            byte[] samples1 = new byte[size];
            byte[] samples2 = new byte[size];

            int diff = samples.Length - size;
            for (int i = 0; i < samples.Length; i++)
            {
                if (i < size)
                    samples1[i] = samples[i];

                if(i >= diff)
                    samples2[i - diff] = samples[i];
            }

            double[] result1 = Converter.WavToDoubleArray(samples1, swap);
            double[] result2 = Converter.WavToDoubleArray(samples2, swap);

            result1 = AMath.FourierTransform.FFTDb(ref result1);
            result2 = AMath.FourierTransform.FFTDb(ref result2);
            
            for (int i = 0; i < result.Length; i++)
            {
                if (i < result1.Length*2)
                    result[i] = result1[i/2];
                else
                    result[i] = result2[i/2 - diff/4];
            }

            if (corrections)
            {
                result = Doubles.Replace(result, double.NaN, 0);
                result = Doubles.Replace(result, double.NegativeInfinity, 0);
                result = Doubles.Replace(result, double.PositiveInfinity, 140);
            }

            return result;
        }
*/
