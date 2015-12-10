using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Asmodat.Abbreviate;
using Asmodat.Types;

using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Asmodat.IO
{
    public partial class DatabseSimpleton
    {

        private FileDictionary<string, object> Data { get; set; }


        public string[] Keys
        {
            get
            {
                return Data.Data.KeysArray;
            }
        }

        public object[] Values
        {
            get
            {
                return Data.Data.ValuesArray;
            }
        }

        public int Count
        {
            get
            {
                if (Data == null || Data.Data == null)
                    return 0;

                return Data.Data.Count;
            }
        }


        public bool ContainsKey(string key)
        {
            if (Data == null || Data.Data == null || Data.Data.Count <= 0)
                return false;


            return Data.Data.ContainsKey(key);
        }

        public bool InstantSaving { get; set; }


        public void Save()
        {
            Data.Save();
        }

        public void Reset()
        {
            Data.Reset();
        }

        /// <summary>
        /// if path is not full then panth = MyDirectory\\File.extention
        /// </summary>
        /// <param name="path"></param>
        /// <param name="useCurrentDirectory"></param>
        /// <param name="saveInstantly"></param>
        public DatabseSimpleton(string path, bool saveInstantly)
        {

           path = Files.GetFullPath(path);
            Data = new FileDictionary<string, object>(path);
            InstantSaving = saveInstantly;
        }


        public V Get<K, V>(K Key)
        {
            string key = null;
            if (typeof(K).IsEnum)
                key = Enums.GetName<K>(Key);
            else
                key = Key.ToString();

            return this.Get<V>(key);
        }

        public void Get<K, V>(K Key, out V value)
        {
            string key = null;
            if (typeof(K).IsEnum)
                key = Enums.GetName<K>(Key);
            else
                key = Key.ToString();

            value = this.Get<V>(key);
        }

        public V Get<V>(string key)
        {
            if (string.IsNullOrEmpty(key) || !Data.Data.ContainsKey(key))
                return default(V);


            if(typeof(V) == typeof(bool))
            {
                bool value = System.Convert.ToBoolean(Data.Data[key]);
                return (V)System.Convert.ChangeType(value, typeof(V));
            }

            return (V)Data.Data[key];
        }


        public object Get<K>(K Key)
        {
            string key = null;
            if (typeof(K).IsEnum)
                key = Enums.GetName<K>(Key);
            else
                key = Key.ToString();

            return this.Get(key);
        }





        public object Get(string key)
        {
            if (string.IsNullOrEmpty(key) || !Data.Data.ContainsKey(key))
                return null;


            return Data.Data[key];
        }


        public void Set<K, V>(K Key, V value)
        {
            string key = null;

            if (typeof(K).IsEnum)
                key = Enums.GetName<K>(Key);
            else
                key = Key.ToString();

            this.Set(key, (object)value);
        }


        public void Set(string key, object value)
        {
            if (string.IsNullOrEmpty(key))
                return;
            else if (!Data.Data.ContainsKey(key))
                Data.Data.Add(key, value);
            else
                Data.Data[key] = value;



            if (InstantSaving)
                Data.Save();
        }

    }
}
