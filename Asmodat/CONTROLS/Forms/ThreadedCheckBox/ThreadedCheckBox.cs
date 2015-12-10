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
    public partial class ThreadedCheckBox : CheckBox
    {

        public Control Invoker { get; private set; }

        public void Initialize(Control Invoker)
        {
            this.Invoker = Invoker;
        }

        public new bool Checked
        {
            get
            {
                bool var = false;
                if (Invoker == null) return base.Checked;
                else
                    Invoker.Invoke((MethodInvoker)(() =>
                    {
                        var = base.Checked;
                    }));

                return var;
            }
            set
            {
                if (Invoker == null) base.Checked = value;
                else
                    Invoker.Invoke((MethodInvoker)(() =>
                    {
                        base.Checked = value;
                    }));
            }
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
