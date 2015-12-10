using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Asmodat.Types;

using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Newtonsoft.Json;
using System.Security;

using Asmodat.Cryptography;
using Asmodat.Extensions.Objects;

namespace Asmodat.IO
{
    public partial class JsonDataBase<TJson> : IDisposable
    {
        public void Delete(string key)
        {
            if (!Files.IsValidFilename(key))
                return;

            string path = GetPath(key);

            Files.Delete(path);
        }
        public void Delete()
        {
            if (Path == null) throw new Exception("Output directory for JsonDataBase is not specified.");

            string[] files = Directory.GetFiles(Path);

            if (files == null || files.Length <= 0) return;

            lock (Data)
            {
                foreach (string file in files)
                {
                    string key = Files.GetName(file);
                    string ext = Files.GetExtension(file);
                    if (ext != extension || key.IsNullOrWhiteSpace())
                        continue;

                    this.Delete(key);
                }

                Data = new Dictionary<string, TJson>();
            }
        }

        public TJson Load(string key)
        {
            if (!Files.IsValidFilename(key))
                return null;

            string path = GetPath(key);

            string data = Files.LoadText(path, GZip);

            

            if (Encryption)
            {
                string pass = Password.Release() + key;
                try
                {
                    data = AES.Decrypt(data, pass);
                }
                catch
                {
                    return null;
                }
            }

            TJson json = JsonConvert.DeserializeObject<TJson>(data);

            return json;
        }
        public void Load()
        {
            if (Path == null) throw new Exception("Output directory for JsonDataBase is not specified.");

            string[] files = Directory.GetFiles(Path);

            if (files == null || files.Length <= 0) return;
       
            lock (Data)
            {
                Data = new Dictionary<string, TJson>();

                foreach (string file in files)
                {
                    string ext = Files.GetExtension(file);
                    string key = Files.GetName(file, false);
                    
                    if (ext != extension || key.IsNullOrWhiteSpace())
                        continue;

                    TJson json = this.Load(key);

                    Data.Add(key, json);
                }
            }

        }


        public void Save(string key, TJson json)
        {
            if (!Files.IsValidFilename(key))
                return;

            string data = JsonConvert.SerializeObject(json);

            if (Encryption)
                data = AES.Encrypt(data, Password.Release() + key);


            string path = GetPath(key);

            Files.SaveText(path, data, GZip);
        }
        public void Save()
        {
            if (Path == null) throw new Exception("Output directory for JsonDataBase is not specified.");

            lock (Data)
            {
                if (Data == null || Data.Keys == null || Data.Keys.Count <= 0) return;
                string[] list = Data.Keys.ToArray();

                for (int i = 0; i < list.Length; i++)
                {
                    string key = list[i];

                    if (!Files.IsValidFilename(key))
                        continue;

                    this.Save(key, Data[key]);
                }
            }
        }


    }
}
