using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

using System.Numerics;
using System.Globalization;
using Asmodat.Extensions.Collections.Generic;

namespace AsmodatMath
{
    /*
    public static double Truncate(double value, uint decimals)
        {
            double p = Math.Pow(10, decimals);
            return Math.Round(Math.Truncate(value * p) / p, (int)decimals);
        }
        */



    public partial class AMath
    {

        public static T Min<T>(params T[] parameters) where T : IComparable<T>
        {
            if (parameters.IsNullOrEmpty())
                throw new ArgumentException("parameters can not be null.");

            T result = parameters[0];

            for (int i = 1; i < parameters.Length; i++)
            {
                if (parameters[i].CompareTo(result) < 0)
                    result = parameters[i];
            }

            return result;
        }
        public static T Max<T>(params T[] parameters) where T : IComparable<T>
        {
            if (parameters.IsNullOrEmpty())
                throw new ArgumentException("parameters can not be null.");

            T result = parameters[0];

            for (int i = 1; i < parameters.Length; i++)
            {
                if (parameters[i].CompareTo(result) > 0)
                    result = parameters[i];
            }

            return result;
        }







        /// <summary>
        /// This method cuts value afrer specified number of decimals, no rounding is done
        /// </summary>
        /// <param name="value"></param>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public static double Truncate(double value, uint decimals)
        {
            double pow = Math.Pow(10, decimals);
            return Math.Truncate(pow * value) / pow;
        }


        /// <summary>
        /// Floor round
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pow"></param>
        /// <returns></returns>
        public static int RoundToPow(double value, double pow)
        {
            return (int)Math.Pow(pow, (int)(Math.Log(value) / Math.Log(pow)));
        }

        /* */
        /*

             int index = sVal.IndexOf(".");
             int offset = (int)(index + decimals + 1);

             if (sVal.Length <= offset)
                 return value;

             int left = sVal.Length - offset;
             sVal.Substring(offset, left);
             //++decimals;
             /*double p = Math.Pow(10, -decimals);
             double dec = Math.Round((value % p), (int)decimals);
             return value - dec;

             return double.Parse(sVal, CultureInfo.InvariantCulture);

         public static double Decimals(decimal value)
         {
             if (value == 0)
                 return 0;

             if (double.IsInfinity(value))
                 return double.PositiveInfinity;

             if (double.IsNaN(value))
                 return double.NaN;

             return BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
         }*/


        public static double Average(List<double[]> data, int index, bool SkipExceptions = false, bool ThrowExceptions = true) 
        { return AMath.Average(data.ToArray(), index, SkipExceptions, ThrowExceptions); }
        public static double Average(double[][] data, int index, bool SkipExceptions = false, bool ThrowExceptions = true)//if (TestLength && data[i].Length <= index) return double.NaN;
        {
            double sum = 0;
            int count = 0;
            for(int i = 0; i < data.Length; i++)
            {
                if(data == null || data[i].Length <= index)
                {
                    if (SkipExceptions)
                        continue;
                    else if(!ThrowExceptions) 
                        return double.NaN;
                }

                sum += data[i][index];
                ++count;
            }

            return (double)sum / count;
        }


        /// <summary>
        /// this method is 3x faster then array.Average()
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double Average(double[] data)
        {
            if (data == null) return double.NaN;

            double sum = 0;
            int i = 0, length = data.Length;
            for (; i < length; i++)
                sum += data[i];

            return (double)sum / length;
        }

        public static double Average(short[] data)
        {
            if (data == null) return double.NaN;

            double sum = 0;
            int i = 0, length = data.Length;
            for (; i < length; i++)
                sum += data[i];

            return (double)sum / length;
        }

        /// <summary>
        /// Calculates average using last one, if last is NaN then it is calculated with standard algorithm
        /// </summary>
        /// <param name="data"></param>
        /// <param name="last"></param>
        /// <returns></returns>
        public static double Average(double[] data, double last)
        {
            if (data == null) return double.NaN;

            double average;
            int LN1 = data.Length - 1;
            if (double.IsNaN(last))
                average = AMath.Average(data);
            else
                average = (double)((last * LN1) + data[LN1]) / (LN1 + 1);

            return average;
        }


        public static double Average(int[] data)
        {
            if (data == null) return double.NaN;

            double sum = 0;
            int i = 0, length = data.Length;
            for (; i < length; i++)
                sum += data[i];

            return (double)sum / length;
        }

        public static double Average(byte[] data)
        {
            if (data == null || data.Length == 0) return double.NaN;

            double sum = 0;
            int i = 0, length = data.Length;
            for (; i < length; i++)
                sum += data[i];

            return (double)sum / length;
        }


        public static double Average(UInt64[] data)
        {
            if (data == null) return double.NaN;

            double sum = 0;
            int i = 0, length = data.Length;
            for (; i < length; i++)
                sum += data[i];

            return (double)sum / length;
        }
        /*
         double[] dat = new double[100000000];
            for (int i = 0; i < dat.Length; i++)
                dat[i] = AMath.Random() * 1000;
            Watch.Run("1");
            double v1 = dat.Average();
            double sp1 = Watch.ms("1");

            Watch.Run("2");
            double v2 = AMath.Average(dat);
            double sp2 = Watch.ms("2");

            if (sp2 > sp1 || v2 != v1)
                return;*/




        

        /// <summary>
        /// Copares two doubles with specified precision
        /// Example:
        /// precision = 0.1
        /// 0.9 == 1 -> true
        /// 0.8 == 1 -> false
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="precision"></param>
        /// <returns>returns true if |v1 - v2| is les or equal to precision, else false </returns>
        public static bool Equ(double v1, double v2, double precision)
        {
            if (Math.Abs(v1 - v2) <= precision) return true;
            return false;
        }


        


        //public static double Percentage(double value, double percentage, double xvalue)
        //{
        //    if (max == 0 && min != 0)
        //        return Math.Sign(min) * double.MaxValue;

        //    return (double)(min * 100) / max;
        //}
    }
}


//public static double Min(double d1, double d2, double d3 = double.MaxValue)
//{
//    return Math.Min(Math.Min(d1, d2), d3);
//}