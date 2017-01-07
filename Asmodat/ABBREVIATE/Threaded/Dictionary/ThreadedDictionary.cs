using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        private readonly object locker = new object();

        public Dictionary<TKey, TValue> ToDictionary()
        {
            lock (locker)
                return new Dictionary<TKey, TValue>(this);
        }

        public ThreadedDictionary(Dictionary<TKey, TValue> dictrionary)
        {
            lock(locker)
            {
                this.Clear();
                if(dictrionary != null)
                foreach(var v in dictrionary)
                    base.Add(v.Key, v.Value);
            }
        }

        
#if (DEBUG)
        [XmlIgnore]
        public new TValue this[TKey key]
        {
            get
            {
                try
                {
                    lock (locker)
                    {
                        if (!base.ContainsKey(key))
                            return default(TValue);

                        return base[key];

                    }
                }
                catch// (ThreadInterruptedException tex)
                {
                    return default(TValue);
                }
            }
            set { this.Add(key, value); }
        }
#else
        [XmlIgnore]
        public new TValue this[TKey key]
        {
            get { lock (this) return base[key]; }
            set { this.Add(key, value); }
        }
#endif
        public int? CountValues<T>(TKey key)
        {
                lock (locker)
                {
                    if (!base.ContainsKey(key))
                        return null;

                    var val = base[key] as IEnumerable<T>;
                    return val.Count();
                }
        }


        //[XmlIgnore]
        //public new TValue this[TKey key]
        //{
        //    get { lock (this) return base[key]; }
        //    set { this.Add(key, value); }
        //}
        [XmlIgnore]
        public new KeyCollection Keys { get { lock (this) return base.Keys; } }
        [XmlIgnore]
        public new ValueCollection Values { get { lock (this) return base.Values; } }
        [XmlIgnore]
        public new int Count
        {
            get
            {
                try
                {
                    lock (locker) return base.Count;
                }
                catch (ThreadInterruptedException tex)
                {
                    Asmodat.Debugging.Output.WriteException(tex);
                    return -1;
                }

            }
        }


        [XmlIgnore]
        public TValue[] ValuesArray { get { lock (locker) return base.Values.ToArray(); } }


        [XmlIgnore]
        public TKey[] KeysArray { get { lock (locker) return base.Keys.ToArray(); } }

        [XmlIgnore]
        public TValue FirstValue
        {
            get
            {
                lock (locker)
                {
                    if (base.Values.Count > 0)
                        return base.Values.First();
                    else
                        return default(TValue);
                }
                    
            }
        }
        [XmlIgnore]
        public TKey FirstKey
        {
            get
            {
                lock (locker)
                {
                    if (base.Keys.Count > 0)
                        return base.Keys.First();
                    else
                        return default(TKey);
                }
            }
        }

      
        /// <summary>
        /// This property defines wheter or not data inside dixionary is in specied state, 
        /// This propery can be set by user in order to determine if dictionary data is valid and up to date, or needs to be updated
        /// </summary>
        [XmlIgnore]
        public bool IsValid{ get; set; }



        /// <summary>
        /// this mehod adds kay and value or updates valueif key already exists
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public new void Add(TKey key, TValue value)
        {
            lock (locker)
            {
                if (!base.ContainsKey(key))
                    base.Add(key, value);
                else base[key] = value;
            }


            /*if (this.ContainsKey(key)) { lock (this) { base[key] = value; } }
            else lock (this) { base.Add(key, value); }*/

            UpdateTime = DateTime.Now;
        }


        public void AddRange(Dictionary<TKey, TValue> data)
        {

            lock(locker)
            {
                foreach(KeyValuePair<TKey, TValue> kvp in data)
                {
                    base.Add(kvp.Key, kvp.Value);
                    //if (kv.Value != null && kv.Key != null)
                    //if (!base.ContainsKey(kv.Key))
                    //    base.Add(kv.Key, kv.Value);
                    //else base[kv.Key] = kv.Value;
                }
            }

            UpdateTime = DateTime.Now;
        }



        /// <summary>
        /// Thi method is threadsafe allows to check if dictionary contains specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new bool ContainsKey(TKey key)
        {
            //if (key == null) return false;
            lock (locker) { return base.ContainsKey(key); }
        }

        /// <summary>
        /// Removes key from dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Returns fale if kay was not removed, else if removed returns true. </returns>
        public new bool Remove(TKey key)
        {
            bool result;
            lock (locker)
            {
               result = base.Remove(key);
            }

            if(result)
                UpdateTime = DateTime.Now;

            return result;
        }

        /// <summary>
        /// Updates existing value for specific key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Update(TKey key, TValue value)
        {
            bool success = false;
            lock (locker)
            {
                if (base.ContainsKey(key))
                {
                    base[key] = value;
                    success = true;
                }
            }

            UpdateTime = DateTime.Now;
            return success;
        }
    }
}
