using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Security.Cryptography;
using System.Web;
using Asmodat.Types;
using AsmodatMath;
using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Net.NetworkInformation;
using System.Net.Sockets;
using Asmodat.Debugging;
using System.Text.RegularExpressions;
using System.Management;


namespace Asmodat.Resources
{
    public class NetworkAdaptersInfo
    {

        
        public static string MACAddress
        {
            get
            {
                try
                {
                    return NetworkAdaptersInfo.GetStringProperty("MACAddress", "IPEnabled");
                }
                catch (Exception ex)
                {
                    Output.WriteException(ex);
                    return null;
                }
            }
        }

        public static object GetProperty(string property)
        {

            if (property.IsNullOrEmpty())
                return null;

            try
            {
                string format = string.Format("select {0} from Win32_NetworkAdapterConfiguration ", property);
                var searcher = new ManagementObjectSearcher(format);

                object val = null;
                foreach (var item in searcher.Get())
                {

                    object temp = item[property];
                    if (temp != null)
                        val = temp;
                }

                return val;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return null;
            }
        }

        public static string GetStringProperty(string property, string trueproperty)
        {

            if (property.IsNullOrEmpty())
                return null;

            try
            {
                string result = "";
                ManagementClass mclass = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection collection = mclass.GetInstances();
                foreach(var mobj in collection)
                {
                    string tp = "";
                    try
                    {
                        tp = mobj[trueproperty].ToString();
                    }
                    catch(Exception ex)
                    {
                        ex.ToOutput();
                    }

                    if (result != "" || tp.IsNullOrEmpty() || tp.ToLower() != "true")
                        continue;

                    try
                    {
                        result = mobj[property].ToString();
                        break;
                    }
                    catch (Exception ex)
                    {
                        ex.ToOutput();
                    }

                }

             
                return result;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return null;
            }
        }

    }
}
