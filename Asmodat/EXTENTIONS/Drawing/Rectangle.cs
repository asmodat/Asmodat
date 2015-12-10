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

namespace Asmodat.Extensions.Drawing
{


    public static partial class RectangleEx
    {
        public static Rectangle[,] Split(this Rectangle rect, int xParts, int yParts)
        {
            int width = rect.Width;
            int height = rect.Height;

            if (xParts <= 0 || yParts <= 0 || width < xParts || height < yParts)
                return null;

            Rectangle[,] array = new Rectangle[xParts, yParts];

            int x = 0, y = 0;
            int rW = (width / xParts);
            int rH = (height / yParts);

            //last rectangle dimentions might be diffrent due to not even division
            int rW_last = width - (rW * (xParts - 1));
            int rH_last = height - (rH * (yParts - 1));

            int w, h;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    if (y == yParts - 1)
                        h = rH_last;
                    else
                        h = rH;

                    if (x == xParts - 1)
                        w = rW_last;
                    else
                        w = rW;

                    array[x, y] = new Rectangle(x * rW, y * rH, w, h);
                }
            }

            return array;
        }


        public static Bitmap ToBitmap(this Rectangle rect)
        {
            return new Bitmap(rect.Width, rect.Height);
        }

        public static readonly Rectangle Default = new Rectangle();

        public static bool IsValid(this Rectangle rect)
        {
            if (rect.Width > 0 && rect.Height > 0 && rect.X >= 0 && rect.Y >= 0)
                return true;

            return false;
        }


        public static bool Fits(this Rectangle rect, Bitmap bmp)
        {
            if (bmp != null && bmp.Width > 0 && bmp.Height > 0
                && rect.X >= 0 && rect.Y >= 0 && rect.Width > 0 && rect.Height > 0 &&
                (rect.Width + rect.X) <= bmp.Width && (rect.Height + rect.Y) <= bmp.Height )
                return true;

            return false;
        }


        public static bool EqualSize(this Rectangle rect, Rectangle cmp)
        {
            if (rect.Width == cmp.Width && rect.Height == cmp.Height)
                return true;
            else 
                return false;
        }

        public static int Area(this Rectangle rect)
        {
            return rect.Width * rect.Height;
        }

        /*public static Bitmap ToBitmap(this Rectangle rect, Graphics graph)
        {
            if (graph == null)
                return null;

            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            graph.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
            return bmp;
        }*/


    }
}
