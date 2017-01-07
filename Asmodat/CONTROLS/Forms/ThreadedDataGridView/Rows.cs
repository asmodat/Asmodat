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
    public partial class ThreadedDataGridView : DataGridView//UserControl
    {
        public int AddRow(object[] values)
        {
            int row = -1;

            if (values != null && values.Count() > 0 && values.Count() <= this.Columns.Count)

                    row = this.Rows.Add();

                    for (int column = 0; column < values.Length; column++)
                        this[column, row].Value = values[column];
            return row;
        }

        public void ClearRows()
        {
            this.Rows.Clear();
        }

        public int AddOrUpdateRow(object[] values)
        {
            int row = -1;

            if (values != null && values.Count() > 0)
            {
                row = this.GetEqualRow(values);

                if (row < 0) row =
                    this.AddRow(values);
                else
                    for (int column = 0; column < values.Length; column++)
                        this[column, row].Value = values[column];
            }

            return row;
        }


        public List<int> AddOrUpdateRows(List<object[]> Rows, bool append = false)
        {
            List<int> Updated = new List<int>();
            if (Rows == null) return Updated;

            foreach (object[] values in Rows)
            {
                int row = AddOrUpdateRow(values);
                if (row >= 0)
                    Updated.Add(row);
            }

            if (append) return Updated;
            else if (Rows.Count == 0)
            {
                this.ClearRows();
                return Updated;
            }

            List<int> All = Integer.ToList(this.Rows.Count - 1, 0);

            foreach (int i in All)
                if (!Updated.Contains(i))
                {
                    this.Rows.RemoveAt(i);
                    Updated.Add(i);
                }


            return Updated;
        }


        public void ClrearRows(bool invoke = true)
        {
            this.Rows.Clear();
        }

        public new DataGridViewRowCollection Rows
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Rows; });
            }
        }


        public new DataGridViewCell this[int columnIndex, int rowIndex]
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base[columnIndex,rowIndex]; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base[columnIndex, rowIndex] = value; });
            }
        }

        public new DataGridViewSelectedRowCollection SelectedRows
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectedRows; });
            }
        }


        /// <summary>
        /// This method searches for first row that has similar key values as passes object array
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public int GetEqualRow(object[] values)
        {
            if (values == null) return -1;

            int iColumnCount = values.Length;
            int iRowsCount = Rows.Count;

            int match = -1;
            for (int row = 0; row < iRowsCount; row++)
            {
                bool found = true;
                for (int col = 0; col < iColumnCount; col++)
                {
                    if (GetColumnTag(col) != ThreadedDataGridView.Tags.Key)
                        continue;


                    if (!Objects.Equals(this[col, row].Value, values[col]))
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    match = row;
                    break;
                }
            }

            return match;
        }

        public List<object> GetSelectedRowsValues(Tags tag)
        {
            DataGridViewSelectedRowCollection Rows = this.SelectedRows;
            List<object> Values = new List<object>();

            int col = this.GetColumns(tag)[0];

            foreach (DataGridViewRow Row in Rows)
                Values.Add(Row.Cells[col].Value);
            

            return Values;
        }


        /// <summary>
        /// Searches for all column indexes with specified tag
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="invoke"></param>
        /// <returns></returns>
        public List<int> GetColumns(Tags tag)
        {
            List<int> LColumns = new List<int>();
            
                for (int i = 0; i < this.Columns.Count; i++)
                    if (Enums.Equals(this.Columns[i].Tag, tag)) LColumns.Add(i);

            return LColumns;
        }
    }
}
