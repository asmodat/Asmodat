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
    public partial class ThreadedRichTextBox : RichTextBox
    {
        //public event KeyDownEventHandler OnThreadedEnterKeyDown = null;
        //public event KeyDownEventHandler OnThreadedDownKeyDown = null;
        public event KeyDownEventHandler OnThreadedUpKeyDown = null;
        public event KeyDownEventHandler OnThreadedKeyDown = null;
        public event KeyDownEventHandler OnThreadedDeleteKeyDown = null;
    }
}
