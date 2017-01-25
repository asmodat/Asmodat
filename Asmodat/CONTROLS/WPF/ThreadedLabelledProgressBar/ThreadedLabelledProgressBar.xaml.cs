using Asmodat.Extensions.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asmodat.WPFControls
{
    /// <summary>
    /// Interaction logic for ThreadedProgressBarText.xaml
    /// </summary>
    public partial class ThreadedLabelledProgressBar : UserControl
    {
        public ThreadedLabelledProgressBar()
        {
            InitializeComponent();
        }

        public new bool IsEnabled
        {
            get { return this.TryInvokeMethodFunction(() => { return base.IsEnabled; }); }
            set { this.TryInvokeMethodAction(() => { base.IsEnabled = value; PB.IsEnabled = value; TL.IsEnabled = value; }); }
        }

        public new double Width
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Width; }); }
            set { this.TryInvokeMethodAction(() => { base.Width = value; PB.Width = value; TL.Width = value; }); }
        }

        public new double Height
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Height; }); }
            set { this.TryInvokeMethodAction(() => { base.Height = value; PB.Height = value; TL.Height = value; }); }
        }

        public new double MaxWidth
        {
            get { return this.TryInvokeMethodFunction(() => { return base.MaxWidth; }); }
            set { this.TryInvokeMethodAction(() => { base.MaxWidth = value; PB.MaxWidth = value; TL.MaxWidth = value; }); }
        }

        public new double MaxHeight
        {
            get { return this.TryInvokeMethodFunction(() => { return base.MaxHeight; }); }
            set { this.TryInvokeMethodAction(() => { base.MaxHeight = value; PB.MaxHeight = value; TL.MaxHeight = value; }); }
        }

        public new double MinHeight
        {
            get { return this.TryInvokeMethodFunction(() => { return base.MinHeight; }); }
            set { this.TryInvokeMethodAction(() => { base.MinHeight = value; PB.MinHeight = value; TL.MinHeight = value; }); }
        }

        public new double MinWidth
        {
            get { return this.TryInvokeMethodFunction(() => { return base.MinWidth; }); }
            set { this.TryInvokeMethodAction(() => { base.MinWidth = value; PB.MinWidth = value; TL.MinWidth = value; }); }
        }
    }
}
