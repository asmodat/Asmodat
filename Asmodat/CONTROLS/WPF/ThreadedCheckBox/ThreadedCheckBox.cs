using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Asmodat.WPFControls
{
    public partial class ThreadedCheckBox : CheckBox
    {
        public new bool? IsChecked
        {
            get { return this.TryInvokeMethodFunction(() => { return base.IsChecked; }); }
            set { this.TryInvokeMethodAction(() => { base.IsChecked = value; }); }
        }

        public new object Content
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Content; }); }
            set { this.TryInvokeMethodAction(() => { base.Content = value; }); }
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
