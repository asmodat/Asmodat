using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using Asmodat.Natives;

namespace Asmodat.IO
{
    public static class Screen
    {
        public static Image CaptureDesktop()
        {
            return Screen.CaptureWindow(user32.GetDesktopWindow());
        }

        public static Image CaptureWindow(IntPtr handle)
        {
            IntPtr hDCSrc = user32.GetWindowDC(handle);
            user32.RECT windowRect = new user32.RECT();
            user32.GetWindowRect(handle, ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = gdi32.CreateCompatibleDC(hDCSrc);
            IntPtr hBitmap = gdi32.CreateCompatibleBitmap(hDCSrc, width, height);

            IntPtr hOld = gdi32.SelectObject(hdcDest, hBitmap);
            gdi32.BitBlt(hdcDest, 0, 0, width, height, hDCSrc, 0, 0, gdi32.SRCCOPY);
            gdi32.SelectObject(hdcDest, hOld);

            gdi32.DeleteDC(hdcDest);
            user32.ReleaseDC(handle, hDCSrc);
            Image img = Image.FromHbitmap(hBitmap);
            gdi32.DeleteObject(hBitmap);

            return img;
        }
    }
}
