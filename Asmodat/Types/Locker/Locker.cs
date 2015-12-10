using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using AsmodatMath;

using Asmodat.Extensions;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions.Objects;
using System.Threading;
using Asmodat.Debugging;

namespace Asmodat.Types
{
   

    public class Locker : IDisposable
    {
        public void Dispose()
        {
            this.ExitAll();
        }

        public void ExitAll()
        {
            if (locks.IsNullOrEmpty())
                return;

            for(int i = 0; i < locks.Length; i++)
            {
                if (locks[i] == null)
                    continue;

                locks[i].Exit();
            }
        }


        private LockerObject[] locks;
        private readonly object locker = new object();

        public int Size { get; private set; }

        /// <summary>
        /// If size is not in range 1, int.MaxValue, size is default 128
        /// </summary>
        /// <param name="size"></param>
        public Locker(int size = 128)
        {
            if (!size.InClosedInterval(1, int.MaxValue))
                size = 128;

            this.Size = size;
            locks = new LockerObject[this.Size];
        }

        public bool Contains(string key)
        {
            if (locks.IsNullOrEmpty() || key == null)
                return false;

            for(int i = 0; i < locks.Length; i++)
            {
                if (locks[i] != null && locks[i].Name == key)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns index of the key, or negaive value if it was not found
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int GetIndex(string key)
        {
            if (key == null || locks.IsNullOrEmpty())
                return -1;

            for (int i = 0; i < locks.Length; i++)
            {
                if (locks[i] != null && locks[i].Name == key)
                    return i;
            }

           return -2;
        }

        /// <summary>
        /// Returns free index or negative value if there is no space left
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int GetFreeIndex(string key)
        {
            if (this.Contains(key))
                return -3;

            if (key == null || locks.IsNullOrEmpty())
                return -1;

            for (int i = 0; i < locks.Length; i++)
            {
                if (locks[i] == null)
                    return i;
            }

            return -2;
        }

        public bool Enter(string key)
        {
            if (key == null)
                return false;

            lock(locker)
            {
                int index = this.GetIndex(key);
                if (index < 0)
                {
                    index = this.GetFreeIndex(key);
                }

                if (index < 0)
                    return false;

                if (locks[index] == null)
                {
                    locks[index] = new LockerObject(key);
                    return locks[index].Enter();
                }
                else
                {
                    return locks[index].Enter();
                }
            }
        }

        public bool Exit(string key)
        {
            lock (locker)
            {
                int index = this.GetIndex(key);

                if (index < 0 || locks[index] == null)
                    return false;

                return locks[index].Exit();
            }
        }



        public bool Locked(string key)
        {
            int index = this.GetIndex(key);


            if (index < 0 || locks[index] == null)
                return false;

            return locks[index].IsLocked;

        }


        public bool EnterWait(string key, long timeout_ms = -1, int intensity_ms = 1)
        {
            TickTime start = TickTime.Now;

            if (!intensity_ms.InClosedInterval(1, int.MaxValue))
                intensity_ms = 1;

            while (!this.Enter(key))
            {
                if (timeout_ms > 0 && TickTime.Timeout(start, timeout_ms, TickTime.Unit.ms))
                    return false;

                try
                {
                    Thread.Sleep(intensity_ms);
                }
                catch//(Exception ex)
                {
                    //Exceptions.Write(ex);
                    return false;
                }
            }

            return true;
        }


        public bool ExitWait(string key, long timeout_ms = -1, int intensity_ms = 1)
        {
            TickTime start = TickTime.Now;

            if (!this.Contains(key))
                return false;

            if (!intensity_ms.InClosedInterval(1, int.MaxValue))
                intensity_ms = 1;

            while (!this.Exit(key))
            {
                if (timeout_ms > 0 && TickTime.Timeout(start, timeout_ms, TickTime.Unit.ms))
                    return false;

                try
                {
                    Thread.Sleep(intensity_ms);
                }
                catch// (Exception ex)
                {
                    //Exceptions.Write(ex);
                    return false;
                }
            }

            return true;
        }

        
    }
}
