using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

using System.Threading;

using System.Diagnostics;

using System.Linq.Expressions;

using AsmodatMath;

using Asmodat.Types;

using System.Numerics;
using System.Windows.Forms;

namespace Asmodat.Abbreviate
{
    public class ThreadedAntiFlood
    {
        private static object locker;
        public TickTime.Unit Unit { get; private set; }
        public long Interval { get; private set; }
        public TickTime Start { get; private set; }


        public ThreadedAntiFlood(TickTime Start, long Interval, TickTime.Unit Unit)
        {
            this.Start = Start;
            this.Interval = Interval;
            this.Unit = Unit;
            locker = new object();
        }
        

        public void Synchronize()
        {

            bool done = false;
            while (!done)
            {
                if (Monitor.TryEnter(locker))
                {
                    try
                    {
                        long constant = ((long)this.Unit * this.Interval);
                        long end = this.Start.Ticks + constant;

                        while (end > TickTime.NowTicks)
                        {
                            int speep = AMath.Random(1, 11);
                            Thread.Sleep(speep);
                            end = this.Start.Ticks + constant;
                        }

                        this.Start = TickTime.Now;
                    }
                    finally
                    {
                        Monitor.Exit(locker);
                        done = true;
                    }
                }
                else
                {
                    done = false;
                    Thread.Sleep(100);
                }
            }
        }
    }
}


/*

lock(locker)
            {
                long constant = ((long)this.Unit * this.Interval);
                long end = this.Start.Ticks + constant;

                while (end > TickTime.NowTicks)
                {
                    int speep = AMath.Random(1, 11);
                    Thread.Sleep(speep);
                    end = this.Start.Ticks + constant;
                }

                this.Start = TickTime.Now;
            }
*/
