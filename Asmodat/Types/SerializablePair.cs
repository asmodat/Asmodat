using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Asmodat.Types
{
    [Serializable]
    public struct SerializablePair<TKey, TValue> where TKey : ISerializable where TValue : ISerializable 
    {
        public TKey Key;
        public TValue Value;

        /// <summary>
        /// Normal constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public SerializablePair(TKey key, TValue value) : this()
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// This constructor is used for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public SerializablePair(SerializationInfo info, StreamingContext context)
            : this()
        {
            Key = (TKey)info.GetValue("Key", Key.GetType());
            Value = (TValue)info.GetValue("Value", Value.GetType());
        }

        /// <summary>
        /// This method is called during serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Key", this.Key, Key.GetType());
            info.AddValue("Value", this.Value, Value.GetType());
        }

    }
}
