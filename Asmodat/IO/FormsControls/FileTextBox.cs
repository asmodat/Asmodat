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
    public partial class FileTextBox : TextBox
    {



        private string BaseDirectory
        {
            get
            {
                return Directories.Create(@"Asmodat\IO\FormsControls").FullName;
            }
        }

        public void Initialize(string ID = null)
        {
            this.ID = ID;
        }

        public string ID { get; private set; } = null;

        public bool AutoSave { get; private set; } = false;



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
            else this.Load();

            this.AutoSave = AutoSave;
        }

        

        public string GetText()
        {
            string result = null;

            if (Database.ContainsKey("Text"))
                result = Database.Get<string>("Text");

            return result;
        }


        public void Save()
        {
            Database.Set("Text", this.Text);
            Database.Set("Enabled", this.Enabled);
            Database.Save();
        }

        public void Load()
        {
            if (Database.ContainsKey("Text"))
                this.Text = Database.Get<string>("Text");
            if (Database.ContainsKey("Enabled"))
                this.Enabled = Database.Get<bool>("Enabled");
        }

        public void Reset()
        {
            Database.Reset();
        }
        


        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (AutoSave)
                this.Save();
        }

        /* public string GetText(bool invoked = true)
        {
            string result = null;
            if (invoked && Invoker != null)
                Invoker.Invoke((MethodInvoker)(() =>
                {
                    result = this.GetText(false);
                }));
            else  if(Database.ContainsKey("Text"))
                result = Database.Get<string>("Text");

            return result;
        }*/
    }
}
