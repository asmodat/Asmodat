using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using Asmodat.Types;
using System.Drawing;

namespace Asmodat.FormsControls
{
    public partial class ThreadedTwoStateButton : Button
    {
        public string TextNull { get; set; } = "";
        public string TextOn { get; set; }
        public string TextOff { get; set; }

        public bool EnabledBackColor { get; set; } = false;
        public Color BackColorNull { get; set; }
        public Color BackColorOn { get; set; }
        public Color BackColorOff { get; set; }




        public event ThreadedTwoStateButtonClickStatesEventHandler OnClickOn = null;
        public event ThreadedTwoStateButtonClickStatesEventHandler OnClickOff = null;
        public new event ThreadedTwoStateButtonClickStatesEventHandler OnClick = null;

        public Control Invoker { get; private set; }

        public new bool Enabled
        {
            get
            {
                bool var = false;
                if (Invoker == null) return base.Enabled;
                else
                    Invoker.Invoke((MethodInvoker)(() =>
                    {
                        var = base.Enabled;
                    }));

                return var;
            }
            set
            {
                if (Invoker == null || !Invoker.IsHandleCreated) base.Enabled = value;
                else
                    Invoker.Invoke((MethodInvoker)(() =>
                    {
                        base.Enabled = value;
                    }));
            }
        }

        public new Color BackColor
        {
            get
            {
                if (Invoker == null) return base.BackColor;

                Color var = new Color();

                Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.BackColor;
                }));

                return var;
            }
            set
            {
                if (Invoker == null || !Invoker.IsHandleCreated) base.BackColor = value;
                else
                    Invoker.Invoke((MethodInvoker)(() =>
                    {
                        base.BackColor = value;
                    }));
            }
        }


        public new string Text
        {
            get
            {
                if (Invoker == null) return base.Text;

                string var = null;

                Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.Text;
                }));

                return var;
            }
            set
            {
                if (Invoker == null || !Invoker.IsHandleCreated) base.Text = value;
                else
                    Invoker.Invoke((MethodInvoker)(() =>
                    {
                        base.Text = value;
                    }));
            }
        }

        

    }
}
