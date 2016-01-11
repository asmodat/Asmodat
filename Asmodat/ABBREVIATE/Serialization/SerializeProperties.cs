using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

namespace Asmodat.Serialization
{
    public static class SerializeProperties
    {
        /// <summary>
        /// Serializes basic properties (not null and CanRead CanWrite) to string,beware string properties cannot contain 'separator' string, otherwise serialization will be faulty
        /// Serialization occurs based on Format, tat mean only in specified order.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="Format"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string Serialize(object source, string[] Format, string separator, string dateTimeFormat ="yyyy-MM-ddTHH:mm:ss.fff")
        {
            string data = "";
            string sValue = "";
            foreach (string sProperty in Format)
            {
                sValue = Objects.ToString(Objects.GetProperty(source, sProperty), dateTimeFormat);
                if (System.String.IsNullOrEmpty(sValue)) sValue = "";
                else if (sValue.Contains(separator)) throw new Exception("Serialize Exception, string parapeters cannot contain serialization separator");

                data += separator + sValue;
            }

            data = data.Remove(0, separator.Length);

            return data;
        }


        /// <summary>
        /// Deserialization can occur only in specified order
        /// </summary>
        /// <param name="source"></param>
        /// <param name="data"></param>
        /// <param name="Format"></param>
        /// <param name="separator"></param>
        /// <returns>False is serialization failed</returns>
        public static bool Deserialize<Type>(ref Type source, string data, string[] Format, string separator, string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff", bool onFailReturn = true)
        {
            string[] LSRateProperties = data.SplitSafe(separator);
            int iPropertiesCount = LSRateProperties.Count();

            bool success = true;


            for (int i = 0; i < iPropertiesCount; i++)
            {
                if (System.String.IsNullOrEmpty(Format[i])) continue;

                if (!Objects.SetParsedProperty(source, Format[i], LSRateProperties[i], true, dateTimeFormat))
                {
                    if (onFailReturn) return false;
                    else success = false;
                }
            }

            return success;
        }
    }
}
