using Asmodat.Extensions.Collections.Generic;
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
        public static string GetHashCodeHexString(this string str)
        {
            if (str.IsNullOrEmpty())
                return null;
            else 
                return String.Format("{0:X}", str.GetHashCode());
        }


        public static string RemoveEdge(this string str, int firstCount, int lastCount)
        {
            str = str.RemoveFirst(firstCount);
            return str.RemoveLast(lastCount);
        }

        public static string RemoveFirst(this string str, int count)
        {
            if (count <= 0 || str == null) return str;
            if (count == str.Length) return "";
            if (count > str.Length) return null;

            int length = str.Length - count;

            return str.GetLast(length);
        }

        public static string RemoveLast(this string str, int count)
        {
            if (count <= 0 || str == null) return str;
            if (count == str.Length) return "";
            if (count > str.Length) return null;

            int length = str.Length - count;

            return str.GetFirst(length);
        }
        public static string Copy(this string str)
        {
            if (str == null)
                return null;
            else if (str == "")
                return "";

            return string.Copy(str);
        }
        public static string Remove(this string str, params string[] subs)
        {
            string result = str.Copy();

            if (subs.IsNullOrEmpty())
                return str;


            foreach(string s in subs)
            {
                if (s.IsNullOrEmpty())
                    continue;

                result = result.Replace(s, "");
            }

            return result;
        }


        public static string GetLettersString(this string str)
        {
            if (str.IsNullOrEmpty())
                return null;

            string result = "";

            foreach(char c in str)
            {
                if (char.IsLetter(c))
                    result += c;
            }

            return result;
        }

        public static string GetDigitsString(this string str)
        {
            if (str.IsNullOrEmpty())
                return null;

            string result = "";

            foreach (char c in str)
            {
                if (char.IsDigit(c))
                    result += c;
            }

            return result;
        }

        public static string GetLettersOrDigitsString(this string str)
        {
            if (str.IsNullOrEmpty())
                return null;

            string result = "";

            foreach (char c in str)
            {
                if (char.IsLetterOrDigit(c))
                    result += c;
            }

            return result;
        }


        public static string GetFirst(this string str, int count)
        {
            if (count < 0) return null;
            if (count == 0) return "";

            if (str == null || count > str.Length)
                return str;

            return str.Substring(0, count);
        }

        public static string GetLast(this string str, int count)
        {
            if (count < 0) return null;
            if (count == 0) return "";

            if (str == null || count >= str.Length)
                return str;

            return str.Substring(str.Length - count);
        }



        public static string LengthEncode(this string str)
        {
            Int32 length;
            if (str == null) length = -1;
            else length = str.Length;

            str = Int32Ex.ToStringValue(length) + str;
            return str;
        }


        public static string LengthDecode(this string str)
        {
            if (str.IsNullOrEmpty() || str.Length < 4)
                return null;

            string value = str.GetFirst(4);

            Int32 length = Int32Ex.FromStringValue(value);

            if (length == -1) return null;
            if (length == 0) return "";

            if (str.Length < length + 4)
                throw new Exception("Decoding failed, str is to short.");

            return str.RemoveFirst(4).GetFirst(length);
        }


        public static string[] ToLower(this string[] array)
        {
            if (array == null || array.Length <= 0) return array;

            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].ToLower();

            return array;
        }

        public static string[] ToUpper(this string[] array)
        {
            if (array == null || array.Length <= 0) return array;

            for (int i = 0; i < array.Length; i++)
                array[i] = array[i].ToUpper();

            return array;
        }


        public static bool EqualsAny(this string str, string[] array, bool casesensitive = true)
        {
            if (str == null || array == null) return false;

            if (!casesensitive)
            {
                str = str.ToLower();
                array = array.ToLower();
            }

            int i = 0, length = array.Length;
            for (; i < length; i++)
                if (str == array[i])
                    return true;

            return false;
        }

        public static bool ContainsAny(this string str, string[] array, bool casesensitive = true)
        {
            if (str == null || array == null) return false;

            if (!casesensitive)
            {
                str = str.ToLower();
                array = array.ToLower();
            }

            int i = 0, length = array.Length;
            for (; i < length; i++)
                if (str.Contains(array[i]))
                    return true;

            return false;
        }

        public static bool ContainsAny(this string str, char[] array, bool casesensitive = true)
        {
            if (str == null || array == null) return false;

            if (!casesensitive)
            {
                str = str.ToLower();
                array = array.ToLower();
            }

            int i = 0, length = array.Length;
            for (; i < length; i++)
                if (str.Contains(array[i]))
                    return true;

            return false;
        }

        public static bool ContainsAll(this string str, char[] array, bool casesensitive = true)
        {
            if (str == null || array == null) return false;

            if (!casesensitive)
            {
                str = str.ToLower();
                array = array.ToLower();
            }

            int i = 0, length = array.Length;
            for (; i < length; i++)
                if (!str.Contains(array[i]))
                    return false;

            return true;
        }

        public static string ReplaceLast(this string str, string substr, string newsubstr, bool casesensitive = true)
        {
            if (str.IsNullOrEmpty() || substr.IsNullOrEmpty() || str.Length < substr.Length) return str;

            if (!casesensitive)
            {
                str = str.ToLower();
                substr = substr.ToLower();
            }

            if (str == substr) return newsubstr;

            int index = str.LastIndexOf(substr);
            if (index < 0)
                return str;

            string result = str.Remove(index, substr.Length);
            result = result.Insert(index, newsubstr);
            return result;
        }


        public static bool IsShorterThen(this string str, int chars)
        {
            if(string.IsNullOrEmpty(str) || str.Length < chars)
                return true;
            
            return false;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string TryTrim(this string str, char[] charsToTrim = null)
        {
            if (charsToTrim.IsNullOrEmpty())
                charsToTrim = new char[] { ' ' };

            if (str.IsNullOrEmpty())
                return "";
            else
                return str.Trim(charsToTrim) + "";
        }
        public static string TryTrimStart(this string str, char[] charsToTrim = null)
        {
            if (charsToTrim.IsNullOrEmpty())
                charsToTrim = new char[] { ' ' };

            if (str.IsNullOrEmpty())
                return "";
            else
                return str.TrimStart(charsToTrim) + "";
        }

        public static string TryTrimEnd(this string str, char[] charsToTrim = null)
        {
            if (charsToTrim.IsNullOrEmpty())
                charsToTrim = new char[] { ' ' };

            if (str.IsNullOrEmpty())
                return "";
            else
                return str.TrimEnd(charsToTrim) + "";
        }

        public static int CountSubstrings(this string str, string sub)
        {
            if (str == null || sub == null)
                return 0;
            else if (str == sub)
                return 1;

            return (str.Length - str.Replace(sub, "").Length) / sub.Length;
        }
        public static int CountChars(this string str, char c)
        {
            if (str == null || str == string.Empty) return 0;
            int count = 0, i = 0, length = str.Length;
            for (; i < length; i++)
                if (str[i] == c) count++;

            return count;
        }





        public static string GZip(this string str)
        {
            if (str == null)
                return null;

            byte[] buffer = Encoding.UTF8.GetBytes(str);
            MemoryStream memory = new MemoryStream();
            using (GZipStream stream = new GZipStream(memory, CompressionMode.Compress, true))
            {
                stream.Write(buffer, 0, buffer.Length);
            }

            memory.Position = 0;
            byte[] data = new byte[memory.Length];
            memory.Read(data, 0, data.Length);

            byte[] zipbuffer = new byte[data.Length + 4];
            Buffer.BlockCopy(data, 0, zipbuffer, 4, data.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, zipbuffer, 0, 4);

            string result = Convert.ToBase64String(zipbuffer);
            return result;
        }

        public static string UnGZip(this string str)
        {
            if (str == null)
                return null;

            byte[] zipbuffer = Convert.FromBase64String(str);

            using (MemoryStream memory = new MemoryStream())
            {
                int length = BitConverter.ToInt32(zipbuffer, 0);
                memory.Write(zipbuffer, 4, zipbuffer.Length - 4);

                byte[] buffer = new byte[length];

                memory.Position = 0;
                using (GZipStream stream = new GZipStream(memory, CompressionMode.Decompress))
                {
                    stream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        /// <summary>
        /// GetRtfUnicodeEscapedString replacement
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Escaped(this string s)
        {
            if (s.IsNullOrEmpty())
                return s;

            int len = s.Length;
            StringBuilder sb = new StringBuilder(len);
            for (int i = 0; i < len; i++)
            {
                char c = s[i];
                if (c >= 0x20 && c < 0x80)
                {
                    if (c == '\\' || c == '{' || c == '}')
                        sb.Append('\\');
                    
                    sb.Append(c);
                }
                else if (c < 0x20 || (c >= 0x80 && c <= 0xFF))
                {
                    sb.Append("\'");
                    sb.Append(c.ToHex());
                }
                else
                {
                    sb.Append("\\u");
                    sb.Append((short)c);
                    sb.Append("??");//two bytes ignored
                }
            }
            return sb.ToString();
        }

        public static string RtfUnicodeEscaped(this string s)
        {
            if (s.IsNullOrEmpty())
                return s;

            var sb = new StringBuilder();
            foreach (var c in s)
            {
                if (c == '\\' || c == '{' || c == '}')
                    sb.Append(@"\" + c);
                else if (c <= 0x7f)
                    sb.Append(c);
                else
                    sb.Append("\\u" + Convert.ToUInt32(c) + "?");
            }
            return sb.ToString();
        }

    }
}
