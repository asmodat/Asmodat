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
using System.IO;
using Asmodat.Extensions.Drawing;

namespace Asmodat.Imaging
{
    
    public partial class Images
    {
        public static Bitmap GetBitmap(string path)
        {
            path = Files.GetFullPath(path);

            if (!Files.Exists(path))
                return null;

            try
            {
                return new Bitmap(path);
            }
            catch
            {
                return null;
            }
        }

        public static bool SetBitmap(Bitmap bmp, string path, ImageFormat format)
        {

            path = Files.GetFullPath(path);
            var info = Directories.Create(Files.GetDirectory(path));
            

            if (info == null)
                return false;
         

            try
            {
                bmp.Save(path,format);
            }
            catch
            {
                return false;
            }

            return true;
        }



        public static bool Compare(Bitmap bmp1, Bitmap bmp2, Rectangle? rect1 = null, Rectangle? rect2 = null)
        {
            if (bmp1 == null || bmp2 == null)
                return false;

            byte[] data1, data2;


            if (rect1 == null) data1 = bmp1.GetRawBytes();
            else data1 = bmp1.GetRawBytes((Rectangle)rect1);

            if (rect2 == null) data2 = bmp2.GetRawBytes();
            else data2 = bmp2.GetRawBytes((Rectangle)rect2);

            return Bytes.Compare(data1, data2);
        }

        public static double Similarity32Bit(Bitmap bmp1, Bitmap bmp2, Rectangle? rect1 = null, Rectangle? rect2 = null)
        {
            if(bmp1 == null || bmp2 == null)
                return 0;

            byte[] data1, data2;

            if (rect1 == null) data1 = bmp1.GetRawBytes();
            else data1 = bmp1.GetRawBytes((Rectangle)rect1);

            if (rect2 == null) data2 = bmp2.GetRawBytes();
            else data2 = bmp2.GetRawBytes((Rectangle)rect2);

            return Bytes.Similarity32Bit(data1, data2);
        }




        /// <summary>
        /// returns sample position inside source bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <param name="sample"></param>
        /// <param name="similarity">percentage of minimum similarity</param>
        /// <returns></returns>
        public static Dictionary<Point2D, double> GetLocations(Bitmap source, Bitmap sample, double similarityMin = 100, double similarityMax = 200, PixelFormat format = PixelFormat.Format24bppRgb)
        {
            Dictionary<Point2D, double> outputs = new Dictionary<Point2D, double>();

            if (source == null || sample == null)
                return outputs;


            int sourceWidth = source.Width;
            int sourceHeight = source.Height;
            int sampleWidth = sample.Width;
            int sampleHeight = sample.Height;
            

            if(sampleWidth > sourceWidth || sampleHeight > sourceHeight)
                return outputs;
            
            Bitmap sourceFormated = Images.Convert(source, format);
            Bitmap sampleFormated = Images.Convert(sample, format);

            byte[] sampleFData = sampleFormated.GetRawBytes();
            byte[] data;
            Size size = sample.Size;
            Point2D location;
            double value;
            double max;
            double valueMax = double.MinValue;
            Point2D locationMax;
            for (int ix = 0; ix < (sourceWidth - sampleWidth); ix++)
            {
                max = double.MinValue;
                for (int iy = 0; iy < (sourceHeight - sampleHeight); iy++)
                {
                    location = new Point2D(ix, iy);
                    data = sourceFormated.GetRawBytes(new Rectangle((Point)location, size));
                    value = Bytes.Similarity8Bit(data, sampleFData);
                    if (value >= similarityMin && value <= similarityMax)
                        outputs.Add(location, value);
                    else if(value < (similarityMin - 1))
                    {
                        int move = (int)((double)(91 - value) * sampleHeight)/100;
                        if (move > 0)
                            iy += move;
                    }

                    if (value > max)
                        max = value;

                    if (valueMax < value)
                    {
                        valueMax = value;
                        locationMax = location;
                    }
                }

                if (max >= 0)
                {
                    int move = (int)((double)(91 - max) * sampleWidth) / 100;
                    if (move > 0)
                        ix += move;
                }                                                                                                                                                                                                                                                                                                                                                

            }


                return outputs;
        }


        public static Bitmap Convert(Bitmap source, PixelFormat format)
        {
            if (source == null)
                return null;

            Bitmap bmp = new Bitmap(source.Width, source.Height, format);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {
                graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height));
            }
            return bmp;
        }

        public static Bitmap Convert(Bitmap source, ImageFormat format)
        {
            if (source == null)
                return null;


            Bitmap bmp = null;
            using (MemoryStream memory = new MemoryStream())
            {
                source.Save(memory, format);
                bmp = new Bitmap(memory);
            }

            return bmp;
        }

        /*public static Bitmap Convert(Bitmap source, PixelFormat format)
        {
            if (source == null)
                return null;

            using (Bitmap bmp = new Bitmap(source.Width, source.Height, format))
            {
                using (Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.DrawImage(source, new Rectangle(0, 0, source.Width, source.Height));
                    return new Bitmap(bmp);
                }
            }
        }

        public static Bitmap Convert(Bitmap source, ImageFormat format)
        {
            if (source == null)
                return null;
            
            using (MemoryStream memory = new MemoryStream())
            {
                source.Save(memory, format);
                return new Bitmap(memory);
            }
        }*/

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
