using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

using System.Numerics;
using System.Globalization;

namespace AsmodatMath
{
    public partial class AMath
    {
        public class FourierTransform
        {
            public const int FREQUENCYSLOTCOUNT = 19;
            private static int[] _meterFrequencies = new int[FREQUENCYSLOTCOUNT] { 30, 55, 80, 120, 155, 195, 250, 375, 500, 750, 1000, 1500, 2000, 3000, 4000, 6000, 8000, 12000, 16000 };
            private static int _frequencySlotCount = FREQUENCYSLOTCOUNT;


            /// <summary>
            /// 	Changes the Frequency Bands to analyze.
            /// 	Affects the other static methods
            /// </summary>
            /// <param name="meterFrequencies"></param>
            public static void SetMeterFrequencies(int[] meterFrequencies)
            {
                _meterFrequencies = meterFrequencies;
                _frequencySlotCount = meterFrequencies.Length;
            }

            public static double[] FFTDb(ref double[] x)
            {
                //uint pow2Samples = FFT.NextPowerOfTwo((uint)x.Length);
                double[] xre = new double[x.Length];
                double[] xim = new double[x.Length];

                Compute((uint)x.Length, x, null, xre, xim, false);

                double[] decibel = new double[xre.Length / 2];

                for (int i = 0; i < decibel.Length; i++)
                    decibel[i] = 10.0 * Math.Log10((float)(Math.Sqrt((xre[i] * xre[i]) + (xim[i] * xim[i]))));
                return decibel;
            }

            public const Double DDC_PI = 3.14159265358979323846;

            /// <summary>
            /// Verifies a number is a power of two
            /// </summary>
            /// <param name="x">Number to check</param>
            /// <returns>true if number is a power two (i.e.:1,2,4,8,16,...)</returns>
            public static Boolean IsPowerOfTwo(UInt32 x)
            {
                return ((x != 0) && (x & (x - 1)) == 0);
            }

            /// <summary>
            /// Get Next power of number.
            /// </summary>
            /// <param name="x">Number to check</param>
            /// <returns>A power of two number</returns>
            public static UInt32 NextPowerOfTwo(UInt32 x)
            {
                x = x - 1;
                x = x | (x >> 1);
                x = x | (x >> 2);
                x = x | (x >> 4);
                x = x | (x >> 8);
                x = x | (x >> 16);
                return x + 1;
            }

            /// <summary>
            /// Get Number of bits needed for a power of two
            /// </summary>
            /// <param name="PowerOfTwo">Power of two number</param>
            /// <returns>Number of bits</returns>
            public static UInt32 NumberOfBitsNeeded(UInt32 PowerOfTwo)
            {
                if (PowerOfTwo > 0)
                {
                    for (UInt32 i = 0, mask = 1; ; i++, mask <<= 1)
                    {
                        if ((PowerOfTwo & mask) != 0)
                            return i;
                    }
                }
                return 0; // error
            }

            /// <summary>
            /// Reverse bits
            /// </summary>
            /// <param name="index">Bits</param>
            /// <param name="NumBits">Number of bits to reverse</param>
            /// <returns>Reverse Bits</returns>
            public static UInt32 ReverseBits(UInt32 index, UInt32 NumBits)
            {
                UInt32 i, rev;

                for (i = rev = 0; i < NumBits; i++)
                {
                    rev = (rev << 1) | (index & 1);
                    index >>= 1;
                }

                return rev;
            }

            /// <summary>
            /// Return index to frequency based on number of samples
            /// </summary>
            /// <param name="Index">sample index</param>
            /// <param name="NumSamples">number of samples</param>
            /// <returns>Frequency index range</returns>
            public static Double IndexToFrequency(UInt32 Index, UInt32 NumSamples)
            {
                if (Index >= NumSamples)
                    return 0.0;
                else if (Index <= NumSamples / 2)
                    return (double)Index / (double)NumSamples;

                return -(double)(NumSamples - Index) / (double)NumSamples;
            }

