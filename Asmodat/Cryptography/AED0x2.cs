using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Cryptography
{
    public class AED0x2
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

     /*   private static char PreProcEncrypt(char[] array)
        {
            int length = array.Length;
            char sum = (char)0;
            for (int i = 0; i < length; i++)
                sum = AED0x2.Add(sum, array[i]);
            
            return sum;
        }

        private static char PreProcDecrypt(char[] array)
        {
            int length = array.Length;
            char sum = (char)0;
            for (int i = 0; i < length; i++)
                sum = AED0x2.Add(sum, array[i]);

            return sum;
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

                array[i] = AED0x2.Add(array[i], array[i + 1]);
                array[i] = AED0x2.Add(array[i], seed);
            }

            array[i] = AED0x2.Add(array[i], seed);


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

                array[i] = AED0x2.Sub(array[i], array[i - 1]);
            }

            array[i] = AED0x2.Sub(array[i], seed);



            return new string(array);
        }
        */

       
    }
}
