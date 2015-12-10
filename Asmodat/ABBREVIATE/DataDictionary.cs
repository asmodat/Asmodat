using Asmodat.Extensions.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Abbreviate
{

    /// <summary>
    /// This is String-Type Dictionary class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataDictionary<T>
    {
        public ThreadedDictionary<string, T> Data { get; private set; } = new ThreadedDictionary<string, T>();

        public bool ChangeKey(string sOldKey, string sNewKey)
        {
            if (!this.Contains(sOldKey) || this.Contains(sNewKey)) return false;

            T TSave = this.Get(sOldKey);

            if (!this.Remove(sOldKey)) return false;

            return this.Set(sNewKey, TSave);
        }



        public bool Contains(string sKey)
        {
            if (Data.ContainsKey(sKey))
                return true;
            else return false;
        }

        public bool Set(string sKey, T tValue, bool bUpdate = true)
        {
            if(sKey == null) return false;

            if (!this.Contains(sKey))
            {
                Data.Add(sKey, tValue);
                return true;
            }

            if (bUpdate)
            {
                Data[sKey] = tValue;
                return true;
            }

            return false;
        }

        public T Get(string sKey)
        {
            if (sKey == null || !Data.ContainsKey(sKey)) return default(T);

            return Data[sKey];
        }

        public T[] GetAll()
        {
            if (Data.IsNullOrEmpty())
                return null;

            return Data.Values.ToArray();
        }


        public bool Remove(string sKey)
        {
            if (!this.Contains(sKey))
                return true;

            Data.Remove(sKey);

            if (!this.Contains(sKey))
                return true;
            else
                return false;
        }


        public string[] Keys
        {
            get
            {
                if (Data == null)
                    return null;

                return Data.KeysArray;
            }
        }

    }
}
