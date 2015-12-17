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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Fits(this Bitmap bmp, Rectangle rect)
        {
            if (bmp != null && bmp.Width > 0 && bmp.Height > 0
                && rect.X >= 0 && rect.Y >= 0 && rect.Width > 0 && rect.Height > 0 &&
                (rect.Width + rect.X) <= bmp.Width && (rect.Height + rect.Y) <= bmp.Height)
                return true;

            return false;
        }

        public static Color[,] GetPixels(this Bitmap bmp)
        {
            if (bmp == null)
                return null;

            int x = bmp.Width;
            int y = bmp.Height;

            Color[,] pixels = new Color[x, y];

            for (int ix = 0; ix < x; ix++)
                for (int iy = 0; iy < y; iy++)
                    pixels[ix, iy] = bmp.GetPixel(ix, iy);

            return pixels;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Fits(this Bitmap bmp, Int32Rect rect)
        {
            if (bmp != null && bmp.Width > 0 && bmp.Height > 0
                && rect.X >= 0 && rect.Y >= 0 && rect.Width > 0 && rect.Height > 0 &&
                (rect.Width + rect.X) <= bmp.Width && (rect.Height + rect.Y) <= bmp.Height)
                return true;

            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] GetRawBytes(this Bitmap bmp)
        {
            return bmp.GetRawBytes(bmp.ToRectangle());
        }

        public static byte[] GetRawBytes(this Bitmap bmp, Rectangle rect)
        {
            if (bmp.IsNullOrEmpty() || !bmp.Fits(rect))
                return null;

            try
            {
                BitmapData data = bmp.LockBitsR(rect);//.LockBits(bmp.ToRectangle(), ImageLockMode.ReadOnly, bmp.PixelFormat);
                int length = data.Stride * data.Height;
                byte[] output = new byte[length];

                Marshal.Copy(data.Scan0, output, 0, length);
                bmp.UnlockBits(data);
                return output;
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return null;
            }
        }


        public static void Clear(this Bitmap bmp, Color color)
        {
           
            if (bmp == null)
                return;

            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.Clear(color);
            }
        }

        
       



        public static byte[] ToByteArray(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(bmp, typeof(byte[]));
        }

        /// <summary>
        /// TODO: this method migt not have complementary frombytearray method
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Bitmap bmp, ImageFormat format)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            if (format == null)
                return bmp.ToByteArray();

            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, format);
                return stream.ToArray();
            }
        }


        public static void WriteText(this Bitmap bmp, string text, decimal fontSizePercentage)
        {
            if (bmp.IsNullOrEmpty() || bmp.Width < 3 || bmp.Height < 3)
                return;// null;

            try
            {
                RectangleF rectf = new RectangleF(1, 1, bmp.Width - 1, bmp.Height - 1);
                Graphics graphics = Graphics.FromImage(bmp);
                //using (Graphics graphics = Graphics.FromImage(bmp))
                //{
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                    int fontSize = (int)Math.Ceiling((decimal)bmp.Height).FindValueByPercentages(100, fontSizePercentage);

                    if (fontSize <= 0)
                        fontSize = 1;

                    graphics.DrawString(text, new Font("Thoma", fontSize), Brushes.Red, rectf);
                //}
                graphics.Flush();
            }
            catch(Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return;
            }
            
        }


        


        public static byte[] PixelData(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            BitmapData bmd = bmp.LockBitsRW();



            return null;
        }

    }
}
