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
    public static partial class Doubles
    {
        static decimal[] decimalPowersOf10 = { 1m, 10m, 100m, 1000m, 1000m, 10000m, 100000m, 1000000m };



        /// <summary>
        /// Parses string by any means to double, if string does not contains any numbers or commas throws exception
        /// </summary>
        /// <param name="s">string to parse</param>
        /// <returns>returnd double value</returns>
        public static double ParseAny(string str)
        {
            string that = str.Replace(",", "."); //0,8A79.127% => 0.87A9.127%
            string data = string.Empty;
            bool bCommaFound = false;
            foreach (char c in that) //0.87A9.127% => 0.879127
            {
                if (char.IsDigit(c))
                    data += c;
                else if (!bCommaFound && c == '.')
                {
                    data += c;
                    bCommaFound = true;
                }
            }

            if (System.String.IsNullOrEmpty(data))
                throw new ArgumentException("Input sting does not contains numbers or comma.");

            if (Objects.IsNumber(data.First())) // .2345
                data = "0" + data;

            if (Objects.IsNumber(data.Last())) // 2345.
                data += "0";

            return double.Parse(data, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static double ParseAny(string str, double exception)
        {
            string that = str.Replace(",", "."); //0,8A79.127% => 0.87A9.127%
            string data = string.Empty;
            bool bCommaFound = false;
            foreach (char c in that) //0.87A9.127% => 0.879127
            {
                if (char.IsDigit(c))
                    data += c;
                else if (!bCommaFound && c == '.')
                {
                    data += c;
                    bCommaFound = true;
                }
            }

            if (System.String.IsNullOrEmpty(data))
                return exception;

            if (Objects.IsNumber(data.First())) // .2345
                data = "0" + data;

            if (Objects.IsNumber(data.Last())) // 2345.
                data += "0";

            return double.Parse(data, System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Fast String parser, it is not sefe, but efficient
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Parse(string input)
        {
            long n = 0;
            int decimalPositions = input.Length;
            char c;
            int k = 0;
            bool negative = false;
            if (input[0] == '-')
            {
                negative = true;
                ++k;
            }

            for (; k < input.Length; k++)
            {
                c = input[k];
                if (c == '.' || c == ',')
                    decimalPositions = k + 1;
                else
                    n = (n * 10) + (int)(c - '0');
            }

            if (negative) return (double)(-n / decimalPowersOf10[input.Length - decimalPositions]);
            else return (double)(n / decimalPowersOf10[input.Length - decimalPositions]);
        }



        public static int CharsCount(string input, char c)
        {
            int count = 0;
            foreach (char ch in input)
                if (ch == c) ++count;

            return count;
        }

        /// <summary>
        /// Parses duble with check of digit format
        /// Fails: 3
        /// 16.76
        /// 19,315
        /// </summary>
        /// <param name="input"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static double Parse(string input, int decimals)
        {
            input = input.Replace(" ", ""); //Remove white spaces

            string sign = "";
            if (input[0] == '-')
            {
                sign = "-";
                input = input.Substring(1, input.Length - 1);
            }

            int position = input.Length - (decimals + 1);
            int cntd1 = Doubles.CharsCount(input, '.');
            int cntd2 = Doubles.CharsCount(input, ',');
            bool exc = false;
            if (cntd1 + cntd2 == 1 && decimals != 0)
                exc = true;

            string correction = "";
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                //if char is a digit, add it and simply continue
                if (char.IsDigit(c))
                {
                    correction += c;
                    continue;
                }

                if (c == '.' || c == ',')
                {
                    //ignore excess formating 
                    if (i == position || exc)
                        correction += c;

                    continue;
                }


                throw new Exception("Unknown Format !");
            }

            correction = correction.Replace(",", ".");
            correction = sign + correction;
            double value = double.Parse(correction, CultureInfo.InvariantCulture);
            if (sign == "-") value = -value;

            #region Bact Test
            //new number of decimals cannot be greater then expected

            string test = value.ToString().Replace(",", ".");
            int index = test.IndexOf(".");
            if (index >= 0)
            {
                int dec = test.Length - (index + 1);

                if (dec > decimals)
                    throw new Exception("Backtest failed !");
            }
            #endregion

            return value;
        }
    }
}
