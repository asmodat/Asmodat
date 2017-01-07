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
    public partial class ThreadedTextBox : TextBox
    {


        public bool EnableKeyControl { get; set; } = false;



        BarrelList<string> KeyControlData = null;

        private void ThreadedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ( !this.EnableKeyControl || !this.Enabled || KeyControlData == null) return;


            if (e.IsEnter())
            {
                if (KeyControlData.Value != this.Text) //save text
                    KeyControlData.Add(this.Text);

                if(OnThreadedEnterKeyDown != null) this.OnThreadedEnterKeyDown(this, e);

                #region used for system sound "ding" fix
                e.Handled = true;
                e.SuppressKeyPress = true;
                #endregion

            }
            else if (e.IsDown())
            {
                if (KeyControlData.PreviousValue != null && !KeyControlData.IsIndexAtStart)
                {
                    this.Text = KeyControlData.ReadPrevious(); //set text to previousvalue
                    this.CarretToEnd();
                }

                if (OnThreadedDownKeyDown != null) this.OnThreadedDownKeyDown(this, e);
            }
            else if (e.IsUp())
            {
                if (KeyControlData.NextValue != null && !KeyControlData.IsIndexAtTheEnd)
                {
                    this.Text = KeyControlData.ReadNext(); //set text tonext value
                    this.CarretToEnd();
                }

                if (OnThreadedUpKeyDown != null) this.OnThreadedUpKeyDown(this, e);
            }
            else if (e.IsDelete()) //Delete text
            {
                this.Text = "";

                if (OnThreadedUpKeyDown != null) this.OnThreadedDeleteKeyDown(this, e);
            }

            if (OnThreadedKeyDown != null) this.OnThreadedKeyDown(this, e);
        }

    }
}