using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Asmodat.WPFControls
{
    public partial class ThreadedLabelledProgressBar : UserControl
    {
        public ThreadedProgressBar ProgressBar { get { return this.PB; } set { this.PB = value; } }

        public double SmallChange
        {
            get { return this.TryInvokeMethodFunction(() => { return PB.SmallChange; }); }
            set { this.TryInvokeMethodAction(() => { PB.SmallChange = value; }); }
        }

        public double LargeChange
        {
            get { return this.TryInvokeMethodFunction(() => { return PB.LargeChange; }); }
            set { this.TryInvokeMethodAction(() => { PB.LargeChange = value; }); }
        }

        public double Maximum
        {
            get { return this.TryInvokeMethodFunction(() => { return PB.Maximum; }); }
            set { this.TryInvokeMethodAction(() => { PB.Maximum = value; }); }
        }

        public double Minimum
        {
            get { return this.TryInvokeMethodFunction(() => { return PB.Minimum; }); }
            set { this.TryInvokeMethodAction(() => { PB.Minimum = value; }); }
        }


    }
}
