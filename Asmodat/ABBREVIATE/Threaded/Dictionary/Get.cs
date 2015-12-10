using System;
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
        //public TValue[] ValuesArray { get { lock (this) return base.Values.ToArray(); } }

        public TValue GetValue(TKey key)
        {
            lock (locker)
            {
                if (base.ContainsKey(key))
                {
                    return base[key];
                }
            }

            return default(TValue);
        }

        public TValue TryGetValue(TKey key, TValue _default = default(TValue))
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
