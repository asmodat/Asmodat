using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Asmodat.JavaScript
{
    public partial class JSON
    {

        /// <summary>
        /// Posts JSON to server url and returns response
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string PostRequest(string url, string json)
        {
            return JSON.Request(url, "POST", json);
        }


        public static string Request(string url, string method, string json)
        {
            string result = null;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.ContentType = "text/json";
                request.Method = method;

                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(json);
                    writer.Flush();
                    writer.Close();
                }

                var response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch
            {
                result = null;
            }

            return result;
        }


        public static string GetElement(string name, string json)
        {
            if (json == null) return null;
            string value = null;


            JObject obj = JObject.Parse(json);
            value = (string)obj[name];

            return value;
        }
    }
}
