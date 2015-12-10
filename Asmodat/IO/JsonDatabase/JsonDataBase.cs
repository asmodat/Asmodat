using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;
using Asmodat.Types;

using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Newtonsoft.Json;
using System.Security;

using Asmodat.Cryptography;
using System.Collections;

namespace Asmodat.IO
{
    public partial class JsonDataBase<TJson> //: IEnumerable, IDisposable where TJson : class
    {
        

        public JsonDataBase(string path, bool GZip, bool Encryption, SecureString Password = null) 
        {
            this.GZip = GZip;
            this.Encryption = Encryption;
            this.InitializePassword(Password);

            var info = Directories.Create(path);
            Path = info.FullName;

            this.Load();
        }


        public int Count
        {
            get
            {
                int result = 0;
                if(Data != null)
                lock (Data)
                {
                    result = Data.Count;
                }
                return result;
            }
        }

        public bool Contains(string key)
        {
            bool result = false;
            lock (Data)
            {
                result = Data.ContainsKey(key);
            }
            return result;
        }

        public void Remove(string key, bool save)
        {
            lock (Data)
            {
                if (Data.ContainsKey(key))
                    Data.Remove(key);

                if (save)
                    this.Delete(key);
            }
        }

        public TJson Get(string key)
        {
            lock(Data)
            {
                if (!Data.ContainsKey(key))
                    return null;

                return Data[key];
            }
        }

      /*  public TJson[] GetAll(Func<string, TJson> keySelector)
        {
            
            lock (Data)
            {
                if (Data.IsNullOrEmpty())
                    return null;

                List<TJson> jsons = new List<TJson>();

                foreach(var v in Data)
                {
                    if(v.Value != null && v.Value.)
                }

                // if (!Data.ContainsKey(key))
                //    return null;

                //return Data[key];
            }

            return jsons.ToArray();
        }*/

        public void Set(string key, TJson value, bool save)
        {
            lock (Data)
            {
                if (!Data.ContainsKey(key))
                    Data.Add(key, value);
                else
                    Data[key] = value;

                if (save)
                    this.Save(key, Data[key]);
            }
        }

        
    }
}
