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


    public static class Int32Ex
    {
        public static BigInteger SumValues(this Int32[] values)
        {
            if (values.IsNullOrEmpty())
                return 0;

            return values.SumValues(0, values.Length);
        }

        public static BigInteger SumValues(this Int32[] values, int offset)
        {
            if (values.IsNullOrEmpty())
                return 0;

            return values.SumValues(offset, values.Length);
        }

        public static BigInteger SumValues(this Int32[] values, int offset, int count)
        {
            if (values.IsNullOrEmpty() || offset < 0 || count <= 0 || (offset + count) > values.Length)
                return 0;

            BigInteger sum = 0;

            for (int i = offset; i < (offset + count); i++)
                sum += values[i];

            return sum;
        }


        public static bool TryParse(this string value, out Int32 result)
        {
            if(value.IsNullOrEmpty())
            {
                result = default(Int32);
                return false;
            }

            return Int32.TryParse(value, out result);
        }


        public static Int32 TryParse(this string value, Int32 _default)
        {
            if (value.IsNullOrEmpty())
                return _default;

            Int32 result;

            if (Int32.TryParse(value, out result))
                return result;

            return _default;
        }

       

        /// <summary>
        /// Returns array of 4 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(Int32 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
        }

        public static Int32 FromBytes(byte[] value, int startindex = 0)
        {
            if (value == null || value.Length < 4 + startindex) throw new Exception("Array is not ULong value");
            Int32 result = BitConverter.ToInt32(value, startindex);
            return result;
        }

        public static string ToStringValue(Int32 value)
        {
            byte[] bytes = Int32Ex.ToBytes(value);

            string result = string.Empty;
            foreach (byte b in bytes)
                result += (char)b;

            return result;// System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static Int32 FromStringValue(string value)
        {
            if (value.IsNullOrEmpty() || value.Length < 4) throw new Exception("value is to small to contain any value");

            byte[] bytes = new byte[4];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)value[i];

            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
            return Int32Ex.FromBytes(bytes);
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
      /*  public static int ToClosedInterval(this Int32 value, Int32 min, int max)
        {
            if (min > max)
                throw new ArgumentException("Invalid interval.");

            if (value < min)
                value = min;

            if (value > max)
                value = max;

            return value;
        }
        */

    }
}
