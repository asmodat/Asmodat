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

using Asmodat.Extensions.Objects;

using System.Collections.Concurrent;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedMethod
    {
        ThreadedDictionary<string, object> Results = new ThreadedDictionary<string, object>();

        public bool RunF<TResult>(Expression<Func<TResult>> expression, string ID, int delay, bool Exceptions, bool waitForAccess, bool invoke)
        {
            if (expression == null)
                return false;


            while (MaxThreadsCount < TDSThreads.Count)
            {
                if (!waitForAccess)
                    return false; ;

                Thread.Sleep(AccessWait);
                this.TerminateAllCompleated();
            }

            if (ID.IsNullOrEmpty())
                ID = Expressions.nameofFull<TResult>(expression);

            if (IsAlive(ID))
                return false;

            try
            {
                if (!LockerRun.EnterWait())
                    return false;

                this.Terminate(ID);

                Func<TResult> Function = expression.Compile();
                Results.Add(ID, default(TResult));

                if (Exceptions)
                {
                    if (invoke) TDSThreads.Add(ID, new Thread(() => { if (delay > 0) Thread.Sleep(delay); TResult result = Function.Invoke(); Results[ID] = result; }));
                    else TDSThreads.Add(ID, new Thread(() => { if (delay > 0) Thread.Sleep(delay); TResult result = Function(); Results[ID] = result; }));
                }
                else
                {
                    if (invoke) TDSThreads.Add(ID, new Thread(() => { try { if (delay > 0) Thread.Sleep(delay); TResult result = Function.Invoke(); Results[ID] = result; } catch { } }));
                    else TDSThreads.Add(ID, new Thread(() => { try { if (delay > 0) Thread.Sleep(delay); TResult result = Function(); Results[ID] = result; } catch { } }));
                }

                TDSTMFlags.Add(ID, new ThreadedMethodFlags { IsAborting = false });

                TDSThreads[ID].Priority = Priority;
                TDSThreads[ID].IsBackground = true;
                TDSThreads[ID].Start();
            }
            finally
            {
                LockerRun.Exit();
            }

            return true;
        }

        public bool Run(Expression<Action> EAMethod, string ID, int delay, bool Exceptions, bool waitForAccess, bool invoke)
        {
            if (EAMethod == null) return false;


            while (MaxThreadsCount < TDSThreads.Count)
            {
                if (!waitForAccess)
                    return false;

                Thread.Sleep(AccessWait);
                this.TerminateAllCompleated();
            }

            if (ID.IsNullOrEmpty())
                ID = Expressions.nameofFull(EAMethod);

            if (IsAlive(ID))
                return false;

            try
            {
                if (!LockerRun.EnterWait())
                    return false;

                    this.Terminate(ID);

                    Action Action = EAMethod.Compile();

                    if (Exceptions)
                    {
                        if (invoke) TDSThreads.Add(ID, new Thread(() => { if (delay > 0) Thread.Sleep(delay); Action.Invoke(); }));
                        else TDSThreads.Add(ID, new Thread(() => { if (delay > 0) Thread.Sleep(delay); Action(); }));
                    }
                    else
                    {
                        if (invoke) TDSThreads.Add(ID, new Thread(() => { try { if (delay > 0) Thread.Sleep(delay); Action.Invoke(); } catch { } }));
                        else TDSThreads.Add(ID, new Thread(() => { try { if (delay > 0) Thread.Sleep(delay); Action(); } catch { } }));
                    }

                    TDSTMFlags.Add(ID, new ThreadedMethodFlags { IsAborting = false });

                    TDSThreads[ID].Priority = Priority;
                    TDSThreads[ID].IsBackground = true;
                    TDSThreads[ID].Start();
                
            }
            catch(Exception e)
            {
                if (e.Message != null)
                    return false;
            }
            finally
            {
                LockerRun.Exit();
            }

            return true;
        }

    }
}
