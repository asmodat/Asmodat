using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

namespace Asmodat.Types.Legacy
{
    [Serializable]
    [XmlRoot("XmlSerializablePair")]
    public class XmlSerializablePair<TKey, TValue> : IXmlSerializable // where TKey : ISerializable where TValue : ISerializable 
    {
        public const string RootAttribute = "XmlSerializablePair";
        
        public TKey Key;
        public TValue Value;

        /// <summary>
        /// Class must have a default contructor to be serialized
        /// </summary>
        public XmlSerializablePair()
        {

        }

        public XmlSerializablePair(TKey key, TValue value)// : base<TKey,TValue>()
        {
            Key = key;
            Value = value;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader XReader)
        {
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            bool bWasEmpty = XReader.IsEmptyElement;
            XReader.Read();

            if (bWasEmpty)
                return;

            XReader.ReadStartElement("Key");
            this.Key = (TKey)KeySerializer.Deserialize(XReader);
            XReader.ReadEndElement();

            XReader.ReadStartElement("Value");
            this.Value = (TValue)ValueSerializer.Deserialize(XReader);
            XReader.ReadEndElement();

            //XReader.ReadEndElement();
        }

        public void WriteXml(XmlWriter XWriter)
        {
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            XWriter.WriteStartElement("Key");
            KeySerializer.Serialize(XWriter, Key);
            XWriter.WriteEndElement();

            XWriter.WriteStartElement("Value");
            KeySerializer.Serialize(XWriter, Value);
            XWriter.WriteEndElement();

            //XWriter.WriteEndElement();
        }


    }
}


/*
 [XmlRoot("XmlSerializablePair")]
    public class XmlSerializablePair<TKey, TValue> : IXmlSerializable // where TKey : ISerializable where TValue : ISerializable 
    {
        public TKey Key;
        public TValue Value;

        /// <summary>
        /// Class must have a default contructor to be serialized
        /// </summary>
        public XmlSerializablePair()
        {

        }

        public XmlSerializablePair(TKey key, TValue value) : this()
        {
            Key = key;
            Value = value;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader XReader)
        {
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            bool bWasEmpty = XReader.IsEmptyElement;
            XReader.Read();

            if (bWasEmpty)
                return;

            XReader.ReadStartElement("Key");
            this.Key = (TKey)KeySerializer.Deserialize(XReader);
            XReader.ReadEndElement();

            XReader.ReadStartElement("Value");
            this.Value = (TValue)ValueSerializer.Deserialize(XReader);
            XReader.ReadEndElement();

            XReader.ReadEndElement();
        }

        public void WriteXml(XmlWriter XWriter)
        {
            XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

            XWriter.WriteStartElement("Key");
            KeySerializer.Serialize(XWriter, Key);
            XWriter.WriteEndElement();

            XWriter.WriteStartElement("Value");
            KeySerializer.Serialize(XWriter, Value);
            XWriter.WriteEndElement();

            XWriter.WriteEndElement();
        }


    }
 */