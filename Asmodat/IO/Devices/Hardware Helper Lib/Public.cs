using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;


using Asmodat.Debugging;


namespace Asmodat.IO.Devices.HardwareHelperLib
{


    public partial class HardwareHelper
    {
        ExceptionBuffer Exceptions = new ExceptionBuffer();


        //Name:     GetAll
        //Inputs:   none
        //Outputs:  string array
        //Errors:   This method may throw the following errors.
        //          Failed to enumerate device tree!
        //          Invalid handle!
        //Remarks:  This is code I cobbled together from a number of newsgroup threads
        //          as well as some C++ stuff I translated off of MSDN.  Seems to work.
        //          The idea is to come up with a list of devices, same as the device
        //          manager does.  Currently it uses the actual "system" names for the
        //          hardware.  It is also possible to use hardware IDs.  See the docs
        //          for SetupDiGetDeviceRegistryProperty in the MS SDK for more details.
        public string[] GetAll()
        {
            List<string> HWList = new List<string>();
            try
            {
                Guid myGUID = System.Guid.Empty;
                IntPtr hDevInfo = Native.SetupDiGetClassDevs(ref myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES | Native.DIGCF_PRESENT);
                if (hDevInfo.ToInt64() == Native.INVALID_HANDLE_VALUE)//changed from Int32
                {
                    throw new Exception("Invalid Handle");
                }
                Native.SP_DEVINFO_DATA DeviceInfoData;
                DeviceInfoData = new Native.SP_DEVINFO_DATA();

                DeviceInfoData.cbSize =  Marshal.SizeOf(typeof(Native.SP_DEVINFO_DATA));
                /*//added fix for x64 from DeviceInfoData.cbSize = 28;
                if (IntPtr.Size == 4)
                {
                    DeviceInfoData.cbSize = 28;// Marshal.SizeOf(DeviceInfoData);
                }
                else if (IntPtr.Size == 8)
                {
                    DeviceInfoData.cbSize = 32;
                }*/

                //is devices exist for class
                DeviceInfoData.devInst = 0;
                DeviceInfoData.classGuid = System.Guid.Empty;
                DeviceInfoData.reserved = 0;
                UInt32 i;
                StringBuilder DeviceName = new StringBuilder("");
                DeviceName.Capacity = Native.MAX_DEV_LEN;
                for (i = 0; Native.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
                {
                    //Declare vars
                    while (!Native.SetupDiGetDeviceRegistryProperty(hDevInfo,
                                                                    DeviceInfoData,
                                                                    Native.SPDRP_DEVICEDESC,
                                                                    0,
                                                                    DeviceName,
                                                                    Native.MAX_DEV_LEN,
                                                                    IntPtr.Zero))
                    {
                        break;
                        //Skip
                    }
                    HWList.Add(DeviceName.ToString());
                    HWList.Sort();
                }
                Native.SetupDiDestroyDeviceInfoList(hDevInfo);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to enumerate device tree!",ex);
            }
            return HWList.ToArray();
        }

        //Name:     SetDeviceState
        //Inputs:   string[],bool
        //Outputs:  bool
        //Errors:   This method may throw the following exceptions.
        //          Failed to enumerate device tree!
        //Remarks:  This is nearly identical to the method above except it
        //          tries to match the hardware description against the criteria
        //          passed in.  If a match is found, that device will the be
        //          enabled or disabled based on bEnable.
        public bool SetDeviceState(string match, bool bEnable)
        {
            bool success = false;
            try
            {
                Guid myGUID = System.Guid.Empty;
                IntPtr hDevInfo = Native.SetupDiGetClassDevs(ref myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES | Native.DIGCF_PRESENT);
                if (hDevInfo.ToInt64() == Native.INVALID_HANDLE_VALUE)//changed from Int32
                {
                    return false;
                }
                Native.SP_DEVINFO_DATA DeviceInfoData;
                DeviceInfoData = new Native.SP_DEVINFO_DATA();

                DeviceInfoData.cbSize = Marshal.SizeOf(typeof(Native.SP_DEVINFO_DATA));

                
                //is devices exist for class
                DeviceInfoData.devInst = 0;
                DeviceInfoData.classGuid = System.Guid.Empty;
                DeviceInfoData.reserved = 0;
                UInt32 i;
                StringBuilder DeviceName = new StringBuilder("");
                DeviceName.Capacity = Native.MAX_DEV_LEN;
                for (i = 0; Native.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
                {
                    if (!Native.SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, Native.SPDRP_DEVICEDESC, 0, DeviceName, Native.MAX_DEV_LEN, IntPtr.Zero))
                        continue;

                    if (DeviceName.ToString().ToLower().Contains(match))
                        if (this.ResetIt(hDevInfo, DeviceInfoData))
                            success = true;
                }
                Native.SetupDiDestroyDeviceInfoList(hDevInfo);
            }
            catch (Exception ex)
            {
                Exceptions.Write(new Exception("Failed to enumerate device tree!", ex));
                return false;
            }
            return success;
        }


        public bool ResetDevice(string match)
        {
            bool success = false;

            try
            {

                Guid myGUID = System.Guid.Empty;
                IntPtr hDevInfo = Native.SetupDiGetClassDevs(ref myGUID, 0, IntPtr.Zero, Native.DIGCF_ALLCLASSES | Native.DIGCF_PRESENT);
                if (hDevInfo.ToInt64() == Native.INVALID_HANDLE_VALUE)//changed from Int32
                {
                    return false;
                }
                Native.SP_DEVINFO_DATA DeviceInfoData;
                DeviceInfoData = new Native.SP_DEVINFO_DATA();

                DeviceInfoData.cbSize = Marshal.SizeOf(typeof(Native.SP_DEVINFO_DATA));

                //is devices exist for class
                DeviceInfoData.devInst = 0;
                DeviceInfoData.classGuid = System.Guid.Empty;
                DeviceInfoData.reserved = 0;
                UInt32 i;
                StringBuilder DeviceName = new StringBuilder("");
                DeviceName.Capacity = Native.MAX_DEV_LEN;
                for (i = 0; Native.SetupDiEnumDeviceInfo(hDevInfo, i, DeviceInfoData); i++)
                {
                    //Declare vars
                    if (!Native.SetupDiGetDeviceRegistryProperty(hDevInfo, DeviceInfoData, Native.SPDRP_DEVICEDESC, 0, DeviceName, Native.MAX_DEV_LEN, IntPtr.Zero))
                        continue;
                    
                    if (DeviceName.ToString().ToLower().Contains(match))
                        if (this.ResetIt(hDevInfo, DeviceInfoData))
                            success = true;
                }
                Native.SetupDiDestroyDeviceInfoList(hDevInfo);
            }
            catch (Exception ex)
            {
                Exceptions.Write(new Exception("Failed to enumerate device tree!", ex));
                return false;
            }

            return success;
        }


        //Name:     HookHardwareNotifications
        //Inputs:   Handle to a window or service, 
        //          Boolean specifying true if the handle belongs to a window
        //Outputs:  false if fail, otherwise true
        //Errors:   This method may log the following errors.
        //          NONE
        //Remarks:  Allow a window or service to receive ALL hardware notifications.
        //          NOTE: I have yet to figure out how to make this work properly
        //          for a service written in C#, though it kicks butt in C++.  At any
        //          rate, it works fine for windows forms in either.
        public bool HookHardwareNotifications(IntPtr callback, bool UseWindowHandle)
        {
            try
            {
                Native.DEV_BROADCAST_DEVICEINTERFACE dbdi = new Native.DEV_BROADCAST_DEVICEINTERFACE();
                dbdi.dbcc_size = Marshal.SizeOf(dbdi);
                dbdi.dbcc_reserved = 0;
                dbdi.dbcc_devicetype = Native.DBT_DEVTYP_DEVICEINTERFACE;
                if (UseWindowHandle)
                {
                    Native.RegisterDeviceNotification(callback, 
                        dbdi, 
                        Native.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES | 
                        Native.DEVICE_NOTIFY_WINDOW_HANDLE);
                }
                else
                {
                    Native.RegisterDeviceNotification(callback, 
                        dbdi, 
                        Native.DEVICE_NOTIFY_ALL_INTERFACE_CLASSES | 
                        Native.DEVICE_NOTIFY_SERVICE_HANDLE);
                }
                return true;
            }
            catch (Exception ex)
            {
                Exceptions.Write(ex);
                return false;
            }
        }
        //Name:     CutLooseHardareNotifications
        //Inputs:   handle used when hooking
        //Outputs:  None
        //Errors:   This method may log the following errors.
        //          NONE
        //Remarks:  Cleans up unmanaged resources.  
        public void CutLooseHardwareNotifications(IntPtr callback)
        {
            try
            {
                Native.UnregisterDeviceNotification(callback);
            }
            catch
            {
                //Just being extra cautious since the code is unmanged
            }
        }
    
       
    }
}
