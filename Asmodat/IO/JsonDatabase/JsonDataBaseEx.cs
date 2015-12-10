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

using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.IO
{
    public static partial class JsonDataBaseEx
    {
        public static bool IsNullOrEmpty<TJson>(this JsonDataBase<TJson> source) where TJson : class
        {
            if (source == null || source.Data.IsNullOrEmpty())
                return true;
            else return false;
        }

    }
}
