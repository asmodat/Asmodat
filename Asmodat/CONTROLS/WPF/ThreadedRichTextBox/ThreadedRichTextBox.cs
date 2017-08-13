using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace Asmodat.WPFControls
{
    public partial class ThreadedRichTextBox : System.Windows.Controls.RichTextBox
    {
        public void WriteLine(string text) => WriteLine(text, Brushes.Black);

        public void WriteLine(string text, Brush brush)
        {
            this.TryInvokeMethodAction(() => 
            {
                var pntr = base.Document.ContentEnd;
                TextRange tr = new TextRange(pntr, pntr);
                tr.Text = $"{text}\r\n";
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            });
        }

        public new void ScrollToEnd() => this.TryInvokeMethodAction(() => base.ScrollToEnd());
        public new void ScrollToHome() => this.TryInvokeMethodAction(() => base.ScrollToHome());
        public new void ScrollToHorizontalOffset(double offset) => this.TryInvokeMethodAction(() => base.ScrollToHorizontalOffset(offset));
        public new void ScrollToVerticalOffset(double offset) => this.TryInvokeMethodAction(() => base.ScrollToVerticalOffset(offset));

        public new object DataContext
        {
            get { return this.TryInvokeMethodFunction(() => { return base.DataContext; }); }
            set { this.TryInvokeMethodAction(() => { base.DataContext = value; }); }
        }

        public new bool IsEnabled
        {
            get { return this.TryInvokeMethodFunction(() => { return base.IsEnabled; }); }
            set { this.TryInvokeMethodAction(() => { base.IsEnabled = value; }); }
        }

        public new bool IsArrangeValid
        {
            get => this.TryInvokeMethodFunction(() => { return base.IsArrangeValid; });
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

        public new Thickness Padding
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Padding; }); }
            set { this.TryInvokeMethodAction(() => { base.Padding = value; }); }
        }
        
        public new double Width
        {
            get => this.TryInvokeMethodFunction(() => { return base.Width; });
            set => this.TryInvokeMethodAction(() => { base.Width = value; });
        }

        public new double Height
        {
            get => this.TryInvokeMethodFunction(() => { return base.Height; });
            set => this.TryInvokeMethodAction(() => { base.Height = value; });
        }

        public new VerticalAlignment VerticalAlignment
        {
            get => this.TryInvokeMethodFunction(() => { return base.VerticalAlignment; });
            set => this.TryInvokeMethodAction(() => { base.VerticalAlignment = value; });
        }

        public new HorizontalAlignment HorizontalAlignment
        {
            get => this.TryInvokeMethodFunction(() => { return base.HorizontalAlignment; });
            set => this.TryInvokeMethodAction(() => { base.HorizontalAlignment = value; });
        }

        public new VerticalAlignment VerticalContentAlignment
        {
            get => this.TryInvokeMethodFunction(() => { return base.VerticalContentAlignment; });
            set => this.TryInvokeMethodAction(() => { base.VerticalContentAlignment = value; });
        }

        public new HorizontalAlignment HorizontalContentAlignment
        {
            get => this.TryInvokeMethodFunction(() => { return base.HorizontalContentAlignment; });
            set => this.TryInvokeMethodAction(() => { base.HorizontalContentAlignment = value; });
        }
    }
}
