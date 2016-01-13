using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Extensions.Objects;

namespace Asmodat.Extensions
{
    

    public static class EnumEx
    {
        public static bool TryParseEnum<T>(this string value, out T? result) where T : struct
        {
            var ret = value.IsNullOrEmpty() ? false : Enum.IsDefined(typeof(T), value);
            result = ret ? (T)Enum.Parse(typeof(T), value) : default(T);
            
            return ret;
        }

        /*/// <summary>
        /// Compares nullable values, if anu nullable is null or does not have value, it returns false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool ValueEquals<T>(this T? first, T? second) where T : struct
        {
            if (first.IsNull() || second.IsNull()) //this line must be added, becouse it is not supportet with EqualityComparer
                return false;

            return EqualityComparer<T>.Default.Equals(first.Value, second.Value);
        }
        */



    }
}
