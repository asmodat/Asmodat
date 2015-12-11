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
using Asmodat.Extensions.Windows.Media.Imaging;

namespace Asmodat.Extensions.Windows
{
    public static partial class Int32RectEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this Bitmap bmp)
        {
            if (bmp == null)
                return Int32Rect.Empty;
            else
                return new Int32Rect(0, 0, bmp.Width, bmp.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this Rectangle rect)
        {
            return new Int32Rect(0, 0, rect.Width, rect.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this System.Drawing.Size size)
        {
            return new Int32Rect(0, 0, size.Width, size.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32Rect ToInt32Rect(this BitmapSource bms)
        {
            if (bms.IsNullOrEmpty())
                return Int32Rect.Empty;

            return new Int32Rect(0, 0, bms.PixelWidth, bms.PixelHeight);
        }





        /// <summary>
        /// Checks if one rect1 surround rect2, this method is location (X,Y), and order (rect1,rect2) sensitive
        /// </summary>
        /// <param name="rect1"></param>
        /// <param name="rect2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Fits(this Int32Rect rect1, Int32Rect rect2)
        {
            if ((rect1.Width + rect1.X) >= (rect2.Width + rect2.X) && 
                (rect1.Height + rect1.Y) >= (rect2.Height + rect2.Y) &&
                rect1.X <= rect2.X && 
                rect1.Y <= rect2.Y)
                return true;
            else
                return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualSize(this Int32Rect rect1, Int32Rect rect2)
        {
            if (rect1.Width == rect2.Width && rect1.Height == rect2.Height)
                return true;
            else
                return false;
        }

    }
}
