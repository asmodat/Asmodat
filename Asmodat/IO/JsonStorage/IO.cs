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
    public partial class JsonStorage<TJson> : IDisposable
    {
        public void Dispose()
        {

        }

        public void Delete()
        {
            if (!Files.IsValidFilename(this.Key))
                return;

            string path = GetPath(this.Key);

            Files.Delete(path);
        }
        

        public void Load()
        {
            if (!Files.IsValidFilename(this.Key))
                return;

            string path = GetPath(this.Key);

            string data = Files.LoadText(path, GZip);

            

            if (Encryption)
            {
                string pass = Password.Release() + this.Key;
                try
                {
                    data = AES.Decrypt(data, pass);
                }
                catch
                {
                    return;
                }
            }

            if(data == null)
            {
                Data = null;
                return;
            }

            Data = JsonConvert.DeserializeObject<TJson>(data);
        }



        public void Save()
        {
            if (!Files.IsValidFilename(this.Key))
                return;

            string data = JsonConvert.SerializeObject(Data);

            if (Encryption)
                data = AES.Encrypt(data, Password.Release() + this.Key);


            string path = GetPath(this.Key);

            Files.SaveText(path, data, GZip);
        }
   


    }
}
