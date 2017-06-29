using System.Windows.Controls;
using Asmodat.Extensions.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.WPFControls
{
    public partial class ThreadedDataGrid : DataGrid
    {
        public new object SelectedItem
        {
            get { return this.TryInvokeMethodFunction(() => { return base.SelectedItem; }); }
            set { this.TryInvokeMethodAction(() => { base.SelectedItem = value; }); }
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

        public void Clear()
        {
            this.TryInvokeMethodAction(() => {
                base.Columns.Clear();
                base.Items.Clear();
            });
        }


        public void AddColumns(params string[] headers)
        {
            if (headers.IsNullOrEmpty())
                return;

            foreach (var h in headers)
                this.AddColumn(h);
        }
            /// <summary>
            /// binding == header.Trim().ToLower();
            /// Colum use example: dataGrid1.Items.Add(new Item() { binding = 1, binding2 = "2012 ... })
            /// </summary>
            public void AddColumn(string header)
        {
            if (header.IsNullOrEmpty())
                throw new System.Exception("Undefined Column header");

            string binding = header?.Trim()?.ToLower()?.Replace(" ", "");

            if (binding.IsNullOrEmpty())
                throw new System.Exception("Undefined Column Item binding");

             this.TryInvokeMethodAction(() => {
                 DataGridTextColumn dgc = new DataGridTextColumn();
                 dgc.Header = header;
                 dgc.Binding = new Binding(binding);

                 foreach (var c in base.Columns)
                     if (c.Header.ToString() == header)
                         return;

                 base.Columns.Add(dgc);
             }); 
        }

        public void UpdateRow(object item)
        {
            if (item == null)
                return;

            this.TryInvokeMethodAction(() => {
                if(!base.Items.Contains(item))
                    base.Items.Add(item);
            });
        }
    }
}
