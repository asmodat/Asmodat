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

namespace Asmodat.Types
{
    /// <summary>
    /// This is list, that acts as array and can be resized
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class FixedList<T>
    {
        private T[] array;

        private int length = 0;
        public int Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
                array = array.ToSafeArray(length);
            }
        }

        public FixedList(int lenght)
        {
            this.Length = length;
        }
        


    }
}
