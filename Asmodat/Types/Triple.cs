using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Asmodat.Types
{
    /// <summary>
    /// Editable Touple
    /// </summary>
    /// <typeparam name="TValue1"></typeparam>
    /// <typeparam name="TValue2"></typeparam>
    /// <typeparam name="TValue3"></typeparam>
    public struct Triple<TValue1, TValue2, TValue3> //where TValue1 : ISerializable where TValue2 : ISerializable where TValue3 : ISerializable
    {
        public TValue1 Value1;
        public TValue2 Value2;
        public TValue3 Value3;

        /// <summary>
        /// Normal constructor
        /// </summary>
        public Triple(TValue1 value1, TValue2 value2, TValue3 value3) : this()
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        /*
        /// <summary>
        /// This constructor is used for serialization
        /// </summary>
        public Triple(SerializationInfo info, StreamingContext context)
            : this()
        {
            Value1 = (TValue1)info.GetValue("Value1", Value1.GetType());
            Value2 = (TValue2)info.GetValue("Value2", Value1.GetType());
            Value3 = (TValue3)info.GetValue("Value3", Value1.GetType());
        }

        /// <summary>
        /// This method is called during serialization
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value1", this.Value1, Value1.GetType());
            info.AddValue("Value2", this.Value2, Value2.GetType());
            info.AddValue("Value3", this.Value3, Value3.GetType());
        }
        */
    }
}
