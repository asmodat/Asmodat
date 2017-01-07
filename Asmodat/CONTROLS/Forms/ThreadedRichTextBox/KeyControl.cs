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


        public bool EnableKeyControl { get; set; } = false;
       

        
        private void ThreadedTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ( !this.EnableKeyControl || !this.Enabled) return;
            
            if (e.IsDelete()) //Delete text
            {
                this.Text = "";

                if (OnThreadedUpKeyDown != null) this.OnThreadedDeleteKeyDown(this, e);
            }

            if (OnThreadedKeyDown != null) this.OnThreadedKeyDown(this, e);
        }

    }
}