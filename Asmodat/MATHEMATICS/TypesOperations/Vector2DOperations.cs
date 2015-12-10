using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Types;

namespace AsmodatMath
{
    public partial class AMath
    {
        

        public static bool InArea(double value, double start, double end)
        {
            if (
                value >= start &&
                value <= end
                )
                return true;
            else return false;
        }

        public static bool InArea(int value, int start, int end)
        {
            if (
                value >= start &&
                value <= end
                )
                return true;
            else return false;
        }


        /// <summary>
        /// checks if point sits inside vector area
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public static bool InArea(Vector2D vector, Point2D point)
        {
            if (
                point.X >= vector.MinX &&
                point.X <= vector.MaxX &&
                point.Y >= vector.MinY &&
                point.Y <= vector.MaxY
                )
                return true;
            else return false;
        }


        /// <summary>
        /// Checks if  value area sits inside vector area
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool InArea(Vector2D vector, Vector2D value)
        {
            if (
                value.MinX >= vector.MinX &&
                value.MaxX <= vector.MaxX &&
                value.MinY >= vector.MinY &&
                value.MaxY <= vector.MaxY
                )
                return true;
            else return false;
        }

    }
}
