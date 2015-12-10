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


namespace Asmodat.FormsControls 
{
    public partial class ThreadedComboBox : ComboBox
    {

        public Control Invoker { get; private set; }

        public void Initialize(Control Invoker)
        {
            this.Invoker = Invoker;

            if(EnableTextAlign)
                DrawMode = DrawMode.OwnerDrawFixed;
        }

        
        public void AddItemsEnum<E>(bool append = true, int index = 0)
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
        }

        public string GetText()
        {
            string result = null;
            Invoker.Invoke((MethodInvoker)(() => {  result = this.Text; }));
            return result;
        }

        public void ClearItems()
        {
            Invoker.Invoke((MethodInvoker)(() =>
            {
                this.Items.Clear();
            }));
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
        public void AddItems(bool append = true, int index = 0, params string[] items) 
        {
            if (items.IsNullOrEmpty()) return;
            //int index_Save = 0;

            if(this.IsHandleCreated)
            Invoker.Invoke((MethodInvoker)(() =>
            {
                //index_Save = this.SelectedIndex;
                var item = this.SelectedItem;

                bool equals = Objects.EqualsItems(items, this.Items.Cast<object>().ToArray());

                if (!equals)
                {
                    if (!append) this.Items.Clear();

                    this.Items.AddRange(items);

                    if (index >= 0 && index < this.Items.Count)
                        this.SelectedIndex = index;
                    //else if(index < 0 && index_Save < this.Items.Count)  this.SelectedIndex = index_Save;

                    if (item != null && this.Items != null && this.Items.Count > 0 && this.Items.Contains(item))
                        this.SelectedItem = item;
                }
            }));


            
        }

        public new ObjectCollection Items
        {
            get
            {
                if (this.Invoker == null || !this.IsHandleCreated)
                    return base.Items;

                ObjectCollection result = null;
                Invoker.Invoke((MethodInvoker)(() =>
                {
                    result = base.Items;
                }));

                return result;
            }
        }

        public new int SelectedIndex
        {
            get
            {
                if (this.Invoker == null || !this.IsHandleCreated)
                    return base.SelectedIndex;

                int result = -1;
                Invoker.Invoke((MethodInvoker)(() =>
                {
                    result = base.SelectedIndex;
                }));

                return result;
            }
            set
            {
                if (this.Invoker == null || !this.IsHandleCreated)
                {
                    base.SelectedIndex = value;
                    return;
                }

                Invoker.Invoke((MethodInvoker)(() =>
                {
                    base.SelectedIndex = value;
                }));
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
