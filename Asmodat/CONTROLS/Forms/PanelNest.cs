using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using Asmodat.Types;

namespace Asmodat.FormsControls
{
    /// <summary>
    /// Panel nest is used to resolve problem of resizing deeply nested controls when their parents are resized
    /// This override can be used for any container 
    /// </summary>
    public class PanelNest : Panel
    {
        protected override void OnSizeChanged(EventArgs e)
        {
            if(this.Handle != null)
            {
                this.BeginInvoke((MethodInvoker)delegate { base.OnSizeChanged(e); });
            }
        }

    }
}
