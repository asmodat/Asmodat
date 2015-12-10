using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Drawing;

namespace Asmodat.Extensions.Collections.Generic
{
    

    public static class KeyValuePairEx
    {
        
        public static bool IsNullKey<TKey, TValue>(this KeyValuePair<TKey,TValue>? source)
        {
            if (source == null || !source.HasValue || source.Value.Key == null)
                return true;
            else return false;
        }

        public static bool IsNullValue<TKey, TValue>(this KeyValuePair<TKey, TValue>? source)
        {
            if (source.IsNullKey() || source.Value.Value == null)
                return true;
            else return false;
        }




    }
}
