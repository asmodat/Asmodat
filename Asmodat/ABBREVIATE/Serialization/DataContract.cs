using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

using System.Windows.Forms;

using System.Data;

using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

using System.Data.Entity;
using System.Data.Entity.Utilities;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Core.Objects.



using System.Reflection;

namespace Asmodat.Abbreviate
{
    class DataContractSerialization
    {
        /// <summary>
        /// Deserializes data string with DataContract Serializer
        /// </summary>
        /// <typeparam name="ObjetType">Type of returned object</typeparam>
        /// <param name="data">serialized data</param>
        /// <returns>returns deserialized object of specified type</returns>
        public static T Deserialize<T>(string data)//this string data
        {

            using (MemoryStream MStream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                XmlDictionaryReader XDReader = XmlDictionaryReader.CreateTextReader(MStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
                DataContractSerializer DCSerializaer = new DataContractSerializer(typeof(T));
                return (T)DCSerializaer.ReadObject(XDReader);
            }
        }

        /// <summary>
        /// Serializes object of specified type with DataContract Serializer
        /// </summary>
        /// <typeparam name="ObjetType">Type of serialized object</typeparam>
        /// <param name="data">Object to serialize</param>
        /// <returns>Returns object serialized into string.</returns>
        public static string Serialize<T>(T data)
        {
            using (MemoryStream MStream = new MemoryStream())
            {
                using (XmlDictionaryWriter XDWriter = XmlDictionaryWriter.CreateTextWriter(MStream, Encoding.UTF8))
                {
                    DataContractSerializer DCSerializaer = new DataContractSerializer(typeof(T));

                    DCSerializaer.WriteObject(XDWriter, data);
                    XDWriter.Flush();
                    string output = XDWriter.ToString();

                    return output;
                }
            }
        }
    }
}

/*
 /// <summary>
        /// Deserializes data string with DataContract Serializer
        /// </summary>
        /// <typeparam name="ObjetType">Type of returned object</typeparam>
        /// <param name="data">serialized data</param>
        /// <returns>returns deserialized object of specified type</returns>
        public static T Deserialize<T>(string data)//this string data
        {

            using (MemoryStream MStream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                //XmlDictionaryReader XDReader = XmlDictionaryReader.CreateTextReader(MStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
                DataContractSerializer DCSerializaer = new DataContractSerializer(typeof(T));
                //return (T)DCSerializaer.ReadObject(XDReader);
                return (T)DCSerializaer.ReadObject(MStream);
            }
        }

        /// <summary>
        /// Serializes object of specified type with DataContract Serializer
        /// </summary>
        /// <typeparam name="ObjetType">Type of serialized object</typeparam>
        /// <param name="data">Object to serialize</param>
        /// <returns>Returns object serialized into string.</returns>
        public static string Serialize<T>(T data)
        {
            using (MemoryStream MStream = new MemoryStream())
            {
                //XmlDictionaryWriter XDWriter = XmlDictionaryWriter.CreateTextWriter(MStream, Encoding.UTF8); //CreateTextWriter(MStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
                DataContractSerializer DCSerializaer = new DataContractSerializer(typeof(T));
                DCSerializaer.WriteObject(MStream, data);

                //DCSerializaer.WriteObject(XDWriter, data);
                //XDWriter.Flush();
                //string output = XDWriter.ToString();
                //XDWriter.Close();

                return Encoding.UTF8.GetString(MStream.ToArray());
            }
        }
 */