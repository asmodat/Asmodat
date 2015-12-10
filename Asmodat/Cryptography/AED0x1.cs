using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Cryptography
{
    public class AED0x1
    {
        private const int min = char.MinValue;//0
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

        public static string Encrypt(string str, UInt16 uiseed = 1)
        {
            if (str == null)
                return null;

            char seed = (char)uiseed;
            char c;
            char[] array = str.ToCharArray();
            int i = 0;
            for (; i < array.Length - 1; i++)
            {
                c = array[i];
                array[i] = array[i + 1];
                array[i + 1] = c;

                array[i] = AED0x1.Add(array[i], array[i + 1]);
                array[i] = AED0x1.Add(array[i], seed);
            }

            array[i] = AED0x1.Add(array[i], seed);


            Array.Reverse(array);
            return new string(array);
        }

        public static string Decrypt(string str, UInt16 uiseed = 1)
        {
            if (str == null)
                return null;

            char seed = (char)uiseed;
            char c;
            char[] array = str.ToCharArray();
            Array.Reverse(array);

            int i = array.Length - 1;
            for (; i > 0; i--)
            {
                c = array[i];
                array[i] = array[i - 1];
                array[i - 1] = c;

                array[i] = AED0x1.Sub(array[i], array[i - 1]);
            }

            array[i] = AED0x1.Sub(array[i], seed);



            return new string(array);
        }


        public static string Encrypt(SecureString str, UInt16 uiseed = 1)
        {
            return AED0x1.Encrypt(str.Release(), uiseed);
        }

        public static SecureString EncryptSecure(string str, UInt16 uiseed = 1)
        {
            return AED0x1.Encrypt(str, uiseed).Secure();
        }

        public static SecureString EncryptSecure(SecureString str, UInt16 uiseed = 1)
        {
            return AED0x1.Encrypt(str.Release(), uiseed).Secure();
        }


        public static string Decrypt(SecureString str, UInt16 uiseed = 1)
        {
            return AED0x1.Decrypt(str.Release(), uiseed);
        }

        public static SecureString DecryptSecure(SecureString str, UInt16 uiseed = 1)
        {
            return AED0x1.Decrypt(str.Release(), uiseed).Secure();
        }

        public static SecureString DecryptSecure(string str, UInt16 uiseed = 1)
        {
            return AED0x1.Decrypt(str, uiseed).Secure();
        }
    }
}
