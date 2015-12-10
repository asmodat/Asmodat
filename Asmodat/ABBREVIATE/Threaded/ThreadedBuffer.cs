using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Diagnostics;
using System.Reflection;

using System.Linq.Expressions;

using System.Windows.Forms;

using System.Collections.Concurrent;

using Asmodat.Types;

namespace Asmodat.Abbreviate
{
    //ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    public class ThreadedBuffer<T> : List<XmlPair<TickTime, T>>
    {
        private readonly object locker = new object();

        private int _Size = 1;
        public int Size
        {
            get
            { 
                return _Size; 
            }
            private set
            {
                if (value < 1) throw new Exception("Buffer size cannot be less then zero");
                else _Size = value;
            }
        }

        private void Trim()
        {
            lock(locker)
            {
                while (base.Count > Size)
                    this.RemoveAt(0);
            }
        }

        public bool IsFull
        {
            get
            {
                lock(locker)
                {
                    if (base.Count >= Size)
                        return true;
                    else return false;
                }
            }
        }


        public ThreadedBuffer(int size = 1)
        {
            Size = size;
        }

        public void Add(T item)
        {
            try
            {
                lock (locker)
                {
                    if (base.Count >= Size)
                        base.RemoveAt(0);
                    else base.Add(new XmlPair<TickTime, T>(TickTime.Now, item));
                }
            }
            catch (ThreadInterruptedException tex)
            {
                Asmodat.Debugging.Output.WriteException(tex);
                return;
            }
        }


        public List<T> Values
        {
            get
            {
                try
                {
                    lock (locker)
                    {
                        List<T> list = new List<T>();
                        foreach (var v in this.ToArray())
                        {
                            if (v == null) continue;
                            list.Add(v.Value);
                        }

                        return list;
                    }
                }
                catch (ThreadInterruptedException tex)
                {
                    Asmodat.Debugging.Output.WriteException(tex);
                    return null;
                }
            }
        }
    }
}
