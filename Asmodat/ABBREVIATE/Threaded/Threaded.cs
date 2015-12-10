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
    public static class Threaded
    {

        /// <summary>
        /// Waits for timeout
        /// </summary>
        /// <param name="start"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <param name="intensity"></param>
        public static void Sleep(TickTime start, long timeout, TickTime.Unit timeoutUnit = TickTime.Unit.ms)
        {

            if (timeoutUnit < TickTime.Unit.ms)
                throw new Exception("timeoutUnit is to small, must be greater or equal to ms");

            long end = start.Ticks + ((long)timeoutUnit * timeout);
            long now = TickTime.NowTicks;
            

            if (end <= now)
                return;

            /*while (end > now)
            {
                Thread.Sleep(1);
                now = TickTime.NowTicks;
            }*/

            long wait = end - now;
            int sleep = (int)(wait / (long)TickTime.Unit.ms);
            if (sleep <= 0)
                return;

            Thread.Sleep(sleep);
        }

        public static void Sleep(ref TickTime start, long timeout, TickTime.Unit timeoutUnit = TickTime.Unit.ms)
        {

            if (timeoutUnit < TickTime.Unit.ms)
                throw new Exception("timeoutUnit is to small, must be greater or equal to ms");

            long end = start.Ticks + ((long)timeoutUnit * timeout);
            long now = TickTime.NowTicks;


            if (end <= now)
                return;

            while (end > now)
            {
                Thread.Sleep(1);
                 end = start.Ticks + ((long)timeoutUnit * timeout);
                 now = TickTime.NowTicks;
            }
        }


        public static void SleepRandom(int min, int max)
        {
            Thread.Sleep(AMath.Random(min, max));
        }

        public static void Block(int us)
        {
            Stopwatch SWatch = new Stopwatch();
            SWatch.Start();
            long lFrequency = System.Diagnostics.Stopwatch.Frequency;
            long lWaitTicks = lFrequency / 1000000;
            long lWait1Ms = lFrequency / 1000;
            if (lWaitTicks <= 0) return;
            while (SWatch.ElapsedTicks < lWaitTicks)
            {
                if(lWaitTicks > lWait1Ms) //Use more precise method
                {
                    Thread.Sleep(1);
                    lWaitTicks -= lWait1Ms;
                }
            }
        }


        /// <summary>
        /// This method waits x ('waitms') miliseconds form specified real time changing DateChange
        /// </summary>
        /// <param name="DateChange">DateTime seed that changes in real time</param>
        /// <param name="miliseconds">Time that must bass fom now untilt DateCange to exit.</param>
        /// <param name="threadSleepIntensity">Interval that method awaits it exit, if this number is larger, it reduces accuracity, but extends system resources.</param>
        /// <param name="timeoutms">Defines after what time, method shoould exit anyway.</param>
        /// <returns>Returns time span that passed during wait state, it can be used to determine real delay.</returns>
        public static double Wait(ref DateTime DateChange, int waitms, int threadSleepIntensity = 1, int timeoutms = int.MaxValue)
        {
            DateTime DTStart = DateTime.Now;

            while (
                (DateTime.Now - DateChange).TotalMilliseconds < waitms &&
                (DateTime.Now - DTStart).TotalMilliseconds < timeoutms
                ) Thread.Sleep(threadSleepIntensity);

            return (DateTime.Now - DTStart).TotalMilliseconds;
        }

        /// <summary>
        /// This method waits until specified value (currentState), that constantly changes Equals  another value (exitState), that is constant
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="waitState"></param>
        /// <param name="exitState"></param>
        /// <param name="threadSleepIntensity"></param>
        /// <param name="timeoutms"></param>
        /// <returns></returns>
        public static bool Wait<Type>(ref Type currentState, Type exitState, int threadSleepIntensity = 1, int timeoutms = int.MaxValue) where Type : IEquatable<Type>
        {
            DateTime DTStart = DateTime.Now;

            while ((DateTime.Now - DTStart).TotalMilliseconds < timeoutms)
            {
                if (currentState.Equals(exitState)) return true;
                Thread.Sleep(threadSleepIntensity);
            }
            
            return false;
        }

        //public static void Wait<RetType>(Func<RetType> Function)
        //{

        //}
    }
}
