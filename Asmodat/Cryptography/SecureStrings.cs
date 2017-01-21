using Asmodat.Extensions.Objects;
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
            if (ss == null)
                return null;
            else if (ss.Length <= 1)
                return ss.Copy();

            SecureString ssc = new SecureString();

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

        public static SecureString RemoveFirst(this SecureString str, int count)
        {
            if (str == null)  return null;

            if (count >= str.Length)
                return new SecureString();

            SecureString ss = str.Copy();

            for (int i = 0; i < count; i++)
                ss.RemoveAt(0);

            return ss;
        }

        public static bool IsNull(this SecureString ss)
        {
            return (ss == null || ss.Length < 0) ? true : false;
        }

        public static bool IsNullOrEmpty(this SecureString ss)
        {
            return (ss == null || ss.Length <= 0) ? true : false;
        }

        public static bool IsNullOrWhiteSpace(this SecureString ss)
        {
            if (ss.IsNullOrEmpty())
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
            if (ss == null && str == null)
                return null;
            else if (str.IsNullOrEmpty())
                return ss.Copy();

            SecureString ssn = new SecureString();

            if (str != null && str.Length > 0)
                foreach (char c in str.ToArray())
                    ssn.AppendChar(c);

            return ssn;
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
            if (str == null)
                return null;

            SecureString ss = new SecureString();
            if (!str.IsNullOrEmpty())
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
            if (ss == null)
                return null;
            else if (ss.Length == 0)
                return "";

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
            if (ss == null)
                return null;
            else if (ss.Length == 0)
                return "";

            return ss.ToString();
        }

    }
}
