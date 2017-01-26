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
    /// Interaction logic for ThreadedPasswordTextBox.xaml
    /// </summary>
    public partial class ThreadedPasswordTextBox : UserControl
    {
        public ThreadedPasswordTextBox()
        {
            InitializeComponent();
            TB.TextChanged += TB_TextChanged;
            PB.PasswordChanged += PB_PasswordChanged;
        }

        private void PB_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(TB.Text != PB.Password)
                this.TryInvokeMethodAction(() => { TB.Text = PB.Password; });
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TB.Text != PB.Password)
                this.TryInvokeMethodAction(() => { PB.Password = TB.Text; });
        }

        public bool PasswordVisible
        {
            get { return this.TryInvokeMethodFunction(() => { return (TB.Visibility == Visibility.Visible); }); }
            set { this.TryInvokeMethodAction(() => { if (value) { TB.Visibility = Visibility.Visible; PB.Visibility = Visibility.Hidden; } else { TB.Visibility = Visibility.Hidden; PB.Visibility = Visibility.Visible; } }); }
        }

        public string Text
        {
            get { return this.TryInvokeMethodFunction(() => { return TB.Text; }); }
            set { this.TryInvokeMethodAction(() => { TB.Text = value; PB.Password = value; }); }
        }


        public string Password
        {
            get { return this.TryInvokeMethodFunction(() => { return PB.Password; }); }
            set { this.TryInvokeMethodAction(() => { TB.Text = value; PB.Password = value; }); }
        }

        public new bool IsEnabled
        {
            get { return this.TryInvokeMethodFunction(() => { return TB.IsEnabled; }); }
            set { this.TryInvokeMethodAction(() => { TB.IsEnabled = value; PB.IsEnabled = value; base.IsEnabled = value; }); }
        }

        public bool IsReadOnly
        {
            get { return this.TryInvokeMethodFunction(() => { return TB.IsReadOnly; }); }
            set { this.TryInvokeMethodAction(() => { TB.IsReadOnly = value; }); }
        }


        public new Brush Background
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Background;  }); }
            set { this.TryInvokeMethodAction(() => { TB.Background = value; PB.Background = value; base.Background = value; }); }
        }

        public new double FontSize
        {
            get { return this.TryInvokeMethodFunction(() => { return base.FontSize; }); }
            set { this.TryInvokeMethodAction(() => { TB.FontSize = value; PB.FontSize = value; base.FontSize = value; }); }
        }

        public bool PasswordBoxAutoAllign { get; set; } = true;

        public TextAlignment TextAlignment
        {
            get { return this.TryInvokeMethodFunction(() => { return TB.TextAlignment; }); }
            set { this.TryInvokeMethodAction(() => {
                TB.TextAlignment = value;

                if (!PasswordBoxAutoAllign) return;

                if (value == TextAlignment.Center)
                {
                    PB.HorizontalContentAlignment = HorizontalAlignment.Center;
                    PB.VerticalContentAlignment = VerticalAlignment.Center;
                }
                else if(value == TextAlignment.Left)
                {
                    PB.HorizontalContentAlignment = HorizontalAlignment.Left;
                    PB.VerticalContentAlignment = VerticalAlignment.Center;
                }
                else if (value == TextAlignment.Right)
                {
                    PB.HorizontalContentAlignment = HorizontalAlignment.Right;
                    PB.VerticalContentAlignment = VerticalAlignment.Center;
                }

            }); }
        }

        public new HorizontalAlignment HorizontalContentAlignment
        {
            get { return this.TryInvokeMethodFunction(() => { return PB.HorizontalContentAlignment; }); }
            set { this.TryInvokeMethodAction(() => { PB.HorizontalContentAlignment = value; }); }
        }

        public new VerticalAlignment VerticalContentAlignment
        {
            get { return this.TryInvokeMethodFunction(() => { return PB.VerticalContentAlignment; }); }
            set { this.TryInvokeMethodAction(() => { PB.VerticalContentAlignment = value; }); }
        }


        public new VerticalAlignment VerticalAlignment
        {
            get { return this.TryInvokeMethodFunction(() => { return base.VerticalAlignment; }); }
            set { this.TryInvokeMethodAction(() => { TB.VerticalAlignment = value; PB.VerticalAlignment = value; base.VerticalAlignment = value; }); }
        }

        public new HorizontalAlignment HorizontalAlignment
        {
            get { return this.TryInvokeMethodFunction(() => { return base.HorizontalAlignment; }); }
            set { this.TryInvokeMethodAction(() => { TB.HorizontalAlignment = value; PB.HorizontalAlignment = value; base.HorizontalAlignment = value; }); }
        }

        public new double Width
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Width; }); }
            set { this.TryInvokeMethodAction(() => { TB.Width = value; PB.Width = value; base.Width = value; }); }
        }
        public new double Height
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Height; }); }
            set { this.TryInvokeMethodAction(() => { TB.Height = value; PB.Height = value; base.Height = value; }); }
        }

        public new double MaxWidth
        {
            get { return this.TryInvokeMethodFunction(() => { return base.MaxWidth; }); }
            set { this.TryInvokeMethodAction(() => { TB.MaxWidth = value; PB.MaxWidth = value; base.MaxWidth = value; }); }
        }
        public new double MaxHeight
        {
            get { return this.TryInvokeMethodFunction(() => { return base.MaxHeight; }); }
            set { this.TryInvokeMethodAction(() => { TB.MaxHeight = value; PB.MaxHeight = value; base.MaxHeight = value; }); }
        }
    }
}
