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
    public partial class FileList<TValue>
    {
        private bool Load()//string FullPatch, ref List<TValue> Items
        {
            return this.LoadData();
        }

        private bool LoadData()//string FullPatch, ref List<TValue> Items
        {
            byte[] bytes;
            lock (Locker.Get("IO"))
                bytes = System.IO.File.ReadAllBytes(FullPath);
            
            if (bytes == null || bytes.Length <= 0) return false;

            string data = Compression.UnZipString(bytes);
            if (System.String.IsNullOrEmpty(data)) return false;


            XmlList<TValue> XData =  Asmodat.Abbreviate.XmlSerialization.Deserialize<XmlList<TValue>>(data);
            lock (Locker.Get("Data"))
            {
               
                Data = XData.Items;
            }


            SaveTime = DateTime.Now;
            return true;
        }


        public bool Save()
        {
            return this.SaveData();
        }

        public bool SaveData()
        {

            XmlList<TValue> XData = new XmlList<TValue>();
                
                if (Data == null || Data.Count == 0) lock (Locker.Get("IO"))
                {
                    FileStream FStream = System.IO.File.Create(FullPath);
                    FStream.Close();
                }
                else
                {
                    lock (Locker.Get("Data"))
                    {
                        XData.Items = new List<TValue>(Data.Count);
                        foreach (TValue value in Data)
                            XData.Items.Add(value);
                    }


                    string data = Asmodat.Abbreviate.XmlSerialization.Serialize<XmlList<TValue>>(XData);
                    byte[] bytes = Compression.Zip(data);
                    if (bytes == null) return false;
                    lock (Locker.Get("IO"))
                        System.IO.File.WriteAllBytes(FullPath, bytes);
                }

            SaveTime = DateTime.Now;
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