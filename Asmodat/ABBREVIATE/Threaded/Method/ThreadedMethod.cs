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
using Asmodat.Types;

using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedMethod
    {
        //ThreadedLocker Locker = new ThreadedLocker(10);
        LockerObject LockerRun = new LockerObject();

        public bool RunF<TResult>(Expression<Func<TResult>> expression, object ID = null, bool Exceptions = true, bool waitForAccess = false)
        {
            string ids = null;
            if (ID != null)
                ids = ID.ToString();

            return this.RunF<TResult>(expression, ids, 0, Exceptions, waitForAccess, false);
        }




        public bool Run(Expression<Action> EAMethod, object ID = null, bool Exceptions = true, bool waitForAccess = false)
        {
            string ids = null;
            if (ID != null)
                ids = ID.ToString();

            return this.Run(EAMethod, ids, 0, Exceptions, waitForAccess, false);
        }

        /// <summary>
        /// Runs Actions Delegate in new thread
        /// </summary>
        /// <param name="EAMethod">Delegate thitm method to run in new thread.</param>
        /// <param name="ID">Identifier of Expression Action that schould be stared, if left "default", ID will change to Methiod Name</param>
        /// <param name="waitForAccess">Determines if method should wait until MaxThreadsCount is greater then currently running thread count or just return false if not.</param>
        /// <param name="Exceptions"></param>
        /// <returns>Returns true is delegate was started, else false</returns>
        public bool Run(Expression<Action> EAMethod, string ID = null, bool Exceptions = true, bool waitForAccess = false)
        {
            return this.Run(EAMethod, ID, 0, Exceptions, waitForAccess, false);
        }

        public bool IsAlive(Expression<Action> EAMethod)
        {
            return IsAlive(Expressions.nameofFull(EAMethod));
        }
        public bool IsAlive(string ID)
        {
            if (ID.IsNullOrEmpty() || TDSThreads.IsNullOrEmpty())
                return false;

            try
            {
                if (!LockerRun.EnterWait())
                    return true;

                var thread = TDSThreads.GetValue(ID);

                if (thread == null)
                    return false;

                return thread.IsAlive;
            }
            catch
            {
                return false;
            }
            finally
            {
                LockerRun.Exit();
            }
        }

        

    }
}

/*

if (IsAlive(ID)) return false;
            else this.Terminate(ID);

            Action CompiledAction = EAMethod.Compile();
            Thread ThrdAction;

            if (Exceptions)
                ThrdAction = new Thread(() => { CompiledAction(); });
            else ThrdAction = new Thread(() => { try { CompiledAction(); } catch { } });

            bool bAdded = TDSThreads.AddOrReplace(ID, ThrdAction);

            if (!TDSThreads.ContainsKey(ID) && !Exceptions)
                return false;

            TDSThreads[ID].Priority = Priority;
            TDSThreads[ID].Start();

            return true;
        }

*/

/*


*/