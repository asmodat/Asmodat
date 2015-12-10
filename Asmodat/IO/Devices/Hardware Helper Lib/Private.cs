using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
/*
 * HardwareHelperLib
 * ===========================================================
 * Windows XP SP2, VS2005 C#.NET, DotNet 2.0
 * HH Lib is a hardware helper for library for C#.
 * It can be used for notifications of hardware add/remove
 * events, retrieving a list of hardware currently connected,
 * and enabling or disabling devices.
 * ===========================================================
 * LOG:      Who?    When?       What?
 * (v)1.0.0  WJF     11/26/07    Original Implementation
 */
namespace Asmodat.IO.Devices.HardwareHelperLib
{


    public partial class HardwareHelper
    {
      
        private bool ResetIt(IntPtr hDevInfo, Native.SP_DEVINFO_DATA devInfoData)
        {
            int szOfPcp;
            IntPtr ptrToPcp;
            int szDevInfoData;
            IntPtr ptrToDevInfoData;

            Native.SP_PROPCHANGE_PARAMS pcp = new Native.SP_PROPCHANGE_PARAMS();
            pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Native.SP_CLASSINSTALL_HEADER));
            pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE;
            pcp.StateChange = Native.DICS_PROPCHANGE; // for reset
            pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC;
            pcp.HwProfile = 0;

            szOfPcp = Marshal.SizeOf(pcp);
            ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
            Marshal.StructureToPtr(pcp, ptrToPcp, true);
            szDevInfoData = Marshal.SizeOf(devInfoData);
            ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);
            Marshal.StructureToPtr(devInfoData, ptrToDevInfoData, true);
            
            bool rslt1 = Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(Native.SP_PROPCHANGE_PARAMS)));
            bool rstl2 = Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);

            if (rslt1 && rstl2)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Inputs:   pointer to hdev, SP_DEV_INFO, bool
        /// Errors:   This method may throw the following exceptions. Unable to change device state!
        /// Remarks:  Attempts to enable or disable a device driver. does not check the reboot flag 
        /// Some devices require you reboot the OS for the change to take affect.  
        /// If this describes your device, you will need to look at the SDK call:
        /// SetupDiGetDeviceInstallParams.  You can call it directly after 
        /// ChangeIt to see whether or not you need to reboot the OS for you change to go into effect.
        /// </summary>
        /// <param name="hDevInfo"></param>
        /// <param name="devInfoData"></param>
        /// <param name="bEnable"></param>
        /// <returns>bool</returns>
        private bool ChangeIt(IntPtr hDevInfo, Native.SP_DEVINFO_DATA devInfoData, bool bEnable)
        {
            try
            {
                //Marshalling vars
                int szOfPcp;
                IntPtr ptrToPcp;
                int szDevInfoData;
                IntPtr ptrToDevInfoData;

                Native.SP_PROPCHANGE_PARAMS pcp = new Native.SP_PROPCHANGE_PARAMS();
                if (bEnable)
                {
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Native.SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE;
                    pcp.StateChange = Native.DICS_ENABLE;
                    pcp.Scope = Native.DICS_FLAG_GLOBAL;
                    pcp.HwProfile = 0;
                    
                    //Marshal the params
                    szOfPcp = Marshal.SizeOf(pcp);
                    ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
                    Marshal.StructureToPtr(pcp, ptrToPcp, true);
                    szDevInfoData = Marshal.SizeOf(devInfoData);
                    ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);

                    //Marshal.StructureToPtr(devInfoData, ptrToDevInfoData, true);//fix ?
                    if (Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(Native.SP_PROPCHANGE_PARAMS))))
                    {
                        Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);
                    }
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Native.SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE;
                    pcp.StateChange = Native.DICS_ENABLE;
                    pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC;
                    pcp.HwProfile = 0;
                }
                else
                {
                    pcp.ClassInstallHeader.cbSize = Marshal.SizeOf(typeof(Native.SP_CLASSINSTALL_HEADER));
                    pcp.ClassInstallHeader.InstallFunction = Native.DIF_PROPERTYCHANGE;
                    pcp.StateChange = Native.DICS_DISABLE;
                    pcp.Scope = Native.DICS_FLAG_CONFIGSPECIFIC;
                    pcp.HwProfile = 0;
                }
                //Marshal the params
                szOfPcp = Marshal.SizeOf(pcp);
                ptrToPcp = Marshal.AllocHGlobal(szOfPcp);
                Marshal.StructureToPtr(pcp, ptrToPcp, true);
                szDevInfoData = Marshal.SizeOf(devInfoData);
                ptrToDevInfoData = Marshal.AllocHGlobal(szDevInfoData);
                Marshal.StructureToPtr(devInfoData, ptrToDevInfoData,true);

                bool rslt1 = Native.SetupDiSetClassInstallParams(hDevInfo, ptrToDevInfoData, ptrToPcp, Marshal.SizeOf(typeof(Native.SP_PROPCHANGE_PARAMS)));
                bool rstl2 = Native.SetupDiCallClassInstaller(Native.DIF_PROPERTYCHANGE, hDevInfo, ptrToDevInfoData);
                if ((!rslt1) || (!rstl2))
                {
                    Exceptions.Write(new Exception("Unable to change device state!"));
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Exceptions.Write(ex);
                return false;
            }
        }
    }
}
