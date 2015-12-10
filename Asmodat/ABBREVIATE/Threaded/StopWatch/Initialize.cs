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

        long _Frequency;

        public long Frequency
        {
            get
            {
                return _Frequency;
            }
            private set
            {
                _Frequency = value;
            }
        }

        private ThreadedDictionary<string, Stopwatch> Data = new ThreadedDictionary<string, Stopwatch>();

        public ThreadedStopWatch()
        {
            _Frequency = System.Diagnostics.Stopwatch.Frequency;
        }

        public const string DefaultID = "~DefaultID~";

        private string ValidateID(object ID)
        {
            string sID;
            if (ID == null) return DefaultID;
            else sID = ID.ToString();

            if (System.String.IsNullOrEmpty(sID))
                return DefaultID;
            else return sID;
        }
    }
}
