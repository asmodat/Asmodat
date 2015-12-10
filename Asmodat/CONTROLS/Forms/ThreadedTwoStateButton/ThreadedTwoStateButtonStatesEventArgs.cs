using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using Asmodat.Types;

namespace Asmodat.FormsControls
{
    public delegate void ThreadedTwoStateButtonClickStatesEventHandler(object source, ThreadedTwoStateButtonClickStatesEventArgs e);

    public class ThreadedTwoStateButtonClickStatesEventArgs : EventArgs
    {
        public bool On { get; private set; }
        public bool Off { get; private set; }

        public ThreadedTwoStateButtonClickStatesEventArgs(bool On, bool Off)
        {
            this.On = On;
            this.Off = Off;
        }
    }
    
}
