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
    public partial class ThreadedSortedSet<T> : SortedSet<T>
    {
        private readonly object locker = new object();

        public T this[int index]
        {
            get { lock (locker) return this.ElementAt(index); }
        }

        public T[] ValuesArray { get { lock (locker) return this.ToArray(); } }
        public List<T> ValuesList { get { lock (locker) return this.ToList(); } }


        public new bool Add(T item) 
        {
            lock (locker)
                return base.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns>true if item was added or existed else false</returns>
        public bool AddDistinct(T item)
        {
            lock (locker)
            {
                if (base.Contains(item))
                    return true;
                else
                    return base.Add(item);
            }
        }


        public bool AddRange(params T[] items)
        {
            if (items == null || items.Length <= 0) return false;
            bool success = true;
            lock (locker)
            {
                int i = 0, l = items.Length;
                for (; i < l; i++)
                    if (!base.Add(items[i]))
                        success = false;
            }

            return success;
        }

        


        public new bool Contains(T item)
        {
            lock (locker)
               return base.Contains(item);
        }

        public new void Clear()
        {
            lock (locker)
                base.Clear();
        }
    }
}
