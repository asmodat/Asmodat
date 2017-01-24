﻿using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Asmodat.WPFControls
{
    public partial class ThreadedTextBox : TextBox
    {
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

        public new bool IsReadOnly
        {
            get { return this.TryInvokeMethodFunction(() => { return base.IsReadOnly; }); }
            set { this.TryInvokeMethodAction(() => { base.IsReadOnly = value; }); }
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


        public new VerticalAlignment VerticalAlignment
        {
            get { return this.TryInvokeMethodFunction(() => { return base.VerticalAlignment; }); }
            set { this.TryInvokeMethodAction(() => { base.VerticalAlignment = value; }); }
        }

        public new HorizontalAlignment HorizontalAlignment
        {
            get { return this.TryInvokeMethodFunction(() => { return base.HorizontalAlignment; }); }
            set { this.TryInvokeMethodAction(() => { base.HorizontalAlignment = value; }); }
        }

        
    }
}
