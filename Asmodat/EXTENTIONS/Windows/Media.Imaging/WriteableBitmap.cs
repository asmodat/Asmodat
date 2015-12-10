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
