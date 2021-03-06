﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        /// <summary>
        /// Returns value from dictionary, firstly checking if it exists, if not, then default value is returned
        /// </summary>
        /// <param name="key"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public TValue GetValue(TKey key, TValue _default = default(TValue))
        {
            lock (locker)
            {
                if (base.ContainsKey(key))
                {
                    return base[key];
                }
            }

            return _default;
        }

        /// <summary>
        /// Returns value from dictionary, firstly checking if it exists, if not, then default value is returned
        /// Does not rise any exceptions like GetValue
        /// </summary>
        /// <param name="key"></param>
        /// <param name="_default"></param>
        /// <returns></returns>
        public TValue TryGetValue(TKey key, TValue _default = default(TValue))
        {
            try
            {
                lock (locker)
                {
                    return base.ContainsKey(key) ? base[key] : _default;
                }
            }
            catch
            {
                return _default;
            }
        }

        /// <summary>
        /// If value of the array is IEnumerable than this method can return specified values from value of this array.
        /// Example: var d = new ThreadedDictionary(TKey, ThreadedSortedSet[T>); -> var result = d.TryGetWhere[T>(someKey, x => x.Prop == someValue);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> TryGetWhere<T>(TKey key, Func<T, bool> predicate)
        {
            try
            {
                lock (locker)
                {
                    return base.ContainsKey(key) ? ((IEnumerable<T>)base[key])?.Where(predicate) : null;
                }
            }
            catch
            {
                return null;
            }
        }


        public T TryGetLastOrDefault<T>(TKey key, T _default = default(T))
        {
            try
            {
                lock (locker)
                {
                    if (!base.ContainsKey(key))
                        return _default;

                    var obj = ((IEnumerable<T>)base[key]);

                    if (obj == null || obj.Count() <= 0)
                        return _default;

                    return obj.Last();
                }
            }
            catch
            {
                return _default;
            }
        }

        public TValue[] GetValuesArray(TKey key)
        {
            lock (locker)
            {
                if (this.ContainsKey(key))
                {
                    return base.Values.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// ThreadedDictionary{TK2, ThreadedDictionary {TK3, TV3>> that 
        /// returns that[key2].ValuesArray;
        /// </summary>
        /// <typeparam name="TK2"></typeparam>
        /// <typeparam name="TK3"></typeparam>
        /// <typeparam name="TV3"></typeparam>
        /// <param name="key1"></param>
        /// <param name="key2"></param>
        /// <returns></returns>
        public TV3[] GetValuesArray<TK2, TK3, TV3>(TKey key1, TK2 key2)
        {
            lock (locker)
            {
                if (base.ContainsKey(key1) && base[key1] != null)
                {

                    ThreadedDictionary<TK2, ThreadedDictionary<TK3, TV3>> that = base[key1] as ThreadedDictionary<TK2, ThreadedDictionary<TK3, TV3>>;

                    if (that != null && that.ContainsKey(key2))
                        return that[key2].ValuesArray;
                }
            }
            

            return null;
        }

    }
}
