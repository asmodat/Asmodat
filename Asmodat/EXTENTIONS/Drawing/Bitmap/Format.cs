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
using Asmodat.Extensions.IO;

namespace Asmodat.Extensions.Drawing
{


    public static partial class BitmapEx
    {
        public static readonly ColorMatrix GrayScaleMatrix = new ColorMatrix(
                new float[][]
                {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });

        public static Bitmap ToGrayscale(this Bitmap bmp)
        {
            if (bmp == null)
                return null;

            Bitmap result = new Bitmap(bmp.Width, bmp.Height);
            Graphics graphics = Graphics.FromImage(result);

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(BitmapEx.GrayScaleMatrix);
            graphics.DrawImage(bmp, bmp.ToRectangle(), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            graphics.Dispose();

            return result;
        }

        public static Bitmap[,] ToGrayscale(this Bitmap[,] bitmaps)
        {
            int xParts = bitmaps.Width();
            int yParts = bitmaps.Height();

            if (1.IsGreaterThenAny(xParts, yParts))
                return null;

            Bitmap[,] result = new Bitmap[xParts, yParts];
            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    result[x, y] = bitmaps[x, y].ToGrayscale();
                }
            }

            return result;
        }


        public static Bitmap Convert(this Bitmap bmp, PixelFormat format)
        {
            if (bmp.IsNullOrEmpty())
                return null;
            
            Bitmap result = new Bitmap(bmp.Width, bmp.Height, format);
            using (Graphics graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(bmp, bmp.ToRectangle());
            }
            return result;
        }

        public static Bitmap ToBitmap(this MemoryStream stream, PixelFormat? format = null)
        {
            if (stream.IsNullOrEmpty())
                return null;

            Bitmap bmp = new Bitmap(stream);

            if (format.IsNull())
                return bmp;
            
            return bmp.Clone(bmp.ToRectangle(), format.Value);
        }

        public static Bitmap[,] Convert(this Bitmap[,] bitmaps, PixelFormat format)
        {
            int xParts = bitmaps.Width();
            int yParts = bitmaps.Height();

            if (1.IsGreaterThenAny(xParts, yParts))
                return null;

            Bitmap[,] result = new Bitmap[xParts, yParts];
            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    result[x, y] = bitmaps[x, y].Convert(format);
                }
            }

            return result;
        }


        public static Bitmap Format(this Bitmap bmp, PixelFormat format)
        {
            if (bmp.IsNullOrEmpty())
                return null;
            
            return bmp.Clone(bmp.ToRectangle(), format);
        }
        public static Bitmap[,] Format(this Bitmap[,] bitmaps, PixelFormat format)
        {
            int xParts = bitmaps.Width();
            int yParts = bitmaps.Height();

            if (1.IsGreaterThenAny(xParts, yParts))
                return null;

            Bitmap[,] result = new Bitmap[xParts, yParts];
            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    result[x, y] = bitmaps[x, y].Format(format);
                }
            }

            return result;
        }

    }
}
