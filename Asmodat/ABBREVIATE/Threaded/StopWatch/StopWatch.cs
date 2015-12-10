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

namespace Asmodat.Abbreviate
{
    public partial class ThreadedStopWatch
    {
        

        /// <summary>
        /// Starts Stopped Timer
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Start(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (Data.ContainsKey(ID))
            {
                if (!Data[ID].IsRunning)
                {
                    Data[ID].Start();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Starts, Creates or restarts stopwatch.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public void Run(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (Data.ContainsKey(ID))  Data[ID].Restart();
            else
            {
                Data.Add(ID, new Stopwatch());
                Data[ID].Start();
            }
        }

        /// <summary>
        /// Returns elapsed time in ms
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public double ms(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (!Data.ContainsKey(ID)) return -1;
            else return Data[ID].ElapsedMilliseconds;
        }

        /// <summary>
        /// Returns elapsed time in us
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public double us(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (!Data.ContainsKey(ID)) return -1;
            else return ((double)Data[ID].ElapsedTicks / Frequency) * 1000000;
        }

        /// <summary>
        /// Returns elapsed ticks
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public double ticks(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (!Data.ContainsKey(ID)) return -1;
            else return Data[ID].ElapsedTicks;
        }


        /// <summary>
        /// Removes timer
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Remove(string ID = null)
        {
            ID = this.ValidateID(ID);

            return Data.Remove(ID);
        }

        /// <summary>
        /// Stops timer
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Stop(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (!Data.ContainsKey(ID)) return false;
            else Data[ID].Stop();

            return Data.Remove(ID);
        }
      

    }
}
