using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace Asmodat.Types
{
    public partial class ConcurrentFixedQueue<T> : ConcurrentQueue<T>
    {
        private readonly object locker = new object();
        public int Size { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Size">If size is negative then size is infinite</param>
        public ConcurrentFixedQueue(int Size = -1)
        {
            this.Size = Size;
        }

        public new void Enqueue(T obj)
        {
            base.Enqueue(obj);
            
            if (this.Size > 0)
                lock (locker)
                {
                    while (base.Count > Size)
                    {
                        T outObj;
                        base.TryDequeue(out outObj);
                    }
                }
        }

        public T TryDequeue(T _default = default(T))
        {
            T outObj;
            return base.TryDequeue(out outObj) ? outObj: _default;
        }


        public T[] DequeueAll()
        {
            List<T> list = new List<T>();

            while(base.Count > 0)
            {
                T outObj;
                if (base.TryDequeue(out outObj))
                    list.Add(outObj);
            }

            return list.ToArray();
        }
    }
}
