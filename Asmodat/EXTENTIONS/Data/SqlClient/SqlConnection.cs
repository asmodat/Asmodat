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

namespace Asmodat.Extensions.Data.SqlClient
{
    

    public static partial class SqlConnectionEx
    {
        public static bool IsClosed(this SqlConnection connection) { if (connection != null && connection.State == System.Data.ConnectionState.Closed) return true; else return false; }
        public static bool IsOpen(this SqlConnection connection) { if (connection != null && connection.State == System.Data.ConnectionState.Open) return true; else return false; }
        public static bool IsBroken(this SqlConnection connection) { if (connection != null && connection.State == System.Data.ConnectionState.Broken) return true; else return false; }
        public static bool IsConnecting(this SqlConnection connection) { if (connection != null && connection.State == System.Data.ConnectionState.Connecting) return true; else return false; }
        public static bool IsExecuting(this SqlConnection connection) { if (connection != null && connection.State == System.Data.ConnectionState.Executing) return true; else return false; }
        public static bool IsFetching(this SqlConnection connection) { if (connection != null && connection.State == System.Data.ConnectionState.Fetching) return true; else return false; }

    }
}
