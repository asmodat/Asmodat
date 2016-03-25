using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using Asmodat.Types;
using System.Drawing;
using Asmodat.Extensions.Windows.Forms;

namespace Asmodat.FormsControls
{


    public partial class ThreadedButton : Button
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
        public new bool Enabled
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Enabled; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Enabled = value; });
            }
        }

        public new Color BackColor
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.BackColor; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.BackColor = value; });
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






    }
}
