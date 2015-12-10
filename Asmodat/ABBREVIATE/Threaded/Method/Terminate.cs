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

using System.Security.Permissions;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Debugging;
using Asmodat.Extensions.Threading;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedMethod
    {
        public void Terminate(Expression<Action> EAMethod)
        {
            Terminate(Expressions.nameofFull(EAMethod));
        }

        ExceptionBuffer Exceptions = new ExceptionBuffer();

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public void Terminate(string ID)
        {

            if ((TDSThreads == null || !TDSThreads.ContainsKey(ID)) && 
                (TDSTMFlags == null || !TDSTMFlags.ContainsKey(ID)))
                return;

            if (TDSTMFlags.ContainsKey(ID))
                TDSTMFlags[ID].IsAborting = true;

            do
            {

                TDSThreads.GetValue(ID).KillInstantly();
                /*
                try
                {
                    if (TDSThreads.ContainsKey(ID) && TDSThreads[ID] != null && TDSThreads[ID].IsAlive)
                    {
                        TDSThreads[ID].Interrupt();
                        //TDSThreads[ID].Join(0);
                        TDSThreads[ID].Abort();
                    }
                }
                catch(Exception ex)
                {
                    Exceptions.Write(ex);
                }*/

                //TDSThreads.Update(ID, null);
                //TDSTMFlags.Update(ID, null);

                TDSThreads.Remove(ID);
                TDSTMFlags.Remove(ID);
                Thread.Sleep(1);

            } while (TDSThreads.ContainsKey(ID) || TDSTMFlags.ContainsKey(ID));

        }

        public bool TerminateAll()
        {
            if (!LockerRun.EnterWait())
                return false;

            try
            {
                if (TDSThreads == null)
                    return true;

                string[] keys = TDSThreads.KeysArray;

                if (keys.IsNullOrEmpty())
                    return true;

                for(int i  = 0; i < keys.Length; i++)
                {
                    this.Terminate(keys[i]);
                }

                /*Parallel.ForEach(saTKeys, key =>
                {
                    this.Terminate(key);
                });*/
            }
            finally
            {
                LockerRun.Exit();
            }

            return true;
        }

        public bool AllTerminated()
        {
            if (TDSThreads == null) return true;
            string[] saTKeys = TDSThreads.Keys.ToArray();

            foreach (string s in saTKeys)
                if (this.IsAlive(s)) return false;

            return true;
        }


        public bool TerminateAllCompleated()
        {
            string[] keys = TDSThreads.Keys.ToArray();
            foreach (string key in keys)
            {
                if (!IsAlive(key))
                    this.Terminate(key);
            }


            return true;
        }
    }
}
