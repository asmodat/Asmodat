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
using Asmodat.IO;
using Asmodat.Cryptography;
using Asmodat.Extensions.Security.Cryptography;

namespace Asmodat.FormsControls 
{
    public partial class ThreadedListBox : ListBox
    {
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

        public void Add(object obj) { Invoker.TryInvokeMethodAction(() => { base.Items.Add(obj); }); }

        public bool AddDistint(object obj)
        {
            return Invoker.TryInvokeMethodFunction(() =>
            {
                bool result = false;

                if (!base.Items.Contains(obj))
                {
                    base.Items.Add(obj);
                    result = true;
                }

                return result;
            });
        }

        public void AddRange(List<object> objs)
        {
            if (objs.IsNullOrEmpty())
                return;
            Invoker.TryInvokeMethodAction(() =>
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    base.Items.Add(objs[i]);
                }
            });
        }

        public void AddDistintRange(List<object> objs)
        {
            if (objs.IsNullOrEmpty())
                return;
            Invoker.TryInvokeMethodAction(() =>
            {
                for (int i = 0; i < objs.Count; i++)
                {
                    if (!base.Items.Contains(objs[i]))
                        base.Items.Add(objs[i]);
                }
            });
        }

        public void Remove(object obj)
        {
            Invoker.TryInvokeMethodAction(() =>
            {
                if (obj != null)
                    base.Items.Remove(obj);
            });
        }

        public void RemoveAt(int index)
        {
            Invoker.TryInvokeMethodAction(() =>
            {
                if (index >= 0 && index < base.Items.Count)
                    base.Items.RemoveAt(index);
            });
        }

        public void Clear() { Invoker.TryInvokeMethodAction(() => { base.Items.Clear(); }); }

        public bool Contains(object obj)
        {
            if (this.Items.Contains(obj))
                return true;
            else
                return false;
        }

        public List<T> GetItems<T>()
        {
            List<T> objs = new List<T>();
            return Invoker.TryInvokeMethodFunction(() => {
                for (int i = 0; i < base.Items.Count; i++)
                        objs.Add((T)base.Items[i]);

                return objs;
            });
        }
    }
}

