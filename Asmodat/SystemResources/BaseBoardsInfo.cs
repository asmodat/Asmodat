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
    public class BaseBoardsInfo
    {

        
        public static string SerialNumber
        {
            get
            {
                try
                {
                    return (string)BaseBoardsInfo.GetProperty("SerialNumber");
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
                string format = string.Format("select {0} from Win32_BaseBoard", property);
                var searcher = new ManagementObjectSearcher(format);

                object val = null;
                foreach (var item in searcher.Get())
                {

                    object temp = item[property];
                    if (temp != null)
                    {
                        val = temp;
                        break;
                    }
                }

                return val;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return null;
            }
        }

    }
}
