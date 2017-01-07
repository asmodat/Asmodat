using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions;

namespace Asmodat.Types
{
    public partial class BufferedArray<T> where T : class
    {
        


        public BufferedArray(int Length)
        {
            if (Length <= 0)
                throw new ArgumentException("BufferedArray, Length parameter must be greater then 0");

            this.Length = Length;
            this.Elements = new T[this.Length];
            this.Times = new TickTime[this.Length];
            this.Reads = new bool[this.Length];
        }

        public T[] Elements { get; private set; }

        /// <summary>
        /// Insertinon time
        /// </summary>
        public TickTime[] Times { get; private set; }

        /// <summary>
        /// Defines if element was read
        /// </summary>
        public bool[] Reads { get; private set; }

        /// <summary>
        /// Defines burror size
        /// </summary>
        public int Length { get; private set; }

        public int Indexer { get; private set; }

        

        public readonly object locker = new object();

        /// <summary>
        /// Writes new element to array
        /// </summary>
        /// <param name="data"></param>
        public void Write(T data)
        {
            lock (locker)
            {
                if (++Indexer >= Length)
                Indexer = 0;

                Elements[Indexer] = data;
                Times[Indexer] = TickTime.Now;
                Reads[Indexer] = false;
            }
        }


        public T[] ReadAll()
        {
            List<T> result = new List<T>();

            while (!this.IsAllRead)
            {
                T value;
                if (this.Read(out value))
                    result.Add(value);
            }

            return result.ToArray();
        }

        public bool GetLastObject(out T data)
        {
            TickTime first = TickTime.Now;
            int index = -1;

            lock (locker)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (Times[i] < first && Times[i] > TickTime.Default)
                    {
                        first = Times[i];
                        index = i;
                        continue;
                    }
                }

                if (index >= 0)
                {
                    data = Elements[index];
                    return true;
                }
                else
                {
                    data = default(T);
                    return false;
                }
            }
        }


        /// <summary>
        /// Reads first element taht was not yet read
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Read(out T data)
        {
            TickTime first = TickTime.Now;
            int index = -1;

            lock (locker)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (!Reads[i] && Times[i] < first && Times[i] > TickTime.Default)
                    {
                        first = Times[i];
                        index = i;
                        continue;
                    }
                }

                if (index >= 0)
                {
                    data = Elements[index];
                    Reads[index] = true;
                    return true;
                }
                else
                {
                    data = default(T);
                    return false;
                }
            }
        }

        public bool Read(out T data, out TickTime time)
        {
            TickTime first = TickTime.Now;
            int index = -1;

            lock (locker)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (!Reads[i] && Times[i] < first && Times[i] > TickTime.MinValue)
                    {
                        first = Times[i];
                        index = i;
                        continue;
                    }
                }

                if (index >= 0)
                {
                    data = Elements[index];
                    time = Times[index];
                    Reads[index] = true;
                    return true;
                }
                else
                {
                    data = default(T);
                    time = TickTime.Default;
                    return false;
                }
            }
        }


        public bool Contains(T data, TickTime? start = null)
        {
            TickTime _start;
            if (start.IsNull())
                _start = TickTime.MinValue;
            else
                _start = start.Value;
            
            lock (locker)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (Times[i] > _start && data.Equals(Elements[i]))
                        return true;
                }
            }

            return false;
        }

        public bool ContainsSubstring(string subData, TickTime? start = null)
        {
            TickTime _start;
            if (start.IsNull())
                _start = TickTime.MinValue;
            else
                _start = start.Value;

            lock (locker)
            {
                for (int i = 0; i < Length; i++)
                {
                    if (Times[i] < _start || Times[i] == TickTime.MinValue)
                        continue;

                    if (Elements[i] == null)
                        continue;

                    string e = Elements[i].ToString();

                    if (e.Contains(subData))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Defines if all Entries are read
        /// </summary>
        public bool IsAllRead
        {
            get
            {
                lock (locker)
                {
                    for (int i = 0; i < Length; i++)
                        if (!Reads[i] && Times[i] > TickTime.Default)
                            return false;
                    return true;
                }
            }
        }


    }
}
