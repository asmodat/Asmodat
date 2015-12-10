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
        public static unsafe Int32 ChecksumInt32(this byte[] array, int offset, int count)
        {
            Int32 checksum = 0, length = offset + count, i = offset;

            if (array.IsNullOrEmpty() || offset < 0 || count <= 0 || length <= 0 || length > array.Length)
                return checksum;

            unchecked
            {
                for (; i < length; i++)
                    checksum += array[i];

                return checksum;
            }
        }


        public static unsafe Int64 ChecksumInt64(this byte[] array, Int64 offset, Int64 count)
        {
            Int64 checksum = 0, length = offset + count, i = offset;

            if (array.IsNullOrEmpty() || offset < 0 || count <= 0 || length <= 0 || length > array.Length)
                return checksum;

            unchecked
            {
                for (; i < length; i++)
                    checksum += array[i];

                return checksum;
            }
        }


        public static Int32 ChecksumInt32(this byte[] array)
        {
            if (array.IsNullOrEmpty())
                return 0;

            return array.ChecksumInt32(0, array.Length);
        }


        public static Int64 ChecksumInt64(this byte[] array)
        {
            if (array.IsNullOrEmpty())
                return 0;

            return array.ChecksumInt64(0, array.LongLength);
        }


    }
}
