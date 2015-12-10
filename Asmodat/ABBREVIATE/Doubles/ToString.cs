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
        public static string ToString(double value, string exception, int decimals, double min, double max)
        {
            if (value > max || value < min)
                return exception;

            string data = value.ToString("N" + decimals);

            return data;
        }

        /// <summary>
        /// This method parses string to double ignoring white spaces and changing first puctuation into specified one
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exception"></param>
        /// <param name="decimals"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString(double value, string exception, int decimals, double min, double max, char punctuation)
        {
            string output = Doubles.ToString(value, exception, decimals, min, max);
            if (System.String.IsNullOrEmpty(output))
                return output;

            string sign = "";
            if (output[0] == '-')
            {
                sign = "-";
                output = output.Substring(1, output.Length - 1);
            }

            string correction = string.Empty;
            bool IsSeparatorFound = false;
            for (int i = 0; i < output.Length; i++)
            {
                char c = output[i];
                if (char.IsDigit(c))
                {
                    correction += c;
                    continue;
                }


                if (!IsSeparatorFound && (c == '.' || c == ','))
                {
                    correction += punctuation;
                    IsSeparatorFound = true;
                }
            }

            return sign + correction;
        }

        public static string ToString(double value, string exception, double min, double max)
        {
            if (value > max || value < min)
                return exception;

            return value.ToString();
        }

        public static string ToString(double value, string exception, double min, double max, char separator)
        {
            string output = Doubles.ToString(value, exception, min, max);
            if (System.String.IsNullOrEmpty(output))
                return output;

            string correction = string.Empty;
            bool IsSeparatorFound = false;
            for (int i = 0; i < output.Length; i++)
            {
                char c = output[i];
                if (char.IsDigit(c)) correction += c;


                if (!IsSeparatorFound)
                {
                    correction += separator;
                    IsSeparatorFound = true;
                }
            }

            return correction;
        }

    }
}
