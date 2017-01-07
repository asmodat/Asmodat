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
using Asmodat.Extensions.Windows.Forms;

namespace Asmodat.FormsControls
{
    public partial class ThreadedDataGridView : DataGridView// UserControl
    {
        public ThreadedDataGridView()
        {
            //InitializeComponent();
            InitializeRowsEnumeration();

            this.CellClick += DgvMain_CellClick;
        }

        private Control _Invoker = null;
        public Control Invoker
        {
            get
            {
                if (_Invoker == null)
                    _Invoker = this.GetFirstParent();

                return _Invoker;
            }
        }



        /// <summary>
        /// Columns is a read only DataGridViewColumnCollection
        /// </summary>
        public new DataGridViewColumnCollection Columns
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Columns; });
            }
        }

        

        public Tags GetColumnTag(int col, bool invoke = true)
        {
            object val = null;

            if (this.Columns.Count <= col)
                return Tags.Null;

             val = this.Columns[col].Tag;


            if (Enums.Equals(val, Tags.Key))
                return Tags.Key;

            return Tags.Null;
        }


      
       


    }

    
}
