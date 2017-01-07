using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Windows.Forms;
using Asmodat.Debugging;
using Asmodat.IO;
using Asmodat.Extensions.Security.Cryptography;
using Newtonsoft.Json;
using Asmodat.SQL;

namespace Asmodat.Extensions.Windows.Forms
{


    public  static partial class ControlEx
    {

        public static void SaveProperty(this Control control, string propertyName)
        {
            if (control == null)
                return;
            
            string directory = Directories.Create(@"Asmodat\Extensions.Windows.Forms.ControlEx").FullName;
            string fileName = Files.RemoveInvalidFilenameCharacters(control.GetFullPathName());
            string path = directory + @"\" + fileName + ".adbs";
          
            DatabseSimpleton dbs = new DatabseSimpleton(path, false);

            object value = control.GetType().GetProperty(propertyName).GetValue(control, null);
            dbs.Set(propertyName, value);
            dbs.Save();
        }

        public static void LoadProperty(this Control control, string propertyName)
        {
            if (control == null)
                return;

            string directory = Directories.Create(@"Asmodat\Extensions.Windows.Forms.ControlEx").FullName;
            string fileName = Files.RemoveInvalidFilenameCharacters(control.GetFullPathName());
            string path = directory + @"\" + fileName + ".adbs";

            if (!Files.Exists(path))
                return;

            DatabseSimpleton dbs = new DatabseSimpleton(path, false);
            object value = dbs.Get<object>(propertyName);
            control.GetType().GetProperty(propertyName).SetValue(control, value);
        }


        public static void SavePropertySQL(this Control control, string propertyName)
        {
            if (control == null)
                return;

            string name = Files.RemoveInvalidFilenameCharacters(control.GetFullPathName());
            object value = control.GetType().GetProperty(propertyName).GetValue(control, null);
            string data = JsonConvert.SerializeObject(value);


            Database DB = new Database("Asmodath.mdf", "Properties");
            
            DB.AddColumn("ID");
            DB.AddColumn("Value");
            DB.AddRow(name);
            DB.InsertValue(name, "Value", data);

            DB.Close();
        }

        public static void LoadPropertySQL(this Control control, string propertyName)
        {
            if (control == null)
                return;

            string name = Files.RemoveInvalidFilenameCharacters(control.GetFullPathName());

            Database DB = new Database("Asmodath.mdf", "Properties");


            if (!DB.ContainsColumn("Value"))
                return;

            string data = DB.ReadValue(name, "Value");
            object value = null;

            if (data == null)
                return;
            else
                value = JsonConvert.DeserializeObject<object>(data);

            control.GetType().GetProperty(propertyName).SetValue(control, value);
        }


    }
}
