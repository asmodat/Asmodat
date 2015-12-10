using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

using Asmodat.IO;

namespace Asmodat.Abbreviate
{
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {
        
        
        public File FileDictionary{ get; private set; }

        public bool IsSavingFile
        {
            get
            {
                return Methods.IsAlive("FileDictionary.Save");
            }
        }

        public void SaveFile(bool threaded)
        {
            if (threaded)
                Methods.Run(() => FileDictionary.Save(this.XmlSerialize()), "FileDictionary.Save", true, true);
            else FileDictionary.Save(this.XmlSerialize());
        }



    }
}
