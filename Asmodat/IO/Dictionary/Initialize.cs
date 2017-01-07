using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Asmodat.Abbreviate;
using Asmodat.Types;

using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Asmodat.IO
{
    public partial class FileDictionary<TKey, TValue>
    {


        public DateTime _UpdateTime = DateTime.MinValue;
        public DateTime UpdateTime { get { return _UpdateTime; } private set { _UpdateTime = value; } }

        public bool IsSaved
        {
            get
            {
                return Data.UpdateTime < this.UpdateTime;
            }
        }
        
        private object locker = new object();
        public ThreadedDictionary<TKey, TValue> Data { get; private set; }
        public string FullPath { get; private set; }
        public string FullDirectory { get; private set; }
        public int SaveInterval { get; private set; }

        ThreadedTimers Timers = new ThreadedTimers(10);
    }
}
