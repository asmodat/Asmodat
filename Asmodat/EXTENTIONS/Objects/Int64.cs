using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Extensions.Objects
{
    

    public static class Int64Ex
    {
       
        public static byte[] ToBytes(Int64 value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            return bytes;
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
