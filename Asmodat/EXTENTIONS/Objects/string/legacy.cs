using Asmodat.Extensions.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Asmodat.Extensions.Objects
{


    public static partial class stringEx
    {
        /// <summary>
        /// Tests similarity between occurance of separate characters count's 
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static double Similarity(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) || string.IsNullOrEmpty(s2)) return 0;


            Dictionary<char, int> dci1 = new Dictionary<char, int>();
            Dictionary<char, int> dci2 = new Dictionary<char, int>();
            Dictionary<char, int> dciTotal = new Dictionary<char, int>();

            int i1 = 0, i2 = 0, i1max = s1.Length, i2max = s2.Length;
            char c;
            int sum = i1max + i2max;

            for (; i1 < i1max; i1++)
            {
                c = s1[i1];
                if (!dci1.ContainsKey(c))
                    dci1.Add(c, 1);
                else ++dci1[c];


                if (!dciTotal.ContainsKey(s1[i1]))
                    dciTotal.Add(s1[i1], 1);
                else ++dciTotal[s1[i1]];
            }

            for (; i2 < i2max; i2++)
            {
                c = s2[i2];
                if (!dci2.ContainsKey(c))
                    dci2.Add(c, 1);
                else ++dci2[c];

                if (!dciTotal.ContainsKey(c))
                    dciTotal.Add(c, 1);
                else ++dciTotal[c];
            }

            if (dciTotal.Count <= 0)
                return 0;

            double sum_element = 0;
            double sum_weight = 0;
            foreach (char key in dciTotal.Keys)
            {
                double weight = dciTotal[key];
                sum_weight += weight;

                if (dci1.ContainsKey(key) && dci2.ContainsKey(key))
                {
                    double median = (double)weight / 2.0;
                    double min = Math.Min(dci1[key], dci2[key]);
                    double similarity = min / median; ///min * 100 / max
                    sum_element += similarity * weight;
                }//else sum_element += weight*0
            }


            return sum_element / sum_weight;
        }


        public static string ReplaceLast(string source, string replace, string replacement)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            int index = source.LastIndexOf(replace);

            if (index < 0)
                return source;

            return source.Remove(index, replace.Length).Insert(index, replacement);

        }


        public static string RemoveNonNumeric(string s)
        {
            if (string.IsNullOrEmpty(s))
                return null;

            return Regex.Replace(s, "[^0-9.]", "");
        }

        /// <summary>
        /// Returns the n'th index occurance  of char from start if n is positive or from end if n is negative
        /// </summary>
        /// <param name="s"></param>
        /// <param name="c"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static int IndexOfByCount(this string s, char c, int n = 1)
        {
           
            if (n == 0)
                return -1;

            if (n > 0)
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == c)
                    {
                        if (--n == 0)
                            return i;
                    }
                }
            }

            if (n < 0)
                for (int i = s.Length - 1; i >= 0; i--)
                {
                    if (s[i] == c)
                    {
                        if (++n == 0)
                            return i;
                    }
                }

            return -1;
        }


        /// <summary>
        /// Count Substrings
        /// </summary>
        /// <param name="str"></param>
        /// <param name="sub"></param>
        /// <returns></returns>
        public static int Count(this string str, string sub)
        {
            if (str.IsNullOrEmpty() || sub.IsNullOrEmpty())
                return 0;
            else if (str == sub)
                return 1;
            else
                return (str.Length - str.Replace(sub, "").Length) / sub.Length;
        }


        /// <summary>
        /// Count Subchars
        /// </summary>
        /// <param name="str"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int Count(this string str, char c)
        {
            return str.Count(c.ToString());
        }

        /// <summary>
        /// Searches from [start to data.Length) for first index of value inside data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int FirstIndex(this string data, string value, int start = 0)
        {
            if (data.IsNullOrEmpty() || value.IsNullOrEmpty() ||
                start < 0 || start > data.Length || value.Length > data.Length)
                return -1;

            int length = data.Length, target = value.Length;
            int count = 0, i = start;
            for (; i < length; i++)
            {
                if (data[i] == value[count])
                {
                    if (++count == target)
                        return (i - count + 1);
                }
                else count = 0;
            }

            return -1;
        }

        /// <summary>
        /// Searches from (data.Length to start] for first index of value inside data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int LastIndex(this string data, string value, int start = 0)
        {
            if (data.IsNullOrEmpty() || value.IsNullOrEmpty() ||
                start < 0 || start > data.Length || value.Length > data.Length)
                return -1;

            int length = data.Length, target = value.Length;
            int count = target - 1, i = length - 1;
            for (; i >= start; i--)
            {
                if (data[i] == value[count])
                {
                    if (--count == -1)
                        return i;
                }
                else count = target - 1;
            }

            return -1;
        }

        public static string GetTagValueFirst(this string sData, string sStartTag, string sEndTag)
        {
            return sData.GetTagValue(sStartTag, sEndTag, true, true);
        }

        public static string GetTagValueLast(this string sData, string sStartTag, string sEndTag)
        {
            return sData.GetTagValue(sStartTag, sEndTag, false, false);
        }

        public static string GetTagValueOuter(this string sData, string sStartTag, string sEndTag)
        {
            return sData.GetTagValue(sStartTag, sEndTag, true, false);
        }

        public static string GetTagValueInner(this string sData, string sStartTag, string sEndTag)
        {
            return sData.GetTagValue(sStartTag, sEndTag, false, true);
        }

        /// <summary>
        /// Extracts String Value enclosed by first opening (start) and first (end) tags.
        /// </summary>
        /// <param name="sData">String Data to be analised. [other data left side][start tag][value][end tag][other data right side]</param>
        /// <param name="sStartTag">Tag indicating start of the string value. [start tag]</param>
        /// <param name="sEndTag">Tag indicating end of the string value. [end tag]</param>
        /// <param name="firstStart">If true, first start tag is assumed to be startTag.</param>
        /// <param name="firstEnd">If true, first end after start tag is assumed to be and endTag.</param>
        /// <returns>String value within tags. [value]</returns>
        public static string GetTagValue(this string sData, string sStartTag, string sEndTag, bool firstStart, bool firstEnd)
        {
            if (sData == null || sStartTag == null || sEndTag == null)
                return null;

            int iST;
            if (firstStart)
                iST = sData.FirstIndex(sStartTag);
            else
                iST = sData.LastIndex(sStartTag);

            int iET;
            if (firstEnd)
                iET = sData.FirstIndex(sEndTag, iST + sStartTag.Length);
            else
                iET = sData.LastIndex(sEndTag, iST + sStartTag.Length);

            if (iST < 0 || iET < 0)
                return null;

            string data = sData;
            int iStartIndex = iST + sStartTag.Length;
            int iLength = iET - iStartIndex;

            if (iLength == 0)
                return "";

            return data.Substring(iStartIndex, iLength);
        }

    }
}
