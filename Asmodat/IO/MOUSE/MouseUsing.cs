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

    public partial class MouseUsing
    {
        int MX = 0;
        int MY = 0;
        int MXOld = 0;
        int MYOld = 0;

        public void SetMousePosition(int x, int y)
        {
            MX = x;
            MY = y;

            /*
            destX = xPos * 65535 / Screen.Width * Screen.TwipsPerPixelX
    destY = mPos.Y * 65535 / Screen.Height * Screen.TwipsPerPixelY
    mouse_event MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_MOVE, destX, destY, 0, 0
             */
            
            
            Mouse.SetCursorPos(MX, MY);

            
            //Mouse.mouse_event(Mouse.MIE_ABSOLUTE + Mouse.MIE_MOVE, (uint)(x * 65535), (uint)(y * 65535), 0, UIntPtr.Zero);
            
            MXOld = MX;
            MYOld = MY;
        }

        public Point GetMousePosition()
        {
            Point p = new Point();
            Mouse.GetCursorPos(out p);

            return p;
        }

        public void MouseEvent(uint code)
        {
           // Mouse.mouse_event(code, 0, 0, 0, 0);
        }



        //0 zwolniony
        //-127 wciśnięty
        //1 puszczony
        //-128 wciśnięty

        KeyboardUsing AKeyboardUsing = new KeyboardUsing();

        public void SetMouseLeftButton(int leftButtonCode)
        {
            //Console.Beep();

            if (AKeyboardUsing.GetKeyStatus((short)leftButtonCode))
            {
                // Console.Beep();UIntPtr.Zero
                Mouse.mouse_event(Mouse.ME_LEFTDOWN , 0, 0, 0, UIntPtr.Zero); //(Mouse.ME_LEFTDOWN, 0, 0, 0, 0);
            }
            else
            {
                Mouse.mouse_event(Mouse.ME_LEFTUP , 0, 0, 0, UIntPtr.Zero);
            }
        }

        public void SetMouseMiddleButton(int middleButtonCode)
        {
            if (AKeyboardUsing.GetKeyStatus((short)middleButtonCode))
            {
                Mouse.mouse_event(Mouse.MIE_MIDDLEDOWN, 0, 0, 0, UIntPtr.Zero);
            }
            else
            {
                Mouse.mouse_event(Mouse.MIE_MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
            }
        }

        public void SetMouseRightButton(int rightButtonCode)
        {
            if (AKeyboardUsing.GetKeyStatus((short)rightButtonCode))
            {
                Mouse.mouse_event(Mouse.ME_RIGHTDOWN , 0, 0, 0, UIntPtr.Zero);
            }
            else
            {
                Mouse.mouse_event(Mouse.ME_RIGHTUP , 0, 0, 0, UIntPtr.Zero);
            }
        }




        public void MouseClick(bool leftButton, bool middleButton, bool rightButton, byte count, int duration, int delay, int delayClick)
        {
            Thread.Sleep(delay);

            if (leftButton)
            {
                for (byte b = 0; b < count; b++)
                {
                    Thread.Sleep(delayClick);
                    Mouse.mouse_event(Mouse.ME_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                    Mouse.mouse_event(Mouse.ME_LEFTDOWN, 0, 0, 0, UIntPtr.Zero);
                    Thread.Sleep(duration);
                    Mouse.mouse_event(Mouse.ME_LEFTUP, 0, 0, 0, UIntPtr.Zero);
                }
            }

            if (middleButton)
            {
                for (byte b = 0; b < count; b++)
                {
                    Thread.Sleep(delayClick);
                     Mouse.mouse_event(Mouse.MIE_MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
                     Mouse.mouse_event(Mouse.MIE_MIDDLEDOWN, 0, 0, 0, UIntPtr.Zero);
                    Thread.Sleep(duration);
                     Mouse.mouse_event(Mouse.MIE_MIDDLEUP, 0, 0, 0, UIntPtr.Zero);
                }
            }

            if (rightButton)
            {
                for (byte b = 0; b < count; b++)
                {
                    Thread.Sleep(delayClick);
                     Mouse.mouse_event(Mouse.ME_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
                     Mouse.mouse_event(Mouse.ME_RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);
                    Thread.Sleep(duration);
                     Mouse.mouse_event(Mouse.ME_RIGHTUP, 0, 0, 0, UIntPtr.Zero);
                }
            }
        }


        public void MousePR(bool leftButton, bool middleButton, bool rightButton, bool release)
        {

            if (leftButton)
            {
                if (release)
                {
                   // Mouse.mouse_event(Mouse.ME_LEFTUP, 0, 0, 0, 0);
                }
                else
                {
                   // Mouse.mouse_event(Mouse.ME_LEFTDOWN, 0, 0, 0, 0);
                }
            }

            if (middleButton)
            {
                if (release)
                {
                   // Mouse.mouse_event(Mouse.MIE_MIDDLEUP, 0, 0, 0, 0);
                }
                else
                {
                   // Mouse.mouse_event(Mouse.MIE_MIDDLEDOWN, 0, 0, 0, 0);
                }
            }

            if (rightButton)
            {
                if (release)
                {
                   // Mouse.mouse_event(Mouse.ME_RIGHTUP, 0, 0, 0, 0);
                }
                else
                {
                    //Mouse.mouse_event(Mouse.ME_RIGHTDOWN, 0, 0, 0, 0);
                }
            }
        }
    }

    
}