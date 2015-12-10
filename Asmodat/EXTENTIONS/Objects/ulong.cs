using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Extensions.Objects
{
    

    public static class ulongEx
    {
        public static char[] ArbitratyBaseChars = new char[] { '0','1','2','3','4','5','6','7','8','9',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};


        public static string ToArbitrary(this ulong value)
        {
            return ulongEx.ToArbitrary(value, ulongEx.ArbitratyBaseChars);
        }

        public static string ToArbitrary(this ulong value, char[] baseChars)
        {
            string result = string.Empty;
            ulong targetBase = (ulong)baseChars.Length;
            
            do
            {
                result = baseChars[value % targetBase] + result;
                value = value / targetBase;

            } while (value > 0);

            return result;
        }


        public static string ToArbitrary(this ulong[] value)
        {
            if (value == null)
                return null;
            else if (value.Length <= 0)
                return string.Empty;

            string result = "";

            for(int i = 0; i < value.Length; i++)
                result += value[i].ToArbitrary();
            
            return result;
        }


    }
}
