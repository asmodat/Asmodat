using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

using System.Net.NetworkInformation;

using System.Management;

namespace Asmodat.Networking
{
    public class WMITasks
    {
        public List<string> GetEnabledNetworkAdapters()
        {
            string sWQuery =
@"SELECT DeviceID, Productname, Description,
 NetEnabled, NetConnectionStatus, NetConnectionID
 FROM Win32_NetworkAdapter
 WHERE Manufacturer <> 'Microsoft' ";

            ObjectQuery OQuery = new ObjectQuery(sWQuery);
            ManagementObjectSearcher MOSearcher = new ManagementObjectSearcher(OQuery);
            ManagementObjectCollection MOCollection = MOSearcher.Get();

            List<string> LSMOEnabled = new List<string>();
            foreach (ManagementObject MO in MOCollection)
            {
                if (MO["NetEnabled"] == null || MO["DeviceID"] == null) continue;

                //string sNC = MO["NetConnectionStatus"].ToString();
                string sNeyEnabled = MO["NetEnabled"].ToString();
                string sDeviceID = MO["DeviceID"].ToString();
                if (!System.Convert.ToBoolean(sNeyEnabled)) continue;
                else LSMOEnabled.Add(sDeviceID);
            }

            return LSMOEnabled;
        }

        public void SetNetworkAdaptersInvoke(string DeviceID, string MethodName)
        {
            string sWQuery =
@"SELECT DeviceID, Productname, Description,
 NetEnabled, NetConnectionStatus, NetConnectionID 
 FROM Win32_NetworkAdapter
 WHERE DeviceID = " + DeviceID;

            ObjectQuery OQuery = new ObjectQuery(sWQuery);
            ManagementObjectSearcher MOSearcher = new ManagementObjectSearcher(OQuery);
            ManagementObjectCollection MOCollection = MOSearcher.Get();

            ManagementObject MOSelected = null;
            foreach (ManagementObject MO in MOCollection)
                MOSelected = MO;


            if (MOSelected == null) return;

            MOSelected.InvokeMethod(MethodName, null);
        }
    }
}
