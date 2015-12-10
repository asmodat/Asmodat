using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Asmodat.Abbreviate;

namespace Asmodat.FormsControls
{
    public partial class ThreadedDataGridView : UserControl
    {
        public ThreadedDataGridView()
        {
            InitializeComponent();
            InitializeRowsEnumeration();

            this.DgvMain.CellClick += DgvMain_CellClick;
        }


        public DataGridView DGV
        {
            get
            {
                return this.DgvMain;
            }
        }


       


        /// <summary>
        /// Columns is a read only DataGridViewColumnCollection
        /// </summary>
        public DataGridViewColumnCollection Columns
        {
            get
            {
                return this.DgvMain.Columns;
            }
        }

        

        public Tags GetColumnTag(int col, bool invoke = true)
        {
            object val = null;

            if (this.DgvMain.Columns.Count <= col)
                return Tags.Null;

             val = this.DgvMain.Columns[col].Tag;


            if (Enums.Equals(val, Tags.Key))
                return Tags.Key;

            return Tags.Null;
        }



        

    }

    
}
