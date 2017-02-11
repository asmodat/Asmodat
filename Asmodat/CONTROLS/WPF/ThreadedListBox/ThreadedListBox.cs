using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Asmodat.WPFControls
{
    public partial class ThreadedListBox : ListBox
    {
        public new void AddChild(object value)
        {
            this.TryInvokeMethodAction(() => { base.AddChild(value); });
        }

        public new void AddLogicalChild(object child)
        {
            this.TryInvokeMethodAction(() => { base.AddLogicalChild(child); });
        }

        public new void AddText(string text)
        {
            this.TryInvokeMethodAction(() => { base.AddText(text); });
        }

        public new void AddVisualChild(Visual child)
        {
            this.TryInvokeMethodAction(() => { base.AddVisualChild(child); });
        }
        
        public new void RemoveVisualChild(Visual child)
        {
            this.TryInvokeMethodAction(() => { base.RemoveVisualChild(child); });
        }

        public new void RemoveLogicalChild(object child)
        {
            this.TryInvokeMethodAction(() => { base.RemoveLogicalChild(child); });
        }

        public new void ClearValue(DependencyProperty dp)
        {
            this.TryInvokeMethodAction(() => { base.ClearValue(dp); });
        }
        public new bool HasItems
        {
            get { return this.TryInvokeMethodFunction(() => { return base.HasItems; }); }
        }

        public new System.Collections.IEnumerator LogicalChildren
        {
            get { return this.TryInvokeMethodFunction(() => { return base.LogicalChildren; }); }
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
