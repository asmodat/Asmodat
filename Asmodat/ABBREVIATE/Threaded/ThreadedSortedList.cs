using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedSortedList<T> : SortedSet<T>
    {
        public T this[int index]
        {
            get { lock (this) return this.ElementAt(index); }
        }

        public T[] ValuesArray { get { lock (this) return this.ToArray(); } }
        public List<T> ValuesList { get { lock (this) return this.ToList(); } }


        public new bool Add(T item) 
        {
            lock (this)
                return base.Add(item);
        }

        public bool AddRange(params T[] items)
        {
            if (items == null || items.Length <= 0) return false;
            bool success = true;
            lock (this)
            {
                int i = 0;
                for (; i < items.Length; i++)
                    if (!base.Add(items[i])) success = false;
            }

            return success;
        }

        public new void Clear()
        {
            lock (this)
                base.Clear();
        }
    }
}
