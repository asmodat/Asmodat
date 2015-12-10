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
    public delegate void ThreadedDataGridCellClickEventHandler(object source, ThreadedDataGridCellClickEvent e);

    public class ThreadedDataGridCellClickEvent : EventArgs
    {
        public ThreadedDataGridCellClickEvent(int ColumnIndex, string ColumnName, object ColumnTag, int RowIndex, string RowName, object RowTag, object Value)
        {
            this.RowIndex = RowIndex;
            this.RowName = RowName;
            this.RowTag = RowTag;
            this.ColumnIndex = ColumnIndex;
            this.ColumnName = ColumnName;
            this.ColumnTag = ColumnTag;
            this.Value = Value;
        }


        
        public int ColumnIndex { get; private set; }
        public string ColumnName { get; private set; }
        public object ColumnTag { get; private set; }

        public int RowIndex { get; private set; }
        public string RowName { get; private set; }
        public object RowTag { get; private set; }

        public object Value { get; private set; }

        
    }
}
