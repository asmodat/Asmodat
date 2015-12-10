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
    public partial class ThreadedMethod
    {
        public TResult JoinF<TResult>(Expression<Func<TResult>> expression, int timeout = int.MaxValue)
        {
            return JoinF<TResult>(Expressions.nameofFull<TResult>(expression), timeout);
        }


        public void Join(Expression<Action> EAMethod, int timeout = int.MaxValue)
        {
            Join(Expressions.nameofFull(EAMethod), timeout);
        }


        public void Join(string ID, int timeout = int.MaxValue)
        {
            if (!IsAlive(ID)) return;

            if (timeout <= 0 || timeout >= int.MaxValue) TDSThreads[ID].Join();
            else TDSThreads[ID].Join(timeout);
        }




        public void JoinAll(int timeout = int.MaxValue)
        {
            if (TDSThreads == null || TDSThreads.Count <= 0) return;
            string[] saTKeys = TDSThreads.Keys.ToArray();

            foreach (string s in saTKeys)
                this.Join(s, timeout);
        }


        public TResult JoinF<TResult>(string ID, int timeout = int.MaxValue)
        {
            if (!IsAlive(ID))
            {
                if (this.Results.ContainsKey(ID))
                    return (TResult)this.Results[ID];
                    else return default(TResult);
            }

            if (timeout <= 0 || timeout >= int.MaxValue) TDSThreads[ID].Join();
            else TDSThreads[ID].Join(timeout);

            return (TResult)this.Results[ID];
        }
    }
}
