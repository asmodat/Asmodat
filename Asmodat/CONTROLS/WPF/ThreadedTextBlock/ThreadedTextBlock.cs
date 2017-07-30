using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace Asmodat.WPFControls
{
    public partial class ThreadedTextBlock : System.Windows.Controls.TextBlock
    {
        public void SetText(string text) => Text = text;

        public void WriteLine(string text) => WriteLine(text, Brushes.Black);

        public void WriteLine(string text, Brush brush)
        {
            var run = new Run(text + "\r\n");
            run.Foreground = brush;
            this.TryInvokeMethodAction(() => { base.Inlines.Add(run); });
        }

        public new InlineCollection Inlines
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Inlines; }); }
        }

        public new string Text
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Text; }); }
            set { this.TryInvokeMethodAction(() => { base.Text = value; }); }
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

        public new TextAlignment TextAlignment
        {
            get { return this.TryInvokeMethodFunction(() => { return base.TextAlignment; }); }
            set { this.TryInvokeMethodAction(() => { base.TextAlignment = value; }); }
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
            get { return this.TryInvokeMethodFunction(() => { return base.VerticalAlignment; }); }
            set { this.TryInvokeMethodAction(() => { base.VerticalAlignment = value; }); }
        }

        public new HorizontalAlignment HorizontalAlignment
        {
            get => this.TryInvokeMethodFunction(() => { return base.HorizontalAlignment; });
            set => this.TryInvokeMethodAction(() => { base.HorizontalAlignment = value; });
        }
    }
}
