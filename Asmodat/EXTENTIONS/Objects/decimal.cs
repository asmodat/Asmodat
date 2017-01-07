using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

namespace Asmodat.Extensions.Objects
{
    

    public static class decimalEx
    {
        /// <summary>
        /// Defines if valu is in [start, stop]
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static bool InRange(this decimal value, decimal min, decimal max)
        {
            if (
                value >= min &&
                value <= max
                )
                return true;
            else return false;
        }


        /// <summary>
        /// parses value to decimal, if operation was not possible then default value is returned
        /// </summary>
        /// <param name="value"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public static decimal ParseDecimal(this string value, decimal _default)
        {
            if (value.IsNullOrWhiteSpace())
                return _default;

            decimal val;
            if(decimal.TryParse(value, out val)) return val;
            else return _default;
        }

        public static int ParseInteger(this string value, int _default)
        {
            if (value.IsNullOrWhiteSpace())
                return _default;

            int val;
            if (int.TryParse(value, out val)) return val;
            else return _default;
        }

        public static bool IsNull(this decimal? value)
        {
            if (value == null || !value.HasValue)
                return true;
            else return false;
        }

        /// <summary>
        /// Counts number of precision digits
        /// </summary>
        /// <param name="value"></param>
        /// <returns>precision digits count</returns>
        public static int CountPrecisionDigits(this decimal value)
        {
            return BitConverter.GetBytes(decimal.GetBits(value)[3])[2];
        }

        /// <summary>
        /// removes leading zeros from decimal precision places
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal Normalize(this decimal value)
        {
            return value / 1.000000000000000000000000000000000m;
        }

        public static int CountDecimalPlaces(this decimal value)
        {
            value = value.Normalize();
            return value.CountPrecisionDigits();
        }

        /// <summary>
        /// value (v) ----  currentPercentage (c)
        ///   X (x)   ----  desiredPercentage (d)
        /// 
        /// returns "X"; x = d*v/c
        /// </summary>
        /// <param name="value"></param>
        /// <param name="currentPercentage"></param>
        /// <param name="desiredPercentage"></param>
        /// <returns></returns>
        public static decimal FindValueByPercentages(this decimal value, decimal currentPercentage, decimal desiredPercentage)
        {
            return (desiredPercentage * value) / currentPercentage;
        }

       /* public static decimal FindPercentageByValue(this decimal currentPercentage, decimal value, decimal desiredPercentage)
        {
            return (desiredPercentage * value) / currentPercentage;
        }*/


        public static decimal AddPercentage(this decimal value, decimal percentage)
        {
            return value + value.Percentage(percentage);
        }

        public static decimal SubPercentage(this decimal value, decimal percentage)
        {
            return value - value.Percentage(percentage);
        }

        public static decimal Percentage(this decimal value, decimal percentage)
        {
            return (value * (percentage / 100m));
        }


        public static bool EqualsRound(this decimal value1, decimal value2, int round)
        {
            return (Math.Round(value1, round) == Math.Round(value2, round));
        }

        public static decimal MathRound(this decimal value, int decimals)
        {
            return Math.Round(value, decimals);
        }

        public static string[] ToStringArray(this decimal[] values)
        {
            if (values == null) return null;

            List<string> result = new List<string>();

            foreach (decimal d in values)
                result.Add(d.ToString());

            return result.ToArray();

        }
    }
}
