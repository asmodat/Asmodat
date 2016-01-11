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


        public static bool SafeContains<TKey>(this List<TKey> source, TKey value)
        {
            if (source.IsNullOrEmpty())
                return false;
            else return source.Contains(value);
        }

        public static bool TryContains<TKey>(this List<TKey> source, TKey value)
        {
            try
            {
                return source.SafeContains(value);
            }
            catch
            {
                return false;
            }
        }


    }
}
