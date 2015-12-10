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

using System.Net.NetworkInformation;
using System.Net.Sockets;
using Asmodat.Debugging;
using System.Text.RegularExpressions;
using System.Management;
using Asmodat.IO;

namespace Asmodat.Resources
{
    public class DrivesInfo
    {

        public static long AvailableFreeSpace(string driveName = null)
        {
            try
            {
                if (driveName == null)
                    driveName = Files.ExeRoot;

                var drive = new System.IO.DriveInfo(driveName);

                return drive.AvailableFreeSpace;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return 0;
            }
        }

        public static long TotalFreeSpace(string driveName = null)
        {
            try
            {
                if (driveName == null)
                    driveName = Files.ExeRoot;

                var drive = new System.IO.DriveInfo(driveName);

                return drive.TotalFreeSpace;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return 0;
            }
        }


        public static string SystemDriveID
        {
            get
            {
                try
                {
                    string drive = Directories.SystemDrive;
                    ManagementObject obj = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive[0] + @":""");
                    obj.Get();
                    string serial = obj["VolumeSerialNumber"].ToString();
                    return serial;
                }
                catch (Exception ex)
                {
                    ex.ToOutput();
                    return null;
                }
            }
        }


    }
}
