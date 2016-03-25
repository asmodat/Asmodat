using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using System.Numerics;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Extensions.Objects
{


    public static class doubleEx
    {
        public static bool TryParse(this string value, out double result)
        {
            if(value.IsNullOrEmpty())
            {
                result = default(double);
                return false;
            }

            return double.TryParse(value, out result);
        }


        public static double TryParse(this string value, double _default)
        {
            if (value.IsNullOrEmpty())
                return _default;

            double result;

            if (double.TryParse(value, out result))
                return result;

            return _default;
        }
    }
}
