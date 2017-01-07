using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using System.Runtime.InteropServices;
using System.Drawing;
using Asmodat.Types;
using Asmodat.Debugging;
using Asmodat.Extensions.Windows.Forms;

namespace Asmodat.FormsControls
{
    public partial class ThreadedTextBox : TextBox
    {

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
        public bool Autosave_Text { get; set; } = false;




        public bool AutoscrollTop { get; set; } = false;
        public bool AutoscrollLeft { get; set; } = false;

        public bool AutoscrollFocusDisable { get; set; } = true;

        public new bool ReadOnly
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.ReadOnly; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.ReadOnly = value; });
            }
        }

        public new string[] Lines
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Lines; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Lines = value; });
            }
        }
        

        public new int SelectionStart
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectionStart; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectionStart = value; });
            }
        }

        public new int SelectionLength
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectionLength; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectionLength = value; });
            }
        }

        public new bool Focus()
        {
            return Invoker.TryInvokeMethodFunction(() => { return base.Focus(); });
        }

        public new void SelectAll()
        {
            if (this.Text.IsNullOrEmpty())
                return;

            this.SelectionStart = 0;
            this.SelectionLength = this.Text.Length;
        }

        public void CarretToEnd()
        {
            if (this.Text.IsNullOrEmpty())
                return;

            this.SelectionStart = this.Text.Length - 1;
            this.SelectionLength = 0;
        }

        public void CarretToStart()
        {
            this.SelectionStart = 0;
            this.SelectionLength = 0;
        }


        public new string SelectedText
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectedText; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectedText = value; });
            }
        }

        ExceptionBuffer Exceptions = new ExceptionBuffer();
        public new string Text
        {
            get { return Invoker.TryInvokeMethodFunction(() => { return base.Text; }); }
            set { Invoker.TryInvokeMethodAction(() => { base.Text = value; }); }
        }

        public new bool Enabled
        {
            get { return Invoker.TryInvokeMethodFunction(() => { return base.Enabled; }); }
            set { Invoker.TryInvokeMethodAction(() => { base.Enabled = value; }); }
        }

        public new Color BackColor
        {
            get { return Invoker.TryInvokeMethodFunction(() => { return base.BackColor; }); }
            set { Invoker.TryInvokeMethodAction(() => { base.BackColor = value; }); }
        }

        public new Color ForeColor
        {
            get { return Invoker.TryInvokeMethodFunction(() => { return base.ForeColor; }); }
            set { Invoker.TryInvokeMethodAction(() => { base.ForeColor = value; }); }
        }

        public int default_Int32 { get; set; } = 0;


        public Int32 GetInt32()
        {
            return Invoker.TryInvokeMethodFunction(() => { return base.Text.TryParse(default_Int32); });
        }

        public void SetInt32(string value)
        {
            Invoker.TryInvokeMethodAction(() => { base.Text = value.ToString(); });
        }
    }
}
