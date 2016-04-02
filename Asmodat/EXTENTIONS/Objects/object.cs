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
        public static bool IsNull(this object o) { return o == null ? true : false; }

        /// <summary>
        /// Can be used in case of overloading == inside 'o' class
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsRefNull(this object o) { return object.ReferenceEquals(null, o) ? true : false; }

        public static T TryCast<T>(this object o)
        {
            if (o == null)
                return default(T);

            try
            {
                T t = (T)o;
                return t;
            }
            catch
            {
                return default(T);
            }
        }

        public static bool IsNullable<T>(this object o)
        {
            if (o == null)
                return true;

            Type t = typeof(T);

            if (
                t.IsValueType ||
                Nullable.GetUnderlyingType(t) != null)
                return true; //ref-type || Nullable<T>
            else
                return false;
        }

       

    }
}
