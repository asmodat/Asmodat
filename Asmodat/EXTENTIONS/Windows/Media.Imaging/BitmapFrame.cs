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
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions.Windows.Media.Imaging;

namespace Asmodat.Extensions.Windows.Media.Imaging
{
    

    public static class BitmapFrameEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this BitmapFrame bf)
        {
            if (bf == null || bf.Width <= 0 || bf.Height <= 0)
                return true;

            return false;
        }

        public static BitmapFrame ToBitmapFrameFast(this Bitmap bmp)
        {
            return bmp.ToBitmapFrameFast(PixelFormats.Bgr24);
        }
        public static BitmapFrame ToBitmapFrameFast(this Bitmap bmp, System.Windows.Media.PixelFormat format)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            var data = bmp.LockBits(bmp.ToRectangle(), ImageLockMode.ReadOnly, bmp.PixelFormat);
            BitmapSource src = BitmapSource.Create(bmp.Width, bmp.Height, 96, 96, format, null,
                data.Scan0, data.Stride * data.Height, data.Stride
                );

            bmp.UnlockBits(data);

            return BitmapFrame.Create(src);
        }

        public static BitmapFrame ToBitmapFrame(this BitmapSource source)
        {
            if (source.IsNullOrEmpty())
                return null;

            return BitmapFrame.Create(source);
        }


        public static BitmapSource CompressToBitmapSource(this BitmapFrame frame, int quality)
        {
            if (frame.IsNullOrEmpty())
                return null;

            return (BitmapSource)frame.CompressToImageSource(quality);
        }
        

        public static ImageSource CompressToImageSource(this BitmapFrame bmf, int quality)
        {
            if (bmf.IsNullOrEmpty())
                return null;

            if (quality <= 0) quality = 1;
            else if (quality > 100) quality = 100;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = quality;
            encoder.Frames.Add(bmf);


            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = new MemoryStream(stream.ToArray());
                bmi.EndInit();
                return bmi;
            }
        }

        

        public static byte[] CompressToByteArray(this BitmapFrame bmf, int quality)
        {
            if (bmf.IsNullOrEmpty())
                return null;

            if (quality <= 0) quality = 1;
            else if (quality > 100) quality = 100;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = quality;
            encoder.Frames.Add(bmf);


            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        public static Bitmap CompressToBitmap(this BitmapFrame bmf, int quality)
        {
            if (bmf.IsNullOrEmpty())
                return null;

            if (quality <= 0) quality = 1;
            else if (quality > 100) quality = 100;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = quality;
            encoder.Frames.Add(bmf);

            MemoryStream stream = new MemoryStream();
            encoder.Save(stream);

            
            return stream.ToBitmap();
            //return new Bitmap(stream);
        }


        
        //not working
            public static BitmapFrame Copy(this BitmapFrame frame)
        {
            if (frame.IsNullOrEmpty())
                return null;

            return frame.Clone().ToBitmapFrame();
        }

        public static BitmapFrame Resize(this BitmapFrame frame, int percentage, BitmapScalingMode mode = BitmapScalingMode.NearestNeighbor)
        {
            if (frame.IsNullOrEmpty() || percentage <= 0)
                return null;

            int width = (int)((decimal)frame.Width).FindValueByPercentages(100, percentage);
            int height = (int)((decimal)frame.Height).FindValueByPercentages(100, percentage);

            return frame.Resize(width, height, mode);
        }

        public static BitmapFrame Resize(this BitmapFrame frame, int width, int height, BitmapScalingMode mode = BitmapScalingMode.NearestNeighbor)
        {
            if (frame.IsNullOrEmpty() || width <= 0 || height <= 0)
                return null;

            if (frame.Width == width && frame.Height == height)
                return (BitmapFrame)frame.Clone();

            DrawingGroup group = new DrawingGroup();
            RenderOptions.SetBitmapScalingMode(group, mode);
            group.Children.Add(new ImageDrawing(frame, new Rect(0, 0, width, height)));

            DrawingVisual visual = new DrawingVisual();
            DrawingContext context = visual.RenderOpen();

            context.DrawDrawing(group);

            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            context.Close();
            rtb.Render(visual);

            BitmapFrame result = BitmapFrame.Create(rtb);
            return result;
        }


        public enum BitmapFrameFormatMode
        {
            Bmp = 0,
            Jpeg = 1,
            Wmp = 2,
            Gif = 3,
            Png = 4,
            Tiff = 5

        }

        public static BitmapEncoder GetBitmapEncoder(this BitmapFrameFormatMode mode)
        {
            BitmapEncoder encoder = null;

            switch (mode)
            {
                case BitmapFrameFormatMode.Bmp:
                    encoder = new BmpBitmapEncoder();
                    break;
                case BitmapFrameFormatMode.Jpeg:
                    encoder = new JpegBitmapEncoder();
                    break;
                case BitmapFrameFormatMode.Gif:
                    encoder = new GifBitmapEncoder();
                    break;
                case BitmapFrameFormatMode.Png:
                    encoder = new PngBitmapEncoder();
                    break;
                case BitmapFrameFormatMode.Tiff:
                    encoder = new TiffBitmapEncoder();
                    break;
                case BitmapFrameFormatMode.Wmp:
                    encoder = new WmpBitmapEncoder();
                    break;
                default:
                    encoder = null;
                    break;
            }

            return encoder;
        }

        public static byte[] ToByteArray(this BitmapFrame frame, BitmapFrameFormatMode mode = BitmapFrameFormatMode.Png)
        {
            if (frame.IsNullOrEmpty())
                return null;

            byte[] result = null;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = mode.GetBitmapEncoder();
                
                if (encoder != null)
                {
                    encoder.Frames.Add(frame);
                    encoder.Save(stream);
                    result = stream.ToArray();
                }

        }

            return result;
        }



        public static Bitmap ToBitmap(this BitmapFrame frame)
        {
            if (frame.IsNullOrEmpty())
                return null;

            byte[] data = frame.ToByteArray(BitmapFrameFormatMode.Png);

            if (data.IsNullOrEmpty())
                return null;

            return data.ToBitmap();
        }
    }
}
