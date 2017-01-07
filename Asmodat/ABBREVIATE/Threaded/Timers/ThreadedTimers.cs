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
            if (TDSTTimers != null)
            {
                TerminateAll();
                TDSTTimers = null;
            }
        }

        private ThreadedDictionary<string, ThreadedTimer> TDSTTimers = new ThreadedDictionary<string, ThreadedTimer>();

        /// <summary>
        /// This constructor allows you to create instance of Threaded Timers that can be used to run and manage multible asychnonic timer at once.
        /// </summary>
        /// <param name="maxThreadsCount">Maximum count of existing timers.</param>
        /// <param name="TPriority">Priority of thread inside timers.</param>
        public ThreadedTimers(int maxThreadsCount = int.MaxValue)
        {
            MaxThreadsCount = maxThreadsCount;
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
            if (TDSTTimers.ContainsKey(ID))
                return TDSTTimers[ID];
            else return null;
        }

        public bool Contains(Expression<Action> EAMethod)
        {
            return Contains(Expressions.nameofFull(EAMethod));
        }
        public bool Contains(string ID)
        {
            return TDSTTimers.ContainsKey(ID);
        }

        public bool Terminate(Expression<Action> EAMethod)
        {
            return Terminate(Expressions.nameofFull(EAMethod));
        }
        public bool Terminate(string ID)
        {
            if (TDSTTimers.ContainsKey(ID))
            {
                try
                {
                    if (TDSTTimers[ID] != null)
                    {
                        TDSTTimers[ID].Stop();
                        bool isLocked = TDSTTimers[ID].IsBusy;
                    }
                }
                catch(Exception ex)
                {
                    ex.ToOutput();
                }
                TDSTTimers[ID].Dispose();
                TDSTTimers[ID] = null;
                TDSTTimers.Remove(ID);
                return true;
            }
            else return false;
        }

        public void TerminateAll()
        {
            if (TDSTTimers.IsNullOrEmpty())
                return;

            string[] saTKeys = TDSTTimers.KeysArray;

            if (saTKeys.IsNullOrEmpty())
                return;

            foreach (string s in saTKeys)
                this.Terminate(s);
        }

    
    }
}
