using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using AsmodatMath;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Extensions
{
    

    public static class IComparableEx
    {

        public static T ToClosedInterval<T>(this T value, T min, T max) where T : IComparable<T>
        {
            //if (min.IsGreaterThen(max)) throw new ArgumentException("Invalid interval.");

            T result = value;

            //note: dont use if else statement, in case min == max
            if (value.IsLessThen(min)) // value < min
                result = min;
            if (value.IsGreaterThen(max)) // value > max
                result = max;

            return result;
        }
        


        /// <summary>
        /// Checks if value is inside closed [min, max] interval
        /// </summary>
        /// <param name="value">checked value</param>
        /// <param name="min">minimum value</param>
        /// <param name="max">maximum value</param>
        /// <returns></returns>
        public static bool InClosedInterval<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (min.IsGreaterThen(max))
                return false;

            if (value.IsGreaterOrEqual(min) && value.IsLessOrEqual(max))
                return true;

            return false;
        }

        /// <summary>
        /// Checks if value is inside open (min, max) interval
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">checked value</param>
        /// <param name="min">minimum value</param>
        /// <param name="max">maximum value</param>
        /// <returns></returns>
        public static bool InOpenInterval<T>(this T value, T min, T max) where T : IComparable<T>
        {
            if (min.IsGreaterOrEqual(max))
                return false;

            if (value.IsGreaterThen(min) && value.IsLessThen(max))
                return true;

            return false;
        }


        /// <summary>
        /// value {= comp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool IsLessOrEqual<T>(this T value, T comp) where T : IComparable<T>
        {
            if (value.CompareTo(comp) <= 0)
                return true;

            return false;
        }

        /// <summary>
        /// value >= comp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool IsGreaterOrEqual<T>(this T value, T comp) where T : IComparable<T>
        {
            if (value.CompareTo(comp) >= 0)
                return true;

            return false;
        }

        /// <summary>
        /// value { comp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool IsLessThen<T>(this T value, T comp) where T : IComparable<T>
        {
            if (value.CompareTo(comp) < 0)
                return true;

            return false;
        }

        /// <summary>
        /// value > comp
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool IsGreaterThen<T>(this T value, T comp) where T : IComparable<T>
        {
            if (value.CompareTo(comp) > 0)
                return true;

            return false;
        }

        /// <summary>
        /// ... value > comp[n - 1] && value > comp[n]; where n belongs to [0, comp.length]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="comp"></param>
        /// <returns></returns>
        public static bool IsGreaterThenAll<T>(this T value, params T[] comps) where T : IComparable<T>
        {
            if (comps.IsNullOrEmpty())
                return false;

            for(int i = 0; i < comps.Length; i++)
            {
                if (value.CompareTo(comps[i]) <= 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// ... value > comp[n - 1] || value > comp[n]; where n belongs to [0, comp.length]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="comps"></param>
        /// <returns></returns>
        public static bool IsGreaterThenAny<T>(this T value, params T[] comps) where T : IComparable<T>
        {
            if (comps.IsNullOrEmpty())
                return false;

            for (int i = 0; i < comps.Length; i++)
            {
                if (value.CompareTo(comps[i]) > 0)
                    return true;
            }

            return false;
        }


        /*public static bool Min<T>(this T[,] value) where T : IComparable<T>
        {
            if (value.CompareTo(comp) <= 0)
                return true;

            return false;
        }*/
    }
}
