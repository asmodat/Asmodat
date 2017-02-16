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

        public T[] Values { get { Cleanup(); _TickerRead.SetNow(); return Buffer.ValuesArray; } }
        public TickTime[] Keys { get { Cleanup(); _TickerRead.SetNow(); return Buffer.KeysArray; } }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">if size is less then zero - infinite size</param>
        /// <param name="timeout">if timeout is less then zero - infinite timeout</param>
        /// <param name="unit"></param>
        public TickBuffer(int size = -1, long timeout = 1, TickTime.Unit unit = TickTime.Unit.ms)
        {
            this.Size = size;
            this.Timeout = timeout;
            this.TimeoutUnit = unit;
        }

        public void Cleanup()
        {
            if (Size < 0 && Timeout < 0)
                return; //no nead for cleanup

            while (Size >= 0 || Buffer.Count > Size)
                if (Buffer.Remove(Buffer.Keys.First()))
                    _TickerCleanup.SetNow();

            var keys = Keys;
            if (keys.IsNullOrEmpty())
                return;

            foreach (var v in keys)
                if (v.Timeout(Timeout, TimeoutUnit))
                    if (Buffer.Remove(v))
                        _TickerCleanup.SetNow();
        }

        public void Write(T data)
        {
            this.Write(data, TickTime.Now);
        }


        public void Write(T data, TickTime time)
        {
            Cleanup();

            Buffer.Add(time.Copy(), data);
            _TickerWrite.SetNow();
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
