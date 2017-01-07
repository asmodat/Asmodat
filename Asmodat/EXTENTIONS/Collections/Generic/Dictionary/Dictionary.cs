using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Drawing;

namespace Asmodat.Extensions.Collections.Generic
{
    public  static partial class DictionaryEx
    {
        public static V TryGetValue<K,V>(this Dictionary<K,V> dictionary, K key, V defaultValue)
        {
            if (key == null || dictionary.IsNullOrEmpty())
                return defaultValue;

            V result;
            if (dictionary.TryGetValue(key, out result) == true)
                return result;
            else
                return defaultValue;
        }

        public static bool ContainsAnyKey<K, V>(this Dictionary<K, V> dictionary, K[] key)
        {
            if (dictionary == null || key == null)
                return false;

            for(int i = 0; i < key.Length; i++)
                if (dictionary.ContainsKey(key[i]))
                    return true;

            return false;
        }

        public static bool ContainsAllKeys<K, V>(this Dictionary<K, V> dictionary, K[] key)
        {
            if (dictionary == null || key == null)
                return false;

            for (int i = 0; i < key.Length; i++)
                if (!dictionary.ContainsKey(key[i]))
                    return false;

            return true;
        }

    }
}
