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

namespace Asmodat.Extensions.Drawing
{


    public static partial class BitmapEx
    {

        


       /* public static bool EqualsAllNotEmpty(this Rectangle[,] rm1, Rectangle[,] rm2)
        {
            if (rm1 == null || rm2 == null)
                return false;

            int xParts = rm1.Width();
            int yParts = rm1.Height();

            int x = 0, y;
            for (; x < xParts; x++)
            {
                for (y = 0; y < yParts; y++)
                {
                    if (rm1[x, y].IsEmpty || rm2[x, y].IsEmpty)
                        continue;

                    if (!rm1[x,y].EqualSize(rm2[x,y]))
                        return false;
                }
            }

            return true;
        }*/


        /// <summary>
        /// not tested
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="bitmaps"></param>
        /// <returns></returns>
        public static Bitmap Join(this Bitmap bmp, Bitmap[,] bitmaps, int width, int height)
        {
            int xParts = bitmaps.Width();
            int yParts = bitmaps.Height();

            if (width <= 0 || height <= 0 || xParts <= 0 || yParts <= 0)
                return null;

            if (xParts == 1 && yParts == 1 && !bitmaps[0, 0].IsNullOrEmpty())
                return bitmaps[0, 0].CopyDeep();

            if (bmp.IsNullOrEmpty() || bmp.Width != width || bmp.Height != height)
            {
                bmp = new Bitmap(width, height);
                bmp.Clear(Color.Green);
            }

            int x = 0, y;

            Bitmap result = bmp.TryCopy();
            Rectangle[,] rectangles = bmp.ToRectangle().Split(xParts, yParts);
            GraphicsUnit unit = GraphicsUnit.Pixel;

            using (Graphics graphics = Graphics.FromImage(result))
            {
                for (; x < xParts; x++)
                {
                    for (y = 0; y < yParts; y++)
                    {
                        if (bitmaps[x, y] == null)
                            continue;

                        graphics.DrawImage(bitmaps[x, y], rectangles[x, y], bitmaps[x, y].ToRectangle(), unit);
                    }
                }
            }

            return result;
        }
    }
}
