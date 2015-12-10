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

namespace Asmodat.IO
{
    public partial class JsonStorage<TJson> : IDisposable
    {


        private void InitializePassword(SecureString Password = null)
        {
            this.Password = new SecureString();

            if (Password == null)
            {
                this.Password.AppendChar('t');
                this.Password.AppendChar('Q');
                this.Password.AppendChar('X');
                this.Password.AppendChar('o');
                this.Password.AppendChar('S');
                this.Password.AppendChar(')');
                this.Password.AppendChar('L');
                this.Password.AppendChar('g');
                this.Password.AppendChar('?');
                this.Password.AppendChar('7');
            }
            else this.Password = Password;
        }


        public const string extension = @".ajs";

        /// <summary>
        /// Path to database Directory
        /// </summary>
        public string Path { get; private set; }// = Directories.Current + @"\Asmodat\IO\JsonDataBase";

        /// <summary>
        /// Returns path to file based on filename
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string GetPath(string filename)
        {
            return Path + @"\" + filename;
        }

        /// <summary>
        /// Indicates if files should be zipped before saving, and uniped before loading
        /// </summary>
        public bool GZip { get; private set; } = true;

        /// <summary>
        /// Indicates if files should be encrypted before saving and decrypted before loading
        /// </summary>
        public bool Encryption { get; private set; } = true;

        /// <summary>
        /// Password used for encryption
        /// </summary>
        public SecureString Password { get; private set; }

        /// <summary>
        /// Encription class
        /// </summary>
        private AES256 AES { get; set; } = new AES256();



    }
}
