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
    public partial class FileDictionary<TKey, TValue>
    {
        private bool Load()
        {
            byte[] bytes;
            lock (locker)
                bytes = System.IO.File.ReadAllBytes(FullPath);
            
            if (bytes == null || bytes.Length <= 0) return false;

            string data = Compression.UnZipString(bytes);
            if (System.String.IsNullOrEmpty(data)) return false;

            lock (locker)
            {
                Data.XmlDeserialize(data);

                //Data = new ThreadedDictionary<TKey, TValue>();

                //List<XmlPair<TKey, TValue>> Items = Asmodat.Abbreviate.XmlSerialization.Deserialize<List<XmlPair<TKey, TValue>>>(data);
                //foreach (XmlPair<TKey, TValue> item in Items)
                //    Data.Add(item.Key, item.Value);
            }


            UpdateTime = DateTime.Now;
            return true;
        }

        public bool Save()
        {
            if (!Enabled)
                return false;

            lock (locker)
            {
                if (Data.Count == 0)
                {
                    FileStream FStream = System.IO.File.Create(FullPath);
                    FStream.Close();
                }
                else
                {
                    string data = Data.XmlSerialize(); //Asmodat.Abbreviate.XmlSerialization.Serialize<List<XmlPair<TKey, TValue>>>(Items);
                    byte[] bytes = Compression.Zip(data);
                    if (bytes == null) return false;
                    System.IO.File.WriteAllBytes(FullPath, bytes);
                }
            }


            UpdateTime = DateTime.Now;
            return true;
        }
    }
}

/*
public partial class FileDictionary<TKey, TValue> 
    {
        private bool LoadData()
        {
            byte[] bytes;
            lock (Locker.Get("IO"))
                bytes = File.ReadAllBytes(FullPath);
            
            if (bytes == null || bytes.Length <= 0) return false;

            string data = Compression.UnZipString(bytes);
            if (System.String.IsNullOrEmpty(data)) return false;

            lock (Locker.Get("Data"))
            {
                Data = new ThreadedDictionary<TKey, TValue>();
                XmlSerializer Serializer = new XmlSerializer(typeof(List<DataPair>));
                using (StringReader Reader = new StringReader(data))
                {
                    List<DataPair> Items = (List<DataPair>)Serializer.Deserialize(Reader);
                    foreach (DataPair item in Items)
                        Data.Add(item.Key, item.Value);
                }
            }


            UpdateTime = DateTime.Now;
            return true;
        }
        
        public bool SaveData()
        {
            
                List<DataPair> Items;
                lock (Locker.Get("Data"))
                {
                    Items = new List<DataPair>(Data.Count);
                    foreach (KeyValuePair<TKey, TValue> KVP in Data)
                        Items.Add(new DataPair(KVP.Key, KVP.Value));
                }

                XmlSerializer Serializer = new XmlSerializer(typeof(List<DataPair>));
                string data;
                using (StringWriter Writer = new StringWriter())
                {
                    XmlSerializerNamespaces Namespaces = new XmlSerializerNamespaces();
                    Namespaces.Add("", "");
                    Serializer.Serialize(Writer, Items, Namespaces);
                    data = Writer.ToString();
                }



                byte[] bytes = Compression.Zip(data);
                if (bytes == null) return false;
                lock (Locker.Get("IO"))
                    File.WriteAllBytes(FullPath, bytes);
                

            UpdateTime = DateTime.Now;
            return true;
        }
    }
*/