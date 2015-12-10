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
    public class ProcessorsInfo
    {

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/aa394373(v=vs.85).aspx
        /// </summary>
        /// <returns></returns>
        public static UInt32 MaxClockSpeed()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select MaxClockSpeed from Win32_Processor");

                UInt32 val = 0;
                foreach (var item in searcher.Get())
                {
                    UInt32 temp = (UInt32)item["MaxClockSpeed"];
                    if (temp > 0)
                        val = temp;
                }

                return val;
            }
            catch(Exception ex)
            {
                Output.WriteException(ex);
                return 0;
            }
        }

        public static UInt32 CurrentClockSpeed()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select CurrentClockSpeed from Win32_Processor");

                UInt32 val = 0;
                foreach (var item in searcher.Get())
                {
                    UInt32 temp = (UInt32)item["CurrentClockSpeed"];
                    if (temp > 0)
                        val = temp;
                }

                return val;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return 0;
            }
        }//NumberOfCores

        public static UInt32 NumberOfCores()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select NumberOfCores from Win32_Processor");

                UInt32 val = 0;
                foreach (var item in searcher.Get())
                {
                    UInt32 temp = (UInt32)item["NumberOfCores"];
                    if (temp > 0)
                        val = temp;
                }

                return val;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return 0;
            }
        }


        public static UInt32 NumberOfEnabledCore()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select NumberOfEnabledCore from Win32_Processor");

                UInt32 val = 0;
                foreach (var item in searcher.Get())
                {
                    UInt32 temp = (UInt32)item["NumberOfEnabledCore"];
                    if (temp > 0)
                        val = temp;
                }

                return val;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return 0;
            }
        }

        public static UInt16 Architecture()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("select Architecture from Win32_Processor");

                UInt16 val = 0;
                foreach (var item in searcher.Get())
                {
                    UInt16 temp = (UInt16)item["Architecture"];
                    if (temp > 0)
                        val = temp;
                }

                return val;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return 0;
            }
        }


        public static string ProcessorId
        {
            get
            {
                try
                {
                    object val = ProcessorsInfo.GetProperty("ProcessorId");

                    if (val != null)
                        return (string)val;
                    else return null;
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
                string format = string.Format("select {0} from Win32_Processor", property);
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

    }
}
