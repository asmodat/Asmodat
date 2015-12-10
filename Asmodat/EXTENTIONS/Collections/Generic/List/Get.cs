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
        public static TKey[] GetArray<TKey>(this List<TKey> source)
        {
            if (source == null)
                return null;

            return source.ToArray();
        }


        /// <summary>
        /// Allows to index flist from the end
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public static TSource GetLast<TSource>(this List<TSource> source, int index = 0, TSource _default = default(TSource))
        {
            if (source.IsNullOrEmpty() || index >= source.Count)
                return _default;

            TSource result = _default;
            try
            {
                result = source[source.Count - index - 1];
            }
            catch
            {
                result = _default;
            }


            return result;
        }
    }
}
