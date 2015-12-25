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
