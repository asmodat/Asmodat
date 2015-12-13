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
    public partial class TickBuffer<T> : IDisposable
    {
        public void Dispose()
        {
            this.Clear();
        }


        ThreadedDictionary<TickTime, T> Buffer = new ThreadedDictionary<TickTime, T>();

        public long Timeout { get; private set; }

        public TickTime.Unit TimeoutUnit { get; private set; }

        public int Size { get; private set; }


        private TickTime _TickerRead = TickTime.Default;
        private TickTime _TickerWrite = TickTime.Default;
        private TickTime _TickerClear = TickTime.Default;
        private TickTime _TickerCleanup  = TickTime.Default;

        /// <summary>
        /// Defines if last operation was a write operation and buffor is greater then zero
        /// </summary>
        public bool IsHot
        {
            get
            {
                if (Buffer.Count > 0 &&
                    _TickerWrite > _TickerRead &&
                    _TickerWrite > _TickerClear &&
                    _TickerWrite > _TickerCleanup)
                    return true;
                else
                    return false;
            }
        }


        public TickBuffer(int size, long timeout, TickTime.Unit unit = TickTime.Unit.ms)
        {
            this.Size = size;
            this.Timeout = timeout;
            this.TimeoutUnit = unit;
        }

        public void Cleanup()
        {
            var keys = Buffer.KeysArray;
            if (keys.IsNullOrEmpty())
                return;

            foreach (var v in keys)
            {
                if (v.Timeout(Timeout, TimeoutUnit))
                    Buffer.Remove(v);
            }

            _TickerCleanup.SetNow();
        }

        public void Write(T data)
        {
            this.Write(data, TickTime.Now);
        }


        public void Write(T data, TickTime time)
        {

            if (Size > 0 && Buffer.Count > Size)
                this.Cleanup();

            Buffer.Add(time.Copy(), data);
            _TickerWrite.SetNow();
        }


        public T[] ReadAllValues()
        {
            this.Cleanup();
            T[] result = Buffer.ValuesArray;
            _TickerRead.SetNow();

            return result;
        }

        public T ReadLast()
        {
            TickTime time;
            T result = ReadLast(out time);
            return result;
        }

        public T ReadLast(out TickTime time)
        {
            return this.ReadNext(out time, TickTime.Default);
        }

        public T ReadNext(out TickTime time, TickTime previous)
        {
            T result = default(T);
            time = previous.Copy();
            var keys = Buffer.KeysArray;

            foreach (var key in keys)
            {
                if (key > time)
                {
                    time = key;
                    result = Buffer[key];
                }
            }

            _TickerRead.SetNow();
            return result;
        }

        public T ReadNext(TickTime previous)
        {
            TickTime time;
            T result = ReadNext(out time, previous);
            return result;
        }


        public void Clear()
        {
            Buffer.Clear();
            _TickerClear.SetNow();
        }

        
    }
}
