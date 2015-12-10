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
        public int AddRow(object[] values, bool invoke = true)
        {
            int row = -1;

            if (values != null && values.Count() > 0 && values.Count() <= this.Columns.Count)


               

                if (invoke)
                    this.Invoke((MethodInvoker)(() =>
                    {
                        row = this.DgvMain.Rows.Add();

                        for (int column = 0; column < values.Length; column++)
                            this.DgvMain[column, row].Value = values[column];
                    }));
                else
                {
                    row = this.DgvMain.Rows.Add();

                    for (int column = 0; column < values.Length; column++)
                        this.DgvMain[column, row].Value = values[column];
                }

            return row;
        }

        public void ClearRows(bool invoke = true)
        {
            if (invoke)
                this.Invoke((MethodInvoker)(() =>
                {
                    this.DgvMain.Rows.Clear();
                }));
            else this.DgvMain.Rows.Clear();
        }

        public int AddOrUpdateRow(object[] values, bool invoke = true)
        {
            int row = -1;

            if (values != null && values.Count() > 0)
                this.Invoke((MethodInvoker)(() =>
                {
                    row = this.GetEqualRow(values);

                    if (row < 0) row =
                        this.AddRow(values);
                    else
                        for (int column = 0; column < values.Length; column++)
                            this.DgvMain[column, row].Value = values[column];
                }));

            return row;
        }


        public List<int> AddOrUpdateRows(List<object[]> Rows, bool append = false, bool invoke = true)
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
                this.ClearRows(invoke);
                return Updated;
            }

            List<int> All = Integer.ToList(this.Rows.Count - 1, 0);

            if (invoke)
                this.Invoke((MethodInvoker)(() =>
                {
                    foreach (int i in All)
                        if (!Updated.Contains(i))
                        {
                            this.Rows.RemoveAt(i);
                            Updated.Add(i);
                        }
                }));
            else
            {
                foreach (int i in All)
                    if (!Updated.Contains(i))
                    {
                        this.Rows.RemoveAt(i);
                        Updated.Add(i);
                    }
            }

            return Updated;
        }


        public void ClrearRows(bool invoke = true)
        {
            if (invoke)
                this.Invoke((MethodInvoker)(() =>
                {
                    this.Rows.Clear();
                }));
            else
            {
                this.Rows.Clear();
            }
        }

        public DataGridViewRowCollection Rows
        {
            get
            {
                return this.DgvMain.Rows;
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


                    if (!Objects.Equals(this.DgvMain[col, row].Value, values[col]))
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

        public List<object> GetSelectedRowsValues(Tags tag, bool invoke = true)
        {
            DataGridViewSelectedRowCollection Rows = this.DgvMain.SelectedRows;
            List<object> Values = new List<object>();

            int col = this.GetColumns(tag, invoke)[0];

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
        public List<int> GetColumns(Tags tag, bool invoke = true)
        {
            List<int> LColumns = new List<int>();

            if (invoke)
                this.Invoke((MethodInvoker)(() =>
                {
                    for (int i = 0; i < this.DgvMain.Columns.Count; i++)
                        if (Enums.Equals(DgvMain.Columns[i].Tag, tag)) LColumns.Add(i);
                }));
            else
                for (int i = 0; i < this.DgvMain.Columns.Count; i++)
                    if (Enums.Equals(DgvMain.Columns[i].Tag, tag)) LColumns.Add(i);

            return LColumns;
        }
    }
}
