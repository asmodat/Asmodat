using System;

using System.Runtime.Serialization;
using System.Security.Permissions;

using System.Xml;
using System.Xml.Serialization;

namespace Asmodat.Types
{
    /// <summary>
    /// This class is serializable, Serialization allows only acces to properties that are public, non static i not read only !
    /// </summary>
    [Serializable]
    [DataContract(Name = "three_state")]
    public struct ThreeState : ISerializable
    {
        //[NonSerialized]
        public static readonly ThreeState Null = new ThreeState(0);
        public static readonly ThreeState False = new ThreeState(-1);
        public static readonly ThreeState True = new ThreeState(1);

        [DataMember(Name = "value")]
        [XmlElement("value")]
        public sbyte Value;

        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsNull
        {
            get
            {
                if (this.Value == 0) return true;
                else return false;
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsTrue
        {
            get
            {
                if (this.Value > 0) return true;
                else return false;
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public bool IsFalse
        {
            get
            {
                if (this.Value < 0) return true;
                else return false;
            }
        }


        /// <summary>
        /// Normal constructor
        /// </summary>
        /// <param name="value"></param>
        public ThreeState(int value) : this() { this.Value = (sbyte)value; }

        /// <summary>
        /// This constructor is used for serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="text"></param>
        public ThreeState(SerializationInfo info, StreamingContext context)
            : this()
        {
            Value = info.GetSByte("Value");
        }

        /// <summary>
        /// This method is called during serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", this.Value);
        }


        public static implicit operator ThreeState(bool x) {  return x ? True : False; }
        public static explicit operator bool(ThreeState x) 
        {
            if (x.Value == 0) throw new InvalidOperationException();
            else return x.Value > 0;
        }

        //public static bool operator ==(ThreeState x, object y)
        //{
        //    if (x.value == 0 || y.value == 0) return Null;
        //    return x.value == y.value ? True : False;
        //}
        //public static bool operator !=(ThreeState x, object y)
        //{
        //    if (x.value == 0 || value == null) return false;
        //    return x.value != y.value ? true : false;
        //}

        public static ThreeState operator ==(ThreeState x, ThreeState y)
        {
            if (x.Value == 0 || y.Value == 0) return Null;
            return x.Value == y.Value ? True : False;
        }
        public static ThreeState operator !=(ThreeState x, ThreeState y)
        {
            if (x.Value == 0 || y.Value == 0) return Null;
            return x.Value != y.Value ? True : False;
        }
        public static ThreeState operator &(ThreeState x, ThreeState y)
        {
            return new ThreeState(x.Value < y.Value ? x.Value : y.Value);
        }
        public static ThreeState operator |(ThreeState x, ThreeState y)
        {
            return new ThreeState(x.Value > y.Value ? x.Value : y.Value);
        }
        public static bool operator true(ThreeState x)
        {
            return x.Value > 0;
        }
        public static bool operator false(ThreeState x)
        {
            return x.Value < 0;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is ThreeState)) return false;
            return Value == ((ThreeState)obj).Value;
        }
        public override int GetHashCode()
        {
            return Value;
        }
        public override string ToString()
        {
            if (Value > 0) return "TreeState.True";
            if (Value < 0) return "TreeState.False";
            return "TreeState.Null";
        }
        
        public static ThreeState Parse(string value)
        {
            if (value == "TreeState.True" || value.ToLower() == "true") return new ThreeState(1);
            else if (value == "TreeState.Null" || value == null || value.ToLower() == "null") return new ThreeState(0);
            else if (value == "TreeState.False" || value.ToLower() == "false") return new ThreeState(-1);
            
            throw new ArgumentException("inalud value parsed");
        }
    }
}


//public void Parse(string value)
//        {
//            if (value == "TreeState.True" || value.ToLower() == "true") this.value = 1;
//            else if (value == "TreeState.Null" || value == null || value.ToLower() == "null") this.value = 0;
//            else if (value == "TreeState.False" || value.ToLower() == "false") this.value = -1;
//            else throw new ArgumentException("inalud value parsed");
//        }