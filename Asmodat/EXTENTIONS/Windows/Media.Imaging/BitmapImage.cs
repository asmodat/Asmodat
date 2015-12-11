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
namespace Asmodat.Extensions.Windows.Media.Imaging
{
    

    public static class BitmapImageEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this BitmapImage bmpi)
        {
            if (bmpi == null || bmpi.Width <= 0 || bmpi.Height <= 0)
                return true;

            return false;
        }

        public static BitmapImage ToBitmapImage(this Bitmap bmp, ImageFormat format)
        {
            if (bmp.IsNullOrEmpty())
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                bmp.Save(stream, format);
                stream.Position = 0;
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = stream;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();
                return bmi;
            }
        }


        public static BitmapImage ToBitmapImage(this BitmapSource bms)
        {
            if (bms.IsNullOrEmpty())
                return null;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapImage bmi = new BitmapImage();

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(bms));
                encoder.Save(stream);

                bmi.BeginInit();
                bmi.StreamSource = new MemoryStream(stream.ToArray());
                bmi.EndInit();
            }

            return bmi;
        }

        


    }
}
