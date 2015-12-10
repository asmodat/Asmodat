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
using Newtonsoft.Json;
using System.Security;

using Asmodat.Cryptography;
using System.Collections;

namespace Asmodat.IO
{
    public partial class JsonDataBase<TJson> : IEnumerable<KeyValuePair<string, TJson>>, IDisposable where TJson : class
    {
        private class JsonDataBaseEnumerator : IEnumerator<KeyValuePair<string, TJson>>
        {
            private int position = -1;
            private JsonDataBase<TJson> DataBase;

            public JsonDataBaseEnumerator(JsonDataBase<TJson> DataBase)
            {
                this.DataBase = DataBase;
            }

            public bool MoveNext()
            {
                if (position < this.DataBase.Count - 1)
                {
                    position++;
                    return true;
                }
                else return false;
            }

            public void Reset()
            {
                position = -1;
            }

            public void Dispose()
            {
                if (DataBase != null)
                    DataBase.Dispose();
            }

            public object Current
            {
                get
                {
                    return DataBase.Data.ToArray()[position];
                }
            }


            KeyValuePair<string, TJson> IEnumerator<KeyValuePair<string, TJson>>.Current
            {
                get
                {
                    return (KeyValuePair<string, TJson>)this.Current;
                }
            }
        }


        public IEnumerator<KeyValuePair<string, TJson>> GetEnumerator()
        {
            return new JsonDataBaseEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
