using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Extensions.Drawing
{
    

    public static class PointEx
    {
        public static bool AllCoordinatesNegative(this Point pnt)
        {
            if (pnt.X < 0 && pnt.Y < 0)
                return true;
            return false;
        }

        public static bool AnyCoordinateNegative(this Point pnt)
        {
            if (pnt.X < 0 || pnt.Y < 0)
                return true;
            return false;
        }

    }
}
