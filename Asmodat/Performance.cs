using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;

namespace Asmodat
{
    public class Performance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SPS">Samples per second</param>
        public Performance(int intervel = 1000)
        {
            Timers.Run(() => Peacemaker(), intervel, null, true, true);
        }

        private static PerformanceCounter PCCPU = new PerformanceCounter("Processor","% Processor Time","_Total");
        private static PerformanceCounter PCRAM = new PerformanceCounter("Memory", "Available Bytes","");

        ThreadedTimers Timers = new ThreadedTimers(10);


        private void Peacemaker()
        {
            CPU = (CPU + PCCPU.NextValue())/2;
            RAM = PCRAM.NextValue();
        }


        /// <summary>
        /// % Processor Time, Total
        /// </summary>
        public double CPU{get; private set;}

        /// <summary>
        /// Available Bytes
        /// </summary>
        public double RAM{get; private set;}

    }
}
