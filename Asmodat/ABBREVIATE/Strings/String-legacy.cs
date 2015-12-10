using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;



namespace Asmodat.Abbreviate
{
    public static partial class Strings
    {

        public static string ToString(string str, string nullFormat)
        {
            if (System.String.IsNullOrEmpty(str)) return nullFormat;
            else return str;
        }

        /// <summary>
        /// Extracts String Value enclosed by opening (start) and losing (end) tags.
        /// </summary>
        /// <param name="sData">String Data to be analised. [other data left side][start tag][value][end tag][other data right side]</param>
        /// <param name="sStartTag">Tag indicating start of the string value. [start tag]</param>
        /// <param name="sEndTag">Tag indicating end of the string value. [end tag]</param>
        /// <returns>String value within tags. [value]</returns>
        public static string Extract(this string sData, string sStartTag, string sEndTag)
        {
            if (sStartTag == sEndTag)
                return null;

            int iST = sData.IndexOf(sStartTag);
            int iET = sData.IndexOf(sEndTag);

            if (iST < 0 || iET < 0) return null;

            if(iST > iET)
            {
                sData = sData.Remove(0, iET + sEndTag.Length);
                return Strings.Extract(sData, sStartTag, sEndTag);
            }


            int iStartIndex = iST + sStartTag.Length;
            int iLength = iET - iStartIndex;

            return sData.Substring(iStartIndex, iLength);
        }

        /// <summary>
        /// Extracts String Value enclosed by opening (start) and losing (end) tags. 
        /// </summary>
        /// <param name="sData">String Data to be analised. [other data left side][start tag][value][end tag][other data right side]</param>
        /// <param name="sStartTag">Tag indicating start of the string value. [start tag]</param>
        /// <param name="sEndTag">Tag indicating end of the string value. [end tag]</param>
        /// <param name="sDataResidue">out's Data residiue, that represents data after extracted value (without end tag) [other data right side], (warning !: [other data left side] will be lost)</param>
        /// <returns>String value within tags. [value]</returns>
        public static string Extract(string sData, string sStartTag, string sEndTag, out string sDataResidue)
        {
            sDataResidue = null;
            if (sStartTag == sEndTag) return null;

            int iST = sData.IndexOf(sStartTag);
            int iET = sData.IndexOf(sEndTag);
            
            if (iST < 0 || iET < 0) return null;

            if(iST > iET)
            {
                sData = sData.Remove(0, iET + sEndTag.Length);
                return Strings.Extract(sData, sStartTag, sEndTag, out sDataResidue);
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

        public static string WebTextControlSum(Control Cntrl)
        {
            string ssum = "";

            if (Cntrl.Controls.Count > 0)

                foreach (Control c in Cntrl.Controls)
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
        public static string[] ToList(string sentence, string separator = null)
        {
            if (System.String.IsNullOrEmpty(sentence)) return null;
            if (System.String.IsNullOrEmpty(separator)) separator = ",";
            if (separator.Length == 1)
                return Split(sentence, separator[0]).ToArray(); //sentence.Split(separator[0]);//
            else
                return Regex.Split(sentence, @"\" + separator);
        }



        public static List<string> Split(string sentence, char separator)
        {
            List<string> Data = new List<string>();
            StringBuilder SBuilder = new StringBuilder();

            foreach (char c in sentence)
            {
                if (c == separator)
                {
                    if (SBuilder.Length > 0)
                    {
                        Data.Add(SBuilder.ToString());
                        SBuilder.Clear();
                    }
                    else Data.Add(null);

                    continue;
                }
                SBuilder.Append(c);
            }

            if (SBuilder.Length > 0)
                Data.Add(SBuilder.ToString());


            return Data;
        }


        /// <summary>
        /// Checks if any of packets is null or empty, if at least one is null, returns true
        /// </summary>
        /// <param name="packets">array of strings to check</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string[] packets)
        {
            int i;
            for (i = 0; i < packets.Length; i++)
                if (System.String.IsNullOrEmpty(packets[i])) return true;

            return false;
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
      
    }
}
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