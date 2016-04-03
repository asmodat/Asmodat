using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Extensions.Objects
{


    public static partial class stringEx
    {
        public static byte[] GetBytes(this string str)
        {
            if (str == null)
                return null;

            if (str.Length <= 0)
                return new byte[0];

            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static byte[] GetBytesASCII(this string str)
        {
            if (str == null)
                return null;

            if (str.Length <= 0)
                return new byte[0];

            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        public static byte[] GetBytesUTF8(this string str)
        {
            if (str == null)
                return null;

            if (str.Length <= 0)
                return new byte[0];

            return System.Text.Encoding.UTF8.GetBytes(str);
        }

        public static byte[] TryGetBytes(this string str, byte[] exception_result = null)
        {
            try
            {
                return str.GetBytes();
            }
            catch
            {
                return exception_result;
            }
        }

        public static byte[] TryGetBytesASCII(this string str, byte[] exception_result = null)
        {
            try
            {
                return str.GetBytesASCII();
            }
            catch
            {
                return exception_result;
            }
        }

        public static byte[] TryGetBytesUTF8(this string str, byte[] exception_result = null)
        {
            try
            {
                return str.GetBytesUTF8();
            }
            catch
            {
                return exception_result;
            }
        }


        public static string GetString(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            if (bytes.Length <= 0)
                return "";

            if (bytes.Length % sizeof(char) != 0)
                throw new Exception("GetString -  invalid array size");

            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static string GetStringASCII(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            if (bytes.Length <= 0)
                return "";

            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        public static string GetStringUTF8(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            if (bytes.Length <= 0)
                return "";

            return System.Text.Encoding.UTF8.GetString(bytes);
        }

        public static string TryGetString(this byte[] bytes, string exception_result = null)
        {
            try
            {
                return bytes.GetString();
            }
            catch
            {
                return exception_result;
            }
        }

        public static string TryGetStringASCII(this byte[] bytes, string exception_result = null)
        {
            try
            {
                return bytes.GetStringASCII();
            }
            catch
            {
                return exception_result;
            }
        }

        public static string TryGetStringUTF8(this byte[] bytes, string exception_result = null)
        {
            try
            {
                return bytes.GetStringUTF8();
            }
            catch
            {
                return exception_result;
            }
        }
    }


}