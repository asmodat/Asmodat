using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using System.Numerics;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Extensions.Objects
{
    

    public static class Int16Ex
    {
        public static BigInteger SumValues(this Int16[] values)
        {
            if (values.IsNullOrEmpty())
                return 0;

            return values.SumValues(0, values.Length);
        }

        public static BigInteger SumValues(this Int16[] values, int offset)
        {
            if (values.IsNullOrEmpty())
                return 0;

            return values.SumValues(offset, values.Length);
        }

        public static BigInteger SumValues(this Int16[] values, int offset, int count)
        {
            if (values.IsNullOrEmpty() || offset < 0 || count <= 0 || (offset + count) > values.Length)
                return 0;

            BigInteger sum = 0;

            for (int i = offset; i < (offset + count); i++)
                sum += values[i];

            return sum;
        }



        /// <summary>
        /// Returns array of 2 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(Int16 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
        }

        public static Int16 FromBytes(byte[] value, int startindex = 0)
        {
            if (value == null || value.Length < 2 + startindex) throw new Exception("Array is not ULong value");
            Int16 result = BitConverter.ToInt16(value, startindex);
            return result;
        }

        public static string ToStringValue(Int16 value)
        {
            byte[] bytes = Int16Ex.ToBytes(value);

            string result = string.Empty;
            foreach (byte b in bytes)
                result += (char)b;

            return result;// System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static Int16 FromStringValue(string value)
        {
            if (value.IsNullOrEmpty() || value.Length < 2) throw new Exception("value is to small to contain any value");

            byte[] bytes = new byte[2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)value[i];

            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
            return Int16Ex.FromBytes(bytes);
        }


        /// <summary>
        /// Closes value inside [min, max] interval,
        /// if value is greater then max, max is returned
        /// if value is less then min, them min is returned
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int ToClosedInterval(this Int16 value, Int16 min, Int16 max)
        {
            if (min > max)
                throw new ArgumentException("Invalid interval.");

            if (value < min)
                value = min;

            if (value > max)
                value = max;

            return value;
        }


    }
}
