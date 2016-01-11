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
    public partial class ThreadedTrackBar : TrackBar
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

        /*
        public Control Invoker { get; private set; }
        */



        public new int Value
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Value; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Value = value; });
            }
        }
        
    }
}
