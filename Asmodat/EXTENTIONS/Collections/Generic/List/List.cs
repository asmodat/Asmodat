using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Drawing;

using Asmodat.Extensions;

namespace Asmodat.Extensions.Collections.Generic
{
    

    public static partial class ListEx
    {
        public static T[]  TryToArray<T>(this List<T> source)
        {
            try
            {
                if (source == null)
                    return null;
                else
                    return source.ToArray();
            }
            catch
            {
                return null;
            }
        }


        public static T[] ToSafeArray<T>(this List<T> source, int length)
        {
            if (length < 0)
                return null;

            if (source == null || length == 0)
                return new T[length];

            List<T> subl = source.GetSafeRange(0, length);

            if (length > source.Count)
                subl.AddRange(new T[length - source.Count]);

            return subl.TryToArray();
        }

        public static bool SafeContains<TKey>(this List<TKey> source, TKey value)
        {
            if (source.IsNullOrEmpty())
                return false;
            else return source.Contains(value);
        }

        public static bool TryContains<TKey>(this List<TKey> source, TKey value)
        {
            try
            {
                return source.SafeContains(value);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Tries to safely gets sub lists, if offset is to small its set to 0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static List<T> GetSafeRange<T>(this List<T> source, int offset)
        {
            if (source == null) return null;

            if (offset < 0) offset = 0;
            if (offset >= source.Count) new List<T>();

            return source.GetSafeRange(offset, source.Count - offset);
        }

        /// <summary>
        /// Tries to safely gets sub lists, if offset is to small its set to 0, if count is to big, it is set to source.Count
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static List<T> GetSafeRange<T>(this List<T> source, int offset, int count)
        {
            if (source == null) return null;

            if (offset < 0) offset = 0;
            if (offset >= source.Count) new List<T>();
            if (offset + count > source.Count) count = source.Count - offset;

            return source.GetRange(offset, count);
        }


    }
}
