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
    public partial class ThreadedDataGridView : UserControl
    {
        public bool RowsEnumaration { get; set; }

        private void InitializeRowsEnumeration()
        {
            RowsEnumaration = true;

            this.DgvMain.RowsAdded += DgvMain_RowsAdded;
            this.DgvMain.RowsRemoved += DgvMain_RowsRemoved;
        }


        void DgvMain_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (RowsEnumaration)
                this.EnumerateRows();
        }

        void DgvMain_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (RowsEnumaration)
                this.EnumerateRows();
        }

        private void EnumerateRows()
        {
            foreach (DataGridViewRow DGVRow in this.DgvMain.Rows)
                DGVRow.HeaderCell.Value = (DGVRow.Index + 1).ToString();
        }

    }
}
