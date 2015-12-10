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

namespace Asmodat.Extensions.Windows.Media.Imaging
{
    

    public static class RenderTargetBitmapEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this RenderTargetBitmap rtb)
        {
            if (rtb == null || rtb.PixelWidth <= 0 || rtb.PixelHeight <= 0)
                return true;

            return false;
        }


        public static Bitmap ToBitmap(this RenderTargetBitmap rtb)
        {
            if (rtb.IsNullOrEmpty())
                return null;

            MemoryStream stream = new MemoryStream();
            BitmapEncoder encoder = new BmpBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(stream);

            Bitmap bmp = new Bitmap(stream);
            return bmp;
        }

    }
}
