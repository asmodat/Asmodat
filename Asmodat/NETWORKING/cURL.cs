using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Asmodat.Networking
{
    public partial class cURL
    {

 
        public static string Request(string url, string H_contentType, string X_method, Dictionary<string, string> H_headers,  string d_data)
        {
            string result = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if(H_contentType != null)
                 request.ContentType = H_contentType;

            if(X_method != null)
                request.Method = X_method;


            if (H_headers != null && H_headers.Count > 0)
                foreach (KeyValuePair<string, string> pair in H_headers)
                    request.Headers.Add(pair.Key, pair.Value);


            //byte[] buffer = Encoding.UTF8.GetBytes(d_data);
            //string result = System.Convert.ToBase64String(buffer);



            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(d_data);
                writer.Flush();
                writer.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
                reader.Close();
            }
            
            return result;
        }


        /*
        public static string Request(string url, string H_contentType, string X_method, Dictionary<string, string> H_headers,  string d_data)
        {
            string result = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if(H_contentType != null)
                 request.ContentType = H_contentType;

            if(X_method != null)
                request.Method = X_method;


            if (H_headers != null && H_headers.Count > 0)
                foreach (KeyValuePair<string, string> pair in H_headers)
                    request.Headers[pair.Key] = pair.Value;


            //byte[] buffer = Encoding.UTF8.GetBytes(d_data);
            //string result = System.Convert.ToBase64String(buffer);



            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(d_data);
                writer.Flush();
                writer.Close();
            }

            var response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
                reader.Close();
            }
            
            return result;
        }*/



        public static string ForceRequest(string url, string H_contentType, string X_method, Dictionary<string, string> H_headers, string d_data)
        {
            string result = null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            if (H_contentType != null)
                request.ContentType = H_contentType;

            if (X_method != null)
                request.Method = X_method;


            if (H_headers != null && H_headers.Count > 0)
                foreach (KeyValuePair<string, string> pair in H_headers)
                    request.Headers[pair.Key] = pair.Value;


            //byte[] buffer = Encoding.UTF8.GetBytes(d_data);
            //string result = System.Convert.ToBase64String(buffer);



            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(d_data);
                writer.Flush();
                writer.Close();
            }

            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException wex)
            {
                if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.BadRequest)
                {
                    //Debug Log
                }
                response = (WebResponse)wex.Response;
            }
            finally
            {

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                    reader.Close();
                }
            }

            return result;
        }

    }
}
