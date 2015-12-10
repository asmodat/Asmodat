using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

using Asmodat.IO;
using System.Drawing;
using Asmodat.Types;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Asmodat.Debugging;

namespace Asmodat.Imaging
{
    
    public partial class Images
    {
       

        public static Color[,] GetPixels(Bitmap bmp)
        {
            if (bmp == null)
                return null;

            int x = bmp.Width;
            int y = bmp.Height;

            Color[,] pixels = new Color[x, y];

            for(int ix = 0; ix < x; ix++)
                for (int iy = 0; iy < y; iy++)
                    pixels[ix,iy] = bmp.GetPixel(ix, iy);

            return pixels;
        }

        /*public static Color[,] GetPixelsLock(Bitmap bmp)
        {
            if (bmp == null)
                return null;

            int x = bmp.Width;
            int y = bmp.Height;

            Color[,] pixels = new Color[x, y];

            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);

            for (int ix = 0; ix < x; ix++)
                for (int iy = 0; iy < y; iy++)
                    pixels[ix, iy] = data.GetPixel(ix, iy); data.

            bmp.UnlockBits(data);
            return pixels;
        }*/

        public static byte[] GetRawBytes(Bitmap bmp)
        {
            
            if (bmp == null)
                return null;

            try
            { 
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
                int length = data.Stride * data.Height;
                byte[] output = new byte[length];

                Marshal.Copy(data.Scan0, output, 0, length);
                bmp.UnlockBits(data);
                return output;
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return null;
            }
        }

        public static byte[] GetRawBytes(Bitmap bmp, Rectangle rectangle)
        {
            if (bmp == null)
                return null;

            Bitmap sample = Images.Copy(bmp, rectangle);

            BitmapData data = sample.LockBits(new Rectangle(0, 0, sample.Width, sample.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int length = data.Stride * data.Height;
            byte[] output = new byte[length];

            Marshal.Copy(data.Scan0, output, 0, length);
            sample.UnlockBits(data);
            return output;
        }

        public static Bitmap Copy(Bitmap source, Rectangle rectangle)
        {
            if (source == null)
                return null;

            Bitmap bmp = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.DrawImage(source, 0, 0, rectangle, GraphicsUnit.Pixel);
            graphics.Dispose();
            return bmp;
        }
    }
}

/*
public static Color[,] GetPixelsArgb32(Bitmap bmp)
        {
            if (bmp == null)
                return null;

            int width = bmp.Width;
            int height = bmp.Height;

            Color[,] pixels = new Color[width, height];

            for (int ix = 0; ix < width; ix++)
                for (int iy = 0; iy < height; iy++)
                    pixels[ix, iy] = bmp.GetPixel(ix, iy);


            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                for (int ix = 0; ix < width; ix++)
                {
                    int* pData = (int*)data.Scan0.ToPointer();
                    pData += ix;
                    for (int iy = 0; iy < height; ++iy)
                    {
                        pixels[ix, iy] = Color.FromArgb(*pData);
                        pData += width;
                    }
                }

            }



            return pixels;
        }

        public static Color[] GetPixelsColumnArgb32(Bitmap bmp, int x)
        {
            if (bmp == null)
                return null;

            Color[] column = new Color[bmp.Height];
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            unsafe
            {
                int* pData = (int*)data.Scan0.ToPointer();
                pData += x;
                for(int i = 0; i < bmp.Height; ++i)
                {
                    column[i] = Color.FromArgb(*pData);
                    pData += bmp.Width;
                }

            }
            return column;
        }
*/
