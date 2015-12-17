using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.IO;
using Asmodat.Extensions.Collections.Generic;
using System.Drawing.Imaging;
using Asmodat.Imaging;
using Asmodat.Debugging;
using System.Runtime.CompilerServices;

using System.Windows.Media.Imaging;
using System.Windows;
using Asmodat.Extensions.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace Asmodat.Extensions.Drawing
{

    public static partial class BitmapEx
    {

        public static BitmapData LockBitsBase(this Bitmap bmp, Rectangle rect, ImageLockMode mode)
        {
            if (bmp.IsNullOrEmpty() || !rect.IsValid() || !bmp.Fits(rect))
                return null;

            BitmapData bmd = bmp.LockBits(rect, mode, bmp.PixelFormat);
            return bmd;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitmapData LockBitsRW(this Bitmap bmp, Rectangle rect)
        {
            return bmp.LockBitsBase(rect, ImageLockMode.ReadWrite);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitmapData LockBitsRW(this Bitmap bmp)
        {
            return bmp.LockBitsRW(bmp.ToRectangle());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static BitmapData LockBitsR(this Bitmap bmp, Rectangle rect)
        {
            return bmp.LockBitsBase(rect, ImageLockMode.ReadOnly);
        }
    }
}
