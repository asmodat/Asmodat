using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.Security.Permissions;

namespace Asmodat.Extensions.Threading
{
    

    public static class ThreadEx
    {
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public static bool KillInstantly(this Thread thread) 
        {
            if (thread == null)
                return true;
            try
            {
                bool done = thread.Join(0);

                if(!done)
                    thread.Abort();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Compares nullable values, if anu nullable is null or does not have value, it returns false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool ValueEquals<T>(this T? first, T? second) where T : struct
        {
            if (first.IsNull() || second.IsNull()) //this line must be added, becouse it is not supportet with EqualityComparer
                return false;

            return EqualityComparer<T>.Default.Equals(first.Value, second.Value);
        }




    }
}
