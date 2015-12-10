using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using Asmodat.Extensions.Collections.Generic;

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




        public static Size ToSizeOfDimentions<TKey>(this TKey[,] source)
        {
            if (source.IsNullOrEmpty())
                return new Size(0,0);

            return new Size(source.Width(), source.Height());
        }
    }
}
