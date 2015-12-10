
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

namespace Asmodat.IO.Devices.DisableDevice
{

    internal class SafeDeviceInfoSetHandle : SafeHandleZeroOrMinusOneIsInvalid
    {

        public SafeDeviceInfoSetHandle()
            : base(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.SetupDiDestroyDeviceInfoList(this.handle);
        }

    }
}