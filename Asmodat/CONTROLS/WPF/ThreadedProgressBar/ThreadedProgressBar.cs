using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Asmodat.WPFControls
{
    public partial class ThreadedProgressBar : ProgressBar
    {

        public new double SmallChange
        {
            get { return this.TryInvokeMethodFunction(() => { return base.SmallChange; }); }
            set { this.TryInvokeMethodAction(() => { base.SmallChange = value; }); }
        }

        public new double LargeChange
        {
            get { return this.TryInvokeMethodFunction(() => { return base.LargeChange; }); }
            set { this.TryInvokeMethodAction(() => { base.LargeChange = value; }); }
        }

        public new Thickness Margin
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Margin; }); }
            set { this.TryInvokeMethodAction(() => { base.Margin = value; }); }
        }

        public new double Maximum
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Maximum; }); }
            set { this.TryInvokeMethodAction(() => { base.Maximum = value; }); }
        }

        public new double Minimum
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Minimum; }); }
            set { this.TryInvokeMethodAction(() => { base.Minimum = value; }); }
        }

        public new string Name
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Name; }); }
            set { this.TryInvokeMethodAction(() => { base.Name = value; }); }
        }

        public new double Opacity
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Opacity; }); }
            set { this.TryInvokeMethodAction(() => { base.Opacity = value; }); }
        }

        public new Visibility Visibility
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Visibility; }); }
            set { this.TryInvokeMethodAction(() => { base.Visibility = value; }); }
        }

        public new double Value
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Value; }); }
            set { this.TryInvokeMethodAction(() => { base.Value = value; }); }
        }

        public new bool IsEnabled
        {
            get { return this.TryInvokeMethodFunction(() => { return base.IsEnabled; }); }
            set { this.TryInvokeMethodAction(() => { base.IsEnabled = value; }); }
        }

        public new Brush Background
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Background; }); }
            set { this.TryInvokeMethodAction(() => { base.Background = value; }); }
        }

        public new double FontSize
        {
            get { return this.TryInvokeMethodFunction(() => { return base.FontSize; }); }
            set { this.TryInvokeMethodAction(() => { base.FontSize = value; }); }
        }
    }
}
