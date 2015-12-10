using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;
using Asmodat.Abbreviate;

namespace Asmodat.Types
{
    class XmlDictionary<TKey, TValue>
    {
        public XmlDictionary()
        {

        }

        //[XmlElement("data")]
        //public string Data;


        public void ToList<K, V>(Dictionary<K, V> dicionary)
        {
            //List<XmlPair<K, V>> List = new List<XmlPair<K, V>>();
            //List = new List<XmlPair<K, V>>(dicionary.Count);

            //foreach (KeyValuePair<K, V> KVP in dicionary)
            //{
            //    K key = KVP.Key;
            //    V value = KVP.Value;

            //    if (Types.IsDictionary<TValue>())
            //    {
            //        Type TSubKey = value.GetType().GetGenericArguments()[0];
            //        Type TSubValue = value.GetType().GetGenericArguments()[0];

            //        value = this.GetType().GetMethod("ToList<,>").MakeGenericMethod(TSubKey, TSubValue).Invoke(this, value);
            //    }

            //    List.Add(new XmlPair<K, V>(key, KVP.Value));
            //}
                

            //if (XList.Items != null && XList.Items.Count > 0)
            //    data = Asmodat.Abbreviate.XmlSerialization.Serialize<XmlList<XmlPair<TKey, TValue>>>(XList);
        }

    }
}