            /// <summary>
            /// Compute FFT
            /// </summary>
            /// <param name="NumSamples">NumSamples Number of samples (must be power two)</param>
            /// <param name="pRealIn">Real samples</param>
            /// <param name="pImagIn">Imaginary (optional, may be null)</param>
            /// <param name="pRealOut">Real coefficient output</param>
            /// <param name="pImagOut">Imaginary coefficient output</param>
            /// <param name="bInverseTransform">bInverseTransform when true, compute Inverse FFT</param>
            public static void Compute(UInt32 NumSamples, Double[] pRealIn, Double[] pImagIn,
                                                    Double[] pRealOut, Double[] pImagOut, Boolean bInverseTransform)
            {
                UInt32 NumBits;    //* Number of bits needed to store indices 
                UInt32 i, j, k, n;
                UInt32 BlockSize, BlockEnd;

                double angle_numerator = 2.0 * DDC_PI;
                double tr, ti;     // temp real, temp imaginary

                if (pRealIn == null || pRealOut == null || pImagOut == null)
                {
                    // error
                    throw new ArgumentNullException("Null argument");
                }
                if (!IsPowerOfTwo(NumSamples))
                {
                    // error
                    throw new ArgumentException("Number of samples must be power of 2");
                }
                if (pRealIn.Length < NumSamples || (pImagIn != null && pImagIn.Length < NumSamples) ||
                         pRealOut.Length < NumSamples || pImagOut.Length < NumSamples)
                {
                    // error
                    throw new ArgumentException("Invalid Array argument detected");
                }

                if (bInverseTransform)
                    angle_numerator = -angle_numerator;

                NumBits = NumberOfBitsNeeded(NumSamples);


                //   Do simultaneous data copy and bit-reversal ordering into outputs...

                for (i = 0; i < NumSamples; i++)
                {
                    j = ReverseBits(i, NumBits);
                    pRealOut[j] = pRealIn[i];
                    pImagOut[j] = (double)((pImagIn == null) ? 0.0 : pImagIn[i]);
                }


                //   Do the FFT itself...

                BlockEnd = 1;
                for (BlockSize = 2; BlockSize <= NumSamples; BlockSize <<= 1)
                {
                    double delta_angle = angle_numerator / (double)BlockSize;
                    double sm2 = Math.Sin(-2 * delta_angle);
                    double sm1 = Math.Sin(-delta_angle);
                    double cm2 = Math.Cos(-2 * delta_angle);
                    double cm1 = Math.Cos(-delta_angle);
                    double w = 2 * cm1;
                    double ar0, ar1, ar2;
                    double ai0, ai1, ai2;

                    for (i = 0; i < NumSamples; i += BlockSize)
                    {
                        ar2 = cm2;
                        ar1 = cm1;

                        ai2 = sm2;
                        ai1 = sm1;

                        for (j = i, n = 0; n < BlockEnd; j++, n++)
                        {
                            ar0 = w * ar1 - ar2;
                            ar2 = ar1;
                            ar1 = ar0;

                            ai0 = w * ai1 - ai2;
                            ai2 = ai1;
                            ai1 = ai0;

                            k = j + BlockEnd;
                            tr = ar0 * pRealOut[k] - ai0 * pImagOut[k];
                            ti = ar0 * pImagOut[k] + ai0 * pRealOut[k];

                            pRealOut[k] = (pRealOut[j] - tr);
                            pImagOut[k] = (pImagOut[j] - ti);

                            pRealOut[j] += (tr);
                            pImagOut[j] += (ti);
                        }
                    }

                    BlockEnd = BlockSize;
                }


                //  Need to normalize if inverse transform...

                if (bInverseTransform)
                {
                    double denom = (double)(NumSamples);

                    for (i = 0; i < NumSamples; i++)
                    {
                        pRealOut[i] /= denom;
                        pImagOut[i] /= denom;
                    }
                }
            }

            /// <summary>
            /// Calculate normal (power spectrum)
            /// </summary>
            /// <param name="NumSamples">Number of sample</param>
            /// <param name="pReal">Real coefficient buffer</param>
            /// <param name="pImag">Imaginary coefficient buffer</param>
            /// <param name="pAmpl">Working buffer to hold amplitude Xps(m) = | X(m)^2 | = Xreal(m)^2  + Ximag(m)^2</param>
            public static void Norm(UInt32 NumSamples, Double[] pReal, Double[] pImag, Double[] pAmpl)
            {
                if (pReal == null || pImag == null || pAmpl == null)
                {
                    // error
                    throw new ArgumentNullException("pReal,pImag,pAmpl");
                }
                if (pReal.Length < NumSamples || pImag.Length < NumSamples || pAmpl.Length < NumSamples)
                {
                    // error
                    throw new ArgumentException("Invalid Array argument detected");
                }

                // Calculate amplitude values in the buffer provided
                for (UInt32 i = 0; i < NumSamples; i++)
                {
                    pAmpl[i] = pReal[i] * pReal[i] + pImag[i] * pImag[i];
                }
            }

