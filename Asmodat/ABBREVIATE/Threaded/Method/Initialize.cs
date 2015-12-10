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
    class ThreadedMethodFlags
    {
        public bool IsAborting = false;
    }

    public partial class ThreadedMethod : IDisposable
    {
        public void Dispose()
        {
            if (TDSThreads != null)
            {
                TerminateAll();
                TDSThreads = null;
            }
        }

        public ThreadPriority Priority
        {
            get;
            set;
        }

        public int MaxThreadsCount
        {
            get;
            set;
        }

        /// <summary>
        /// This property defines for how long in [ms] should run method wait until another access check
        /// </summary>
        public int AccessWait
        {
            get;
            set;
        }


        public ThreadedMethod(int maxThreadsCount = int.MaxValue, ThreadPriority TPriority = ThreadPriority.Lowest, int AccessWait = 1)
        {
            Priority = TPriority;
            MaxThreadsCount = maxThreadsCount;
            this.AccessWait = AccessWait;
        }


        private ThreadedDictionary<string, Thread> TDSThreads = new ThreadedDictionary<string, Thread>();
        private ThreadedDictionary<string, ThreadedMethodFlags> TDSTMFlags = new ThreadedDictionary<string, ThreadedMethodFlags>();

        public bool? IsAborting(string ID)
        {
            if (TDSTMFlags.ContainsKey(ID) && TDSTMFlags[ID] != null)
                return TDSTMFlags[ID].IsAborting;

            return null;
        }
        
        public int Count
        {
            get
            {
                return TDSThreads.Count;
            }
        }

    }
}
