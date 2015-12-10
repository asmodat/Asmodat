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
    public class ThreadedLocker
    {
        private static readonly object Lock = new object();
        private readonly object[] Locks;// = new object[10000];

        private static int _MaxLocks = 10000;
        public int MaxLocks { get { return _MaxLocks; } }

        public ThreadedLocker(int lockCount = 1000)
        {
            if (lockCount <= 0) lockCount = 1000;
            _MaxLocks = lockCount;

            Locks = new object[_MaxLocks];
        }


        private ThreadedDictionary<string, int> Data = new ThreadedDictionary<string, int>();
        private int lockIndexer = 0;
        const string defaultLock = "~DefaultLock~";
        public object Get(string ID = null)
        {
            ID = ThreadedLocker.CheckID(ID);

            lock (Lock)
            {
                if (!Data.ContainsKey(ID))
                {
                    if (lockIndexer >= _MaxLocks) throw new Exception("ThreadedLocker.Get exception, locks limit exhausted, maximum of " + _MaxLocks + "locks");

                    Data.Add(ID, lockIndexer);
                    Locks[lockIndexer] = new object();
                    ++lockIndexer;
                }

                return Locks[Data[ID]];
            }
        }

        public object Get<T>(Expression<Func<T>> labda)
        {
            string id = Objects.fullname(labda);

            return this.Get(id); ;
        }


        ///// <summary>
        ///// This propery checks out if object is locked, be carefoult it might be locked again before value is returned !, so use with caution.
        ///// </summary>
        ///// <param name="ID">Object ID</param>
        ///// <returns>Returns false if object is locked or whole locker is locked, else true.</returns>
        //public bool IsLocked(string ID)
        //{
        //    ID = ThreadedLocker.CheckID(ID);

        //    bool acquiredLock = false;
        //    bool acquiredObject = false;
        //    try
        //    {
        //        acquiredLock = Monitor.TryEnter(Lock);
        //        if (acquiredLock)
        //        {
        //            if (!Data.ContainsKey(ID))
        //                acquiredObject = true;
        //            else acquiredObject = Monitor.TryEnter(Data[ID]);
        //        }
        //    }
        //    finally
        //    {
        //        if (acquiredLock)
        //        {
        //            if (acquiredObject && Data.ContainsKey(ID))
        //                Monitor.Exit(Data[ID]);

        //            Monitor.Exit(Lock);
        //        }
        //    }

        //    return acquiredObject;
        //}


        private static string CheckID(string ID)
        {
            if (System.String.IsNullOrEmpty(ID))
                return defaultLock;

            return ID;
        }

        public object this[object obj]
        {
            get
            {
                string ID = nameOf(obj, 2);
                return this.Get(ID);
            }
        }



        private Dictionary<string, string> nameOfAltreadyAccessed = new Dictionary<string, string>();
        private System.IO.StreamReader SReader;
        /// <summary>
        /// This method is a peace of art 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public string nameOf(object obj, int level = 1)
        {
            
                StackFrame SFrame = new StackTrace(true).GetFrame(level);
                string file = SFrame.GetFileName();
                int line = SFrame.GetFileLineNumber();
                string id = file + line;

                lock (Lock)
                {
                    if (nameOfAltreadyAccessed.ContainsKey(id))
                        return nameOfAltreadyAccessed[id];
                    else
                    {

                        SReader = new System.IO.StreamReader(file);
                        for (int i = 0; i < line - 1; i++)
                            SReader.ReadLine();
                        string name = SReader.ReadLine().Split(new char[] { '[', ']' })[1];
                        SReader.Close();

                        nameOfAltreadyAccessed.Add(id, name);
                        return name;
                    }
                }
        }


    }
}
