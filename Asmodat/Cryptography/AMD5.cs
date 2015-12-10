using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Debugging;


namespace Asmodat.Cryptography
{
    public class AMD5
    {
        
        /// <summary>
        /// This hash operation cannot be reversed
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
       public static string ComputeHash(string text)
        {
            if (text.IsNullOrEmpty())
                return null;

            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] data = Encoding.Unicode.GetBytes(text);
                byte[] result = md5.ComputeHash(data);

                return BitConverter.ToString(result);
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return null;
            }
        }
    }
}
