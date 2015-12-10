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
        static DisableHardware()
        {
            DisableHardware.DEVPKEY_Device_DeviceDesc = new DEVPROPKEY();
            DEVPKEY_Device_DeviceDesc.fmtid = new Guid(
                    0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67,
                    0xd1, 0x46, 0xa8, 0x50, 0xe0);
            DEVPKEY_Device_DeviceDesc.pid = 2;

            DEVPKEY_Device_HardwareIds = new DEVPROPKEY();
            DEVPKEY_Device_HardwareIds.fmtid = new Guid(
                0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67,
                0xd1, 0x46, 0xa8, 0x50, 0xe0);
            DEVPKEY_Device_HardwareIds.pid = 3;
        }


        

        

    }
}
