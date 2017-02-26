using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Collections.Generic;
using static Asmodat.Types.TickTime;

namespace Asmodat.Types
{
    public partial class TickStopWatch
    {
        private TickTime _StartTime;
        private readonly object locker = new object();
        public long _Ticks = 0;

        public bool IsStarded { get; private set; }
        public bool IsStopped { get { return !IsStarded; } }

        public long Ticks
        {
            get
            {
                lock (locker)
                {
                    if (IsStopped)
                    {
                        return _Ticks;
                    }
                    else
                    {
                        return (TickTime.Now - _StartTime).Ticks + _Ticks;
                    }
                }
            }
        }

        public TickTime StartTime { get { lock (locker) { return _StartTime.Copy(); } } }

        public TickStopWatch(bool startNow = false)
        {
            this.Reset(startNow);
        }


        public double Total(Unit unit) { return (new TickTime(Ticks)).Total(unit); }
        



        public void Start()
        {
            lock (locker)
            {
                _StartTime = TickTime.Now;
                IsStarded = true;
            }
        }

        public void Stop()
        {
            lock (locker)
            {
                if (IsStarded)
                    _Ticks += (TickTime.Now - _StartTime).Ticks;

                IsStarded = false;
            }
        }

        
        public void Reset(bool startNow = false)
        {
            lock (locker)
            {
                //Clear ticks on start
                _Ticks = 0;

                if (startNow)
                    this.Start();
                else
                {
                    _StartTime = TickTime.Default;
                    IsStarded = false;
                }
            }
        }

    }
}
