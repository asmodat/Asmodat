using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Primitives;

namespace Asmodat.WPFControls
{
    public partial class ThreadedDateTimePicker : DateTimePicker
    {

        public void SetBackgroundToDefault()
        {
            this.TryInvokeMethodAction(() => { base.ClearValue(BackgroundProperty); });
        }

        public new void ClearValue(DependencyProperty dp)
        {
            this.TryInvokeMethodAction(() => { base.ClearValue(dp); });
        }
        public new DateTimePart CurrentDateTimePart
        {
            get { return this.TryInvokeMethodFunction(() => { return base.CurrentDateTimePart; }); }
            set { this.TryInvokeMethodAction(() => { base.CurrentDateTimePart = value; }); }
        }

        public new object DataContext
        {
            get { return this.TryInvokeMethodFunction(() => { return base.DataContext; }); }
            set { this.TryInvokeMethodAction(() => { base.DataContext = value; }); }
        }

        public new DateTime? Value
        {
            get => this.TryInvokeMethodFunction(() => { return base.Value; });
            set => this.TryInvokeMethodAction(() => { base.Value = value; });
        }

        public new DateTimeFormat Format
        {
            get => this.TryInvokeMethodFunction(() => { return base.Format; });
            set => this.TryInvokeMethodAction(() => { base.Format = value; });
        }

        public new string Text
        {
            get => this.TryInvokeMethodFunction(() => { return base.Text; });
            set => this.TryInvokeMethodAction(() => { base.Text = value; });
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

        public ImageBrush ImageBrush
        {
            get { return this.TryInvokeMethodFunction(() => { return (base.Background is ImageBrush) ? (ImageBrush)base.Background : null; }); }
            set { this.TryInvokeMethodAction(() => { base.Background = value; }); }
        }

        /// <summary>
        /// Use example: someControlName.ImageBrushSource = new BitmapImage(new Uri("pack://application:,,,/someImagesFolder/someimage.png"));
        /// </summary>
        public ImageSource ImageBrushSource
        {
            get { return this.TryInvokeMethodFunction(() => { return ((ImageBrush)base.Background)?.ImageSource; });  }
            set { this.TryInvokeMethodAction(() => { if (base.Background is ImageBrush) ((ImageBrush)base.Background).ImageSource = value; });  }
        }

        public new double FontSize
        {
            get { return this.TryInvokeMethodFunction(() => { return base.FontSize; }); }
            set { this.TryInvokeMethodAction(() => { base.FontSize = value; }); }
        }
    }
}
