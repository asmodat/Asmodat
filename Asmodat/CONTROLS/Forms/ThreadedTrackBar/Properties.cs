using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using System.Runtime.InteropServices;
using System.Drawing;
using Asmodat.Types;

namespace Asmodat.FormsControls
{
    public partial class ThreadedTrackBar : TrackBar
    {

        public Control Invoker { get; private set; }


        


        public new int Value
        {
            get
            {
                if (Invoker == null || !this.IsHandleCreated) return base.Value;

                int var = 0;

                try
                {
                    Invoker.Invoke((MethodInvoker)(() =>
                    {
                        var = base.Value;
                    }));
                }
                catch
                {
                    return 0;
                }

                return var;
            }
            set
            {
                if (Invoker == null || !this.IsHandleCreated) base.Value = value;
                else
                    try
                    {
                        Invoker.Invoke((MethodInvoker)(() =>
                        {

                            base.Value = value;

                        }));
                    }
                    catch
                    {
                    }

            }
        }
    }
}
