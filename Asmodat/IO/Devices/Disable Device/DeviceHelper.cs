
using System;
using System.Text;
using System.Collections.Generic;
using Asmodat.IO.Devices.DisableDevice;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;
using System.Security;
using System.Runtime.ConstrainedExecution;
using System.Management;

using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Debugging;
using Asmodat.Types;

namespace Asmodat.IO.Devices.DisableDevice
{

   

   
    

    public  sealed partial class DeviceHelper
    {

        private DeviceHelper()
        {
        }

        /// <summary>
        /// Enable or disable a device.
        /// </summary>
        /// <param name="classGuid">The class guid of the device. Available in the device manager.</param>
        /// <param name="instanceId">The device instance id of the device. Available in the device manager.</param>
        /// <param name="enable">True to enable, False to disable.</param>
        /// <remarks>Will throw an exception if the device is not Disableable.</remarks>
        public static void SetDeviceEnabled(Guid classGuid, string instanceId, bool enable)
        {
            SafeDeviceInfoSetHandle diSetHandle = null;
            try
            {
                // Get the handle to a device information set for all devices matching classGuid that are present on the 
                // system.
                diSetHandle = NativeMethods.SetupDiGetClassDevs(ref classGuid, null, IntPtr.Zero, SetupDiGetClassDevsFlags.Present);
                // Get the device information data for each matching device.
                DeviceInfoData[] diData = GetDeviceInfoData(diSetHandle);
                // Find the index of our instance. i.e. the touchpad mouse - I have 3 mice attached...
                int index = GetIndexOfInstance(diSetHandle, diData, instanceId);
                // Disable...
                EnableDevice(diSetHandle, diData[index], enable);
            }
            finally
            {
                if (diSetHandle != null)
                {
                    if (diSetHandle.IsClosed == false)
                    {
                        diSetHandle.Close();
                    }
                    diSetHandle.Dispose();
                }
            }
        }



        private static DeviceInfoData[] GetDeviceInfoData(SafeDeviceInfoSetHandle handle)
        {
            List<DeviceInfoData> data = new List<DeviceInfoData>();
            DeviceInfoData did = new DeviceInfoData();
            int didSize = Marshal.SizeOf(did);
            did.Size = didSize;
            int index = 0;
            while (NativeMethods.SetupDiEnumDeviceInfo(handle, index, ref did))
            {
                data.Add(did);
                index += 1;
                did = new DeviceInfoData();
                did.Size = didSize;
            }
            return data.ToArray();
        }

        // Find the index of the particular DeviceInfoData for the instanceId.
        private static int GetIndexOfInstance(SafeDeviceInfoSetHandle handle, DeviceInfoData[] diData, string instanceId)
        {
            const int ERROR_INSUFFICIENT_BUFFER = 122;
            for (int index = 0; index <= diData.Length - 1; index++)
            {
                StringBuilder sb = new StringBuilder(1);
                int requiredSize = 0;
                bool result = NativeMethods.SetupDiGetDeviceInstanceId(handle.DangerousGetHandle(), ref diData[index], sb, sb.Capacity, out requiredSize);
                if (result == false && Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    sb.Capacity = requiredSize;
                    result = NativeMethods.SetupDiGetDeviceInstanceId(handle.DangerousGetHandle(), ref diData[index], sb, sb.Capacity, out requiredSize);
                }
                if (result == false)
                    throw new Win32Exception();
                if (instanceId.Equals(sb.ToString()))
                {
                    return index;
                }
            }
            // not found
            return -1;
        }

        


