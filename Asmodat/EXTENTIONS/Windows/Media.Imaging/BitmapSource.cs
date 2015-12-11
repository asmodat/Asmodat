using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using Asmodat.Extensions.Drawing;
using System.Windows;

using System.Windows.Interop;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Globalization;
using Asmodat.Natives;

namespace Asmodat.Extensions.Windows.Media.Imaging
{
    

    public static class BitmapSourceEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this BitmapSource bmps)
        {
            if (bmps == null || bmps.Width <= 0 || bmps.Height <= 0)
                return true;

            return false;
        }
        
        
        public static BitmapSource ToBitmapSource(this Bitmap bmp)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            IntPtr hBitmap = bmp.GetHbitmap();
            BitmapSource result;
            try
            {
                result = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                gdi32.DeleteObject(hBitmap);
            }

            return result;
        }

        public static BitmapSource ToBitmapSourceFast(this Bitmap bmp)
        {
            return bmp.ToBitmapSourceFast(PixelFormats.Pbgra32);
        }

        public static BitmapSource ToBitmapSourceFast(this Bitmap bmp, System.Windows.Media.PixelFormat format)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            var data = bmp.LockBits(bmp.ToRectangle(), ImageLockMode.ReadOnly, bmp.PixelFormat);
            BitmapSource src = BitmapSource.Create(bmp.Width, bmp.Height, 96, 96, format, null,
                data.Scan0, data.Stride * data.Height, data.Stride
                );

            bmp.UnlockBits(data);

            return src;
        }


        public static Bitmap ToBitmap(this BitmapSource source)
        {
            return source.ToBitmap(BitmapFrameEx.BitmapFrameFormatMode.Png);
        }

        public static Bitmap ToBitmap(this BitmapSource source, BitmapFrameEx.BitmapFrameFormatMode mode)
        {
            if (source.IsNullOrEmpty())
                return null;

            Bitmap result;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = mode.GetBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(stream);
                result = new Bitmap(stream);
            }

            return result;
        }


        public static Bitmap ToBitmapFast(this BitmapSource source)
        {
            return source.ToBitmapFast(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        public static Bitmap ToBitmapFast(this BitmapSource source, System.Drawing.Imaging.PixelFormat format)
        {
            if (source.IsNullOrEmpty())
                return null;

            Bitmap bmp = new Bitmap(source.PixelWidth, source.PixelHeight, format);

            BitmapData data = bmp.LockBits(bmp.ToRectangle(), ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);// format);

            source.CopyPixels(Int32Rect.Empty, data.Scan0, data.Height * data.Stride, data.Stride);

            bmp.UnlockBits(data);

            return bmp;
        }

       /* public static Bitmap ToBitmapFaster(this BitmapSource bms)
        {
            if (bms.IsNullOrEmpty())
                return null;

            int stride = bms.GetStride();
            byte[] data = bms.ToPixelsByteArray();

            unsafe
            {
                fixed(byte* pData = data)
                {
                    IntPtr ptr = new IntPtr(pData);
                    System.Drawing.Bitmap bmp = new Bitmap(bms.PixelWidth, bms.PixelHeight, stride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, ptr);
                    return bmp;
                }
            }
        }*/


        

        

        

        public static RenderTargetBitmap WriteTextToRenderTargetBitmap(this BitmapSource bms, string text, decimal fontSizePercentage)
        {
            if (bms.IsNullOrEmpty() || bms.Width <= 2 || bms.Height <= 2)
                return null;

            int fontSize = (int)Math.Ceiling((decimal)bms.Height).FindValueByPercentages(100, fontSizePercentage);

            if (fontSize <= 0)
                fontSize = 1;

            FormattedText ftxt =
                new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                new Typeface("Thoma"), fontSize, System.Windows.Media.Brushes.Red);//, new Point(0, 0));

            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext context = visual.RenderOpen())
            {
                context.DrawImage(bms, bms.ToRect());
                context.DrawText(ftxt, new System.Windows.Point(1, 1));
            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(bms.PixelWidth, bms.PixelHeight, 96, 96, PixelFormats.Default);
            rtb.Render(visual);

            return rtb;
        }


        public static Bitmap WriteTextToBitmap(this BitmapSource bms, string text, decimal fontSizePercentage)
        {
            return bms.WriteTextToRenderTargetBitmap(text, fontSizePercentage).ToBitmap();
        }

       /* public static WriteableBitmap WriteTextToWritableBitmap(this BitmapSource bms, string text, decimal fontSizePercentage)
        {
            if (bms.IsNullOrEmpty() || bms.Width <= 2 || bms.Height <= 2)
                return null;

            int fontSize = (int)Math.Ceiling((decimal)bms.Height).FindValueByPercentages(100, fontSizePercentage);

            if (fontSize <= 0)
                fontSize = 1;

            FormattedText ftxt =
                new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                new Typeface("Thoma"), fontSize, System.Windows.Media.Brushes.Red);//, new Point(0, 0));

            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext context = visual.RenderOpen())
            {
                context.DrawImage(bms, bms.ToRect());
                context.DrawText(ftxt, new System.Windows.Point(1, 1));

            }

            RenderTargetBitmap rtb = new RenderTargetBitmap(bms.PixelWidth, bms.PixelHeight, 96, 96, PixelFormats.Default);
            rtb.Render(visual);

            return rtb.ToWriteableBitmap();
        }*/


        public static int GetStride(this BitmapSource bms)
        {
            if (bms.IsNullOrEmpty())
                return 0;

            return bms.PixelWidth * (bms.Format.BitsPerPixel / 8);
        }

        public static byte[] ToPixelsByteArray(this BitmapSource bms)
        {
            if (bms.IsNullOrEmpty())
                return null;

            int stride = bms.GetStride();

            byte[] data = new byte[stride * bms.PixelHeight];

            bms.CopyPixels(data, stride, 0);

            return null;
        }


        public static BitmapSource Copy(this BitmapSource bms)
        {
            if (bms.IsNullOrEmpty())
                return null;

            return bms.Clone();
        }

        public static BitmapFrame ResizeToBitmapFrame(this BitmapSource source, int percentage, BitmapScalingMode mode = BitmapScalingMode.NearestNeighbor)
        {
            if (source.IsNullOrEmpty() || percentage <= 0)
                return null;

            int width = (int)((decimal)source.Width).FindValueByPercentages(100, percentage);
            int height = (int)((decimal)source.Height).FindValueByPercentages(100, percentage);

            return source.ResizeToBitmapFrame(width, height, mode);
        }

        public static BitmapFrame ResizeToBitmapFrame(this BitmapSource source, int width, int height, BitmapScalingMode mode = BitmapScalingMode.NearestNeighbor)
        {
            if (source.IsNullOrEmpty() || width <= 0 || height <= 0)
                return null;

            if (source.Width == width && source.Height == height)
                return source.ToBitmapFrame();

            DrawingGroup group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, mode);
            group.Children.Add(new ImageDrawing(source, new Rect(0, 0, width, height)));

            DrawingVisual visual = new DrawingVisual();
            DrawingContext context = visual.RenderOpen();

            context.DrawDrawing(group);

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            context.Close();
            rtb.Render(visual);
            
            BitmapFrame result = BitmapFrame.Create(rtb);
            
            return result;
        }


    }
}
