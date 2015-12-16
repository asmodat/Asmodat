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

    public sealed class JpegCodecInfoSingleton
    {
        private JpegCodecInfoSingleton()
        {
            CodecInfo = BitmapEx.GetEncoder(ImageFormat.Jpeg);
        }

        public ImageCodecInfo CodecInfo = null;

        static readonly JpegCodecInfoSingleton _instance = new JpegCodecInfoSingleton();
        public static JpegCodecInfoSingleton Instance
        {
            get { return _instance; }
        }
    }

    public static partial class BitmapEx
    {
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                    return codec;
            }

            return null;
        }


        public static Bitmap Compress(this Bitmap bmp, long quality)
        {
            if (bmp == null)
                return null;

            return bmp.Compress(quality, null);//, bmp.PixelFormat);
        }


        /// <summary>
        /// System.Drawing.Imaging.PixelFormat.Format32bppArgb allows for faster compression, as it matches graphical adapter format
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="quality"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Bitmap Compress(this Bitmap bmp, long quality, PixelFormat? format = System.Drawing.Imaging.PixelFormat.Format32bppArgb)
        {
            if (bmp.IsNullOrEmpty() || quality < 0)
                return null;

            if (quality == 100 && format.IsNull())
                return bmp.TryCopy();

            if (quality == 100 && !format.IsNull())
                return bmp.Format(format.Value);

            try
            {
                ImageCodecInfo jpgEncoder = JpegCodecInfoSingleton.Instance.CodecInfo;
                EncoderParameters parameters = new EncoderParameters(1);
                EncoderParameter parameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                parameters.Param[0] = parameter;

                MemoryStream stream = new MemoryStream();
                bmp.Save(stream, jpgEncoder, parameters);

                return stream.ToBitmap(format);

            }
            catch
            {
                return null;
            }
        }
    }
}

/*

    

*/
