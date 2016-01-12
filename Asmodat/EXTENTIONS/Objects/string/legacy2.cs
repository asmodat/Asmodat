using Asmodat.Abbreviate;
using Asmodat.Extensions.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Asmodat.Extensions.Objects
{
    public static partial class stringEx
    {
        /// <summary>
        /// Checks if any of packets is null or empty, if at least one is null, returns true
        /// </summary>
        /// <param name="packets">array of strings to check</param>
        /// <returns></returns>
        public static bool IsAnyNullOrEmpty(this string[] values)
        {
            
            if (values.IsNullOrEmpty())
                return false;

            int i = 0;
            for (; i < values.Length; i++)
                if (System.String.IsNullOrEmpty(values[i]))
                    return true;

            return false;
        }

        public static string ToNullSafeString(string str, string nullFormat)
        {
            if (str == null)
                return nullFormat;
            else
                return str;
        }

        /// <summary>
        /// Extracts String Value enclosed by opening (start) and losing (end) tags.
        /// </summary>
        /// <param name="sData">String Data to be analised. [other data left side][start tag][value][end tag][other data right side]</param>
        /// <param name="sStartTag">Tag indicating start of the string value. [start tag]</param>
        /// <param name="sEndTag">Tag indicating end of the string value. [end tag]</param>
        /// <returns>String value within tags. [value]</returns>
        public static string ExtractTag(this string sData, string sStartTag, string sEndTag)
        {
            string sDataResidue;
            return sData.ExtractTag(sStartTag, sEndTag, out sDataResidue);
        }

        /// <summary>
        /// Extracts String Value enclosed by opening (start) and losing (end) tags. 
        /// </summary>
        /// <param name="sData">String Data to be analised. [other data left side][start tag][value][end tag][other data right side]</param>
        /// <param name="sStartTag">Tag indicating start of the string value. [start tag]</param>
        /// <param name="sEndTag">Tag indicating end of the string value. [end tag]</param>
        /// <param name="sDataResidue">out's Data residiue, that represents data after extracted value (without end tag) [other data right side], (warning !: [other data left side] will be lost)</param>
        /// <returns>String value within tags. [value]</returns>
        public static string ExtractTag(this string sData, string sStartTag, string sEndTag, out string sDataResidue)
        {
            sDataResidue = null;
            if (sStartTag == sEndTag) return null;

            int iST = sData.IndexOf(sStartTag);
            int iET = sData.IndexOf(sEndTag);

            if (iST < 0 || iET < 0) return null;

            if (iST > iET)
            {
                sData = sData.Remove(0, iET + sEndTag.Length);
                return ExtractTag(sData, sStartTag, sEndTag, out sDataResidue);
            }

            int iStartIndex = iST + sStartTag.Length;
            int iLength = iET - iStartIndex;
            int iRStartIndex = iStartIndex + iLength + sEndTag.Length;
            int iRLength = sData.Length - iRStartIndex;

            sDataResidue = sData.Substring(iRStartIndex, iRLength);

            return sData.Substring(iStartIndex, iLength);
        }

        
        public static string RemoveTags(string sText, string sStartTag, string sEndTag)
        {
            int iStart = sText.IndexOf(sStartTag);
            int iEnd = sText.IndexOf(sEndTag);

            if (iStart < 0 && iEnd < 0 || iStart > iEnd)
                return sText;

            string sSubOne = "";
            string sSubTwo = "";

            if (iStart > 0) sSubOne += sText.Substring(0, iStart);

            if (iEnd < sText.Length - 1) sSubTwo += sText.Substring(iEnd + sEndTag.Length, sText.Length - iEnd - sEndTag.Length);

            return RemoveTags(sSubOne + sSubTwo, sStartTag, sEndTag);
        }





        public static string WebTextControlSum(System.Web.UI.Control Cntrl)
        {
            string ssum = "";

            if (Cntrl.Controls.Count > 0)

                foreach (System.Web.UI.Control c in Cntrl.Controls)
                {
                    if (c == null) continue;
                    else if (c.Controls.Count > 0) ssum += WebTextControlSum(c);
                    else if (c is TextBox) ssum += ((TextBox)c).Text;
                    else if (c is Button) ssum += ((Button)c).Text;
                    else if (c is Label) ssum += ((Label)c).Text;
                }

            return ssum;
        }

        /// <summary>
        /// Parses string sentence separated by chars into List of words.
        /// </summary>
        /// <param name="sentence">String sentence separated by chars.</param>
        /// <param name="separator">Char that separates diffrent words, if null the default separators is ','</param>
        /// <returns>List of words without separators.</returns>
        public static string[] SplitSafe(this string sentence, string separator)
        {
            if (sentence.IsNullOrEmpty())
                return null;
            else if (separator.IsNullOrEmpty())
                return new string[1] { sentence };
            else if (separator.Length == 1)
                return sentence.Split(separator[0]); //sentence.Split(separator[0]);//
            else if (!sentence.Contains(separator))
                return new string[0];
            else
            {
                string[] result = Regex.Split(sentence, @"\" + separator);
                if(!result.IsNullOrEmpty() && result[result.Length - 1].IsNullOrEmpty())
                    result = result.SubArray(0, result.Length - 1);

                return result;
            }
        }

        public static string[] SplitSafe(this string sentence, char separator)
        {
            if (sentence.IsNullOrEmpty())
                return null;
            else if (!sentence.Contains(separator))
                return new string[0];
            else
                return sentence.Split(separator);
        }
    }
}

/*   /// <summary>
       /// Count Substrings
       /// </summary>
       /// <param name="str"></param>
       /// <param name="sub"></param>
       /// <returns></returns>
       public static int Count(string str, string sub)
       {
           if (System.String.IsNullOrEmpty(str) || System.String.IsNullOrEmpty(sub))
               return 0;
           else if (str == sub)
               return 1;

           return (str.Length - str.Replace(sub, "").Length) / sub.Length;
       }

       /// <summary>
       /// Count Subchars
       /// </summary>
       /// <param name="str"></param>
       /// <param name="c"></param>
       /// <returns></returns>
       public static int Count(string str, char c)
       {
           if (System.String.IsNullOrEmpty(str))
               return 0;

           int count = 0, i = 0, length = str.Length;

           for (; i < length; i++)
               if (str[i] == c) ++count;

           return count;
      }*/

//public static List<string> Split(string sentence, char separator)
//        {
//            List<string> Data = new List<string>();
//            StringBuilder SBuilder = new StringBuilder();

//            foreach (char c in sentence)
//            {
//                if (c == separator)
//                {
//                    if (SBuilder.Length > 0)
//                    {
//                        Data.Add(SBuilder.ToString());
//                        SBuilder.Clear();
//                    }
//                    else Data.Add(null);

//                    continue;
//                }
//                SBuilder.Append(c);
//            }

//            if (SBuilder.Length > 0)
//                Data.Add(SBuilder.ToString());


//            return Data;
//        }

/*
public static List<string> ToList(string sentence, string separator = null)
        {
            if (System.String.IsNullOrEmpty(sentence)) return null;
            if (System.String.IsNullOrEmpty(separator)) separator = ",";

            string save = sentence;

            List<string> lsParts = new List<string>();
            int iSentenceLength = sentence.Length;
            int iSeparatorLength = separator.Length;
            while (iSentenceLength > 0)
            {
                int iIndex = sentence.IndexOf(separator);

                if (iIndex < 0)
                {
                    lsParts.Add(sentence);
                    break;
                }
                else if (iIndex == 0)
                {
                    sentence = sentence.Substring(iSeparatorLength, iSentenceLength - iSeparatorLength);
                    lsParts.Add(null);
                }
                else
                {
                    lsParts.Add(sentence.Substring(0, iIndex));
                    sentence = sentence.Substring(iIndex + iSeparatorLength, iSentenceLength - iIndex - iSeparatorLength);
                }


                iSentenceLength = sentence.Length;
            }


            string[] option = Regex.Split(save, @"\"+ separator);


            return lsParts;
        }

*/
