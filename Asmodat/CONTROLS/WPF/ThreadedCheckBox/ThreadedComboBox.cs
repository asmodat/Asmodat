using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.WPFControls
{
    public partial class ThreadedComboBox : ComboBox
    {
        public void Clear()
        {
            this.TryInvokeMethodAction(() => { base.Items.Clear(); });
        }

        public void AddText(string[] text)
        {
            if (text.IsNullOrEmpty()) return;

            foreach (string s in text)
                this.AddText(s);
        }

        public new void AddText(string text)
        {
            if (text.IsNullOrEmpty())
                return;

            this.TryInvokeMethodAction(() => { base.AddText(text); });
        }

        public new void AddChild(object value)
        {
            if (value == null)
                return;
            
            this.TryInvokeMethodAction(() => { base.AddChild(value); });
        }


        public new string Text
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Text; }); }
            set { this.TryInvokeMethodAction(() => { base.Text = value; }); }
        }

        public new object SelectionBoxItem
        {
            get { return this.TryInvokeMethodFunction(() => { return base.SelectionBoxItem; }); }
        }

        public new string SelectedValuePath
        {
            get { return this.TryInvokeMethodFunction(() => { return base.SelectedValuePath; }); }
            set { this.TryInvokeMethodAction(() => { base.SelectedValuePath = value; }); }
        }

        public new object SelectedValue
        {
            get { return this.TryInvokeMethodFunction(() => { return base.SelectedValue; }); }
            set { this.TryInvokeMethodAction(() => { base.SelectedValue = value; }); }
        }

        public new object SelectedItem
        {
            get { return this.TryInvokeMethodFunction(() => { return base.SelectedItem; }); }
            set { this.TryInvokeMethodAction(() => { base.SelectedItem = value; }); }
        }

        public new int SelectedIndex
        {
            get { return this.TryInvokeMethodFunction(() => { return base.SelectedIndex; }); }
            set { this.TryInvokeMethodAction(() => { base.SelectedIndex = value; }); }
        }


        public new ItemCollection Items
        {
            get { return this.TryInvokeMethodFunction(() => { return base.Items; }); }
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
