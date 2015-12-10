using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
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
    public partial class JsonStorage<TJson> where TJson : class
    {
        public string Key { get; private set; } //KeyToDataContent

        public JsonStorage(string directory, string name, bool GZip, bool Encryption, SecureString Password = null) 
        {
            if (Files.HasExtension(name))
                name = name + extension;

            Path = Directories.Create(directory).FullName;
            this.Key = name;

            this.GZip = GZip;
            this.Encryption = Encryption;
            this.InitializePassword(Password);
            

            this.Load();

        }

        public TJson Data { get; private set; }

        
        public TJson Get()
        {
            return Data;
        }

        public void Set(TJson data, bool save = true)
        {
            this.Data = data;
            if (save)
                this.Save();
        }
    }
}
