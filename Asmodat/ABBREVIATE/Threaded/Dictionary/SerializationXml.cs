using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;

using System.Threading;

using System.IO;
using Asmodat.Types;

using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Asmodat.Abbreviate
{
    /// <summary>
    /// This is Sorted, thread safe, xml serializable dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public partial class ThreadedDictionary<TKey, TValue> : SortedDictionary<TKey, TValue>
    {



        [XmlElement("threaded_dictionary_data")]
        public string XmlData
        {
            get
            {
                return this.XmlSerialize();
            }
            set
            {
                this.XmlDeserialize(value);
            }
        }


        /// <summary>
        /// This property serilizes dictionary into xml string
        /// </summary>
        /// <returns></returns>
        public string XmlSerialize()
        {
            string data = "";
            lock (locker)
            {
                XmlList<XmlPair<TKey, TValue>> XList = new XmlList<XmlPair<TKey, TValue>>();
                XList.Items = new List<XmlPair<TKey, TValue>>(this.Count);

                foreach (KeyValuePair<TKey, TValue> KVP in this)
                    XList.Items.Add(new XmlPair<TKey, TValue>(KVP.Key, KVP.Value));

                if (XList.Items != null && XList.Items.Count > 0)
                    data = Asmodat.Abbreviate.XmlSerialization.Serialize<XmlList<XmlPair<TKey, TValue>>>(XList);
            }

            return data;
        }

        /// <summary>
        /// This property deserializes dictionary from string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void XmlDeserialize(string data)
        {
            lock (locker)
            {
                base.Clear();
                if (!System.String.IsNullOrEmpty(data))
                {
                    XmlList<XmlPair<TKey, TValue>> XList = Asmodat.Abbreviate.XmlSerialization.Deserialize<XmlList<XmlPair<TKey, TValue>>>(data);

                    foreach (XmlPair<TKey, TValue> XKVP in XList.Items)
                        this.Add(XKVP.Key, XKVP.Value);
                }
            }
        }


    }
}
