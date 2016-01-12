using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using Asmodat.Extensions.Windows.Forms;

namespace Asmodat.FormsControls 
{
    public partial class ThreadedComboBox : ComboBox
    {
        public ThreadedComboBox() : base()
        {
            if (EnableTextAlign)
                DrawMode = DrawMode.OwnerDrawFixed;
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


        /* public Control Invoker { get; private set; }

         public void Initialize(Control Invoker)
         {
             this.Invoker = Invoker;

             if(EnableTextAlign)
                 DrawMode = DrawMode.OwnerDrawFixed;
         }*/


        /* public void AddItemsEnum<E>(bool append = true, int index = 0)
         {
             string[] items = Enums.ToString<E>().ToArray();
             this.AddItems(append, index, items);
         }

         public void AddItemsEnumDescriptions<E>(bool append = true, int index = 0)
         {
             string[] items = Enums.ToStringDescription<E>().ToArray();
             this.AddItems(append, index, items);
         }

         public void AddItemsEnumDescriptions<E>(E[] source, bool append = true, int index = 0)
         {

             if (source.IsNullOrEmpty())
                 return;
             List<string> items = new List<string>();
             foreach (E e in source)
                 items.Add(Enums.GetEnumDescription(e));

             this.AddItems(append, index, items.ToArray());
         }

         public E GetEnum<E, TInvoker>(TInvoker Invoker) where TInvoker : Control
         {
             string txt = this.GetText();
             if (System.String.IsNullOrEmpty(txt))
                 return default(E);

             return (E)Enum.Parse(typeof(E), txt);
         }*/

        public new string Text
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.Text; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.Text = value; });
            }
        }

        public void ClearItems()
        {
            Invoker.TryInvokeMethodAction(() => { base.Items.Clear(); });
        }


        public double GetDouble()
        {
            double result = 0;
            Invoker.Invoke((MethodInvoker)(() =>
            {
                try
                {
                    result = double.Parse(this.GetTextValue);
                }
                catch
                {
                    this.Text = DoubleDefault + Unit;
                    result = DoubleDefault;
                }
            }));

            return result;
        }

        //"A chart element with the name 'XETHZCAD' already exists in the 'SeriesCollection
        /* public void AddItems(bool append = true, int index = 0, params string[] items)
         {
             if (items.IsNullOrEmpty())
                 return;


             var item = this.SelectedItem;

             if (Objects.EqualsItems(items, this.Items.Cast<object>().ToArray()))
                 return;

             if (!append) this.Items.Clear();

             this.Items.AddRange(items);

             if (index >= 0 && index < this.Items.Count)
                 this.SelectedIndex = index;

             if (item != null && this.Items != null && this.Items.Count > 0 && this.Items.Contains(item))
                 this.SelectedItem = item;

             /* if (this.IsHandleCreated)
              Invoker.Invoke((MethodInvoker)(() =>
              {

              }));
              /


         }*/
        
        /// <summary>
        /// Removes items and sets new ones
        /// </summary>
        /// <param name="values"></param>
        public void SetItems(List<object> values)
        {
            this.ClearItems();

            if (values.IsNullOrEmpty())
                return;

            this.SetItems(values.ToArray());
        }

        /// <summary>
        /// Removes items and sets new ones
        /// </summary>
        /// <param name="values"></param>
        public void SetItems(params object[] values)
        {
            this.ClearItems();

            if (values.IsNullOrEmpty())
                return;
        
            this.AddItems(values);
        }

        public void AddItems(List<object> values)
        {
            if (values.IsNullOrEmpty())
                return;

            this.AddItems(values.ToArray());
        }


        public void AddItems(params object[] values)
        {
            if (values.IsNullOrEmpty())
                return;

             Invoker.TryInvokeMethodAction(() => 
             {
                // int index = this.SelectedIndex;

                 foreach (var v in values)
                 {
                     if (v == null)
                         continue;

                     if (!base.Items.Contains(v))
                         base.Items.Add(v);
                 }

                // this.SelectedIndex = index;
             });
        }


        public new object SelectedItem
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectedItem; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectedItem = value; });
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

        public new string SelectedText
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectedText; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectedText = value; });
            }
        }

        public new object SelectedValue
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectedValue; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectedValue = value; });
            }
        }

        public new int SelectedIndex
        {
            get
            {
                return Invoker.TryInvokeMethodFunction(() => { return base.SelectedIndex; });
            }
            set
            {
                Invoker.TryInvokeMethodAction(() => { base.SelectedIndex = value; });
            }
        }


        public void SelectIndex(int index)
        {
            if (this.Invoker == null || !this.IsHandleCreated)
                return;

            //ObjectCollection

            if (this.Items == null || index < 0 || index >= this.Items.Count)
                return;

            if (index != this.SelectedIndex)
            {
                this.SelectedIndex = index;
            }
        }


        public void AddItems(bool append = true, int index = 0, params double[] items)
        {
            Invoker.Invoke((MethodInvoker)(() =>
            {
                List<string> newItems = new List<string>();

                foreach (double item in items)
                    newItems.Add(item + Unit);

                bool equals = Objects.EqualsItems(newItems.ToArray(), this.Items.Cast<object>().ToArray());

                if (!equals)
                {
                    if (append) this.Items.Clear();

                    this.Items.AddRange(newItems.ToArray());

                    if (index >= 0 && index < this.Items.Count)
                        this.SelectedIndex = index;
                }
            }));

            
        }
    }
}


/*
public void AddItems(bool append = true, int index = 0, params string[] items) 
        {
            if (items.IsNullOrEmpty()) return;
            int index_Save = 0;

            if(this.IsHandleCreated)
            Invoker.Invoke((MethodInvoker)(() =>
            {
                index_Save = this.SelectedIndex;
                var item = this.SelectedItem;

                bool equals = Objects.EqualsItems(items, this.Items.Cast<object>().ToArray());

                if (!equals)
                {
                    if (!append) this.Items.Clear();

                    this.Items.AddRange(items);

                    if (index >= 0 && index < this.Items.Count)
                        this.SelectedIndex = index;
                    else if(index < 0 && index_Save < this.Items.Count)
                        this.SelectedIndex = index_Save;
                }
            }));


            
        }
*/
