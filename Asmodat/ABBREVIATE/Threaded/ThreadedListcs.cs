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
    public partial class ThreadedList<T> : List<T>
    {
        private readonly object locker = new object();

        public new T this[int index]
        {
             get { lock (locker) return this.ElementAt(index); }
             set { lock (locker) base[index] = value; } //fixed ?
        }

        public T[] ValuesArray { get { lock (locker) return this.ToArray(); } }
        public List<T> ValuesList { get { lock (locker) return this.ToList(); } }


        public new void Add(T item)
        {
            lock (locker)
                base.Add(item);
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            this.AddRange(collection);
        }

        public new void RemoveAt(int index)
        {
            if (index < 0)
                return;

            lock (locker)
            {
                if (this.Count > index)
                    base.RemoveAt(index);
            }
        }


        public void AddRange(params T[] items)
        {
            if (items == null || items.Length <= 0) return;

            lock (locker)
            {
                int i = 0;
                for (; i < items.Length; i++)
                    base.Add(items[i]);
            }
        }

        public new void Clear()
        {
            lock (locker)
                base.Clear();
        }
    }
}
