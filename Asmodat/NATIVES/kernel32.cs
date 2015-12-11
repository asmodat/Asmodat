using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Natives
{


    public static class kernel32
    {
        [DllImport("kernel32.dll", EntryPoint ="RtlMoveMemory")]
        public static extern int CopyMemory(IntPtr destination, IntPtr source, int length);
    }
}
