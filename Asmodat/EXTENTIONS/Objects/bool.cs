using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Extensions.Objects
{
    

    public static class boolEx
    {
        public static void SetAllValues(this bool[] array, bool value)
        {
            if (array.IsNullOrEmpty())
                return;

            int l = array.Length;
            if (value)
            {
                int i = 0;
                for (; i < l; i++)
                    array[i] = value;
            }
            else
            {
                Array.Clear(array, 0, l);
            }
        }

       



        public static void InvertAllValues(this bool[] array)
        {
            if (array.IsNullOrEmpty())
                return;

            int i = 0, l = array.Length;
            for (; i < l; i++)
                array[i] = !array[i];
        }

        public static void InvertAllValues(this bool[,] array)
        {
            if (array.IsNullOrEmpty())
                return;

            int x = 0, y, w = array.Width(), h = array.Height();
            for (; x < w; x++)
            {
                for (y = 0; y < h; y++)
                {
                    array[x, y] = !array[x, y];
                }
            }
        }

        public static bool[,] Copy(this bool[,] array)
        {
            if (array.IsNullOrEmpty())
                return null;

            int w = array.Width(), h = array.Height();
            bool[,] result = new bool[w, h];

            Array.Copy(array, result, w * h);

            return result;
        }

        public static bool[,] SurroundAllValues(this bool[,] array, bool surround, bool value, int deep = 1)
        {
            if (array.IsNullOrEmpty())
                return null;
            
            int x, y, w = array.Width(), h = array.Height(), wm1, hm1;
            wm1 = w - 1;
            hm1 = h - 1;

            bool[,] _array = array.Copy();
            bool[,] result = array.Copy();
            bool changed;
            while (--deep >= 0)
            {
                changed = false;

                for (x = 0; x < w; x++)
                    for (y = 0; y < h; y++)
                    {
                        if (_array[x, y] != surround)
                            continue;
                        else
                            changed = true;


                        if (x > 0)
                        {
                            if (y > 0 && _array[x - 1, y - 1] != surround) result[x - 1, y - 1] = value;
                            if (_array[x - 1, y] != surround) result[x - 1, y] = value;
                            if (y < hm1 && _array[x - 1, y + 1] != surround) result[x - 1, y + 1] = value;
                        }

                        if (x < wm1)
                        {
                            if (y > 0 && _array[x + 1, y - 1] != surround) result[x + 1, y - 1] = value;
                            if (_array[x + 1, y] != surround) result[x + 1, y] = value;
                            if (y < hm1 && _array[x + 1, y + 1] != surround) result[x + 1, y + 1] = value;
                        }

                        if (y > 0 && _array[x, y - 1] != surround) result[x, y - 1] = value;
                        if (y < hm1 && _array[x, y + 1] != surround) result[x, y + 1] = value;
                    }

                if (surround == value || changed)
                    break;
                else
                {
                    _array = result.Copy();
                }
            }


            return result;
        }



    }
}
