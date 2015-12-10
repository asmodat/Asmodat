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

using System.Reflection;

namespace Asmodat.Abbreviate
{
    public static partial class Serialization
    {
        const int VERSION = 1;
        public static void Save<DataBase>(DataBase DB, string FileName, bool exceptions = false)
        {
            Stream stream = null;

            if (!exceptions)
            {
                try
                {
                    IFormatter IFBinary = new BinaryFormatter();
                    stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                    IFBinary.Serialize(stream, VERSION);
                    IFBinary.Serialize(stream, DB);
                }
                catch { }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            else
            {
                IFormatter IFBinary = new BinaryFormatter();
                stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                IFBinary.Serialize(stream, VERSION);
                IFBinary.Serialize(stream, DB);
                if (stream != null)
                    stream.Close();
            }
        }

        public static DataBase Load<DataBase>(string FileName, bool exceptions = false)
        {
            Stream stream = null;
            DataBase DB = default(DataBase);

            if (!exceptions)
            {
                try
                {
                    IFormatter IFBinary = new BinaryFormatter();
                    stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    int version = (int)IFBinary.Deserialize(stream);
                    Debug.Assert(version == VERSION);
                    DB = (DataBase)IFBinary.Deserialize(stream);
                }
                catch { }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
            else
            {
                IFormatter IFBinary = new BinaryFormatter();
                stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                int version = (int)IFBinary.Deserialize(stream);
                Debug.Assert(version == VERSION);
                DB = (DataBase)IFBinary.Deserialize(stream);

                if (stream != null)
                    stream.Close();
            }

            return DB;
        }



        public static string Serialize<ObjectType>(object data)
        {
            using(MemoryStream MStream = new MemoryStream())
            using(StreamReader SReader = new StreamReader(MStream))
            {
                DataContractSerializer DCSerializer = new DataContractSerializer(typeof(ObjectType));
                DCSerializer.WriteObject(MStream, data);
                MStream.Position = 0;
                return SReader.ReadToEnd();
            }
        }

        public static ObjectType Deserialize<ObjectType>(string data)
        {
            using (MemoryStream MStream = new MemoryStream())
            {
                byte[] baData = System.Text.Encoding.UTF8.GetBytes(data);
                MStream.Write(baData, 0, baData.Length);
                MStream.Position = 0;
                DataContractSerializer DCSerializer = new DataContractSerializer(typeof(ObjectType));
                return (ObjectType)DCSerializer.ReadObject(MStream);
            }
        }
    }


}
