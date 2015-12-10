using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Types
{
    public partial class TickBuffer<T>
    {
        ThreadedDictionary<TickTime, T> Buffer = new ThreadedDictionary<TickTime, T>();

        public long Timeout { get; private set; }

        public TickTime.Unit TimeoutUnit { get; private set; }

        public int Size { get; private set; }

        private readonly object locker = new object();

        public TickBuffer(int size, long timeout, TickTime.Unit unit = TickTime.Unit.ms)
        {
            this.Size = size;
            this.Timeout = timeout;
            this.TimeoutUnit = unit;
        }

        public void Cleanup()
        {
            lock (locker)
            {
                var keys = Buffer.KeysArray;
                if (keys.IsNullOrEmpty())
                    return;

                foreach (var v in keys)
                {
                    if (v.Timeout(Timeout, TimeoutUnit))
                        Buffer.Remove(v);
                }
            }
        }

        public void Write(T data)
        {
            
            if (Size > 0 && Buffer.Count > Size)
                this.Cleanup();

            lock (locker)
                Buffer.Add(TickTime.Now, data);
        }


        public T[] ReadAllValues()
        {
            this.Cleanup();

            lock (locker)
                return Buffer.ValuesArray;
        }

        public T ReadLast()
        {
            
            T result = default(T);
            TickTime time = TickTime.Default;
            lock (locker) foreach (var v in Buffer)
                {
                    if(v.Key > time)
                    {
                        time = v.Key;
                        result = v.Value;
                    }
                }

            return result;
        }

        public void Clear()
        {
            lock (locker)
            {
                Buffer.Clear();
            }
        }

    }
}
