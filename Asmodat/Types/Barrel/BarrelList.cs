using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions;

namespace Asmodat.Types
{
    /// <summary>
    /// This is list, that acts as array and can be resized
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class BarrelList<T>
    {
        private T[] array = null;

        public int Length { get { return array.GetCount(); } }

        private void Resize(int length)
        {
            array = array.ToSafeArray(length);
        }

        private int AdjustIndex(int value)
        {
            int idx = value;

            if (!RollEnable)
                return idx.ToClosedInterval(0, Length - 1);
            
            

            if (idx < 0)
            {
                idx = Math.Abs(idx);
                idx = Length - (idx - ((idx / Length) * Length));
            }

            if (idx >= Length) idx = idx - ((idx / Length) * Length);

            return idx;
        }

        private int index = 0;
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                index = this.AdjustIndex(value);
            }
        }

        /// <summary>
        /// Enables and disables barrel rool
        /// </summary>
        public bool RollEnable { get; set; } = true;

        public T this[int index]
        {
            get { if (Length <= 0) return default(T); else return array[AdjustIndex(index)]; }
            set { if (Length > 0) array[AdjustIndex(index)] = value;  }
        }


        public void Flush()
        {
            this.Index = 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lenght">0 - no elements</param>
        /// <param name="rollEnable">Enables Barrel roll</param>
        public BarrelList(bool RollEnable = true)
        {
            this.RollEnable = RollEnable;
        }

        public void Add(T value)
        {
            this.Resize(this.Length + 1);
            this.index = this.Length - 1;
            this.Value = value;
        }

        public T Value
        {
            get
            {
                return this[Index];
            }
            set
            {
                this[Index] = value;
            }
        }

        public T ReadPrevious()
        {
            return this[--Index];
        }

        public T ReadNext()
        {
            return this[++Index];
        }

        public void WritePrevious(T value)
        {
            this[--Index] = value;
        }

        public void WriteNext(T value)
        {
            this[++Index] = value;
        }

        public T NextValue
        {
            get {  return this[Index + 1]; }
            set { this[Index + 1] = value;  }
        }

        public T PreviousValue
        {
            get { return this[Index - 1]; }
            set { this[Index - 1] = value; }
        }

        public bool IsIndexAtStart
        {
            get { return (Index == 0);  }
        }

        public bool IsIndexAtTheEnd
        {
            get { return (Index == Length - 1); }
        }
    }
}
