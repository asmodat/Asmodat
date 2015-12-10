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

namespace Asmodat.Extensions.Drawing
{
  


    public static partial class BitmapEx
    {
        public static Bitmap Crop(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            return Crop(bmp, 0, 0, bmp.Width, bmp.Height);
        }

        public static Bitmap Crop(this Bitmap bmp, Rectangle rect)
        {
            if (bmp.IsNullOrEmpty() || rect.IsEmpty)
                return null;

            return Crop(bmp, rect.X, rect.Y, rect.Width, rect.Height);
        }

        /// <summary>
        /// this method should only be used to crop very big images into small ones
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap Crop(this Bitmap bmp, int x, int y, int width, int height)
        {
            if (bmp.IsNullOrEmpty() || x < 0 || y < 0 || width <= 0 || height <= 0 || (width + x) > bmp.Width || (height + y) > bmp.Height)
                return null;

            int BytesPerPixel = bmp.PixelFormat.GetBitsPerPixel() / 8;

            if (BytesPerPixel <= 0)
                return null;

            BitmapData data = bmp.LockBits(bmp.ToRectangle(), ImageLockMode.ReadOnly, bmp.PixelFormat);

            int byteCount = data.Stride * data.Height;
            byte[] bytes = new byte[byteCount];
            Marshal.Copy(data.Scan0, bytes, 0, byteCount);

            byte[] croppBytes = new byte[width * height * BytesPerPixel];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width * BytesPerPixel; j += BytesPerPixel)
                {
                    int srcIndex = (x * data.Stride) + (i * data.Stride) + (y * BytesPerPixel) + (j);
                    int dstInex = (i * width * BytesPerPixel) + (j);

                    for (int k = 0; k < BytesPerPixel; k++)
                    {
                        croppBytes[dstInex + k] = bytes[srcIndex + k];
                    }
                }
            }

            Bitmap croppBmp = new Bitmap(width, height);
            BitmapData croppData = croppBmp.LockBits(croppBmp.ToRectangle(), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(croppBytes, 0, croppData.Scan0, croppBytes.Length);

            bmp.UnlockBits(data);
            croppBmp.UnlockBits(croppData);
            return croppBmp;
        }
    }
}

/*

    

*/
