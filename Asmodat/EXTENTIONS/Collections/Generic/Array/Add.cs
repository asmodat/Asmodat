using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;

using System.Drawing;

using Asmodat.Extensions;
using System.Runtime.CompilerServices;

namespace Asmodat.Extensions.Collections.Generic
{
    

    public static partial class ArrayEx
    {
        

        public static TKey[] AddDistinct<TKey>(this TKey[] source, TKey value)
        {
            if (value == null)
                return source;

            List<TKey> list;
            if (source.IsNullOrEmpty())
                list = new List<TKey>();
            else list = new List<TKey>(source);

            list.AddDistinct(value);

            source = list.ToArray();

            return list.ToArray();
        }

        public static TKey[] AddRangeDistinct<TKey>(this TKey[] source, List<TKey> values)
        {
            if (values.IsNullOrEmpty())
                return source;

            List<TKey> list;
            if (source.IsNullOrEmpty())
                list = new List<TKey>();
            else list = new List<TKey>(source);

            foreach (var v in values)
                list.AddDistinct(v);

            return list.ToArray();
        }

        public static TKey[] AddRangeDistinct<TKey>(this TKey[] source, TKey[] values)
        {
            if (values.IsNullOrEmpty())
                return source;

            return source.AddRangeDistinct(values.ToList());
        }

        public static T[] Prepend<T>(this T[] source, T[] values)
        {
            if (source == null && values == null)
                return null;

            if (source != null && values == null)
                return source.Copy();

            if (source == null && values != null)
                return values.Copy();

            int sl = source.Length, vl = values.Length;

            T[] result = new T[sl + vl];

            if(sl > 0) Array.Copy(source, 0, result, vl, sl);
            if(vl > 0) Array.Copy(values, 0, result, 0, vl);

            return result;
        }

    }
}

