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
using Asmodat.Abbreviate;
using Asmodat.Debugging;

namespace Asmodat
{

    public partial class KeyboardUsing
    {
        VirtualKeyCodes AVKCodes = new VirtualKeyCodes();

        public ThreadedDictionary<int, string> Codes { get; private set; }
        public int[] CodeKeys { get; private set; }
        public string[] CodeValues { get; private set; }

        public int CodesCounter { get; private set; }
        public KeyboardUsing()
        {
            Codes = (ThreadedDictionary<int, string>)AVKCodes.Data;
            CodeKeys = Codes.KeysArray;
            CodeValues = Codes.ValuesArray;
            CodesCounter = AVKCodes.CodesCounter;
        }

        public byte[] GetKeysStates()
        {
            try
            {
                byte[] kyboardState = new byte[256];
                
                int key, i;
                for (i = 0; i < CodesCounter; i++)
                {
                    key = CodeKeys[i];
                    if (key < 0 || key > 255)
                        continue;

                    kyboardState[key] = (byte)Keyboard.GetKeyState(key);
                }

                return kyboardState;
            }
            catch(Exception ex)
            {
                Output.WriteException(ex);
                return null;
            }
        }



        public bool SetKeySendInputDX(ushort keyCode,bool keyDown)
        {

                Keyboard.INPUT AKINPUT = new Keyboard.INPUT();
                
                AKINPUT.ki.wScan = 0;

                bool succesedSearch = false;

                for (int i = 0; i < AVKCodes.CodesCounter; i++)
                {
                    if (ushort.Parse(AVKCodes.Codes[i, 1], NumberStyles.HexNumber) == keyCode)
                    {

                        for (int i1 = 0; i1 < AVKCodes.CounterCodesDx; i1++)
                        {
                            if (AVKCodes.Codes[i, 0] == AVKCodes.CodesDx[i1, 0])
                            {
                                AKINPUT.ki.wScan = ushort.Parse(AVKCodes.CodesDx[i1, 1]);
                                succesedSearch = true;
                                break;
                            }
                        }
                    }
                }

                if (succesedSearch)
                {


                    if (keyDown)
                    {
                        AKINPUT.ki.dwFlags = 0;

                    }
                    else
                    {
                        AKINPUT.ki.dwFlags = (ushort)Keyboard.dwFlgsSI.SCANCODE;
                        AKINPUT.ki.dwFlags += (ushort)Keyboard.dwFlgsSI.KEYUP;
                    }

                    AKINPUT.type = (uint)Keyboard.Win32.INPUT_KEYBOARD;
                    AKINPUT.ki.wVk = (UInt16)keyCode;
                    AKINPUT.ki.dwExtraInfo = IntPtr.Zero;
                    AKINPUT.ki.time = 0;


                    Keyboard.SendInput(1, ref AKINPUT, Marshal.SizeOf(new Keyboard.INPUT()));

                    return true;
                }
                else
                {

                } return false;

        }


        public void SetKeySendInputDX2(ushort keyCode, bool keyDown)
        {

            Keyboard.INPUT AKINPUT = new Keyboard.INPUT();
            AKINPUT.type = (uint)Keyboard.Win32.INPUT_KEYBOARD;

            AKINPUT.ki.wScan = keyCode;// 0;
            AKINPUT.ki.time = 0;

            if (keyDown)
                AKINPUT.ki.dwFlags = (ushort)Keyboard.dwFlgsSI.KEYDOWN | (ushort)Keyboard.dwFlgsSI.SCANCODE;
            else
                AKINPUT.ki.dwFlags = (ushort)Keyboard.dwFlgsSI.KEYUP | (ushort)Keyboard.dwFlgsSI.SCANCODE;
            

            AKINPUT.type = 1;// (uint)Keyboard.Win32.INPUT_KEYBOARD;
            //AKINPUT.ki.wVk = (UInt16)keyCode;
            AKINPUT.ki.dwExtraInfo = IntPtr.Zero;
            AKINPUT.ki.time = 0;


            Keyboard.SendInput(1, ref AKINPUT, Marshal.SizeOf(AKINPUT));

        }


        public void SetKeySendInput(ushort keyCode, bool keyDown)
        {
            if (SetKeySendInputDX(keyCode, keyDown)) return;

            Keyboard.INPUT AKINPUT = new Keyboard.INPUT();
            AKINPUT.type = (uint)Keyboard.Win32.INPUT_KEYBOARD;

            AKINPUT.ki.wScan = 0;
            AKINPUT.ki.time = 0;
            AKINPUT.ki.wVk = keyCode;

            if (keyDown)
            {
                AKINPUT.ki.dwFlags = 0;
            }
            else
            {
                AKINPUT.ki.dwFlags = (ushort)Keyboard.dwFlgsSI.KEYUP;
            }


            Keyboard.SendInput(1, ref AKINPUT, Marshal.SizeOf(new Keyboard.INPUT()));
        }


        public void SetAllKeys(bool up)
        {
            for (byte b = 0; b < byte.MaxValue; b++)
            {
                if (this.GetKeyState(b))
                {
                    if (!up)
                    {
                        Keyboard.keybd_event(b, 0, (uint)Keyboard.dwFlags.KE_SysKeyDown, 0);
                    }
                    else { Keyboard.keybd_event(b, 0, (uint)Keyboard.dwFlags.KE_SysKeyUp, 0); }
                }
            }
        }


        public void Click(byte keyCode)
        {
            Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.KE_SysKeyUp, 0);
            Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.KE_SysKeyDown, 0);
            Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.KE_SysKeyUp, 0);
        }


        public void SetKeySysWM(byte keyCode, bool keyDown)
        {
            if (keyDown)
            {

                Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.WM_SysKeyDown, 0);

            }
            else
            {
                Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.WM_SysKeyUp, 0);
            }
        }

        public void SetKeyWM(byte keyCode, bool keyDown)
        {
            if (keyDown)
            {

                Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.WM_KeyDown, 0);

            }
            else
            {
                Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.WM_KeyUp, 0);
            }
        }

        public void SetKeySysKE(byte keyCode, bool keyDown)
        {
            if (keyDown)
            {

                Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.KE_SysKeyDown, 0);

            }
            else
            {
                Keyboard.keybd_event(keyCode, 0, (uint)Keyboard.dwFlags.KE_SysKeyUp, 0);
            }
        }




        public bool GetKeyState(int keyCode)
        {

            short stat = Keyboard.GetKeyState(keyCode);

            if (stat == 0 || stat == 1)
            {
                return false;
            }
            else
            {
                
                return true;
            }
        }

        public bool GetKeyStatus(short stat)
        {
            if (stat == 0 || stat == 1)
            {
                return false;
            }
            else
            {

                return true;
            }
        }


        public short GetKeyStateCode(int keyCode)
        {
            return Keyboard.GetKeyState(keyCode);

            //0 zwolniony
            //-127 wciśnięty
            //1 puszczony
            //-128 wciśnięty
        }




        
    }

    
}