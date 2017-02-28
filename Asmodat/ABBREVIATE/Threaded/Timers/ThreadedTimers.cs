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
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Debugging;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedTimers : IDisposable
    {
        /// <summary>
        /// This custom dispose method allows to destroy timer theads before disposing object
        /// </summary>
        public void Dispose()
        {
            if (Workers != null)
            {
                TerminateAll();
                Workers = null;
            }
        }

        private readonly object locker = new object();

        public ThreadedDictionary<string, ThreadedTimer> Workers { get; private set; }

        /// <summary>
        /// This constructor allows you to create instance of Threaded Timers that can be used to run and manage multible asychnonic timer at once.
        /// </summary>
        /// <param name="maxThreadsCount">Maximum count of existing timers.</param>
        /// <param name="TPriority">Priority of thread inside timers.</param>
        public ThreadedTimers(int maxThreadsCount = int.MaxValue)
        {
            MaxThreadsCount = maxThreadsCount;
            Workers = new ThreadedDictionary<string, ThreadedTimer>();
        }

        public int MaxThreadsCount
        {
            get;
            set;
        }

        public ThreadedTimer Timer(Expression<Action> EAMethod)
        {
            return Timer(Expressions.nameofFull(EAMethod));
        }

        public ThreadedTimer Timer(string ID)
        {
            lock (locker)
            {
                return Workers.ContainsKey(ID) ? Workers[ID] : null;
            }
        }

        public bool Contains(Expression<Action> EAMethod)
        {
            return Contains(Expressions.nameofFull(EAMethod));
        }
        public bool Contains(string ID)
        {
                return Workers.ContainsKey(ID);
        }

        public bool Terminate(Expression<Action> EAMethod)
        {
            return Terminate(Expressions.nameofFull(EAMethod));
        }
        public bool Terminate(string ID)
        {
            lock (locker)
            {
                if (!Workers.ContainsKey(ID))
                    return false;

                try
                {
                    if (Workers[ID] != null)
                    {
                        Workers[ID].Enabled = false;
                        Workers[ID].Stop();
                        bool isLocked = Workers[ID].IsBusy;
                    }
                }
                catch (Exception ex)
                {
                    ex.ToOutput();
                }

                Workers[ID].Dispose();
                Workers[ID] = null;
                Workers.Remove(ID);
                return true;
            }
        }

        public void TerminateAll()
        {
            Methods.TerminateAll();

            string[] saTKeys = Workers?.KeysArray;

            if (saTKeys.IsNullOrEmpty())
                return;

            foreach (string s in saTKeys)
                this.Terminate(s);
        }

    
    }
}
