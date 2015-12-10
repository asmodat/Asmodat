using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.Windows.Forms;
using System.IO;

namespace Asmodat.IO
{
    public partial class FileCheckBox : CheckBox
    {
        private string BaseDirectory
        {   get
            {
                return Directories.Create(@"Asmodat\IO\FormsControls").FullName;
            }
        }

        public string ID { get; private set; } = null;

        public bool AutoSave { get; private set; } = false;

        public void Reset()
        {
            Database.Reset();
        }

        private DatabseSimpleton Database;
        //public Control Invoker { get; set; } = null;

        /// <summary>
        /// if path is null, data is saved as @"Asmodat\IO\FormsControls\" + base.Name + ".cds"
        /// </summary>
        /// <param name="AutoSave"></param>
        /// <param name="path"></param>
        public void InitializeDatabase(bool AutoSave, string ID, string path)//, Control invoker)
        {
            //if (invoker != null) this.Invoker = invoker;

            if (path == null)
                path = Files.GetFullPath(string.Format("{0}\\{1}{2}.{3}", BaseDirectory, base.Name, ID, "cds"));
            else
            {
                if (!path.Contains(":"))
                    path = BaseDirectory + "\\" + path;

                path = Files.GetFullPath(path);
            }

            Database = new DatabseSimpleton(path, false);

            if (Database.Count <= 0)
                this.Save();
            
             this.Load();

            this.AutoSave = AutoSave;
        }

        

        public bool GetChecked()
        {
            bool result = false;

            if (Database.ContainsKey("Checked"))
                result = Database.Get<bool>("Checked");

            return result;
        }

        public void Save()
        {
            Database.Set("Checked", this.Checked);
            Database.Set("Enabled", this.Enabled);
            Database.Save();
        }

        public void Load()
        {
            if (Database.ContainsKey("Checked"))
                this.Checked = Database.Get<bool>("Checked");
            if (Database.ContainsKey("Enabled"))
                this.Enabled = Database.Get<bool>("Enabled");
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);

            if (AutoSave)
                this.Save();
        }

        
    }
}
