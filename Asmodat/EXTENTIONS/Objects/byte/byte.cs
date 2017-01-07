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

using AsmodatMath;
using Asmodat.Natives;
using System.Runtime.CompilerServices;

namespace Asmodat.Extensions.Objects
{
    

    public  static partial class byteEx
    {
        public static ExceptionBuffer Exceptions { get; private set; } = new ExceptionBuffer();


        public static byte[] AddToAllValues(this byte[] data, byte value)
        {
            if (data.IsNullOrEmpty())
                return null;

            int i = 0, length = data.Length;
            byte[] result = new byte[length];

            unchecked
            {
                for (; i < length; i++)
                    result[i] = (byte)(data[i] + value);
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsFast(this byte[] arr1, byte[] arr2)
        {
            return msvcrtEx.memcmp(arr1, arr2);
        }

        public static bool EqualsSlow(this byte[] arr1, byte[] arr2)
        {
            if (arr1 == null && arr2 == null)
                return true;
            else if(arr1 == null || arr2 == null || arr1.Length != arr2.Length)
                return false;

            int i = 0;

            for(;i< arr1.Length; i++)
                if (arr1[i] != arr2[i])
                    return false;
            

            return true;
        }

        public static bool EqualsSequence(this byte[] arr1, byte[] arr2)
        {
            if (arr1 == null && arr2 == null)
                return true;
            else if (arr1 == null || arr2 == null || arr1.Length != arr2.Length)
                return false;

            return arr1.SequenceEqual(arr2);
        }



        /// <summary>
        /// Puts random values into array
        /// </summary>
        /// <param name="array"></param>
        public static void Randomize(this byte[] array)
        {
            if (array.IsNullOrEmpty())
                return;
            int min = 0;
            int max = byte.MaxValue + 1;
            for (int i = 0; i < array.Length; i++)
                array[i] = (byte)AMath.Random(min, max);
        }


        public static string ToStringFromByteArray(byte[] bytes)
        {
            if (bytes == null)
                return null;

            byte[] length = Int32Ex.ToBytes(bytes.Length);

            List<byte> data = new List<byte>();

            data.AddRange(length);
            data.AddRange(bytes);

            while (data.Count % 8 != 0)
                data.Add(0);

            return Convert.ToBase64String(data.ToArray());
        }

        public static byte[] ToByteArrayFromString(string str)
        {
            if (str.IsNullOrEmpty())
                return null;

            int mod4 = str.Length % 4;
            if (mod4 > 0)
                str += new string('*', 4 - mod4);
            
            byte[] data = Convert.FromBase64String(str);

            if (data.Length < 4)
                return null;

            int length = Int32Ex.FromBytes(data);

            if (length == 0)
                return new byte[0];

            if (length < 0 || data.Length < 4 + length)
                return null;

            return data.Skip(4).Take(length).ToArray();
        }


        public static string ToDataString(this byte[] bytes)
        {


            return null;
        }

        public static int[] ToIntArray(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            int length = bytes.Length;
            

            if (length % 4 != 0)
                throw new Exception("Array must be modulo 4 to convert it to int.");

            int[] buffer = new int[length / 4];

            for(int i = 0, i2 = 0; i < length;i2++)
            {
                buffer[i2] = BitConverter.ToInt32(bytes, i);
                i += 4;
            }

            return buffer;
        }

        public static long[] ToLongArray(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            int length = bytes.Length;


            if (length % 8 != 0)
                throw new Exception("Array must be modulo 8 to convert it to long.");

            long[] buffer = new long[length / 8];

            for (int i = 0, i2 = 0; i < length; i2++)
            {
                buffer[i2] = BitConverter.ToInt64(bytes, i);
                i += 8;
            }

            return buffer;
        }

        public static ulong[] ToULongArray(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            int length = bytes.Length;


            if (length % 8 != 0)
                throw new Exception("Array must be modulo 8 to convert it to ulong.");

            ulong[] buffer = new ulong[length / 8];

            for (int i = 0, i2 = 0; i < length; i2++)
            {
                buffer[i2] = (ulong)BitConverter.ToInt64(bytes, i);
                i += 8;
            }

            return buffer;
        }


    }
}
