using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

namespace Asmodat.Types
{
    [XmlRoot("xml_list")]
    public partial class XmlList<T>
    {
        public XmlList() {this.Items = new List<T>();}

        [XmlElement("t")]
        public List<T> Items { get; set; }
    }
}
