using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

namespace Asmodat.Extensions.Objects
{
    

    public static class objectEx
    {
        public static T TryCast<T>(this object obj)
        {
            if (obj == null)
                return default(T);

            try
            {
                T to = (T)obj;
                return to;
            }
            catch
            {
                return default(T);
            }
        }


    }
}
