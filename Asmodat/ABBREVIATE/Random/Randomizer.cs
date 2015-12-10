using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmodatMath;

namespace Asmodat.Abbreviate
{
    public static class Randomizer
    {
        public const string Seed_NumbersLowerCaseLetters = "qwertyuiopasdfghjklzxcvbnm0123456789";
        public const string Seed_NumbersLowerUpperCaseLetters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789";
        public const string Seed_Numbers = "0123456789";
        public const string Seed_LowerUpperCaseLetters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        public const string Seed_LowerCaseLetters = "qwertyuiopasdfghjklzxcvbnm";
        public const string Seed_UpperCaseLetters = "QWERTYUIOPASDFGHJKLZXCVBNM";

        /// <summary>
        /// random substring of specified length
        /// </summary>
        /// <param name="source_seed"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GetString(string source_seed, int length = 3)
        {
            if (length < 1)
                return null;

            return new string(Enumerable.Repeat(source_seed, length).Select(s => s[AMath.Random(0, s.Length)]).ToArray());
        }

        /// <summary>
        /// Random substring with random length
        /// </summary>
        /// <param name="source_seed"></param>
        /// <param name="lengthMin"></param>
        /// <param name="lengthMax"></param>
        /// <returns></returns>
        public static string GetString(string source_seed, int lengthMin = 1, int lengthMax = 3)
        {
            if (lengthMin < 1)
                return null;

            if (lengthMin > lengthMax)
                Objects.Swap<int>(ref lengthMin, ref lengthMax);

            int length = 0;

            if (lengthMin == lengthMax) length = lengthMin;
            else length = AMath.Random(lengthMin, lengthMax);


            return GetString(source_seed, length);
        }


        /*public static DateTime GetDateTime(DateTime start, DateTime end)
        {


            return DateTime.Now;
        }*/
    }
}
