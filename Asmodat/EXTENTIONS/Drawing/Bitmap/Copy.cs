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

using Asmodat.Extensions.Windows.Media;
using Asmodat.Extensions.Windows.Media.Imaging;
using Asmodat.Extensions.Windows;

namespace Asmodat.Extensions.Drawing
{
  


    public static partial class BitmapEx
    {
        public static Bitmap WPF_Copy(this Bitmap bmp, Rectangle rect)
        {
            if (!bmp.Fits(rect))
                return null;

            var src = bmp.ToBitmapSource();
            
            Bitmap result = null;

            if (bmp.ToRectangle().EqualSize(rect))
            {
                result = src.ToBitmapFast(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            else
            {
                CroppedBitmap cropped = new CroppedBitmap(src, bmp.ToInt32Rect());
                result = cropped.ToBitmapFast(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            }
            
            return result;
        }

        public static Bitmap WPF_Copy(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            return bmp.WPF_Copy(bmp.ToRectangle());

        }


        public static Bitmap DrawImage_Copy(this Bitmap bmp, Rectangle rect)
        {
            if (!bmp.Fits(rect))
                return null;

            Bitmap result = rect.ToBitmap();

            using (Graphics graphics = Graphics.FromImage(result))
                graphics.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);

            return result;
        }
        public static Bitmap DrawImage_Copy(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            return bmp.DrawImage_Copy(bmp.ToRectangle());
        }

        public static Bitmap Copy(this Bitmap bmp, Rectangle rect)
        {
            if (!bmp.Fits(rect))
                return null;

            try
            {
                return bmp.Clone(rect, bmp.PixelFormat);
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return null;
            }
        }
        public static Bitmap Copy(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            return bmp.Copy(bmp.ToRectangle());
        }

        public static Bitmap CopyDeep(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            return new Bitmap(bmp);// bmp.Copy(bmp.ToRectangle());
        }

        public static Bitmap TryCopyDeep(this Bitmap bmp)
        {
            try
            {
                if (bmp.IsNullOrEmpty())
                    return null;

                return new Bitmap(bmp);
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return null;
            }
        }



        public static Bitmap TryCopy(this Bitmap bmp, Rectangle rect)
        {
            if (!bmp.Fits(rect))
                return null;

            try
            {
                return bmp.Clone(rect, bmp.PixelFormat);
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return null;
            }
        }
        public static Bitmap TryCopy(this Bitmap bmp)
        {
            try
            {
                if (bmp.IsNullOrEmpty())
                return null;

            return bmp.TryCopy(bmp.ToRectangle());
                }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return null;
            }
        }
        
        public static Bitmap[,] Copy(this Bitmap[,] bitmaps)
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
                    result[x, y] = bitmaps[x, y].Copy();
                }
            }

            return result;
        }
        public static Bitmap[,] TryCopy(this Bitmap[,] bitmaps)
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
                    result[x, y] = bitmaps[x, y].TryCopy();
                }
            }

            return result;
        }

    }
}