        // enable/disable...
        private static void EnableDevice(SafeDeviceInfoSetHandle handle, DeviceInfoData diData, bool enable)
        {

            PropertyChangeParameters @params = new PropertyChangeParameters();
            // The size is just the size of the header, but we've flattened the structure.
            // The header comprises the first two fields, both integer.
            @params.Size = 8;

            // if (IntPtr.Size == 8)   @params.Size = 8; else @params.Size = 4 + Marshal.SystemDefaultCharSize;


            @params.DiFunction = DiFunction.PropertyChange;
            @params.Scope = Scopes.Global;
            if (enable)
                @params.StateChange = StateChangeAction.Enable;
            else
                @params.StateChange = StateChangeAction.Disable;

            bool result = NativeMethods.SetupDiSetClassInstallParams(handle, ref diData, ref @params, Marshal.SizeOf(@params));
            if (result == false) throw new Win32Exception();
            result = NativeMethods.SetupDiCallClassInstaller(DiFunction.PropertyChange, handle, ref diData);
            if (result == false)
            {
                int err = Marshal.GetLastWin32Error();
                if (err == (int)SetupApiError.NotDisableable)
                    throw new ArgumentException("Device can't be disabled (programmatically or in Device Manager).");
                else if (err >= (int)SetupApiError.NoAssociatedClass && err <= (int)SetupApiError.OnlyValidateViaAuthenticode)
                    throw new Win32Exception("SetupAPI error: " + ((SetupApiError)err).ToString());
                else
                    throw new Win32Exception();
            }
        }


        

        public static void SetDevicesEnabled(Guid classGuid, bool enable)
        {
            SafeDeviceInfoSetHandle diSetHandle = null;
            try
            {
                // Get the handle to a device information set for all devices matching classGuid that are present on the 
                // system.
                diSetHandle = NativeMethods.SetupDiGetClassDevs(ref classGuid, null, IntPtr.Zero, SetupDiGetClassDevsFlags.Present);
                // Get the device information data for each matching device.
                DeviceInfoData[] diData = GetDeviceInfoData(diSetHandle);

                for (int i = 0; i < diData.Length; i++)
                {
                    try
                    {
                        EnableDevice(diSetHandle, diData[i], enable);
                    }
                    catch
                    {

                    }
                }
            }
            finally
            {
                if (diSetHandle != null)
                {
                    if (diSetHandle.IsClosed == false)
                    {
                        diSetHandle.Close();
                    }
                    diSetHandle.Dispose();
                }
            }
        }

        public static void SetDeviceEnabled(string classGuid, string instanceId, bool enable)
        {
            if (classGuid.IsNullOrWhiteSpace() || instanceId.IsNullOrWhiteSpace())
                return;

            Guid guid = new Guid(classGuid);
            SafeDeviceInfoSetHandle diSetHandle = NativeMethods.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, SetupDiGetClassDevsFlags.Present);
            DeviceInfoData[] diData = GetDeviceInfoData(diSetHandle);
            int index = GetIndexOfInstance(diSetHandle, diData, instanceId);

            EnableDevice(diSetHandle, diData[index], enable);

            if (diSetHandle != null)
            {
                if (diSetHandle.IsClosed == false)
                {
                    diSetHandle.Close();
                }
                diSetHandle.Dispose();
            }
        }


        public static void SetDevicesEnabled(string classGuid, bool enable)
        {
            if (classGuid.IsNullOrWhiteSpace())
                return;

            Guid guid = new Guid(classGuid);
            SafeDeviceInfoSetHandle diSetHandle = NativeMethods.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, SetupDiGetClassDevsFlags.Present);
            DeviceInfoData[] diData = GetDeviceInfoData(diSetHandle);

            if (diData.IsNullOrEmpty())
                return;

            ExceptionBuffer Exceptions = new ExceptionBuffer();

            foreach (var data in diData)
            {
                try
                {
                    EnableDevice(diSetHandle, data, enable);
                }
                catch (Exception ex)
                {
                    Exceptions.Write(ex);
                    continue;
                }
            }

