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
    public  static partial class Bytes
    {
        public static bool Compare(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
                return false;

            if (a1.Length == 0)
                return true;
          
            unsafe
            {
                fixed (byte* p1 = a1, p2 = a2)
                {
                    byte* x1 = p1, x2 = p2;
                    int l = a1.Length;

                    for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                        if (*((long*)x1) != *((long*)x2)) return false;

                    if ((l & 4) != 0) { if (*((int*)x1) != *((int*)x2)) return false; x1 += 4; x2 += 4; }
                    if ((l & 2) != 0) { if (*((short*)x1) != *((short*)x2)) return false; x1 += 2; x2 += 2; }
                    if ((l & 1) != 0) if (*((byte*)x1) != *((byte*)x2)) return false;

                    return true;
                }
            }
        }

        /// <summary>
        /// compares 2 arrays with 4 bytes resolution
        /// </summary>
        /// <param name="a1"></param>
        /// <param name="a2"></param>
        /// <returns></returns>
        public static double Similarity32Bit(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
                return 0;

            if (a1.Length == 0)
                return 100;

            double count = a1.Length;
            double sum = 0;

            unsafe
            {
                fixed (byte* p1 = a1, p2 = a2)
                {
                    byte* x1 = p1, x2 = p2;
                    int l = a1.Length;

                    for (int i = 0; i < l / 8; i++, x1 += 8, x2 += 8)
                        if (*((long*)x1) == *((long*)x2)) sum += 8;

                    if ((l & 4) != 0) { if (*((int*)x1) == *((int*)x2)) sum += 4; x1 += 4; x2 += 4; }
                    if ((l & 2) != 0) { if (*((short*)x1) == *((short*)x2)) sum += 2; x1 += 2; x2 += 2; }
                    if ((l & 1) != 0) if (*((byte*)x1) == *((byte*)x2)) sum += 1;

                    return (double)((double)sum /  count) * 100;
                }
            }
        }

        public static double Similarity8Bit(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
                return 0;

            if (a1.Length == 0)
                return 100;

            double sum = 0;

            unsafe
            {
                fixed (byte* p1 = a1, p2 = a2)
                {
                    byte* x1 = p1, x2 = p2;
                    int i = 0, l = a1.Length;
                    
                    for (i = 0; i < l; i++, x1 += 1, x2 += 1)
                        if (*((byte*)x1) == *((byte*)x2)) ++sum;

                    return (double)((double)sum / l) * 100;
                }
            }
        }


        public static bool Similarity8Bit(byte[] a1, byte[] a2, double minimum)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
                return false;

            if (a1.Length == 0)
                return true;

            double sum = 0;

            minimum /= 100;

            unsafe
            {
                fixed (byte* p1 = a1, p2 = a2)
                {
                    byte* x1 = p1, x2 = p2;
                    int i = 0, l = a1.Length;

                    for (i = 0; i < l; i++, x1 += 1, x2 += 1)
                    {
                        if (*((byte*)x1) == *((byte*)x2) && (double)++sum / l > minimum)
                            return true;
                    }
                }
            }

            return false;
        }


        public static double SimilarityAbs(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
                return 0;

            if (a1.Length == 0)
                return 100;


            double sum = 0;
            byte max = byte.MaxValue;
            int temp;
            int i = 0;
            int length = a1.Length;

            unsafe
            {
                fixed (byte* p1 = a1, p2 = a2)
                {
                    byte* x1 = p1, x2 = p2;

                    for (i = 0; i < length; i++, x1 += 1, x2 += 1)
                    {
                        temp = (*((byte*)x1)) - (*((byte*)x2));
                        if (temp < 0)
                            sum += max + temp;
                        else
                            sum += max - temp;
                    }

                    return (double)((double)sum / ((double)length * max)) * 100;
                }
            }
        }


        public static string ToHexString(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            if (bytes.Length <= 0)
                return string.Empty;

            StringBuilder builder = new StringBuilder(bytes.Length * 2);
            int i = 0;
            for(;i<bytes.Length;i++)
            {
                builder.Append(string.Format("{0:x2}", bytes[i]));
            }

            return builder.ToString();
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