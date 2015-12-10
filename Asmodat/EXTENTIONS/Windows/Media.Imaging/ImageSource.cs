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

namespace Asmodat.Extensions.Windows.Media.Imaging
{
    

    public static class ImageSourceEx
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this ImageSource ims)
        {
            if (ims == null || ims.Width <= 0 || ims.Height <= 0)
                return true;

            return false;
        }


     


        public static ImageSource Compress(this ImageSource ims, int quality)
        {
            if (ims.IsNullOrEmpty())
                return null;

            if (quality <= 0) quality = 1;
            else if (quality > 100) quality = 100;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = quality;
            BitmapFrame frame = ((BitmapSource)ims).ToBitmapFrame();
            encoder.Frames.Add(frame);
            
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

    }
}
