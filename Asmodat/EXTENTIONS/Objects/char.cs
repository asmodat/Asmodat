using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

namespace Asmodat.Extensions.Objects
{
    

    public static class charEx
    {
        public static string ToStringValue(this char[] array)
        {
            if (array == null || array.Length < 0) return null;
            if (array.Length == 0) return "";
            return new string(array);
        }

        public static char[] ToLower(this char[] array)
        {
            if (array == null || array.Length <= 0) return array;

            string str = array.ToStringValue();
            str = str.ToLower();
            return str.ToArray();
        }

        public static char[] ToUpper(this char[] array)
        {
            if (array == null || array.Length <= 0) return array;

            string str = array.ToStringValue();
            str = str.ToUpper();
            return str.ToArray();
        }

        public static string ToHex(this char c)
        {
            return System.Convert.ToByte(c).ToString("x2");
        }

        public static bool IsPrintableASCII(this char c)
        {
            if (c >= 32 && c <= 127)
                return true;
            else
                return false;
        }

        public static bool IsSimpleEscapeSequence(this char c)
        {
            if (SimpleEscapeSequences.Contains(c))
                return true;
            else
                return false;
        }

        public static readonly char[] SimpleEscapeSequences = { '\'', '\"', '\\', '\0', '\a', '\b', '\f', '\n', '\r', '\t', '\v' };


    }
}