            if (diSetHandle != null)
            {
                if (diSetHandle.IsClosed == false)
                {
                    diSetHandle.Close();
                }
                diSetHandle.Dispose();
            }
        }



        public static void SetDevicesEnabled(string classGuid, string instance_match, bool enable)
        {
            if (classGuid.IsNullOrWhiteSpace())
                return;

            Guid guid = new Guid(classGuid);
            SafeDeviceInfoSetHandle diSetHandle = NativeMethods.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, SetupDiGetClassDevsFlags.Present);
            DeviceInfoData[] diData = GetDeviceInfoData(diSetHandle);

            if (diData.IsNullOrEmpty())
                return;

            int[] indexes = GetIndexesOfInstance(diSetHandle, diData, instance_match);

            if (indexes.IsNullOrEmpty())
                return;

            ExceptionBuffer Exceptions = new ExceptionBuffer();

            foreach (int idx in indexes)
            {
                try
                {
                    EnableDevice(diSetHandle, diData[idx], enable);
                }
                catch(Exception ex)
                {
                    Exceptions.Write(ex);
                    continue;
                }
            }
            

            if (diSetHandle != null)
            {
                if (diSetHandle.IsClosed == false)
                {
                    diSetHandle.Close();
                }
                diSetHandle.Dispose();
            }
        }


        public static void SetDevicesEnabledByInstancePatch(string instance_match, bool enable)
        {
            if (instance_match.IsNullOrWhiteSpace())
                return;
            
            Guid guid = Guid.Empty;
            SafeDeviceInfoSetHandle diSetHandle = NativeMethods.SetupDiGetClassDevs(ref guid, null, IntPtr.Zero, SetupDiGetClassDevsFlags.AllClasses);
            DeviceInfoData[] diData = GetDeviceInfoData(diSetHandle);

            if (diData.IsNullOrEmpty())
                return;

            int[] indexes = GetIndexesOfInstance(diSetHandle, diData, instance_match);

            if (indexes.IsNullOrEmpty())
                return;

            ExceptionBuffer Exceptions = new ExceptionBuffer();

            TasksManager Tasks = new TasksManager(indexes.Length);
            foreach (int idx in indexes)
            {
                Tasks.Run(() => TryEnable(diSetHandle, diData[idx], enable), "" + idx, true);
                /*
                try
                {
                    ResetDevice(diSetHandle, diData[idx]);
                    ++success_counter;
                }
                catch (Exception ex)
                {
                    Exceptions.Write(ex);
                    continue;
                }*/
            }

            /*foreach (int idx in indexes)
            {
                try
                {
                    EnableDevice(diSetHandle, diData[idx], enable);
                }
                catch (Exception ex)
                {
                    Exceptions.Write(ex);
                    continue;
                }
            }*/

            Tasks.JoinStopAll(100);

            if (diSetHandle != null)
            {
                if (diSetHandle.IsClosed == false)
                {
                    diSetHandle.Close();
                }
                diSetHandle.Dispose();
            }
        }

        private static void TryEnable(SafeDeviceInfoSetHandle handle, DeviceInfoData diData, bool enable)
        {
            try
            {

                EnableDevice(handle, diData, enable);
            }
            catch
            {

            }
        }

        private static int[] GetIndexesOfInstance(SafeDeviceInfoSetHandle handle, DeviceInfoData[] diData, string instanceId_match)
        {
            if (instanceId_match.IsNullOrEmpty())
                return null;

            instanceId_match = instanceId_match.GetLettersOrDigitsString();
            instanceId_match = instanceId_match.ToLower();

            if (instanceId_match.IsNullOrEmpty())
                return null;

            List<int> list = new List<int>();
            const int ERROR_INSUFFICIENT_BUFFER = 122;
            for (int index = 0; index <= diData.Length - 1; index++)
            {
                StringBuilder sb = new StringBuilder(1);
                int requiredSize = 0;
                bool result = NativeMethods.SetupDiGetDeviceInstanceId(handle.DangerousGetHandle(), ref diData[index], sb, sb.Capacity, out requiredSize);
                if (result == false && Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    sb.Capacity = requiredSize;
                    result = NativeMethods.SetupDiGetDeviceInstanceId(handle.DangerousGetHandle(), ref diData[index], sb, sb.Capacity, out requiredSize);
                }
                if (result == false)
                    continue;

                string ids = sb.ToString();

                ids = ids.GetLettersOrDigitsString();
                ids = ids.ToLower();

                if (ids.IsNullOrEmpty())
                    continue;

                

                if (ids.Contains(instanceId_match))
                {
                    list.Add(index);
                }
            }

            return list.ToArray();
        }




        


    }
}