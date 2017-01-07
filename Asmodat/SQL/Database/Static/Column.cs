using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Extensions.Collections.Generic;
using Asmodat.IO;
using System.Data.SqlClient;
using Asmodat.Extensions.Data.SqlClient;

using Asmodat.Debugging;

namespace Asmodat.SQL
{
    public partial class Database
    {
        public static bool ContainsColumn(SqlConnection con, string table_name, string column_name)
        {
            if (!con.IsOpen())
                return false;

            try
            {
                string command = string.Format(
                    "SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @tableName AND COLUMN_NAME = @columnName");

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = command;
                    cmd.Parameters.AddWithValue("@tableName", table_name);
                    cmd.Parameters.AddWithValue("@columnName", column_name);

                    var obj = cmd.ExecuteScalar();

                    if (obj != null && (int)obj > 0)
                        return true;
                    else
                        return false;

                }
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return false;
            }
        }

        public static bool AddColumn(SqlConnection con, string table_name, string column_name, string column_type)
        {
            bool exist = ContainsColumn(con, table_name, column_name);

            if (exist)
                return true;
            else if (!con.IsOpen())
                return false;

            return CommandExecuteNonQuery(con, string.Format("ALTER TABLE {0} ADD {1} {2}", table_name, column_name, column_type));
        }

        public static bool DeleteColumn(SqlConnection con, string table_name, string column_name)
        {
            bool exist = ContainsColumn(con, table_name, column_name);

            if (!con.IsOpen())
                return false;
            else if (!exist)
                return true;

            return CommandExecuteNonQuery(con, string.Format("ALTER TABLE {0} DROP COLUMN {1}", table_name, column_name));
        }
    }
}
