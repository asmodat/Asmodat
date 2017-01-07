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
using Asmodat.Extensions.Windows.Media.Imaging;

using Asmodat.Natives;

namespace Asmodat.Extensions.Drawing
{

    public static partial class BitmapEx
    {
        /// <summary>
        /// Can be used in conjunction with ToByteArray extention method
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Bitmap ToBitmap(this byte[] data)
        {
            if (data.IsNullOrEmpty())
                return null;
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    stream.Write(data, 0, data.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    Bitmap bmp = new Bitmap(stream);
                    return bmp;
                }
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return null;
            }
        }

        public static Bitmap ToBitmap(this WriteableBitmap wbm)
        {
            if (wbm.IsNullOrEmpty())
                return null;

            Bitmap bmp;
            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create((BitmapSource)wbm));
                encoder.Save(stream);
                bmp = new Bitmap(stream);
            }

            return bmp;
        }

        public static Bitmap ToBitmap(this BitmapImage bmi)
        {
            if (bmi.IsNullOrEmpty())
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmi));
                encoder.Save(stream);
                Bitmap bmp = new Bitmap(stream);
                return new Bitmap(bmp);
            }
        }

        public static Bitmap ToBitmap(this Rectangle rect)
        {
            return new Bitmap(rect.Width, rect.Height);
        }

        public static Bitmap ToBitmap(this System.Drawing.Size size)
        {
            return new Bitmap(size.Width, size.Height);
        }

        public static Bitmap ToBitmap(this Image img)
        {
            if (img == null)
                return null;

            return (Bitmap)img;
        }
    }
}
