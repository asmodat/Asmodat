using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using AsmodatMath;
using Asmodat.Extensions.Collections.Generic;
using System.Security.Cryptography;

using Asmodat.Debugging;

using Asmodat.Extensions.Objects;

namespace Asmodat.Extensions.Security.Cryptography
{
    

    public static partial class MD5Ex
    {
        public static byte[] MD5_ComputeHash(this string text)
        {
            if (text.IsNullOrEmpty())
                return null;

            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] data = Encoding.Unicode.GetBytes(text);
                return md5.ComputeHash(data);
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return null;
            }
        }

        public static string MD5_ComputeHashString(this string text)
        {
            var data = text.MD5_ComputeHash();
            
            if (data.IsNullOrEmpty())
                return null;

            byte[] bytes = Encoding.Unicode.GetBytes(text);
            return BitConverter.ToString(bytes);
        }

        public static string MD5_ComputeHashStringClear(this string text)
        {
            string result = text.MD5_ComputeHashString();

            if (result.IsNullOrEmpty())
                return null;
            
            return result.Replace("-", "");
        }
    }
}
