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
        //public const string TagKey = "<KeyTag/>";

        public enum Tags
        {
            Null = 0,
            Key = 1,
            Product = 2
        }

        /// <summary>
        /// Columns must consist of Name, Header and KeyTag indicator !
        /// Key tag allows to define if column is a key value.
        /// Tag is used in order to update row without clearing all rows and adding another one.
        /// Row cannot be updated if one or multiple keys doesn't changed.
        /// 
        /// exaple:
        /// 
        /// string[,] TextColumns = new string[,] 
        ///    { 
        ///        {"ID", "ID", ThreadedDataGridView.KeyTag}, 
        ///        {"Value", "Value [USD]", null},
        ///    };
        /// </summary>
        /// <param name="NamesAndHeaders"></param>
        /// <param name="invoke"></param>
        public void AddTextColumns(object[,] NamesHeadersTags)
        {
            if (NamesHeadersTags.Length % 3 != 0)
                throw new ArgumentException("Columns must consist of Name, Header and KeyTag indicator !");

            int length = NamesHeadersTags.Length / 3;
            List<string> names = new List<string>();
            List<string> headers = new List<string>();
            List<object> tags = new List<object>();

            for(int i = 0; i < length; i++)
            {
                names.Add(NamesHeadersTags[i, 0].ToString());
                headers.Add(NamesHeadersTags[i, 1].ToString());
                tags.Add(NamesHeadersTags[i, 2]);
            }

            this.AddTextColumns(names.ToArray(), headers.ToArray(), tags.ToArray());
        }

        public void AddTextColumns(string[] names, string[] headers, object[] tags)
        {
            if (names == null) return;

            for (int i = 0; i < names.Length; i++)
            {
                string header = null;
                string name = names[i];
                object tag = null;

                if (headers != null && headers.Length > i)
                    header = headers[i];

                if (tags != null && tags.Length > i)
                    tag = tags[i];

                this.AddTextColumns(name, header, tag);
            }
        }

        public void AddTextColumns(string name, string header = null, object tag = null)
        {

            DataGridViewTextBoxColumn GdvTbxC = new DataGridViewTextBoxColumn();

            if (System.String.IsNullOrEmpty(header)) GdvTbxC.HeaderText = name; else GdvTbxC.HeaderText = header;

            GdvTbxC.ReadOnly = true;
            GdvTbxC.Name = name;
            GdvTbxC.Tag = tag;

                this.Columns.Add(GdvTbxC);

            if (this.GetColumnTagsCount(Tags.Key) > 1)
                throw new Exception("There can be only one Columne Key Tag");

            if (this.GetColumnTag(this.Columns.Count - 1, false) == Tags.Key)
                KeyColumnIndex = this.Columns.Count - 1;
        }

        public void AddButtonColumns(string name, string header = null, object tag = null)
        {
            DataGridViewButtonColumn GdvBtnC = new DataGridViewButtonColumn();

            if (System.String.IsNullOrEmpty(header)) GdvBtnC.HeaderText = name; else GdvBtnC.HeaderText = header;

            GdvBtnC.ReadOnly = true;
            GdvBtnC.Tag = tag;
            GdvBtnC.Name = name;

           this.Columns.Add(GdvBtnC);

            if (this.GetColumnTagsCount(Tags.Key) > 1)
                throw new Exception("There can be only one Columne Key Tag");

            if (this.GetColumnTag(this.Columns.Count - 1, false) == Tags.Key)
                KeyColumnIndex = this.Columns.Count - 1;
        }

        public int GetColumnTagsCount(Tags tag, bool invoke = true)
        {
            int counter = 0;

             for (int i = 0; i < this.Columns.Count; i++)
                    if (this.GetColumnTag(i) == tag)
                        ++counter;

            return counter;
        }

        private int _KeyColumnIndex = -1;
        public int KeyColumnIndex
        {
            get
            {
                return _KeyColumnIndex;
            }
            private set
            {
                _KeyColumnIndex = value;
            }
        }


        public int GetColumn(string name)
        {
            int col = -1;

                for (int i = 0; i < this.Columns.Count; i++)
                    if (this.Columns[i].Name == name) { col = i; break; }

            return col;
        }

        public void VisibleColumn(string name, bool visible)
        {
            int col = this.GetColumn(name);

                if (col < 0) return;
                this.Columns[col].Visible = visible;
        }

        public void AutosizeColumnMode(string name, DataGridViewAutoSizeColumnMode mode)
        {
            int col = this.GetColumn(name);

                if (col < 0) return;
            this.Columns[col].AutoSizeMode = mode;
        }

    }
}
