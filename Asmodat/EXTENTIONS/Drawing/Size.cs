using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using Asmodat.Extensions.Collections.Generic;
using System.Windows.Media.Imaging;
using Asmodat.Extensions.Windows.Media.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace Asmodat.Extensions.Drawing
{
    

    public static class SizeEx
    {
        public static Size Add(this Size size, int width, int height)
        {
            Size _new = new Size(size.Width, size.Height);
            _new.Width += width;
            _new.Height += height;
            return _new;
        }

        public static int Area(this Size size)
        {
            if (size == null)
                return 0;

            return size.Width * size.Height;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size ToSizeOfDimentions<TKey>(this TKey[,] source)
        {
            if (source.IsNullOrEmpty())
                return new Size(0,0);
            else 
                return new Size(source.Width(), source.Height());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size ToSize(this WriteableBitmap wbm)
        {
            if (wbm.IsNullOrEmpty())
                return new Size(0, 0);
            else 
                return new Size(wbm.PixelWidth, wbm.PixelHeight);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size ToSize(this BitmapSource bms)
        {
            if (bms.IsNullOrEmpty())
                return new Size(0, 0);
            else 
                return new Size(bms.PixelWidth, bms.PixelHeight);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Size ToSize(this System.Windows.Size size)
        {
            if (size == null)
                return new Size(0, 0);
            else
                return new Size((int)size.Width, (int)size.Height);
        }
    }
}
