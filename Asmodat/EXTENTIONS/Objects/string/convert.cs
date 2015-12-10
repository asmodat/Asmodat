﻿using System;
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
    }


}