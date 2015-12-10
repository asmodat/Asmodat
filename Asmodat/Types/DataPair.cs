using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace Asmodat.Types
{
    public struct DataPair
    {
        [XmlElement("key")]
        public string Key;
        [XmlElement("value")]
        public string Value;

        public DataPair(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
