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
    public static class UInt16Ex
    {
        public static bool TryParse(this string value, out UInt16 result)
        {
            if (value.IsNullOrEmpty())
            {
                result = default(UInt16);
                return false;
            }

            return UInt16.TryParse(value, out result);
        }


        public static UInt16 TryParse(this string value, UInt16 _default)
        {
            if (value.IsNullOrEmpty())
                return _default;

            UInt16 result;

            if (UInt16.TryParse(value, out result))
                return result;

            return _default;
        }
        
        /// <summary>
        /// Returns array of 2 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(UInt16 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
        }

        public static UInt16 FromBytes(byte[] value, int startindex = 0)
        {
            if (value == null || value.Length < 2 + startindex) throw new Exception("Array is not ULong value");
            UInt16 result = BitConverter.ToUInt16(value, startindex);
            return result;
        }

        public static string ToStringValue(UInt16 value)
        {
            byte[] bytes = UInt16Ex.ToBytes(value);

            string result = string.Empty;
            foreach (byte b in bytes)
                result += (char)b;

            return result;// System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static UInt16 FromStringValue(string value)
        {
            if (value.IsNullOrEmpty() || value.Length < 2) throw new Exception("value is to small to contain any value");

            byte[] bytes = new byte[2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)value[i];

            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
            return UInt16Ex.FromBytes(bytes);
        }

    }
}
