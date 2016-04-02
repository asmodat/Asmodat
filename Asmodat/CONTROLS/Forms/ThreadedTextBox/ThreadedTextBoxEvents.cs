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
using Asmodat.Extensions.Windows.Forms;
using Asmodat.Types;

namespace Asmodat.FormsControls
{
    public delegate void ThreadedTextBoxEnterKeyDownEventHandler(object source, KeyEventArgs e);
    public delegate void ThreadedTextBoxDownKeyDownEventHandler(object source, KeyEventArgs e);
    public delegate void ThreadedTextBoxUpKeyDownEventHandler(object source, KeyEventArgs e);
    public delegate void ThreadedTextBoxKeyDownEventHandler(object source, KeyEventArgs e);

    public partial class ThreadedTextBox : TextBox
    {
        public event ThreadedTextBoxEnterKeyDownEventHandler OnThreadedEnterKeyDown = null;
        public event ThreadedTextBoxDownKeyDownEventHandler OnThreadedDownKeyDown = null;
        public event ThreadedTextBoxUpKeyDownEventHandler OnThreadedUpKeyDown = null;
        public event ThreadedTextBoxKeyDownEventHandler OnThreadedKeyDown = null;
    }

}
/*

*/
