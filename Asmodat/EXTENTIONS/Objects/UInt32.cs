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


    public static class UInt32Ex
    {
 

        public static bool TryParse(this string value, out UInt32 result)
        {
            if(value.IsNullOrEmpty())
            {
                result = default(UInt32);
                return false;
            }

            return UInt32.TryParse(value, out result);
        }


        public static UInt32 TryParse(this string value, UInt32 _default)
        {
            if (value.IsNullOrEmpty())
                return _default;

            UInt32 result;

            if (UInt32.TryParse(value, out result))
                return result;

            return _default;
        }

       

        /// <summary>
        /// Returns array of 4 bytes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToBytes(UInt32 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
        }

        public static UInt32 FromBytes(byte[] value, int startindex = 0)
        {
            if (value == null || value.Length < 4 + startindex) throw new Exception("Array is not ULong value");
            UInt32 result = BitConverter.ToUInt32(value, startindex);
            return result;
        }

        public static string ToStringValue(UInt32 value)
        {
            byte[] bytes = UInt32Ex.ToBytes(value);

            string result = string.Empty;
            foreach (byte b in bytes)
                result += (char)b;

            return result;// System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static UInt32 FromStringValue(string value)
        {
            if (value.IsNullOrEmpty() || value.Length < 4) throw new Exception("value is to small to contain any value");

            byte[] bytes = new byte[4];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)value[i];

            //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
            return UInt32Ex.FromBytes(bytes);
        }


    }
}
