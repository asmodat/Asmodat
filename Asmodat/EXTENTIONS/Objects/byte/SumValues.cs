using Asmodat.Debugging;
using Asmodat.Extensions.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Extensions.Objects
{
    

    public  static partial class byteEx
    {
        /// <summary>
        /// Sums all bytes to BigIntegere
        /// </summary>
        /// <param name="bytes"></param>
        public static BigInteger SumValues(this byte[] bytes)
        {
            if (bytes.IsNullOrEmpty())
                return 0;

            return bytes.SumValues(0, bytes.Length);
        }

        public static BigInteger SumValues(this byte[] bytes, int offset)
        {
            if (bytes.IsNullOrEmpty())
                return 0;

            return bytes.SumValues(offset, bytes.Length);
        }

        public static BigInteger SumValues(this byte[] bytes, int offset, int count)
        {
            if (bytes.IsNullOrEmpty() || offset < 0 || count <= 0 || (offset + count) > bytes.Length)
                return 0;

            BigInteger sum = 0;

            for (int i = offset; i < (offset + count); i++)
                sum += bytes[i];

            return sum;
        }


        public static Int32 SumValuesInt32(this byte[] bytes, int offset, int count)
        {
            if (bytes.IsNullOrEmpty() || offset < 0 || count <= 0 || (offset + count) > bytes.Length)
                return 0;

            Int32 sum = 0;

            for (int i = offset; i < (offset + count); i++)
                sum += bytes[i];

            return sum;
        }

    }
}
