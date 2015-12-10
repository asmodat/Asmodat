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

using Asmodat.Types;

namespace Asmodat.Abbreviate
{
    public class ThreadedThreeStates
    {
         private static readonly object Lock = new object();
        private readonly ThreeState[] States;// = new object[10000];

        private static int _MaxStates = 10000;
        public int MaxStates { get { return _MaxStates; } }

        public ThreadedThreeStates(int MaxStates = 1000)
        {
            if (MaxStates <= 0) MaxStates = 1000;
            _MaxStates = MaxStates;

            States = new ThreeState[_MaxStates];
        }

        private ThreadedDictionary<string, int> Data = new ThreadedDictionary<string, int>();
        private int indexer = 0;
        const string defaultName = "~DefaultThreeState~";


        public ThreeState Get(string ID)
        {
            ID = ThreadedThreeStates.CheckID(ID);

            lock (Lock)
            {
                if (!Data.ContainsKey(ID))
                {
                    if (indexer >= _MaxStates) throw new Exception("ThreadedThreeStates.Get exception, states limit exhausted, maximum of " + _MaxStates + "states");

                    Data.Add(ID, indexer);
                    States[indexer] = ThreeState.Null;
                    ++indexer;
                }

                return States[Data[ID]];
            }
        }

        public ThreeState Get(string ID, ThreeState value)
        {
            ID = ThreadedThreeStates.CheckID(ID);

            lock (Lock)
            {
                if (!Data.ContainsKey(ID))
                {
                    if (indexer >= _MaxStates) throw new Exception("ThreadedThreeStates.Get exception, states limit exhausted, maximum of " + _MaxStates + "states");

                    Data.Add(ID, indexer);
                    States[indexer] = value;
                    ++indexer;
                }

                return States[Data[ID]];
            }
        }


        public void Set(string ID, ThreeState value)
        {
            ID = ThreadedThreeStates.CheckID(ID);

            lock (Lock)
            {
                this.Get(ID);
                States[Data[ID]] = value;
            }
        }

        public ThreeState Get<T>(Expression<Func<T>> labda)
        {
            string id = Objects.fullname(labda);

            return this.Get(id);
        }
        public void Set<T>(Expression<Func<T>> labda, ThreeState value)
        {
            string id = Objects.fullname(labda);

             this.Set(id, value);
        }


        public static string CheckID(string ID)
        {
            if (System.String.IsNullOrEmpty(ID))
                return defaultName;

            return ID;
        }


       
        public ThreeState this[object obj]
        {
            get
            {
                string ID = nameOf(obj, 2);

                return this.Get(ID, ThreeState.Null);
            }
            set
            {
                string ID = nameOf(obj, 2);

                this.Set(ID, value);
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

//public ThreeState this[object obj]
//        {
//            get
//            {

//                string ID = Objects.nameofmember(() => obj);

//                return this.Get(ID);
//            }
//            set
//            {
//                string ID = Objects.nameofmember(() => obj);

//                this.Set(ID, value);
//            }
//        }

//[XmlIgnore]
//        public new TValue this[TKey key]
//        {
//            get { lock (this) return base[key]; }
//            set { this.Add(key, value); }
//        }