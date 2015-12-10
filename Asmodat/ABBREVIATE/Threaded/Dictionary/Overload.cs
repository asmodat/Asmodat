using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        public static explicit operator Dictionary<TKey, TValue>(ThreadedDictionary<TKey, TValue> TDKV)
        {
            return TDKV.ToDictionary();
        }
        public static explicit operator ThreadedDictionary<TKey, TValue>(Dictionary<TKey, TValue> DKV)
        {
            return new ThreadedDictionary<TKey, TValue>(DKV);
        }


        public static ThreadedDictionary<K, ThreadedDictionary<K2, V2>> ToThreadedDictionary<K, K2, V2>(Dictionary<K, Dictionary<K2, V2>> D2KV)
        {
            ThreadedDictionary<K, ThreadedDictionary<K2, V2>> TD2KV = new ThreadedDictionary<K, ThreadedDictionary<K2, V2>>();
            foreach (KeyValuePair<K, Dictionary<K2, V2>> KVP2 in D2KV)
                TD2KV.Add(KVP2.Key, (ThreadedDictionary<K2, V2>)KVP2.Value);

            return TD2KV;
        }

        public static Dictionary<K, Dictionary<K2, V2>> ToDictionary<K, K2, V2>(ThreadedDictionary<K, ThreadedDictionary<K2, V2>> TD2KV)
        {
            Dictionary<K, Dictionary<K2, V2>> D2KV = new Dictionary<K, Dictionary<K2, V2>>();
            foreach (KeyValuePair<K, ThreadedDictionary<K2, V2>> KVP2 in TD2KV)
                D2KV.Add(KVP2.Key, (Dictionary<K2, V2>)KVP2.Value);

            return D2KV;
        }

    }
}