            /// <summary>
            /// Find Peak frequency in Hz
            /// </summary>
            /// <param name="NumSamples">Number of samples</param>
            /// <param name="pAmpl">Current amplitude</param>
            /// <param name="samplingRate">Sampling rate in samples/second (Hz)</param>
            /// <param name="index">Frequency index</param>
            /// <returns>Peak frequency in Hz</returns>
            public static Double PeakFrequency(UInt32 NumSamples, Double[] pAmpl, Double samplingRate, ref UInt32 index)
            {
                UInt32 N = NumSamples >> 1;   // number of positive frequencies. (numSamples/2)

                if (pAmpl == null)
                {
                    // error
                    throw new ArgumentNullException("pAmpl");
                }
                if (pAmpl.Length < NumSamples)
                {
                    // error
                    throw new ArgumentException("Invalid Array argument detected");
                }

                double maxAmpl = -1.0;
                double peakFreq = -1.0;
                index = 0;

                for (UInt32 i = 0; i < N; i++)
                {
                    if (pAmpl[i] > maxAmpl)
                    {
                        maxAmpl = (double)pAmpl[i];
                        index = i;
                        peakFreq = (double)(i);
                    }
                }

                return samplingRate * peakFreq / (double)(NumSamples);
            }

            public static byte[] GetPeaks(double[] leftChannel, double[] rightChannel, int sampleFrequency)
            {
                byte[] peaks = new byte[_frequencySlotCount];
                byte[] channelPeaks = GetPeaksForChannel(leftChannel, sampleFrequency);

                ComparePeaks(peaks, channelPeaks);
                return peaks;
            }

            private static void ComparePeaks(byte[] overallPeaks, byte[] channelPeaks)
            {
                for (int i = 0; i < _frequencySlotCount; i++)
                {
                    overallPeaks[i] = Math.Max(overallPeaks[i], channelPeaks[i]);
                }
            }

            private static byte[] GetPeaksForChannel(double[] normalizedArray, int sampleFrequency)
            {
                double maxAmpl = (32767.0 * 32767.0);

                byte[] peaks = new byte[_frequencySlotCount];
                // update meter
                int centerFreq = (sampleFrequency / 2);
                byte peak;
                for (int i = 0; i < _frequencySlotCount; ++i)
                {
                    if (_meterFrequencies[i] > centerFreq)
                    {
                        peak = 0;
                    }
                    else
                    {
                        int index = (int)(_meterFrequencies[i] * normalizedArray.Length / sampleFrequency);
                        peak = (byte)Math.Max(0, (17.0 * Math.Log10(normalizedArray[index] / maxAmpl)));
                    }

                    peaks[i] = peak;
                }

                return peaks;
            }
        }




    }
}

