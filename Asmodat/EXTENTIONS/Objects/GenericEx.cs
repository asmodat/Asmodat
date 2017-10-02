using System;
using System.Collections.Generic;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Extensions.Objects
{
    public static class GenericEx
    {
        /// <summary>
        /// returns 'value' if 'value' == null else '_default'
        /// </summary>
        public static T ValueOrDefault<T>(this T value, T _default) where T : class
            => (value == null) ? _default : value;

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
            => !EqualityComparer<T>.Default.Equals(val1, val2);
    }
}
