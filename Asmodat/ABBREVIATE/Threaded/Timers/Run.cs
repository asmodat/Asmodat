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

using Asmodat.Extensions.Objects;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedTimers : IDisposable
    {

        private ThreadedMethod Methods = new ThreadedMethod(int.MaxValue, ThreadPriority.Lowest, 1);

        public bool RunParallel(Expression<Action> EAMethod, int interval, int delay, string ID, bool waitForAccess)
        {
            if (System.String.IsNullOrEmpty(ID))
                ID = Expressions.nameofFull(EAMethod);

            return Methods.Run(() => this.Run(EAMethod, interval, ID, waitForAccess, true), ID, delay, true, true);
        }


        public bool Run(Expression<Action> EAMethod, int interval, string ID = null, bool waitForAccess = true, bool autostart = true)
        {

            if (EAMethod == null) return false;

            if (MaxThreadsCount < Workers.Count)
            {
                if (!waitForAccess) return false;

                while (MaxThreadsCount <= Workers.Count) Thread.Sleep(1);
            }

            if (ID.IsNullOrEmpty())
                ID = Expressions.nameofFull(EAMethod);

            lock (locker)
            {
                ThreadedTimer TTimer = Workers.ContainsKey(ID) ? Workers[ID] : null;

                if (TTimer != null && TTimer.Enabled)
                    return false;

                TTimer = new ThreadedTimer(EAMethod, interval, autostart);

                Workers.Add(ID, TTimer, true);
                Workers[ID].Start();
            }

            return true;
        }


    }
}
