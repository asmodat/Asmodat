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
    public partial class ThreadedTabControl : TabControl
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

        public new int SelectedIndex
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectedIndex; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectedIndex = value; });
            }
        }

        public new TabPage SelectedTab
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectedTab; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectedTab = value; });
            }
        }
    }
}
