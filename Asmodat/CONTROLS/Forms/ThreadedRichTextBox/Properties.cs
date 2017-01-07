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
using Asmodat.Extensions.Windows.Forms;

namespace Asmodat.FormsControls
{
    public partial class ThreadedRichTextBox : RichTextBox
    {
        

        //public Control Invoker { get; private set; }

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

        public new Color SelectionColor
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectionColor; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectionColor = value; });
            }
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

        public new string Text
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Text; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Text = value; });
            }
        }


        public new string Rtf
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Rtf; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Rtf = value; });
            }
        }

        public new int TextLength
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.TextLength; });
            }
        }
    }
}
