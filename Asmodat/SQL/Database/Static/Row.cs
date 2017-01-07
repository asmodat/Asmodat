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
        /// <summary>
        /// SQL Injection-safe row append
        /// </summary>
        /// <param name="con"></param>
        /// <param name="table_name"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool AddRow(SqlConnection con, string table_name, string id)
        {
            bool exist = ContainsRow(con, table_name, id);


            if (exist)
                return true;
            else if (!con.IsOpen())
                return false;

            try
            {

                string command = string.Format(
                    "INSERT INTO {0} (Id) VALUES (@val1)", table_name);

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = command;
                    cmd.Parameters.AddWithValue("@val1", id);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return false;
            }
        }


        public static bool ContainsRow(SqlConnection con, string table_name, string id)
        {
            if (!con.IsOpen())
                return false;

            try
            {
                string command = string.Format(
                    "SELECT COUNT(*) FROM {0} WHERE Id LIKE @val1", table_name);

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = command;
                    cmd.Parameters.AddWithValue("@val1", id);

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

        public static bool DeleteRow(SqlConnection con, string table_name, string id)
        {
            if (!con.IsOpen())
                return false;

            try
            {

                string command = string.Format(
                    "DELETE FROM {0} WHERE Id LIKE @Id", table_name);
                //"UPDATE TEST SET Name='Bogdan' WHERE Id=i1"
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = command;
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return false;
            }
        }
    }
}
