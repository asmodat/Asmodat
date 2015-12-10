using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

namespace Asmodat.Types
{
    public class XmlPair<TKey, TValue>
    {
        public XmlPair()
        {

        }

        public XmlPair(TKey Key, TValue Value)
        {
            this.Key = Key;
            this.Value = Value;
        }


        [XmlElement("key")]
        public TKey Key { get; set; }

        [XmlElement("value")]
        public TValue Value { get; set; }
    }
}
