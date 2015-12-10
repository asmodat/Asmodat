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
    }
}
