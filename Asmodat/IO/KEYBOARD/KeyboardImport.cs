using System;

using System.IO;

using System.Drawing;
using System.Drawing.Imaging;

using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;

using System.Windows.Forms;
using System.Text;

namespace Asmodat
{

    public partial class Keyboard
    {
        
        [DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(byte keyCode);

        [DllImport("user32.dll")]
        public static extern int GetKeyboardState(byte[] lpKeyState);


        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "SendInput", SetLastError = true)]
        public static extern uint SendInput( uint nInputs,ref INPUT pInput, int cbSize);

        [DllImport("user32.dll")]
        public static extern int MapVirtualKey(uint uCode, uint uMapType);

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint virtualKeyCode, 
            uint scanCode, byte[] keyboardState, 
            [Out, MarshalAs(UnmanagedType.LPWStr,SizeConst =64)] StringBuilder receivingBuffer, 
            int bufferSize, uint flags);


        [StructLayout(LayoutKind.Explicit, Size = 28)]
        public struct INPUT
        {
            [FieldOffset(0)]
            public uint type;
            [FieldOffset(4)]
            public KEYBOARDINPUT ki;
        }

        public struct KEYBOARDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public ushort dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        public enum Win32
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2,
        }



        public enum dwFlgsSI
        {
            KEYDOWN = 0x0000,
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }

        public enum dwFlags
        {
            KE_SysKeyDown = 0x01,
            KE_SysKeyUp = 0x02,
            WM_KeyDown = 0x100,
            WM_KeyUp = 0x101,
            WM_SysKeyDown = 0x104,
            WM_SysKeyUp = 0x105
        }


        [Flags]
        private enum KeyStates
        {
            None = 0,
            Down = 1,
            Toggled = 2
        }

        /*
        KeyCodes[++i, 0] = "Shift"; KeyCodes[i, 1] = "10";
            KeyCodes[++i, 0] = "Control"; KeyCodes[i, 1] = "11";
            KeyCodes[++i, 0] = "Alt"; KeyCodes[i, 1] = "12";*/
        public static string KeyToString(int key)
        {
            try
            {
                StringBuilder buf = new StringBuilder(256);
                byte[] kyboardState = new byte[256];

                for(int i = 0; i < 256; i++)
                    kyboardState[i] = (byte)GetKeyState(i);
                

                Keyboard.ToUnicode((uint)key, 0, kyboardState, buf, 256, 0);

                return buf.ToString();

            }
            catch
            {
                return null;
            }
        }

        public static string KeyToString(int key, byte[] kyboardState)
        {
            if (kyboardState.Length != 256)
                throw new ArgumentException("KeyboardState must be 256 length", "kyboardState");

            try
            {
                StringBuilder buf = new StringBuilder(256);
                Keyboard.ToUnicode((uint)key, 0, kyboardState, buf, 256, 0);
                return buf.ToString();
            }
            catch
            {
                return null;
            }
        }

        public static char? KeyToChar(int key)
        {
            try
            {
                int nonvirtual = Keyboard.MapVirtualKey((uint)key, 2);
                return Convert.ToChar(nonvirtual);
            }
            catch
            {
                return null;
            }
        }


        private static KeyStates GetState(int key)
        {
            KeyStates state = KeyStates.None;

            short value = Keyboard.GetKeyState(key);

            if ((value & 0x8000) == 0x8000)
                state |= KeyStates.Down;

            if ((value & 1) == 1)
                state |= KeyStates.Toggled;

            return state;
        }

        public static bool IsKeyDown(int key)
        {
            return KeyStates.Down == (GetState(key) & KeyStates.Down);
        }

        public static bool IsKeyToggled(int key)
        {
            return KeyStates.Toggled == (GetState(key) & KeyStates.Toggled);
        }
    }

    
}