using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using AsmodatMath;
using Asmodat.Extensions.Collections.Generic;
using System.Security.Cryptography;

using Asmodat.Debugging;

using Asmodat.Extensions.Objects;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Asmodat.Extensions.Data.SqlClient
{
    

    public static partial class DbSetEx
    {
        public static bool IsNullOrEmpty<T>(this DbSet<T> o) where T : class
        {
            if (o == null || o.Count() <= 0)
                return true;
            else
                return false;
        }
        
    }
}
