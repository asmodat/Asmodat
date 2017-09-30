using System;

namespace Asmodat.Extensions.Objects
{
    public static class Int64Ex
    {
        public static bool TryParse(this string value, out Int64 result)
        {
            if (value.IsNullOrEmpty())
            {
                result = default(Int64);
                return false;
            }

            return Int64.TryParse(value, out result);
        }


        public static Int64 TryParse(this string value, Int64 _default)
        {
            if (value.IsNullOrEmpty())
                return _default;

            Int64 result;

            if (Int64.TryParse(value, out result))
                return result;

            return _default;
        }


        public static byte[] ToBytes(Int64 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
        }

        public static bool TryFromBytes(out Int64 result, byte[] value, int startindex = 0)
        {
            if (value == null || value.LongLength < 8 + startindex)
            {
                result = 0;
                return false;
            }

            try
            {
                result = BitConverter.ToInt64(value, startindex);
            }
            catch
            {
                result = 0;
                return false;
            }

            return true;
        }


        public static Int64 FromBytes(byte[] value, int startindex = 0)
        {
            if (value == null || value.Length < 8 + startindex) throw new Exception("Array is not ULong value");
            Int64 result = BitConverter.ToInt64(value, startindex);
            return result;
        }

        public static string ToStringValue(Int64 value)
        {
            byte[] bytes = Int64Ex.ToBytes(value);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static Int64 FromStringValue(string value)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(value);
            return Int64Ex.FromBytes(bytes);
        }


    }
}
