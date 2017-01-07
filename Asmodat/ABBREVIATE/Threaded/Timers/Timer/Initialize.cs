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

using System.Runtime.CompilerServices;

namespace Asmodat.Abbreviate
{

    //Action CompiledAction = EAMethod.Compile();
    public partial class ThreadedTimer : IDisposable
    {
        private Object Lock = new Object();//static readonly 

        public System.Threading.Timer Timer;
        public System.Threading.TimerCallback TimerCallback;
        public Action Method { private get; set; }

        private double _Timeout = 0;
        /// <summary>
        /// This parameter can be set in order to stop timer after specified period of time in ms
        /// Precision of this field is bounded by Interval time.
        /// Timeout event wont be executed if ithis value is smaller or equal to 0
        /// </summary>
        public double Timeout
        {
            get
            {
                return _Timeout;
            }
            set
            {
                _Timeout = value;
                TimeoutTime = StartTime.AddMilliseconds(value);
            }
        }

        /// <summary>
        /// This property is used to calculate timeout
        /// </summary>
        public DateTime StartTime { get; private set; }

        /// <summary>
        /// Gets or sets time of timer expirations.
        /// </summary>
        public DateTime TimeoutTime { get; set; }


        private int _Interval = 1000;
        /// <summary>
        /// This field gets and sets Interval property of threaded timer.
        /// </summary>
        public int Interval
        {
            get
            {
                return _Interval;
            }
            set
            {
                _Interval = value;
            }
        }


        private bool _Enabled = false;
        /// <summary>
        /// This Field Enables or Disables timer
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                if (value)
                    this.Start();
                else
                    this.Stop();
            }
        }

        

    }
}
//private bool _Invoked = false;

//        /// <summary>
//        /// This property determines wheter or not action should be invoked
//        /// </summary>
//        public bool Invoked
//        {
//            get
//            {
//                return _Invoked;
//            }
//            set
//            {
//                _Invoked = value;
//            }
//        }