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
        public ThreadedTwoStateButton() : base()
        {
            this.Click += ThreadedTwoStateButton_Click;
        }


        private Control _Invoker = null;
        public Control Invoker
        {
            get
            {
                if (_Invoker == null)
                {
                    _Invoker = this.GetFirstParent();
                    this.UpdateState();
                }

                return _Invoker;
            }
        }

        private void ThreadedTwoStateButton_Click(object sender, EventArgs e)
        {
            ThreadedTwoStateButtonClickStatesEventArgs args = new ThreadedTwoStateButtonClickStatesEventArgs(this.On, this.Off);

            if (this.On && OnClickOn != null) OnClickOn(this, args);
            else if (this.Off && OnClickOff != null) OnClickOff(this, args);

            if (OnClick != null) OnClick(this, args);
        }

        /// <summary>
        /// Updates control state
        /// </summary>
        public void UpdateState()
        {
            if (this.On)
            {
                this.Text = this.TextOn;

                if (EnabledBackColor)
                    this.BackColor = this.BackColorOn;

            }
            else if (this.Off)
            {
                this.Text = this.TextOff;

                if (EnabledBackColor)
                    this.BackColor = this.BackColorOff;
            }
            else
            {
                this.Text = TextNull;

                if (EnabledBackColor)
                    this.BackColor = this.BackColorNull;
            }
        }


        public void SetBackColorStates(Color BackColorOn, Color BackColorOff, Color BackColorNull, bool EnabledBackColor)
        {
            this.EnabledBackColor = EnabledBackColor;
            this.BackColorOn = BackColorOn;
            this.BackColorOff = BackColorOff;
            this.BackColorNull = BackColorNull;
        }

        public void SetTextStates(string TextOn, string TextOff, string TextNull = null)
        {
            this.TextOn = TextOn;
            this.TextOff = TextOff;
            this.TextNull = TextNull;
        }

        private bool _On = false;
        public bool On
        {
            get
            {
                return _On;
            }
            set
            {
                _On = value;
                _Off = !_On;

                this.UpdateState();
            }
        }

        public void Switch()
        {
            if (this.On) this.On = false;
            else if (this.Off) this.Off = false;
        }


        private bool _Off = false;
        public bool Off
        {
            get
            {
                return _Off;
            }
            set
            {
                _Off = value;
                _On = !_Off;

                this.UpdateState();
            }
        }

        


    }
}
