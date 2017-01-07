using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asmodat.FormsControls
{
    public partial class ThreadedDataGridView : DataGridView//UserControl
    {
        public new event ThreadedDataGridCellClickEventHandler OnCellClick;
        public event ThreadedDataGridCellClickEventHandler OnKeyCellClick;
        public event ThreadedDataGridCellClickEventHandler OnProductCellClick;
        public event ThreadedDataGridCellClickEventHandler OnButtonCellClick;

        void DgvMain_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (OnCellClick == null && OnButtonCellClick == null && OnKeyCellClick == null) return;

            int row = e.RowIndex;
            int column = e.ColumnIndex;

            if (row < 0 || column < 0) return;

            string columnName = this.Columns[column].Name;
            string rowName = this.Rows[row]?.HeaderCell?.Value?.ToString();

            object columnTag = this.Columns[column].Tag;
            object rowTag = this.Rows[row].Tag;


            object value = this[column, row].Value;


            ThreadedDataGridCellClickEvent TDGCCEvent = new ThreadedDataGridCellClickEvent(column, columnName, columnTag,  row, rowName, rowTag, value);

            if (OnCellClick != null)
                OnCellClick(this, TDGCCEvent);

            if (OnButtonCellClick != null)
            if (this[column, row] is DataGridViewButtonCell)
                OnButtonCellClick(this, TDGCCEvent);

            if (OnKeyCellClick != null && this.GetColumnTag(column) == ThreadedDataGridView.Tags.Key)
                OnKeyCellClick(this, TDGCCEvent);

            if (OnProductCellClick != null && this.GetColumnTag(column) == ThreadedDataGridView.Tags.Product)
                OnProductCellClick(this, TDGCCEvent);
            
        }

    }
}
