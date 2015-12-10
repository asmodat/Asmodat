using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {

        [XmlIgnore]
        public DateTime UpdateTime { get; private set; }
        [XmlIgnore]
        public double UpdateSpan { get { return (DateTime.Now - UpdateTime).TotalMilliseconds; } }

        //[XmlIgnore]
        //public List<TKey> KeysList
        //{
        //    get
        //    {
        //        lock (this)
        //            return new List<TKey>(base.Keys);
        //    }
        //}

    }
}
