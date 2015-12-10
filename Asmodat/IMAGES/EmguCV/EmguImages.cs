using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

using Asmodat.IO;
using System.Drawing;
using Asmodat.Types;
//using System.Drawing.Imaging;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using Asmodat.Debugging;

namespace Asmodat.Imaging
{
    
    public partial class EmguImages
    {
        public static KeyValuePair<Point2D, double>? MatchBest(Bitmap source, Bitmap template)
        {
            if (source == null || template == null)
                return null;
            
            Image<Bgr, byte> src = new Image<Bgr, byte>(source);
            Image<Bgr, byte> tmp = new Image<Bgr, byte>(template);
            KeyValuePair<Point2D, double> pair = new KeyValuePair<Point2D, double>();

            try
            {
                using (Image<Gray, float> result = src.MatchTemplate(tmp, Emgu.CV.CvEnum.TemplateMatchingType.CcoeffNormed))
                {
                    double[] minV, maxV;
                    Point[] minL, maxL;

                    result.MinMax(out minV, out maxV, out minL, out maxL);

                    int idx = AsmodatMath.AMath.GetIndexMax(maxV, true);
                    if (idx >= 0)
                        pair = new KeyValuePair<Point2D, double>((Point2D)maxL[idx], maxV[idx]);
                    else return null;


                }
            }
            catch(Exception ex)
            {
                Output.WriteException(ex);
                return null;
            }

            return pair;
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
