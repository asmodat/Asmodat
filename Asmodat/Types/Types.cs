using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Types
{
    public static class Types
    {
        public static bool IsDictionary<T>()
        {
            if (typeof(T).GetInterface(typeof(IDictionary<,>).Name) != null || typeof(T).Name.Contains("IDictionary"))
                return true;
            else return false;
        }
    }
}