/*
public class FourierTransform
        {
            public const int FREQUENCYSLOTCOUNT = 19;
            private static int[] _meterFrequencies = new int[FREQUENCYSLOTCOUNT] { 30, 55, 80, 120, 155, 195, 250, 375, 500, 750, 1000, 1500, 2000, 3000, 4000, 6000, 8000, 12000, 16000 };
            private static int _frequencySlotCount = FREQUENCYSLOTCOUNT;


            /// <summary>
            /// 	Changes the Frequency Bands to analyze.
            /// 	Affects the other static methods
            /// </summary>
            /// <param name="meterFrequencies"></param>
            public static void SetMeterFrequencies(int[] meterFrequencies)
            {
                _meterFrequencies = meterFrequencies;
                _frequencySlotCount = meterFrequencies.Length;
            }

            public static double[] FFTDb(ref double[] x)
            {
                //uint pow2Samples = FFT.NextPowerOfTwo((uint)x.Length);
                double[] xre = new double[x.Length];
                double[] xim = new double[x.Length];

                Compute((uint)x.Length, x, null, xre, xim, false);

                double[] decibel = new double[xre.Length / 2];

                for (int i = 0; i < decibel.Length; i++)
                    decibel[i] = 10.0 * Math.Log10((float)(Math.Sqrt((xre[i] * xre[i]) + (xim[i] * xim[i]))));
                return decibel;
            }

            public const Double DDC_PI = 3.14159265358979323846;

            /// <summary>
            /// Verifies a number is a power of two
            /// </summary>
            /// <param name="x">Number to check</param>
            /// <returns>true if number is a power two (i.e.:1,2,4,8,16,...)</returns>
            public static Boolean IsPowerOfTwo(UInt32 x)
            {
                return ((x != 0) && (x & (x - 1)) == 0);
            }

            /// <summary>
            /// Get Next power of number.
            /// </summary>
            /// <param name="x">Number to check</param>
            /// <returns>A power of two number</returns>
            public static UInt32 NextPowerOfTwo(UInt32 x)
            {
                x = x - 1;
                x = x | (x >> 1);
                x = x | (x >> 2);
                x = x | (x >> 4);
                x = x | (x >> 8);
                x = x | (x >> 16);
                return x + 1;
            }

            /// <summary>
            /// Get Number of bits needed for a power of two
            /// </summary>
            /// <param name="PowerOfTwo">Power of two number</param>
            /// <returns>Number of bits</returns>
            public static UInt32 NumberOfBitsNeeded(UInt32 PowerOfTwo)
            {
                if (PowerOfTwo > 0)
                {
                    for (UInt32 i = 0, mask = 1; ; i++, mask <<= 1)
                    {
                        if ((PowerOfTwo & mask) != 0)
                            return i;
                    }
                }
                return 0; // error
            }

            /// <summary>
            /// Reverse bits
            /// </summary>
            /// <param name="index">Bits</param>
            /// <param name="NumBits">Number of bits to reverse</param>
            /// <returns>Reverse Bits</returns>
            public static UInt32 ReverseBits(UInt32 index, UInt32 NumBits)
            {
                UInt32 i, rev;

                for (i = rev = 0; i < NumBits; i++)
                {
                    rev = (rev << 1) | (index & 1);
                    index >>= 1;
                }

                return rev;
            }

            /// <summary>
            /// Return index to frequency based on number of samples
            /// </summary>
            /// <param name="Index">sample index</param>
            /// <param name="NumSamples">number of samples</param>
            /// <returns>Frequency index range</returns>
            public static Double IndexToFrequency(UInt32 Index, UInt32 NumSamples)
            {
                if (Index >= NumSamples)
                    return 0.0;
                else if (Index <= NumSamples / 2)
                    return (double)Index / (double)NumSamples;

                return -(double)(NumSamples - Index) / (double)NumSamples;
            }

            /// <summary>
            /// Compute FFT
            /// </summary>
            /// <param name="NumSamples">NumSamples Number of samples (must be power two)</param>
            /// <param name="pRealIn">Real samples</param>
            /// <param name="pImagIn">Imaginary (optional, may be null)</param>
            /// <param name="pRealOut">Real coefficient output</param>
            /// <param name="pImagOut">Imaginary coefficient output</param>
            /// <param name="bInverseTransform">bInverseTransform when true, compute Inverse FFT</param>
            public static void Compute(UInt32 NumSamples, Double[] pRealIn, Double[] pImagIn,
                                                    Double[] pRealOut, Double[] pImagOut, Boolean bInverseTransform)
            {
                UInt32 NumBits;    //* Number of bits needed to store indices 
UInt32 i, j, k, n;
UInt32 BlockSize, BlockEnd;

double angle_numerator = 2.0 * DDC_PI;
double tr, ti;     // temp real, temp imaginary

                if (pRealIn == null || pRealOut == null || pImagOut == null)
                {
                    // error
                    throw new ArgumentNullException("Null argument");
                }
                if (!IsPowerOfTwo(NumSamples))
                {
                    // error
                    throw new ArgumentException("Number of samples must be power of 2");
                }
                if (pRealIn.Length<NumSamples || (pImagIn != null && pImagIn.Length<NumSamples) ||
                         pRealOut.Length<NumSamples || pImagOut.Length<NumSamples)
                {
                    // error
                    throw new ArgumentException("Invalid Array argument detected");
                }

                if (bInverseTransform)
                    angle_numerator = -angle_numerator;

                NumBits = NumberOfBitsNeeded(NumSamples);

                
                //   Do simultaneous data copy and bit-reversal ordering into outputs...
               
                for (i = 0; i<NumSamples; i++)
                {
                    j = ReverseBits(i, NumBits);
pRealOut[j] = pRealIn[i];
                    pImagOut[j] = (double)((pImagIn == null) ? 0.0 : pImagIn[i]);
                }

                
                //   Do the FFT itself...
                
                BlockEnd = 1;
                for (BlockSize = 2; BlockSize <= NumSamples; BlockSize <<= 1)
                {
                    double delta_angle = angle_numerator / (double)BlockSize;
double sm2 = Math.Sin(-2 * delta_angle);
double sm1 = Math.Sin(-delta_angle);
double cm2 = Math.Cos(-2 * delta_angle);
double cm1 = Math.Cos(-delta_angle);
double w = 2 * cm1;
double ar0, ar1, ar2;
double ai0, ai1, ai2;

                    for (i = 0; i<NumSamples; i += BlockSize)
                    {
                        ar2 = cm2;
                        ar1 = cm1;

                        ai2 = sm2;
                        ai1 = sm1;

                        for (j = i, n = 0; n<BlockEnd; j++, n++)
                        {
                            ar0 = w* ar1 - ar2;
                            ar2 = ar1;
                            ar1 = ar0;

                            ai0 = w* ai1 - ai2;
                            ai2 = ai1;
                            ai1 = ai0;

                            k = j + BlockEnd;
                            tr = ar0* pRealOut[k] - ai0* pImagOut[k];
ti = ar0* pImagOut[k] + ai0* pRealOut[k];

pRealOut[k] = (pRealOut[j] - tr);
                            pImagOut[k] = (pImagOut[j] - ti);

                            pRealOut[j] += (tr);
                            pImagOut[j] += (ti);
                        }
                    }

                    BlockEnd = BlockSize;
                }

              
                //  Need to normalize if inverse transform...
                
                if (bInverseTransform)
                {
                    double denom = (double)(NumSamples);

                    for (i = 0; i<NumSamples; i++)
                    {
                        pRealOut[i] /= denom;
                        pImagOut[i] /= denom;
                    }
                }
            }

            /// <summary>
            /// Calculate normal (power spectrum)
            /// </summary>
            /// <param name="NumSamples">Number of sample</param>
            /// <param name="pReal">Real coefficient buffer</param>
            /// <param name="pImag">Imaginary coefficient buffer</param>
            /// <param name="pAmpl">Working buffer to hold amplitude Xps(m) = | X(m)^2 | = Xreal(m)^2  + Ximag(m)^2</param>
            public static void Norm(UInt32 NumSamples, Double[] pReal, Double[] pImag, Double[] pAmpl)
{
    if (pReal == null || pImag == null || pAmpl == null)
    {
        // error
        throw new ArgumentNullException("pReal,pImag,pAmpl");
    }
    if (pReal.Length < NumSamples || pImag.Length < NumSamples || pAmpl.Length < NumSamples)
    {
        // error
        throw new ArgumentException("Invalid Array argument detected");
    }

    // Calculate amplitude values in the buffer provided
    for (UInt32 i = 0; i < NumSamples; i++)
    {
        pAmpl[i] = pReal[i] * pReal[i] + pImag[i] * pImag[i];
    }
}

/// <summary>
/// Find Peak frequency in Hz
/// </summary>
/// <param name="NumSamples">Number of samples</param>
/// <param name="pAmpl">Current amplitude</param>
/// <param name="samplingRate">Sampling rate in samples/second (Hz)</param>
/// <param name="index">Frequency index</param>
/// <returns>Peak frequency in Hz</returns>
public static Double PeakFrequency(UInt32 NumSamples, Double[] pAmpl, Double samplingRate, ref UInt32 index)
{
    UInt32 N = NumSamples >> 1;   // number of positive frequencies. (numSamples/2)

    if (pAmpl == null)
    {
        // error
        throw new ArgumentNullException("pAmpl");
    }
    if (pAmpl.Length < NumSamples)
    {
        // error
        throw new ArgumentException("Invalid Array argument detected");
    }

    double maxAmpl = -1.0;
    double peakFreq = -1.0;
    index = 0;

    for (UInt32 i = 0; i < N; i++)
    {
        if (pAmpl[i] > maxAmpl)
        {
            maxAmpl = (double)pAmpl[i];
            index = i;
            peakFreq = (double)(i);
        }
    }

    return samplingRate * peakFreq / (double)(NumSamples);
}

public static byte[] GetPeaks(double[] leftChannel, double[] rightChannel, int sampleFrequency)
{
    byte[] peaks = new byte[_frequencySlotCount];
    byte[] channelPeaks = GetPeaksForChannel(leftChannel, sampleFrequency);

    ComparePeaks(peaks, channelPeaks);
    return peaks;
}

private static void ComparePeaks(byte[] overallPeaks, byte[] channelPeaks)
{
    for (int i = 0; i < _frequencySlotCount; i++)
    {
        overallPeaks[i] = Math.Max(overallPeaks[i], channelPeaks[i]);
    }
}

private static byte[] GetPeaksForChannel(double[] normalizedArray, int sampleFrequency)
{
    double maxAmpl = (32767.0 * 32767.0);

    byte[] peaks = new byte[_frequencySlotCount];
    // update meter
    int centerFreq = (sampleFrequency / 2);
    byte peak;
    for (int i = 0; i < _frequencySlotCount; ++i)
    {
        if (_meterFrequencies[i] > centerFreq)
        {
            peak = 0;
        }
        else
        {
            int index = (int)(_meterFrequencies[i] * normalizedArray.Length / sampleFrequency);
            peak = (byte)Math.Max(0, (17.0 * Math.Log10(normalizedArray[index] / maxAmpl)));
        }

        peaks[i] = peak;
    }

    return peaks;
}
        }
*/

