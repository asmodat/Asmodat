using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Cryptography
{
    public static class SecureStrings
    {

        public static SecureString Reverse(this SecureString ss)
        {
            SecureString ssc = new SecureString();

            if (ss.IsNull())
                return ssc;

            int length = ss.Length;
            IntPtr ptr = Marshal.SecureStringToBSTR(ss);
            try
            {
                int i = length - 1;
                byte[] buff;
                char c;
                for (;i >= 0; i--)
                {
                    buff = new byte[2];
                    buff[0] = Marshal.ReadByte(ptr, (i * 2));
                    buff[1] = Marshal.ReadByte(ptr, (i * 2) + 1);
                    c = Encoding.Unicode.GetChars(buff)[0];
                    ssc.AppendChar(c);
                }
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }


            return ssc;
        }

        //()

        public static bool IsNull(this SecureString ss)
        {
            if (ss == null || ss.Length <= 0)
                return true;

            else return false;
        }


        public static SecureString RemoveFirst(this SecureString str, int count)
        {
            
            if (str.IsNull() || count >= str.Length)
                return new SecureString();

            SecureString ss = str.Copy();

            for (int i = 0; i < count; i++)
                ss.RemoveAt(0);

            return ss;
        }


        public static bool IsNullOrWhiteSpace(this SecureString ss)
        {
            if (ss.IsNull())
                return true;

            char c;
            for (int i = 0; i < ss.Length; i++)
            {
                c = ss.GetChar(i);

                if (c != '\0' && c != ' ')
                    return false;
            }

            return true;
        }

        public static SecureString Add(this SecureString ss, string str)
        {
            if(ss.IsNull())
                ss = new SecureString();

            if (str != null && str.Length > 0)
                foreach (char c in str.ToArray())
                    ss.AppendChar(c);

            return ss;
        }

        

        public static char GetChar(this SecureString ss, int index)
        {
            IntPtr ptr = Marshal.SecureStringToBSTR(ss);
            char c;
            try
            {
                byte[] buff = new byte[2];
                buff[0] = Marshal.ReadByte(ptr, index * 2);
                buff[1] = Marshal.ReadByte(ptr, (index * 2) + 1);
                c = Encoding.Unicode.GetChars(buff)[0];
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }


            return c;
        }


        /* public static void SetChar(this SecureString ss, char c, int index)
         {
             IntPtr ptr = Marshal.SecureStringToBSTR(ss);
             try
             {
                 byte[] buff = Encoding.Unicode.GetBytes(c.ToString());
                 Marshal.WriteByte(ptr, (index * 2), buff[0]);
                 Marshal.WriteByte(ptr, (index * 2) + 1, buff[1]);
             }
             finally
             {
                 Marshal.ZeroFreeBSTR(ptr);
             }
         }
         */

        public static void SetChar(this SecureString ss, char c, int index)
        {
            IntPtr ptr = Marshal.SecureStringToBSTR(ss);
            try
            {
                byte[] buff = Encoding.Unicode.GetBytes(c.ToString());
                Marshal.WriteByte(ptr, (index * 2), buff[0]);
                Marshal.WriteByte(ptr, (index * 2) + 1, buff[1]);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
        }

        public static SecureString Secure(this string str)
        {

            SecureString ss = new SecureString();
            if (str != null && str.Length > 0)
            {
                int i = 0;
                char[] array = str.ToArray();
                for(; i < array.Length; i++)
                    ss.AppendChar(array[i]);
            }

            return ss;
        }

        public static string Release(this SecureString ss)
        {
            if (ss.IsNull())
                return null;

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(ss);
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeCoTaskMemUnicode(ptr);
            }

            
        }


        public static string ToString(this SecureString ss)
        {
            if (ss.IsNull())
                return null;

            return ss.ToString();
        }

    }
}
