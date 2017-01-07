using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Windows.Controls;
using System.ComponentModel;
using Asmodat;
using System.Runtime.InteropServices;
using System.Drawing;
using Asmodat.Types;
using Asmodat.Debugging;

using Asmodat.Extensions.Windows.Controls;

namespace Asmodat.WPFControls
{
    public partial class ThreadedTabControl : TabControl
    {
        public new int SelectedIndex
        {
            get { return this.TryInvokeMethodFunction(() => { return base.SelectedIndex; }); }
            set { this.TryInvokeMethodAction(() => { base.SelectedIndex = value; }); }
        }

        public new int TabIndex
        {
            get { return this.TryInvokeMethodFunction(() => { return base.TabIndex; }); }
            set { this.TryInvokeMethodAction(() => { base.TabIndex = value; }); }
        }
    }
}