//------------------------------------------------------------------------------------------------------
/*
public partial class FourierTransform
    {

       // static private int n, nu;

        static private int BitReverse(int j, int nu)
        {
            int j2;
            int j1 = j;
            int k = 0;
            for (int i = 1; i <= nu; i++)
            {
                j2 = j1 / 2;
                k = 2 * k + j1 - 2 * j2;
                j1 = j2;
            }
            return k;
        }

        /// <summary>
        /// Number of samples must be power of 2
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static public double[] FFTDb(ref double[] x)
        {
            // Assume n is a power of 2
            int n = x.Length;
            int nu = (int)(Math.Log(n) / Math.Log(2));
            int n2 = n / 2;
            int nu1 = nu - 1;
            double[] xre = new double[n];
            double[] xim = new double[n];
            double[] decibel = new double[n2];
            double tr, ti, p, arg, c, s;
            for (int i = 0; i < n; i++)
            {
                xre[i] = x[i];
                xim[i] = 0.0f;
            }
            int k = 0;
            for (int l = 1; l <= nu; l++)
            {
                while (k < n)
                {
                    for (int i = 1; i <= n2; i++)
                    {
                        p = BitReverse(k >> nu1, nu);
                        arg = 2 * (double)Math.PI * p / n;
                        c = (double)Math.Cos(arg);
                        s = (double)Math.Sin(arg);
                        tr = xre[k + n2] * c + xim[k + n2] * s;
                        ti = xim[k + n2] * c - xre[k + n2] * s;
                        xre[k + n2] = xre[k] - tr;
                        xim[k + n2] = xim[k] - ti;
                        xre[k] += tr;
                        xim[k] += ti;
                        k++;
                    }
                    k += n2;
                }
                k = 0;
                nu1--;
                n2 = n2 / 2;
            }
            k = 0;
            int r;
            while (k < n)
            {
                r = BitReverse(k, nu);
                if (r > k)
                {
                    tr = xre[k];
                    ti = xim[k];
                    xre[k] = xre[r];
                    xim[k] = xim[r];
                    xre[r] = tr;
                    xim[r] = ti;
                }
                k++;
            }
            for (int i = 0; i < n / 2; i++)
                decibel[i] = 10.0 * Math.Log10((float)(Math.Sqrt((xre[i] * xre[i]) + (xim[i] * xim[i]))));
            return decibel;
        }
    }
*/
