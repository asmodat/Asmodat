using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

using System.Numerics;
using System.Globalization;

namespace AsmodatMath
{
    public partial class AMath
    {

        public static void GetFirstMax(double[] array, out int index, out double value)
        {
            int i = 0, count = array.Length, tempi = -1;
            double tempv = double.MinValue, val;
            for (; i < array.Length; i++)
            {
                val = array[i];
                if (double.IsNaN(val))
                    continue;


                if (val > tempv)
                {
                    tempv = val;
                    tempi = i; ;
                }
            }

            index = tempi;
            value = tempv;
        }

        public static int GetIndexMax(double[] array, bool first = true)
        {
            if (array == null || array.Length <= 0)
                return -1;

            int i = 0, count = array.Length, tempi = -1;
            double tempv = double.MinValue, val;
            for (; i < array.Length; i++)
            {
                val = array[i];
                if (double.IsNaN(val))
                    continue;

                if (first && val > tempv)
                {
                    tempv = val;
                    tempi = i;
                }
                else if (!first && val >= tempv)
                {
                    tempv = val;
                    tempi = i;
                }
            }

            return tempi;
        }

        /// <summary>
        /// Counts coumber of values above specified treshold
        /// </summary>
        /// <param name="values"></param>
        /// <param name="treshold"></param>
        /// <returns></returns>
        public static int CountAbove(double[] values, double treshold)
        {
            if (values == null || double.IsNaN(treshold))
                return -1;

            int i = 0, count = values.Length, counter = 0;
            for (; i < count; i++)

                if (!double.IsNaN(values[i]) && values[i] > treshold)
                    ++counter;

            return counter;
        }

        public static int CountBelow(double[] values, double treshold)
        {
            if (values == null || double.IsNaN(treshold))
                return -1;

            int i = 0, count = values.Length, counter = 0;
            for (; i < count; i++)

                if (!double.IsNaN(values[i]) && values[i] < treshold)
                    ++counter;

            return counter;
        }

        /// <summary>
        /// Counts parts of continuous array that are below cerain level
        /// </summary>
        /// <param name="values"></param>
        /// <param name="treshold"></param>
        /// <returns></returns>
        public static int CountPartsBelow(double[] values, double treshold)
        {
            if (values == null || double.IsNaN(treshold))
                return -1;

            int i = 0, count = values.Length, parts = 0;
            bool started = false;
            for (; i < count; i++)
            {
                if (!double.IsNaN(values[i]))
                    continue;

                if (values[i] < treshold && !started)
                {
                    ++parts;
                    started = true;
                }

                if (values[i] >= treshold)
                    started = false;
            }

            return parts;
        }


        public static int CountPartsAbove(double[] values, double treshold)
        {
            if (values == null || double.IsNaN(treshold))
                return -1;

            int i = 0, count = values.Length, parts = 0;
            bool started = false;
            for (; i < count; i++)
            {
                if (double.IsNaN(values[i]))
                    continue;

                if (values[i] > treshold && !started)
                {
                    ++parts;
                    started = true;
                }

                if (values[i] <= treshold)
                    started = false;
            }

            return parts;
        }

        /// <summary>
        /// Returns indexes of global minimas specified amount of global maxima parts in array
        /// </summary>
        /// <param name="values"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        public static int[] GetSplitIndexesMinima(double[] values, int parts)
        {
            if (values == null || values.Length < 1)
                return null;

            double[] sorted = values.ToArray().OrderByDescending(d => d).ToArray();
            double treshold_upper = double.NaN;
            for (int i = 0; i < sorted.Length; i++)
            {
                int counter = AMath.CountPartsAbove(values, sorted[i]);
                if (counter == parts)
                {   //select smallest
                    treshold_upper = sorted[i];
                }
                //else if (counter > parts)
               //     break;
            }


            if (double.IsNaN(treshold_upper))
                return null;


            List<int> selected = new List<int>();

            bool started = false;
            bool bottom_found = false;
            double min = double.MaxValue;
            int index = -1, index_start = -1;
            for (int i = 0; i < values.Length; i++)
            {
                double value = values[i];

                if (!started && value <= treshold_upper)
                    continue;
                else started = true;

                if (!bottom_found && value <= treshold_upper)
                {
                    bottom_found = true;
                    min = double.MaxValue;
                }


                if (value > treshold_upper)
                {
                    if (bottom_found)
                    {
                        int result = -1;
                        if (index_start == index) result = index;
                        else result = (int)(index_start + index) / 2;

                        selected.Add(result);

                        index_start = -1;
                    }

                    bottom_found = false;

                    if (selected.Count == parts - 1)
                        break;
                }


                if (bottom_found && value <= min)
                {
                    if (index_start < 0)
                        index_start = i;
                    
                    min = value;
                    index = i;
                }
            

                
            }

            if (selected.Count != parts - 1)
                return null;
            else return selected.ToArray();
        }

        
    }
}

