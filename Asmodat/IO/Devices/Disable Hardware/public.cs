using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.IO.Devices.DisableHardware
{
    public static partial class DisableHardware
    {

        public static void DisableDevice(string match, bool disable = true)
        {
            DisableDevice(n => n.ToUpperInvariant().Contains(match), disable);
        }

        public static void DisableDevice(Func<string, bool> filter, bool disable = true)
        {
            IntPtr info = IntPtr.Zero;
            Guid NullGuid = Guid.Empty;
            try
            {
                info = SetupDiGetClassDevsW(
                    ref NullGuid,
                    null,
                    IntPtr.Zero,
                    DIGCF_ALLCLASSES);
                CheckError("SetupDiGetClassDevs");

                SP_DEVINFO_DATA devdata = new SP_DEVINFO_DATA();
                devdata.cbSize = (UInt32)Marshal.SizeOf(devdata);

                // Get first device matching device criterion.
                for (uint i = 0; ; i++)
                {
                    SetupDiEnumDeviceInfo(info,
                        i,
                        out devdata);
                    // if no items match filter, throw
                    if (Marshal.GetLastWin32Error() == ERROR_NO_MORE_ITEMS)
                        CheckError("No device found matching filter.", 0xcffff);
                    CheckError("SetupDiEnumDeviceInfo");

                    string devicepath = GetStringPropertyForDevice(info,
                                               devdata, 1); // SPDRP_HARDWAREID

                    // Uncomment to print name/path
                    //Console.WriteLine(GetStringPropertyForDevice(info,
                    //                         devdata, DEVPKEY_Device_DeviceDesc));
                    //Console.WriteLine("   {0}", devicepath);


                    if (devicepath != null && filter(devicepath)) break;

                }

                SP_CLASSINSTALL_HEADER header = new SP_CLASSINSTALL_HEADER();
                header.cbSize = (UInt32)Marshal.SizeOf(header);
                header.InstallFunction = DIF_PROPERTYCHANGE;

                SP_PROPCHANGE_PARAMS propchangeparams = new SP_PROPCHANGE_PARAMS();
                propchangeparams.ClassInstallHeader = header;
                propchangeparams.StateChange = disable ? DICS_DISABLE : DICS_ENABLE;
                propchangeparams.Scope = DICS_FLAG_GLOBAL;
                propchangeparams.HwProfile = 0;

                SetupDiSetClassInstallParams(info,
                    ref devdata,
                    ref propchangeparams,
                    (UInt32)Marshal.SizeOf(propchangeparams));
                CheckError("SetupDiSetClassInstallParams");

                SetupDiChangeState(
                    info,
                    ref devdata);
                CheckError("SetupDiChangeState");
            }
            finally
            {
                if (info != IntPtr.Zero)
                    SetupDiDestroyDeviceInfoList(info);
            }
        }


        public static string FindInstancePatch(string match)
        {
            return DisableHardware.FindInstancePatch(n => n.ToUpperInvariant().Contains(match));
        }

        public static string FindInstancePatch(Func<string, bool> filter)
        {
            IntPtr info = IntPtr.Zero;
            Guid NullGuid = Guid.Empty;
            try
            {
                info = SetupDiGetClassDevsW(
                    ref NullGuid,
                    null,
                    IntPtr.Zero,
                    DIGCF_ALLCLASSES);
                CheckError("SetupDiGetClassDevs");

                SP_DEVINFO_DATA devdata = new SP_DEVINFO_DATA();
                devdata.cbSize = (UInt32)Marshal.SizeOf(devdata);

                // Get first device matching device criterion.
                for (uint i = 0; i < 1024; i++)
                {
                    SetupDiEnumDeviceInfo(info,
                        i,
                        out devdata);
                    // if no items match filter, throw
                    if (Marshal.GetLastWin32Error() == ERROR_NO_MORE_ITEMS)
                        CheckError("No device found matching filter.", 0xcffff);
                    CheckError("SetupDiEnumDeviceInfo");

                    string devicepath = GetStringPropertyForDevice(info,
                                               devdata, 1); 

                    if (devicepath != null && filter(devicepath))
                        return devicepath;
                }
            }
            finally
            {
                if (info != IntPtr.Zero)
                    SetupDiDestroyDeviceInfoList(info);
            }
            return null;
        }

    }
}
