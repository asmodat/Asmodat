using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System;

namespace Asmodat.WPFControls
{
    public partial class ThreadedButton : Button
    {

        public void SetBackgroundToDefault()
        {
            this.ClearValue(BackgroundProperty);
        }

        public new void ClearValue(DependencyProperty dp)
        {
            this.TryInvokeMethodAction(() => { base.ClearValue(dp); });
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
