using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Cryptography
{
    public class AED0x1s
    {
     /*   private const int min = char.MinValue;//0
        private const int max = char.MaxValue;//65536

        private static char Add(char c1, char c2)
        {
            int result = (int)c1 + (int)c2;

            if (result > max)
                return (char)((result - max) + min - 1);
            else return (char)result;
        }

        private static char Sub(char c1, char c2)
        {
            int result = (int)c1 - (int)c2;

            if (result < min)
                return (char)((result - min) + max + 1);
            else return (char)result;
        }

        public static SecureString Encrypt(SecureString str, UInt16 uiseed = 1)
        {

            SecureString ssc = new SecureString();
            if (str.IsNull())
                return ssc;

            ssc = str.Copy();
            int length = str.Length;
            char seed = (char)uiseed;
            char c;

            int i = 0;
            for (; i < length - 1; i++)
            {
                c = str.GetChar(i);
                ssc.SetAt(i, str.GetChar(i + 1));
                ssc.SetAt(i + 1, c);

                ssc.SetAt(i, AED0x1s.Add(str.GetChar(i), str.GetChar(i + 1)));
                ssc.SetAt(i, AED0x1s.Add(str.GetChar(i), seed));
            }

            ssc.SetAt(i, AED0x1s.Add(str.GetChar(i), seed));

            return ssc.Reverse();
        }

        public static SecureString Decrypt(SecureString str, UInt16 uiseed = 1)
        {
            if (str.IsNull())
                return new SecureString();

            SecureString ssc = str.Reverse();
            int length = str.Length;
            char seed = (char)uiseed;
            char c;

            int i = length - 1;
            for (; i > 0; i--)
            {
                c = str.GetChar(i);
                ssc.SetAt(i, str.GetChar(i - 1));
                ssc.SetAt(i - 1, c);

                ssc.SetAt(i, AED0x1s.Sub(str.GetChar(i), str.GetChar(i - 1)));
            }

            ssc.SetAt(i, AED0x1s.Sub(str.GetChar(i), seed));
            return ssc;
        }


        public static SecureString Encrypt(string str, UInt16 uiseed = 1)
        {
            return AED0x1s.Encrypt(str.Secure(), uiseed);
        }

        public static SecureString Decrypt(string str, UInt16 uiseed = 1)
        {
            return AED0x1s.Decrypt(str.Secure(), uiseed);
        }*/
    }
}
