using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using Asmodat.Extensions.Windows.Forms;
using Asmodat.IO;
using Asmodat.Cryptography;
using Asmodat.Extensions.Security.Cryptography;

namespace Asmodat.FormsControls 
{
    public partial class ThreadedCheckBox : CheckBox
    {
        public void Initialize()
        {
            if (Autosave_Checked)
                this.LoadProperty("Checked");

            Initialized = true;
            this.CheckedChanged += ThreadedCheckBox_CheckedChanged;
        }

        private void ThreadedCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Initialized && Autosave_Checked)
                this.SaveProperty("Checked");
        }

        private Control _Invoker = null;
        public Control Invoker
        {
            get
            {
                if (_Invoker == null)
                    _Invoker = this.GetFirstParent();
                
                return _Invoker;
            }
        }


        public bool Initialized { get; private set; } = false;
        public bool Autosave_Checked { get; set; } = false;

        public new bool Checked
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Checked; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Checked = value; });
            }
        }



    }
}

