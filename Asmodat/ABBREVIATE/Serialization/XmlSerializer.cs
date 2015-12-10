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

using System.Reflection;

namespace Asmodat.Abbreviate
{
    public static partial class XmlSerialization
    {
        /// <summary>
        /// Deserializes data string with Xml Serializer
        /// </summary>
        /// <typeparam name="ObjetType">Type of returned object</typeparam>
        /// <param name="data">serialized data</param>
        /// <returns>returns deserialized object of specified type</returns>
        public static T Deserialize<T>(string data, string rootAttribute = null)//this string data
        {
            XmlSerializer XSerializer;
            if (System.String.IsNullOrEmpty(rootAttribute))
                XSerializer = new XmlSerializer(typeof(T));
            else XSerializer = new XmlSerializer(typeof(T), rootAttribute);

            StringReader SReader = new StringReader(data);
            T output = (T)XSerializer.Deserialize(SReader);
            SReader.Close();

            return output;
        }

        /// <summary>
        /// Serializes object of specified type with Xml Serializer
        /// </summary>
        /// <typeparam name="ObjetType">Type of serialized object</typeparam>
        /// <param name="data">Object to serialize</param>
        /// <returns>Returns object serialized into string.</returns>
        public static string Serialize<T>(T data, string rootAttribute = null)
        {
            XmlSerializer XSerializer;
            if (System.String.IsNullOrEmpty(rootAttribute))
                XSerializer = new XmlSerializer(typeof(T));
            else XSerializer = new XmlSerializer(typeof(T), rootAttribute);

            StringWriter SWriter = new StringWriter();
            XmlTextWriter XTWriter = new XmlTextWriter(SWriter);
            XTWriter.Formatting = Formatting.Indented;
            XSerializer.Serialize(XTWriter, data);
            string output = SWriter.ToString();

            XTWriter.Close();
            SWriter.Close();

            return output;
        }


    }


}


/*
 /// <summary>
        /// Deserializes data string with Xml Serializer
        /// </summary>
        /// <typeparam name="ObjetType">Type of returned object</typeparam>
        /// <param name="data">serialized data</param>
        /// <returns>returns deserialized object of specified type</returns>
        public static ObjetType Deserialize<ObjetType>(string data)//this string data
        {
            XmlSerializer XSerializer = new XmlSerializer(typeof(ObjetType));
            StringReader SReader = new StringReader(data);

            return (ObjetType)XSerializer.Deserialize(SReader);
        }

        /// <summary>
        /// Serializes object of specified type with Xml Serializer
        /// </summary>
        /// <typeparam name="ObjetType">Type of serialized object</typeparam>
        /// <param name="data">Object to serialize</param>
        /// <returns>Returns object serialized into string.</returns>
        public static string Serialize<ObjetType>(ObjetType data)
        {
            XmlSerializer XSerializer = new XmlSerializer(typeof(ObjetType));
            StringWriter SWriter = new StringWriter();
            XSerializer.Serialize(SWriter, data);
            return SWriter.ToString();
        }
*/