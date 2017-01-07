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
        public string Path { get; } = null;
        public string TableName { get; set; } = null;
        public SqlConnection Connection { get; private set; } = null;


        public bool IsOpen() { return Connection.IsOpen(); }
        public bool IsClosed() { return Connection.IsClosed(); }




        public Database(string path)
        {
            this.Path = Files.GetFullPath(path);
            Database.Create(Path);

            this.Connection = Database.Connect(Path);
        }

        public Database(string path, string tableName)
        {
            this.Path = Files.GetFullPath(path);
            Database.Create(Path);

            this.Connection = Database.Connect(Path);

            this.TableName = tableName;

            this.CreateTable(this.TableName);
        }


        public bool Close()
        {
            if (this.IsClosed())
                return true;

             this.Connection.Close();

            if (this.IsClosed())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Ctreates table with 'Id' TEXT column 
        /// </summary>
        /// <param name="table_name"></param>
        /// <returns></returns>
        public bool CreateTable(string table_name)
        {
            if (ContainsTable(table_name))
                return true;

            return Database.CreateTable(this.Connection, table_name);
        }

        public bool ContainsTable(string table_name)
        {
            return Database.ContainsTable(this.Connection, table_name);
        }

        public bool ContainsRow(string table_name, string id)
        {
            return Database.ContainsRow(this.Connection, table_name, id);
        }

        public bool ContainsRow(string id)
        {
            return Database.ContainsRow(this.Connection, this.TableName, id);
        }
        public bool ContainsColumn(string table_name, string column_name)
        {
            return Database.ContainsColumn(this.Connection, table_name, column_name);
        }
        public bool ContainsColumn(string column_name)
        {
            return Database.ContainsColumn(this.Connection, this.TableName, column_name);
        }
        public bool DropTable(string table_name)
        {
            return Database.DropTable(this.Connection, table_name);
        }

        
        public bool AddColumn(string table_name, string column_name)
        {
            return Database.AddColumn(this.Connection, table_name, column_name, "TEXT");
        }

        public bool AddColumn(string column_name)
        {
            return Database.AddColumn(this.Connection, this.TableName, column_name, "TEXT");
        }

        public bool DeleteColumn(string table_name, string column_name)
        {
            return Database.DeleteColumn(this.Connection, table_name, column_name);
        }

        public bool AddRow(string table_name, string id)
        {
            return Database.AddRow(this.Connection, table_name, id);
        }

        public bool AddRow(string id)
        {
            return Database.AddRow(this.Connection, this.TableName, id);
        }

        public bool DeleteRow(string table_name, string id)
        {
            return Database.DeleteRow(this.Connection, table_name, id);
        }

        public bool InsertValue(string table_name, string id, string column_name, string value)
        {
            if (!ContainsColumn(table_name, column_name)) this.AddColumn(table_name, column_name);
            if (!ContainsRow(table_name, id)) this.AddRow(table_name, id);
            

            return Database.InsertValue(this.Connection, table_name, id, column_name, value);
        }

        public bool InsertValue(string id, string column_name, string value)
        {
            if (!ContainsColumn(column_name)) this.AddColumn(column_name);
            if (!ContainsRow(id)) this.AddRow(id);
            

            return Database.InsertValue(this.Connection, this.TableName, id, column_name, value);
        }

        public string ReadValue(string table_name, string id, string column_name)
        {
            return Database.ReadValue(this.Connection, table_name, id, column_name);
        }

        public string ReadValue(string id, string column_name)
        {
            return Database.ReadValue(this.Connection, this.TableName, id, column_name);
        }
    }
}
