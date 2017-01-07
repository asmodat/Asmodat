using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using System.Runtime.InteropServices;
using System.Drawing;
using Asmodat.Types;
using Asmodat.Extensions.Windows.Forms;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.FormsControls
{
    public partial class ThreadedCheckedListBox : CheckedListBox
    {
        public ThreadedCheckedListBox() : base()
        {
            base.DoubleBuffered = true;
            base.ItemCheck += ThreadedCheckedListBox_ItemCheck;
        }

        public bool SelectEnabled { get; set; } = true;

        private void ThreadedCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!SelectEnabled)
            {
                ClearSelected();
            }
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        { 
            if (e == null || e.Index < 0 || this.Count <= 0) return;

            Size checkSize = CheckBoxRenderer.GetGlyphSize(e.Graphics, System.Windows.Forms.VisualStyles.CheckBoxState.MixedNormal);
            int dx = (e.Bounds.Height - checkSize.Width) / 2;
            e.DrawBackground();
            bool isChecked = GetItemChecked(e.Index);//For some reason e.State doesn't work so we have to do this instead.
            CheckBoxRenderer.DrawCheckBox(e.Graphics, new Point(dx, e.Bounds.Top + dx), isChecked ? System.Windows.Forms.VisualStyles.CheckBoxState.CheckedNormal : System.Windows.Forms.VisualStyles.CheckBoxState.UncheckedNormal);
            using (StringFormat sf = new StringFormat { LineAlignment = StringAlignment.Center })
            {
                using (Brush brush = new SolidBrush(isChecked ? CheckedItemColor : ForeColor))
                {
                    e.Graphics.DrawString(Items[e.Index].ToString(), Font, brush, new Rectangle(e.Bounds.Height, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height, e.Bounds.Height), sf);
                }
            }
        }
        Color checkedItemColor = Color.DarkRed;
        public Color CheckedItemColor
        {
            get { return checkedItemColor; }
            set
            {
                checkedItemColor = value;
                Invalidate();
            }
        }
        
        //return 
        public new bool GetItemChecked(int index)
        {
            return Invoker.TryInvokeMethodFunction(() => { return base.GetItemChecked(index); });
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

        public new ObjectCollection Items
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Items; });
            }
        }

        public int Count
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Items.Count; });
            }
        }


        public CheckState ToCheckState(bool? state)
        {
            if (state == null) return CheckState.Indeterminate;
            else if (state.Value == true) return CheckState.Checked;
            else return CheckState.Unchecked;
        }

        public bool? ToBool(CheckState state)
        {
            if (state == CheckState.Checked) return true;
            else if (state == CheckState.Unchecked) return false;
            return null;
        }


        public bool AddOrUpdate(object obj, bool? isChecked = null)
        {
            return Invoker.TryInvokeMethodFunction(() => 
            {
                bool result = false;

                if (!base.Items.Contains(obj))
                {
                    if (isChecked == null) base.Items.Add(obj);
                    else base.Items.Add(obj, isChecked.Value);
                    result = true;
                }
                else
                    for (int i = 0; i < base.Items.Count; i++)
                    {
                        if (base.Items[i].Equals(obj) && !this.ToBool(base.GetItemCheckState(i)).Equals(isChecked))
                        {
                            result = true;
                            if (isChecked != null && isChecked.Value)
                                base.SetItemChecked(i, true);
                            else
                                base.SetItemChecked(i, false);
                        }
                    }

                return result;
            });
        }

        public void Add(object obj, bool? isChecked = null)
        {
            Invoker.TryInvokeMethodAction(() =>
            {
                if (isChecked == null) base.Items.Add(obj);
                else base.Items.Add(obj, isChecked.Value);
            });
        }

        public void AddRange(List<object> objs, List<bool> checks = null)
        {
            if (objs.IsNullOrEmpty())
                return;
            Invoker.TryInvokeMethodAction(() =>
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    if (checks.Count() >= i)
                        base.Items.Add(objs[i], checks[i]);
                    else
                        base.Items.Add(objs[i]);
                }
            });
        }



        public new void SetItemCheckState(int index, CheckState value)
        {
            Invoker.TryInvokeMethodAction(() =>
            {
                if(index >= 0 && base.Items.Count > index)
                    base.SetItemCheckState(index, value);
            });
        }

        public void SetItemCheckState(int index, bool? value)
        {
            if (value == null)
                this.SetItemCheckState(index, CheckState.Indeterminate);
            else if(value.Value)
                this.SetItemCheckState(index, CheckState.Checked);
            else
                this.SetItemCheckState(index, CheckState.Unchecked);
        }

        public void Remove(object obj)
        {
            Invoker.TryInvokeMethodAction(() =>
            {
                if(obj != null)
                    base.Items.Remove(obj);
            });
        }

        public void Clear(){ Invoker.TryInvokeMethodAction(() =>{ base.Items.Clear();});}


        public bool Contains(object obj)
        {
            if (this.Items.Contains(obj))
                return true;
            else
                return false;
        }
        

        /// <summary>
        /// Adds, updates or removes items
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="isCheckeds"></param>
        public bool Update<T>(List<T> objs, List<bool> checks = null)
        {
            bool result = false;
            if (objs.IsNullOrEmpty())
            {
                this.Clear();

                if(this.Count > 0)
                    result = true;

                return result;
            }

            for (int i = 0; i < this.Count; i++)
                if (!objs.Contains((T)this.Items[i]))
                {
                    result = true;
                    this.Remove(this.Items[i]);
                }
            

            for (int i = 0; i < objs.Count; i++)
            {
                bool? isChecked = null; 
                if (!checks.IsNullOrEmpty() && checks.Count >= (i - 1))
                    isChecked = checks[i];

                if(this.AddOrUpdate(objs[i], isChecked))
                    result = true;
            }

            return result;
        }

        /// <summary>
        /// Replaces old items with new ones if any of new objects changed
        /// </summary>
        /// <param name="objs"></param>
        /// <param name="checks"></param>
        /// <returns></returns>
        public void SetRange(List<object> objs, List<bool> checks = null)
        {
            if (objs.IsNullOrEmpty())
            {
                this.Clear();
                return;
            }

            if (this.Count <= 0 && objs.IsNullOrEmpty())
                return;

            bool change = false;
            if (this.Count != objs.Count)
                change = true;
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if ((!checks.IsNullOrEmpty() && checks.Count >= (i - 1) && ToBool(GetItemCheckState(i)) != checks[i])
                        || !this.Items[i].Equals(objs[i]))
                    {
                        change = true;
                        break;
                    }
                }
            }

            if(change)
            {
                this.Clear();
                this.AddRange(objs.ToList<object>(), checks);
            }
        }

        /// <summary>
        /// Returns objects based on checked state, all by default
        /// </summary>
        /// <param name="isChecked"></param>
        /// <returns></returns>
        public List<T> GetItems<T>(bool? isChecked = null)
        {
            List<T> objs = new List<T>();
            return Invoker.TryInvokeMethodFunction(() => {
                for (int i = 0; i < base.Items.Count; i++)
                    if (isChecked == null ||
                        (isChecked.Value == true && (base.GetItemCheckState(i) == CheckState.Checked)) ||
                         (isChecked.Value == true && (base.GetItemCheckState(i) == CheckState.Checked)))
                        objs.Add((T)base.Items[i]);
                    
                return objs;
            });
        }


        public void CheckAll(bool _checked)
        {
            for (int i = 0; i < this.Count; i++)
                this.SetItemCheckState(i, _checked);
        }

    }
}
//set
//{
//Invoker.TryInvokeMethodAction(() => { base.Items = value; });
//}