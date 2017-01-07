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
        /* public static bool AddToEnd<TKey>(this List<TKey> source, params TKey[] values)
         {
             if (source.IsNullOrEmpty() && values.IsNullOrEmpty())
                 return false;
             else if (source.IsNullOrEmpty() && !values.IsNullOrEmpty())
             {
                 source = new List<TKey>();
                 source.AddRange(values);
             }
             else if (!source.IsNullOrEmpty() && !values.IsNullOrEmpty())
                 source.AddRange(values);

             return true;
         }*/

        public static List<T> Create<T>(int _size, T _default)
        {
            if (_size < 0) return null;
            return Enumerable.Repeat(_default, _size).ToList();
        }
        
        public static bool AddToEnd<TKey>(this List<TKey> source, List<TKey> values)
        {
            if (source == null && values == null)
            {
                return false;
            }
            else if (source == null && values != null)
            {
                source = new List<TKey>();
                source.AddRange(values);
                return true;
            }
            else if (source != null && values != null)
            {
                source.AddRange(values);
                return true;
            }
            else return true;
        }

        public static bool AddToEnd<TKey>(this List<TKey> source, params TKey[] values)
        {
            return source.AddToEnd(values.GetList());
        }



        /// <summary>
        /// Creates new list or adds element if its not already added
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns>True if distinct wass added to array, else false</returns>
        public static bool AddDistinct<TKey>(this List<TKey> source, TKey value)
        {
            if (source == null && value == null)
                return false;
            else if (source == null && value != null)
            {
                source = new List<TKey>() { value };
                return true;
            }
            else if (source != null && value != null)
            {
                if (!source.Contains(value))
                {
                    source.Add(value);
                    return true;
                }
            }

            return false;
        }

        public static bool AddRangeDistinct<TKey>(this List<TKey> source, List<TKey> values)
        {
            if (source == null && values.IsNullOrEmpty())
                return false;

            bool result = false;
            foreach (var v in values)
            {
                if (source.AddDistinct(v))
                    result = true;
            }

            return result;
        }

        public static bool AddRangeDistinct<TKey>(this List<TKey> source, TKey[] value)
        {
            return ListEx.AddRangeDistinct(source, value.GetList());
        }

        



        public static void AddValue<TKey>(this List<TKey> source, TKey? value) where TKey : struct
        {
            if (source == null && value.IsNull())
                return;
            else if (source == null && !value.IsNull())
                source = new List<TKey>() { value.Value };
            else if (source != null && !value.IsNull())
                source.Add(value.Value);
        }

        public static void AddValueDistinct<TKey>(this List<TKey> source, TKey? value) where TKey : struct
        {
            if (source == null && value.IsNull())
                return;
            else if (source == null && !value.IsNull())
                source = new List<TKey>() { value.Value };
            else if (source != null && !value.IsNull())
            {
                if (!source.Contains(value.Value))
                    source.Add(value.Value);
            }
        }

        /// <summary>
        /// Adds value to list if value is not null, or creates list in the first place if it is null.
        /// If list is null and value is null, the result is null.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        public static void AddIfValueIsNotNull<TKey>(this List<TKey> source, TKey value)
        {
            if (source == null && value == null)
                return;
            else if (source == null && value != null)
                source = new List<TKey>() { value };
            else if (source != null && value != null)
                source.Add(value);
        }

        /// <summary>
        /// Adds Value of nullable parameter to list if value.Value is not null, or creates list in the first place if it is null.
        /// If list is null and value.Value is null, the result is null.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="value"></param>
        public static void AddIfValueIsNotNull<TKey>(this List<TKey> source, TKey? value) where TKey : struct
        {
            if (source == null && value.IsNull())
                return;
            else if (source == null && !value.IsNull())
                source = new List<TKey>() { value.Value };
            else if (source != null && !value.IsNull())
                source.Add(value.Value);
        }

    }
}
