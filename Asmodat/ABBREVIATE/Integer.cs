using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using Asmodat.Extensions.Objects;

namespace Asmodat.Abbreviate
{
    public static class Integer
    {
        public static string ToString(int value, string exception, int min, int max)
        {
            if (value > max || value < min)
                return exception;

            return value.ToString();
        }


        /// <summary>
        /// Creates ordered list from start to stop
        /// example 
        /// ToList(1,10) => {1,2,3,4,5,6,7,8,9,10}
        /// ToList(10,1) => {10,9,8,7,6,5,4,3,2,1}
        /// </summary>
        /// <param name="start"></param>
        /// <param name="stop"></param>
        /// <returns></returns>
        public static List<int> ToList(int start, int stop)
        {
            List<int> All = new List<int>();

            if (start > stop)
            
                for (int i = start; i >= stop; i--)
                    All.Add(i);
            
            else
            
                    for (int i = start; i < stop; i++)
                        All.Add(i);

            return All;
        }


        public static int Parse(string value, int exceptionvalue, int min, int max)
        {
            int iValue = exceptionvalue;
            try
            {
                iValue = int.Parse(value);
            }
            catch { }

            if (iValue > max || iValue < min)
                return exceptionvalue;

            return iValue;
        }


        /// <summary>
        /// Parses string sentence separated by chars into List of integers.
        /// </summary>
        /// <param name="sentence">String sentence separated by chars.</param>
        /// <param name="separator">Char that separates diffrent integers, if null the default separators is ','</param>
        /// <returns>List of integers</returns>
        public static List<int> ParseToList(string sentence, string separator)
        {
            List<int> liParts = new List<int>();

            string[] saParts = sentence.SplitSafe(separator);

            foreach (string s in saParts)
            {
                try
                {
                    int iValue = int.Parse(s);
                    liParts.Add(iValue);
                }
                catch
                {

                }
            }

            return liParts;
        }
    }
}
