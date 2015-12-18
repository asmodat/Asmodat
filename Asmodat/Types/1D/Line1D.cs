using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

using Asmodat.Abbreviate;

using System.Threading;
using System.Runtime.Serialization;

using System.Drawing;

namespace Asmodat.Types
{
    [Serializable]
    [DataContract(Name = "line_1d")]
    class Line1D
    {
        public Line1D(double Start, double End)
        {
            this.Start = Start;
            this.End = End;
        }

        [XmlElement("start")]
        public double Start;
        [XmlElement("end")]
        public double End;
    }
}
