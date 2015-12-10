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
        private ThreadedDictionary<string, DateTime> DataDate = new ThreadedDictionary<string, DateTime>();

        /// <summary>
        /// Sets Date point
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DateSet(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (DataDate.ContainsKey(ID)) DataDate[ID] = DateTime.Now;
            else DataDate.Add(ID, DateTime.Now);
            
            return false;
        }

        /// <summary>
        /// Returns set date point else min value if not set
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public DateTime DateGet(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (DataDate.ContainsKey(ID)) return DataDate[ID];
            else return DateTime.MinValue;
        }

        /// <summary>
        /// Removes date
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DateRemove(string ID = null)
        {
            ID = this.ValidateID(ID);

            return DataDate.Remove(ID);
        }


        /// <summary>
        /// Returns span between now and set date
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TimeSpan DateSpan(string ID = null)
        {
            ID = this.ValidateID(ID);

            if (!DataDate.ContainsKey(ID)) throw new Exception("Date was not set.");

            return (DateTime.Now - DataDate[ID]);
        }

    }
}
