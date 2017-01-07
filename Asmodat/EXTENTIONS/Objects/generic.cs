using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Extensions.Objects
{
    

    public static class genericEx
    {
        /// <summary>
        /// Returns value if object is Equale
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public static T ValueOrDefault<T>(this T value, T _default) where T : class
        {
            if (value == null)
                return _default;
            else
                return value;
        }




        public static bool IsOneOf<T>(this T val, params T[] ps) where T : IEquatable<T>
        {
            if (ps.IsNullOrEmpty())
                return false;

            foreach (T p in ps)
                if (val.Equals(p))
                    return true;

            return false;
        }

        public static bool Equals<T>(this T val1, T val2)
        {
            return !EqualityComparer<T>.Default.Equals(val1, val2);
        }


    }
}
