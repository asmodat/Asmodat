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
using AForge.Imaging.Filters;

namespace Asmodat.Extensions.Drawing
{


    public static partial class BitmapEx
    {
        public static Bitmap Resize(this Bitmap bmp, decimal percentage)
        {
            if (bmp == null)
                return null;

            

            int width = (int)((decimal)bmp.Width).FindValueByPercentages(100, percentage);
            int height = (int)((decimal)bmp.Height).FindValueByPercentages(100, percentage);

            return bmp.Resize(width, height);
        }

        public static Bitmap Resize(this Bitmap bmp, int width, int height)
        {
            if (bmp == null || width <= 0 || height <= 0)
                return null;

            if (bmp.Width == width && bmp.Height == height)
                return bmp.TryCopy();

            return new Bitmap(bmp, width, height);
        }

        public static Bitmap ResizeFast(this Bitmap bmp, decimal percentage)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            int width = (int)((decimal)bmp.Width).FindValueByPercentages(100, percentage);
            int height = (int)((decimal)bmp.Height).FindValueByPercentages(100, percentage);

            return bmp.ResizeFast(width, height);
        }

        public static Bitmap ResizeFast(this Bitmap source, int width, int height)
        {
            if (source.IsNullOrEmpty() || width <= 0 || height <= 0)
                return null;

            if (source.Width == width && source.Height == height)
                return source.TryCopy();

            Bitmap bmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(source, bmp.ToRectangle());
                g.Save();
            }

            return bmp;
        }

        public static Bitmap[,] ResizeAll(this Bitmap[,] bmps, int width, int height)
        {
            if (bmps.IsNullOrEmpty() || width <= 0 || height <= 0)
                return null;

            int xParts = bmps.Width();
            int yParts = bmps.Height();

            Bitmap[,] result = new Bitmap[xParts, yParts];
            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    if(bmps[x,y] != null)
                        result[x, y] = new Bitmap(bmps[x, y], width, height);
                }
            }

            return result;
        }


        public static Bitmap[,] ResizeFastAll(this Bitmap[,] bmps, int width, int height)
        {
            if (bmps.IsNullOrEmpty() || width <= 0 || height <= 0)
                return null;
            
            int xParts = bmps.Width();
            int yParts = bmps.Height();

            Bitmap[,] result = new Bitmap[xParts, yParts];
            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    if (bmps[x, y] != null)
                    {
                        result[x, y] = bmps[x, y].ResizeFast(width, height);
                    }
                }
            }

            return result;
        }



        public static Bitmap AForge_ResizeFast(this Bitmap bmp, decimal percentage)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            int width = (int)((decimal)bmp.Width).FindValueByPercentages(100, percentage);
            int height = (int)((decimal)bmp.Height).FindValueByPercentages(100, percentage);

            return bmp.AForge_ResizeFast(width, height);
        }


        public static Bitmap AForge_ResizeFast(this Bitmap bmp, Size size)
        {
            return bmp.AForge_ResizeFast(size.Width, size.Height);
        }

        public static Bitmap AForge_ResizeFast(this Bitmap source, int width, int height)
        {
            if (source.IsNullOrEmpty() || width <= 0 || height <= 0)
                return null;

            if (source.Width == width && source.Height == height)
                return source.TryCopy();

            try
            {
                ResizeNearestNeighbor filter = new ResizeNearestNeighbor(width, height);
                return filter.Apply(source);
            }
            catch(Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return null;
            }
        }



        public static Bitmap[,] AForge_ResizeFastAll(this Bitmap[,] bmps, int width, int height)
        {
            if (bmps.IsNullOrEmpty() || width <= 0 || height <= 0)
                return null;

            int xParts = bmps.Width();
            int yParts = bmps.Height();

            Bitmap[,] result = new Bitmap[xParts, yParts];
            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    if (bmps[x, y] != null)
                    {
                        result[x, y] = bmps[x, y].AForge_ResizeFast(width, height);
                    }
                }
            }

            return result;
        }
    }
}
