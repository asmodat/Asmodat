using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using Asmodat.Types;

namespace Asmodat.FormsControls
{
    public class ThreeStateButton : Button
    {

        public string TextTrue { get; set; }
        public string TextFalse { get; set; }
        public string TextNull { get; set; }

        public void SetText(string TextTrue, string TextFalse, string TextNull)
        {
            this.TextTrue = TextTrue;
            this.TextFalse = TextFalse;
            this.TextNull = TextNull;
        }



        private ThreeState _State = ThreeState.Null;
        public ThreeState State
        {
            get
            {
                return _State;
            }
            set
            {
                if (State.IsNull)
                {
                    Text = TextNull;
                    Enabled = false;
                }
                else if (State.IsTrue)
                {
                    Text = TextTrue;
                    Enabled = true;
                }
                else if (State.IsFalse)
                {
                    Text = TextFalse;
                    Enabled = true;
                }
            }
        }


    }
}
