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

namespace Asmodat.Types
{
    /// <summary>
    /// This locker class is for multithread object managment, and can be used to enable and disable 
    /// </summary>
    public class LockerObject : IDisposable
    {
        public void Dispose()
        {
            this.Exit();
        }

        public LockerObject()
        {
            this.name = "default_" + TickTime.Now;
            locked = false;
        }

        public LockerObject(string name)
        {
            this.name = name;
            locked = false;
        }

        private string name;
        private bool locked = false;
        private readonly object locker = new object();

        public string Name
        {
            get
            {
                return name;
            }
        }

        public bool IsLocked
        {
            get
            {
                return locked;
            }
        }

        /// <summary>
        /// If object is locked returns false else sets lock to true and returns true
        /// </summary>
        /// <returns></returns>
        public bool Enter()
        {
            lock(locker)
            {
                if (this.IsLocked)
                    return false;

                locked = true;
                return true;
            }
        }

        public bool Exit()
        {
            lock (locker)
            {
                if (!this.IsLocked)
                    return false;

                locked = false;
                return true;
            }
        }

        public bool EnterWait(long timeout_ms = -1, int intensity_ms = 1)
        {
            TickTime start = TickTime.Now;

            if (!intensity_ms.InClosedInterval(1, int.MaxValue))
                intensity_ms = 1;

            while (!this.Enter())
            {
                if (timeout_ms > 0 && start.Timeout(timeout_ms))
                    return false;

                try
                {
                    Thread.Sleep(intensity_ms);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        public bool ExitWait(long timeout_ms = -1, int intensity_ms = 1)
        {
            TickTime start = TickTime.Now;

            if (!intensity_ms.InClosedInterval(1, int.MaxValue))
                intensity_ms = 1;

            while (!this.Exit())
            {
                if (timeout_ms > 0 && start.Timeout(timeout_ms))
                    return false;

                try
                {
                    Thread.Sleep(intensity_ms);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }


    }
    
}
