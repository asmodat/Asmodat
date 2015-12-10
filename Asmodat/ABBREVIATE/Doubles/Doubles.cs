using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

using System.Reflection;

using System.Data;

using System.Windows.Forms;

using System.Collections;

using System.Diagnostics;

using System.Globalization;

using System.Linq.Expressions;

using Asmodat.Types;

namespace Asmodat.Abbreviate
{
    public  static partial class Doubles
    {
        public static double[] ToArray(double[][] data, int index, bool SkipExceptions = false, bool ThrowExceptions = true)//if (TestLength && data[i].Length <= index) return double.NaN;
        {
            List<double> output = new List<double>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == null || data[i].Length <= index)
                {
                    if (SkipExceptions)
                        continue;
                    else if (!ThrowExceptions)
                        return null;
                }

                output.Add(data[i][index]);
            }

            return output.ToArray();
        }
        
        public static double[] ToArray(object[] array)
        {
            if (array == null) return null;
            return array.Cast<double>().ToArray();
        }

        public static double[] Replace(double[] array, double replace, double replacement)
        {
            if (array == null)
                return null;

            double[] newArray = new double[array.Length];
            int i = 0;
            for (; i < array.Length; i++)
                if (array[i] == replace)
                    newArray[i] = replacement;
                else
                    newArray[i] = array[i];

            return newArray;
        }


        /// <summary>
        /// returns fractional digits count of double
        /// ihis is not fractional part and it can't be negative
        /// 0 -> 0
        /// 0.1 -> 1
        /// 12.34 -> 34
        /// -0.1 -> 1
        /// -12.34 -> 34
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int FractionalDigits(double input)
        {
            string value = input.ToString().Replace(",", ".");

            int position = value.IndexOf(".");

            if(position < 0)
                return 0;

            string correction = value.Substring(position + 1, value.Length - position - 1);

            return int.Parse(correction);
        }


        public static bool IsNaN(params double[] data)
        {
            int i = 0;
            for(; i < data.Length; i++)
            {
                if (double.IsNaN(data[i]))
                    return true;
            }

            return false;
        }


        public static bool IsInfinity(params double[] data)
        {
            int i = 0;
            for (; i < data.Length; i++)
            {
                if (double.IsInfinity(data[i]))
                    return true;
            }

            return false;
        }

        public static bool IsInfinityOrNaN(params double[] data)
        {
            int i = 0;
            for (; i < data.Length; i++)
            {
                if (double.IsNaN(data[i]) || double.IsInfinity(data[i]))
                    return true;
            }

            return false;
        }




    }
}


///// <summary>

             //int[] ia = new int[10];
             //   int ind = -1;


             //   ia[++ind] = Doubles.Digits(0.001);
             //   ia[++ind] = Doubles.Digits(6.001);
             //   ia[++ind] = Doubles.Digits(66.001);
             //   ia[++ind] = Doubles.Digits(-0.001);
             //   ia[++ind] = Doubles.Digits(-6.001);
             //   ia[++ind] = Doubles.Digits(-660.001);
             //   ia[++ind] = Doubles.Digits(66);
             //   ia[++ind] = Doubles.Digits(-66);
             //   ia[++ind] = Doubles.Digits(6666.7);
             //   ia[++ind] = Doubles.Digits(0);
//        /// Returns number of digits inside double value before comma
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public static int Digits(double input)
//        {
//            if(input > 1)
//                return (int)Math.Floor(Math.Log10(input) + 1);
//            else if(input < 1)
//                return (int)Math.Floor(Math.Log10(Math.Abs(input)) + 1);
//            return 0;
//        }

//        /// <summary>
//        /// Returns number of decimals inside double value
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        public static int Decimals(double input)
//        {
//            int digits = Doubles.Digits(input);
//            string test = input.ToString().Replace(",", ".");
//            int dec = test.Length - (test.IndexOf(".") + 1);

//            return (int)Math.Floor(Math.Log10(input) + 1);
//        }