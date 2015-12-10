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
using Asmodat.Extensions.Windows.Media.Imaging;
using System.Windows.Media.Imaging;
using Asmodat.Extensions.Windows;

namespace Asmodat.Extensions.Drawing
{


    public static partial class BitmapEx
    {

        /// <summary>
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="vCunts"></param>
        /// <param name="hCuts"></param>
        /// <returns></returns>
        public static Bitmap[,] Split(this Bitmap bmp, int xParts, int yParts)
        {
            if (bmp == null || bmp.Width < xParts || bmp.Height < yParts)
                return null;

            Rectangle[,] rectangles = bmp.ToRectangle().Split(xParts, yParts);

            if (rectangles == null)
                return null;

            Bitmap[,] bitmaps = new Bitmap[xParts, yParts];

            if (xParts == 1 && yParts == 1)
            {
                bitmaps[0, 0] = bmp.TryCopy();
                return bitmaps;
            }

            int x = 0, y;


            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    //bitmaps[x, y] = bmp.DrawImage_Copy(rectangles[x, y]);
                    bitmaps[x, y] = bmp.TryCopy(rectangles[x, y]);
                }
            }

            return bitmaps;
        }










        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this Bitmap bmp)
        {
            if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Estimates rectangle size based on bitmaps 2D array
        /// </summary>
        /// <param name="bitmaps"></param>
        /// <returns></returns>
        public static Rectangle ToRectangle(this Bitmap[,] bitmaps)
        {
            int xParts = bitmaps.Width();
            int yParts = bitmaps.Height();

            if (1.IsGreaterThenAny(xParts, yParts))
                return Rectangle.Empty;

            int wInner = 0, hInner = 0, wOuter = 0, hOuter = 0;

            int x = 0, y, w, h;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    if (bitmaps[x, y].IsNullOrEmpty())
                        continue;

                    w = bitmaps[x, y].Width;
                    h = bitmaps[x, y].Height;

                    if (x < xParts - 1)
                    {
                        if (w > wInner)
                            wInner = w;
                    }
                    else
                    {
                        if (w > wOuter)
                            wOuter = w;
                    }

                    if (y < yParts - 1)
                    {
                        if (h > hInner)
                            hInner = h;
                    }
                    else
                    {
                        if (h > hOuter)
                            hOuter = h;
                    }
                }
            }

            int wi, wo, hi, ho;

            if (wInner > 0) wi = wInner; else wi = wOuter;
            if (wOuter > 0) wo = wOuter; else wo = wInner;
            if (hInner > 0) hi = hInner; else hi = hOuter;
            if (hOuter > 0) ho = hOuter; else ho = hInner;

            int width = wi * (xParts - 1) + wo;
            int height = hi * (yParts - 1) + ho;

            return new Rectangle(0, 0, width, height);
        }

        /// <summary>
        /// Returns rectangles matrix, null bitmaps return empty rectanges
        /// </summary>
        /// <param name="bitmaps"></param>
        /// <returns></returns>
        public static Rectangle[,] ToRectangles(this Bitmap[,] bitmaps)
        {
            if (bitmaps == null)
                return null;

            int x = 0, y, xParts = bitmaps.Width(), yParts = bitmaps.Height();

            Rectangle[,] result = new Rectangle[xParts, yParts];

            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    result[x, y] = bitmaps[x, y].ToRectangle();
                }
            }

            return result;
        }

        /*public static int ToIndex2D(int x, int y, int width, int height)
        {
            if (x < 0 || y < 0 || width <= 0 || height <= 0 || x >= width || y >= height)
                return -1;

            return x + (width * y);
        }

        public static Point FromIndex2D(int index, int width, int height)
        {
            if (index < 0 || width <= 0 || height <= 0 || index >= (width * height))
                return new Point(-1, -1);


            int y = index / width;
            int x = index + (width * y);


            return new Point(x, y);
        }*/

        /// <summary>
        /// maximum allowed matrix dimentions are 32767x32767
        /// </summary>
        /// <param name="bitmaps"></param>
        /// <returns></returns>
        public static byte[] ToByteArray_16(this Bitmap[,] bitmaps, ImageFormat format = null)
        {
            if (bitmaps.IsNullOrEmpty())
                return null;

            int xParts = bitmaps.Width();
            int yParts = bitmaps.Height();

            if (!xParts.InClosedInterval(1, Int16.MaxValue)) return null;
            if (!yParts.InClosedInterval(1, Int16.MaxValue)) return null;

            
            List<byte> result = new List<byte>();
            result.AddRange(Int16Ex.ToBytes((Int16)xParts));
            result.AddRange(Int16Ex.ToBytes((Int16)yParts));

            Int16 x = 0, y;
            byte[] dd;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    dd = bitmaps[x, y].ToByteArray(format);

                    if (dd.IsNullOrEmpty())
                        continue;

                    result.AddRange(Int16Ex.ToBytes(x));
                    result.AddRange(Int16Ex.ToBytes(y));
                    result.AddRange(Int32Ex.ToBytes(dd.Length));
                    result.AddRange(dd);
                }
            }

            if (result.IsCountLessOrEqual(24))
                return null;

            return result.ToArray();
        }

        public static Bitmap[,] ToBitmap2D_16(this byte[] data, int offset)
        {
            if (data.IsCountLessOrEqual(24 + offset))
                return null;

            int xParts = Int16Ex.FromBytes(data);
            int yParts = Int16Ex.FromBytes(data, 2);

            if (1.IsGreaterThenAny(xParts, yParts))
                return null;

            Bitmap[,] result = new Bitmap[xParts, yParts];

            int i = 4;
            for (; i < data.Length;)
            {
                int x = Int16Ex.FromBytes(data, i);
                int y = Int16Ex.FromBytes(data, i + 2);
                int l = Int32Ex.FromBytes(data, i + 4);

                if (
                    !x.InClosedInterval(0, xParts - 1) ||
                    !y.InClosedInterval(0, yParts - 1) ||
                        l <= 0)
                    return null;


                byte[] packet = data.SubArray(i + 8, l);

                result[x, y] = packet.ToBitmap();

                i += (8 + l);
            }

            return result;
        }
    }
}
