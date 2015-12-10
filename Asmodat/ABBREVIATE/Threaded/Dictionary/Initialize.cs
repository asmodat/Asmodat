using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

using Asmodat.IO;

namespace Asmodat.Abbreviate
{
    

        public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        public ThreadedDictionary() : base()
        {
            UpdateTime = DateTime.MinValue.AddMilliseconds(1);
            //SortedTime = DateTime.MinValue;
        }

        public ThreadedDictionary(string file, bool gzip)
            : base()
        {
            UpdateTime = DateTime.MinValue.AddMilliseconds(1);
            FileDictionary = new File(file, gzip);
            Methods = new ThreadedMethod(1, ThreadPriority.Lowest, 10);

            string data = FileDictionary.Load();
            if (System.String.IsNullOrEmpty(data))
                return;

            this.XmlDeserialize(data);
        }


       /* public static implicit operator ThreadedDictionary<TKey, TValue>(Dictionary<TKey, TValue> d)
        {
            return new ThreadedDictionary<TKey, TValue>(d);
        }
        */
        private ThreadedMethod Methods;

    }
}
