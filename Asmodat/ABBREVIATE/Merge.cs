using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Abbreviate
{
    public static class Merge
    {
        public static void Dictionary<K, V>(Dictionary<K, V> DKVSource, ref Dictionary<K, V> DKVDestination, bool update = false)
        {
            foreach(var v in DKVSource)
            {
                if (!DKVDestination.ContainsKey(v.Key))
                    DKVDestination.Add(v.Key, v.Value);
                else if(update)
                    DKVDestination[v.Key] = v.Value;
            }
        }

    }
}
