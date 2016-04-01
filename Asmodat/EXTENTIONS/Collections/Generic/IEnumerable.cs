using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Drawing;

namespace Asmodat.Extensions.Collections.Generic
{
    

    public static class IEnumerableEx
    {
        public static TSource[] DistinctArray<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) return null;
            else return source.Distinct().ToArray();
        }

        /// <summary>
        /// Checks if Enumerable is null or it's count is less or equal zero.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<TSource, Tkey>(this IEnumerable<TSource> source)
        {
            if (source == null || source.Count() <= 0)
                return true;
            else return false;
        }

        /// <summary>
        /// Checks if enumerable count is less then value
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCountLessThen<TSource>(this IEnumerable<TSource> source, int value)
        {
            if (source == null || source.Count() < value)
                return true;
            else return false;
        }

        /// <summary>
        /// Checks if enumerable count is less or equal value
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsCountLessOrEqual<TSource>(this IEnumerable<TSource> source, int value)
        {
            if (source == null || source.Count() <= value)
                return true;
            else return false;
        }

        public static bool IsCountEqual<TSource>(this IEnumerable<TSource> source, int value)
        {
            if (source != null && source.Count() == value)
                return true;
            else return false;
        }

        /// <summary>
        /// Compares count of collections, 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="collection1"></param>
        /// <param name="collection2"></param>
        /// <returns>if any of collections is undefined (null) or count is unequal then false is returned</returns>
        public static bool EqualsCount<TSource>(this IEnumerable<TSource> collection1, IEnumerable<TSource> collection2)
        {
            if (collection1 != null && collection2 != null && collection1.Count() == collection2.Count())
                return true;
            else return false;
        }



        /// <summary>
        /// Removes repeating values inside table
        /// Use Example:
        /// IEnumerable[Foo] distinct = someList.DistinctBy(x => x.FooProperty);
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="Tkey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, Tkey>(this IEnumerable<TSource> source, Func<TSource, Tkey> keySelector)
        {
            if (source == null || keySelector == null || source.Count() <= 1)
                return source;

            var knownKeys = new HashSet<Tkey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }

        
        public static IEnumerable<TSource> SortAscending<TSource, Tkey>(this IEnumerable<TSource> source, Func<TSource, Tkey> keySelector)
        {
            if (source == null || keySelector == null || source.Count() <= 1)
                return source;

            return source.OrderBy(keySelector);
        }

        public static IEnumerable<TSource> SortDescending<TSource, Tkey>(this IEnumerable<TSource> source, Func<TSource, Tkey> keySelector)
        {
            if (source == null || source.Count() <= 1)
                return source;

            if (keySelector == null)
                return null;

                return source.OrderByDescending(keySelector);
        }

        /// <summary>
        /// Distinctincts list by key property, and then adds to dictionary
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="valueSelector"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TElement> ToDistinctKeyDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> valueSelector)
        {

            

            if (source == null || keySelector == null || valueSelector == null)
                return null;

            if (source.Count() <= 0)
                return new Dictionary<TKey, TElement>();

            List<TSource> distinct = source.DistinctBy(keySelector).ToList();

            if (distinct == null)
                return null;

            if (distinct.Count <= 0)
                return new Dictionary<TKey, TElement>();


            
            return distinct.ToDictionary(keySelector, valueSelector);
        }


       




    }
}
