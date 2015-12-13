using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using Asmodat.Extensions.Drawing;
using System.Windows;

using System.Windows.Interop;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Controls;
using Asmodat.Natives;
using Asmodat.Extensions.Windows.Media.Imaging;

namespace Asmodat.Extensions.Windows.Media.Imaging
{
    

    public static class WriteableBitmapEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this WriteableBitmap wbm)
        {
            if (wbm == null || wbm.PixelWidth <= 0 || wbm.PixelHeight <= 0)
                return true;
            else
                return false;
        }


       /* public static bool Fits(this WriteableBitmap wbm, Bitmap bmp)
        {
            if (wbm.ToInt32Rect().)
                return true;
            else
                return false;
        }*/


        public static WriteableBitmap ToWriteableBitmap(this BitmapSource bms)
        {
            if (bms.IsNullOrEmpty())
                return null;

            int stride = bms.GetStride();
            byte[] data = bms.ToPixelsByteArray();

            WriteableBitmap wbm = new WriteableBitmap(bms.PixelWidth, bms.PixelHeight, bms.DpiX, bms.DpiY, bms.Format, null);

            wbm.WritePixels(bms.ToInt32Rect(), data, stride, 0);

            return wbm;
        }


        public static WriteableBitmap ToWriteableBitmap(this RenderTargetBitmap rtb)
        {
            if (rtb.IsNullOrEmpty())
                return null;

            return new WriteableBitmap(rtb);
        }

        public static void DrawImage(this WriteableBitmap wbm, Bitmap bmp)
        {
            if (wbm.IsNullOrEmpty() || bmp.IsNullOrEmpty())
                return;
            

            BitmapData data = bmp.LockBits(bmp.ToRectangle(), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            try
            {
                wbm.Lock();
                kernel32.CopyMemory(wbm.BackBuffer, data.Scan0, (wbm.BackBufferStride * bmp.Height));
                wbm.AddDirtyRect(bmp.ToInt32Rect());
                wbm.Unlock();
            }
            finally
            {
                bmp.UnlockBits(data);
                bmp.Dispose();
            }
        }


        public static void WritePixels(this WriteableBitmap wbm, Bitmap bmp)
        {
            if (wbm.IsNullOrEmpty() || bmp.IsNullOrEmpty())
                return;
            
            BitmapData data = bmp.LockBits(bmp.ToRectangle(), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

            try
            {
                wbm.WritePixels(bmp.ToInt32Rect(), data.Scan0, data.Stride, 0);
            }
            finally
            {
                bmp.UnlockBits(data);
                bmp.Dispose();
            }
        }


        public static void WritePixels(this WriteableBitmap wbm, BitmapSource bms)
        {
            if (wbm.IsNullOrEmpty() || bms.IsNullOrEmpty())
                return;

            int stride = bms.GetStride();
            byte[] data = new byte[stride * bms.PixelHeight];
            bms.CopyPixels(data, stride, 0);
            wbm.WritePixels(bms.ToInt32Rect(), data, stride, 0);
        }

        public static WriteableBitmap ToWriteableBitmap(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            WriteableBitmap wbm = new WriteableBitmap(bmp.Width, bmp.Height, 96, 96, PixelFormats.Pbgra32, null);
            wbm.DrawImage(bmp);

            return wbm;
        }


        public static WriteableBitmap CopyDeep(this WriteableBitmap wbm)
        {
            if (wbm.IsNullOrEmpty())
                return null;

            if (wbm.Format != PixelFormats.Pbgra32)
            {
                var temp = BitmapFactory.ConvertToPbgra32Format(wbm);
                return temp.Crop(wbm.ToRect());
            }
            else
                return wbm.Crop(wbm.ToRect());
        }



        /*public static void WriteTextToBitmap(this WriteableBitmap wbm, string text, decimal fontSizePercentage)
        {
            if (wbm.IsNullOrEmpty() || wbm.PixelWidth <= 2 || wbm.PixelHeight <= 2)
                return;

            Rect rect = new Rect(1, 1, wbm.PixelWidth - 1, wbm.PixelHeight - 1);

            int fontSize = (int)Math.Ceiling((decimal)wbm.Height).FindValueByPercentages(100, fontSizePercentage);

            if (fontSize <= 0)
                fontSize = 1;

            TextBlock tblck = new TextBlock();
            tblck.Text = text;
            tblck.FontSize = fontSize;

           // wbm.Render();

            // tblck.FontFamily = new Typeface("Thoma");

            //wbm.Render();
        }*/
    }
}
