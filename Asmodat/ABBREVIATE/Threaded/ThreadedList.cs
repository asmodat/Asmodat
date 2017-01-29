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
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Abbreviate;

namespace Asmodat.Abbreviate
{
    public static class ThreadedListEx
    {
        public static ThreadedList<T> Clone<T>(this ThreadedList<T> list) where T : ICloneable
        {
            if (list == null)
                return null;
            else if (list.IsNullOrEmpty())
                return new ThreadedList<T>();

            ThreadedList<T> tlg = new ThreadedList<T>();
            tlg.AddRange(list.ToList().Clone());
            return tlg;
        }

        
    }


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

        /// <summary>
        /// Adds new value to dictionary if dictionary does not cointains this item already.
        /// base.Contains(item) is used as comparator, for diffrent comparator please create a difrent method.
        /// </summary>
        /// <param name="item">Unique item that shuld exist inside dictionary.</param>
        /// <returns>true if vale was added to dictionary, false if it already existed inside dictionary</returns>
        public bool AddDistinct(T item)
        {
            bool exists = true;
            lock (locker)
            {
                if (!base.Contains(item))
                    base.Add(item);
                else
                    exists = false;
            }

            return exists;
        }


        //Clears list and add's new range
        public void SetRange(params IEnumerable<T>[] collection)
        {
            if (collection.IsNullOrEmpty())
                return;

            this.Clear();

            int i = 0, l = collection.Count();
            for (; i < l; i++)
                this.AddRange(collection[i]?.ToArray());
        }


        public void AddRange(params IEnumerable<T>[] collection)
        {
            if (collection.IsNullOrEmpty())
                return;

            int i = 0, l = collection.Count();
            for(;i<l;i++)
                this.AddRange(collection[i]?.ToArray());
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            this.AddRange(collection?.ToArray());
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
            if (items.IsNullOrEmpty()) return;

            lock(locker)
                base.AddRange(items);
        }

        public new void Clear()
        {
            lock (locker)
                base.Clear();
        }
        

    }
}
