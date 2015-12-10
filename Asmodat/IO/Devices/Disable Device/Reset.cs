
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
    public sealed partial class DeviceHelper
    {
        private static void ResetDevice(SafeDeviceInfoSetHandle handle, DeviceInfoData diData)
        {

            PropertyChangeParameters @params = new PropertyChangeParameters();
            // The size is just the size of the header, but we've flattened the structure.
            // The header comprises the first two fields, both integer.
            @params.Size = 8;

            // if (IntPtr.Size == 8)   @params.Size = 8; else @params.Size = 4 + Marshal.SystemDefaultCharSize;


            @params.DiFunction = DiFunction.PropertyChange;
            @params.Scope = Scopes.Global;

            @params.StateChange = StateChangeAction.PropChange; //reset

            

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

        public static void ResetDevicesByInstancePatch(string instance_match)
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
            //int success_counter = 0;
            foreach (int idx in indexes)
            {
                Tasks.Run(() => TryReset(diSetHandle, diData[idx]), "" + idx, true);
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


        private static void TryReset(SafeDeviceInfoSetHandle handle, DeviceInfoData diData)
        {
            try
            {
                ResetDevice(handle, diData);
            }
            catch
            {

            }
        }
    }
}