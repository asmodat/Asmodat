using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Extensions.Collections.Generic;
using System.Security;

namespace Asmodat.Natives
{

    public static class msvcrtEx
    {
        public static bool memcmp(byte[] arr1, byte[] arr2)
        {
            if (!arr1.EqualsCount(arr2))
                return false;
            else
                return msvcrt.memcmp(arr1, arr2, new UIntPtr((uint)arr1.Length)) == 0;
        }
    }

    public static class msvcrt
    {
        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int memcmp(byte[] arr1, byte[] arr2, UIntPtr cnt);

        [DllImport("mscrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false), SuppressUnmanagedCodeSecurity]
        public static unsafe extern void* CopyMemory(void* dest, void* src, ulong count);
    }
}
