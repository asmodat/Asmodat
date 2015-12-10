using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Abbreviate
{
    public partial class DataBuffer
    {
        ThreadedLocker Locker = new ThreadedLocker(100);
        ThreadedDictionary<TickTime,  string> DDTSBuffer;

        /// <summary>
        /// Defines how big the buffor can be, if buffor overflows data is overrided, -1 sets bufffor to infinite size
        /// </summary>
        public int Size { get; private set; } = -1;

        public int Indexer { get; private set; }
        public int Count { get
            {
                return DDTSBuffer.Count;
            }
        }

        public void IndexerIcrement()
        {
            if (!IsDataAvalaible)
                return;
            ++Indexer;
        }
        public void IndexerAbandon()
        {
            Indexer = this.Count;
        }
        public void IndexerFlush()
        {
            Indexer = -1;
        }


        /// <summary>
        /// This is data buffer, that consist of Dictionary of DataTime and string data, where data is in conjuicntion with time in relation to Set Time.
        /// </summary>
        public DataBuffer()
        {
            DDTSBuffer = new ThreadedDictionary<TickTime, string>();
        }


        public void Set(string sData)
        {
            lock (Locker.Get("DDTSBuffer"))
            {
                DDTSBuffer.Add(TickTime.Now, sData);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="DTime"></param>
        /// <param name="bIncrement"></param>
        /// <returns></returns>
        public string Get(TickTime DTime, bool bIncrement = false)
        {
            if (!DDTSBuffer.ContainsKey(DTime)) return null;

            if (bIncrement) ++Indexer;
            return DDTSBuffer[DTime];
        }

        public string Get(bool bIncrement = false)
        {
            return this.GetValue(Indexer, bIncrement);
        }

        public KeyValuePair<TickTime, string> GetPair(bool increment = false)
        {
            if (Indexer < 0) 
                return new KeyValuePair<TickTime, string>(TickTime.Default, null);

            TickTime key = this.GetKey(Indexer);
            string value = this.GetValue(Indexer);
            var kvp = new KeyValuePair<TickTime, string>(key, value);

            if(increment)
                this.IndexerIcrement();

            return kvp;
        }

        //int iLast = this.Count - 1;
        //    if (iLast >= 0)
        //        return new KeyValuePair<DateTime, string>(DDTSBuffer.ElementAt(iLast).Key, DDTSBuffer.ElementAt(iLast).Value);
        //    else return new KeyValuePair<DateTime, string>(DateTime.MinValue, null);
        /*
         
         */
        /// <summary>
        /// Returns string (data) based on index in dictionary
        /// </summary>
        /// <param name="iIndex">Index in dictionary</param>
        /// <param name="bIncrement">Defines if read index schuld be incremented.</param>
        /// <returns>Returns string if dictionary contains index, otherwise null</returns>
        public string GetValue(int iIndex, bool bIncrement = false)
        {
            lock (Locker.Get("DDTSBuffer"))
            {
                if (DDTSBuffer.Count <= iIndex) return null;
                if (bIncrement) ++Indexer;
                return DDTSBuffer.ElementAt(iIndex).Value;
            }
        }


        /// <summary>
        /// Returnd DateTime Key based on data Index
        /// </summary>
        /// <param name="iIndex">Index in dictionary</param>
        /// <returns>DataTime in conjuntion to index, if index does not exist, return defult(DataTime)</returns>
        public TickTime GetKey(int iIndex)
        {
            if (this.Count <= iIndex) return TickTime.Default;
            lock (Locker.Get("DDTSBuffer")) return DDTSBuffer.ElementAt(iIndex).Key;
        }


        /// <summary>
        /// Determines if 
        /// </summary>
        /// <param name="DTKey"></param>
        /// <returns></returns>
        public bool IsValid(TickTime DTKey)
        {
            if (DTKey == TickTime.Default) return false;
            else return true;
        }

        /// <summary>
        /// Determines if all data was readed from Dictionary, returns true if yes, else false
        /// </summary>
        public bool IsDataAvalaible
        {
            get
            {
                if (Indexer >= this.Count) return false;
                else return true;
            }
        }


    }
}
