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
    public static partial class RectEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this Bitmap bmp)
        {
            if (bmp == null)
                return Rect.Empty;
            else
                return new Rect(0, 0, bmp.Width, bmp.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this BitmapSource source)
        {
            if (source == null)
                return Rect.Empty;
            else 
                return new Rect(0, 0, source.Width, source.Height);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rect ToRect(this WriteableBitmap wbmp)
        {
            if (wbmp == null)
                return Rect.Empty;
            else
                return new Rect(0, 0, wbmp.Width, wbmp.Height);
        }

        public static Rect ToRect(this BitmapSource source, double locationX, double locationY)
        {
            if (source.IsNullOrEmpty() || locationX < 0 || locationY < 0)
                return Rect.Empty;
            else
                return new Rect(locationX, locationY, source.Width, source.Height);
        }
    }
}
