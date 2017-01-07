using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;

namespace Asmodat.Extensions.Collections.Generic
{
    public static partial class DictionaryEx
    {
        /// <summary>
        /// Adds new elements to dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static void AddOrUpdate<TKey, TElement>(this Dictionary<TKey, TElement> source, Dictionary<TKey, TElement> elements)
        {
            if (source == null && elements == null)
                return;

            if (source != null && elements == null)
                return;
            

            if (source == null && elements != null)
            {
                source = new Dictionary<TKey, TElement>(elements);
                return;
            }


            Dictionary<TKey, TElement> result = new Dictionary<TKey, TElement>(source);

            foreach (var e in elements)
            {
                if (source.ContainsKey(e.Key))
                    source[e.Key] = e.Value;
                else
                    source.Add(e.Key, e.Value);
            }
        }

        public static void AddOrUpdate<TKey, TElement>(this Dictionary<TKey, TElement> source, KeyValuePair<TKey, TElement> pair)
        {
            if (pair.Key == null)
                return;

            if (source == null)
            {
                source = new Dictionary<TKey, TElement>();
                source.AddOrUpdate(pair);
                return;
            }

            if (source.ContainsKey(pair.Key))
                source[pair.Key] = pair.Value;
            else
                source.Add(pair.Key, pair.Value);
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (key == null)
                return;

            if (source == null)
            {
                source = new Dictionary<TKey, TValue>();
                source.AddOrUpdate(key, value);
                return;
            }

            if (source.ContainsKey(key))
                source[key] = value;
            else
                source.Add(key, value);
        }

    }
}
