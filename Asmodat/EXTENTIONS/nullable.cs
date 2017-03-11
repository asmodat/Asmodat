using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Extensions
{
    

    public static class nullableEx
    {
        public static T Value<T>(this T? source, T _default = default(T)) where T : struct
        {
            return (source == null || !source.HasValue) ? _default : source.Value;
        }

        public static T TryGetValue<T>(this T? source, T _default = default(T)) where T : struct
        {
            try
            {
                return Value(source, _default);
            }
            catch
            {
                return _default;
            }
        }

        public static bool IsNull<T>(this T? source) where T : struct
        {
            if (source == null || !source.HasValue)
                return true;
            
            return false;
        }

        /// <summary>
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


        public static bool TryGetValue<T>(this T? source, out T result) where T : struct
        {
            if (source == null || !source.HasValue)
            {
                result = default(T);
                return false;
            }
            else
            {
                result = source.Value;
                return true;
            }
        }

        

    }
}
