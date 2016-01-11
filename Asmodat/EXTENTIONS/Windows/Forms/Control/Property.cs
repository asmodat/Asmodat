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


    }
}
