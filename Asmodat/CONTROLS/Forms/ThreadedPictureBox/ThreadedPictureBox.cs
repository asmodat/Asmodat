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
using Asmodat.Types;

namespace Asmodat.FormsControls
{
    public partial class ThreadedPictureBox : PictureBox
    {

        public void Initialize(Control Invoker)
        {
            this.Invoker = Invoker;
        }

        /// <summary>
        /// By default 50ms / 20fps
        /// </summary>
        public TickTimeout UpdateTimeout { get; set; } = new TickTimeout(50, TickTime.Unit.ms);
       
    }
}
