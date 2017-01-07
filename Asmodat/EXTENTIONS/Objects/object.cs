using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Newtonsoft.Json;
using Asmodat.Debugging;

namespace Asmodat.Extensions.Objects
{
    

    public static class objectEx
    {
        public static string TryToString(this object o) { if (o == null) return null; else return o.ToString(); }

        public static bool IsNull(this object o) { return o == null ? true : false; }

        /// <summary>
        /// Can be used in case of overloading == inside 'o' class
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool IsRefNull(this object o) { return object.ReferenceEquals(null, o) ? true : false; }


        


        public static string SerializeJson(this object o)
        {
            if (o == null)
                return null;
            try
            {
                return JsonConvert.SerializeObject(o);
            }
            catch(Exception ex)
            {
                ex.ToOutput();
            }

            return null;
        }

        public static T DeserializeJson<T>(this string o)
        {
            if (o == null)
                return default(T);
            try
            {
                return JsonConvert.DeserializeObject<T>(o);
            }
            catch (Exception ex)
            {
                ex.ToOutput();
            }

            return default(T);
        }

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

        public static T TryConvert<T>(this object o)
        {
            if (o == null)
                return default(T);

            try
            {
                return (T)System.Convert.ChangeType(o, typeof(T));
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
