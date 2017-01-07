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
        public static void Delete(string filename)
        {
            string path = Files.GetFullPath(filename);
            string name = Files.GetNameWithoutExtention(path);
            string extention = Files.GetExtension(path);

            if (path == null || extention == null || name == null)
                return;


            string directory = Files.GetDirectory(path);

            Files.Delete(directory + @"\" + name + "_log.ldf");
            Files.Delete(directory + @"\" + name + extention);
        }

        public static void Create(string filename)
        {
            string path = Files.GetFullPath(filename);
            if (path == null)
                return;

            if (Files.Exists(path))
                return;

            string name = Files.GetNameWithoutExtention(path);
            if (name == null)
                return;

            using (SqlConnection connection = new SqlConnection("Data Source = (LocalDB)\\v11.0; Initial Catalog = master; Integrated Security = true;"))
            {
                connection.Open();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = String.Format("CREATE DATABASE {0} ON PRIMARY (NAME={0}, FILENAME='{1}')", name, path);
                    command.ExecuteNonQuery();

                    command.CommandText = String.Format("EXEC sp_detach_db '{0}', 'true'", name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static SqlConnection Connect(string filename)
        {
            string path = Files.GetFullPath(filename);
            if (path == null)
                return null;

            string name = Files.GetNameWithoutExtention(path);
            if (name == null)
                return null;

            //to remove - delete folowing files:
            //name_log.ldf
            //name.mdf 

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = string.Format("Data Source = (LocalDB)\\v11.0; AttachDbFilename={0}; Integrated Security = true; Connect Timeout=30;", path);
            connection.Open();

            return connection;
        }


        public static bool CommandExecuteNonQuery(SqlConnection con, string text)
        {
            if (!con.IsOpen())
                return false;

            try
            {
                using (SqlCommand cmd = new SqlCommand(text, con))
                    cmd.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return false;
            }
        }

        

        


        public static bool ContainsTable(SqlConnection con, string table_name)
        {
            if (!con.IsOpen())
                return false;

            try
            {
                string command = string.Format(
                    "IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='{0}') SELECT 1 ELSE SELECT 0", table_name);

                using (SqlCommand cmd = new SqlCommand(command, con))
                {
                    int x = Convert.ToInt32(cmd.ExecuteScalar());
                    if (x == 1)
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

        

        public static bool CreateTable(SqlConnection con, string table_name)
        {
            return CommandExecuteNonQuery(con, "CREATE TABLE " + table_name + " (Id TEXT) ");
        }

        public static bool ClearTable(SqlConnection con, string table_name)
        {
            return CommandExecuteNonQuery(con, "DELETE FROM " + table_name + "");
        }

        /// <summary>
        /// Removes table incluting its data, indexes, triggers, constrains and permisions 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public static bool DropTable(SqlConnection con, string table_name)
        {
            return CommandExecuteNonQuery(con, "DROP TABLE " + table_name);
        }


        

        


        public static bool InsertValue(SqlConnection con, string table_name, string id, string column_name, string value)
        {
            if (!con.IsOpen())
                return false;

            try
            {

                string command = string.Format(
                    "UPDATE {0} SET {1}=@val1 WHERE Id LIKE @Id", table_name,column_name);
                //"UPDATE TEST SET Name='Bogdan' WHERE Id=i1"
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = command;

                    if (value == null)
                        cmd.CommandText = cmd.CommandText.Replace("@val1", "NULL");
                    else
                        cmd.Parameters.AddWithValue("@val1", value);

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

        public static string ReadValue(SqlConnection con, string table_name, string id, string column_name)
        {
            if (!con.IsOpen())
                return null;

            try
            {

                string command = string.Format(
                    "SELECT {0} FROM {1} WHERE Id LIKE @Id", column_name, table_name);
                //"UPDATE TEST SET Name='Bogdan' WHERE Id=i1"
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = command;
                    cmd.Parameters.AddWithValue("@Id", id);

                    var exr = cmd.ExecuteReader();

                    if (exr == null || exr.FieldCount <= 0)
                        return null;

                    if (exr.Read())
                        return exr.GetValue(0).ToString();
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return null;
            }
        }

    }
}
