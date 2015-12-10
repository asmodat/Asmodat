using System;

using System.IO;
 

using System.Drawing;
using System.Drawing.Imaging;

using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;

using System.Windows.Forms;

namespace Asmodat
{

    public partial class Mouse
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern void mouse_event(UInt32 dwFlags, Int32 dx, Int32 dy, UInt32 cButtons, UIntPtr dwExtraInfo);// int dwExtraInfo);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point pt);

        [DllImport("user32.dll")]
        public static extern uint SendInput(ushort wVk, ushort wScan, uint dwFlags, uint time, IntPtr dwExtraInfo);

        //ushort wVk;         public ushort wScan;         public uint dwFlags;         public uint time;         public IntPtr dwExtraInfo


        public const UInt32 ME_RIGHTDOWN = 0x0008;
        public const UInt32 ME_RIGHTUP = 0x0010;
        public const UInt32 ME_LEFTDOWN = 0x02;
        public const UInt32 ME_LEFTUP = 0x04;
        public const UInt32 MIE_LEFTDOWN = 0x00000002;
        public const UInt32 MIE_LEFTUP = 0x00000004;
        public const UInt32 MIE_MIDDLEDOWN = 0x00000020;
        public const UInt32 MIE_MIDDLEUP = 0x00000040;
        public const UInt32 MIE_MOVE = 0x00000001;
        public const UInt32 MIE_ABSOLUTE = 0x00008000;
        public const UInt32 MIE_RIGHTDOWN = 0x00000008;
        public const UInt32 MIE_RIGHTUP = 0x00000010;



    }

    
}