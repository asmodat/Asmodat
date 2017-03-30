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


#pragma warning disable IDE1006 // Naming Styles
    public static class doubleEx
#pragma warning restore IDE1006 // Naming Styles
    {
        public static double GetSign(this double d)
        {
            if (double.IsNaN(d))
                return double.NaN;
            else if (d == 0)
                return 0;
            else return d > 0 ? 1 : -1;
        }

        public static bool IsNaN(this double d) { return double.IsNaN(d); }
        public static bool IsAnyNaN(params double[] t)
        {
            if (t.IsNullOrEmpty())
                return false;

            foreach (var d in t)
                if (double.IsNaN(d))
                    return true;

           return false;
        }

        public static bool IsValOrNaN(this double d, double val) { return double.IsNaN(d) || d == val; }

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

            if (double.TryParse(value, out double result))
                return result;

            return _default;
        }

        /// <summary>
        /// This method only works to some precision extend, not all combinations can be asserted successfully for example:
        /// 10000000000000.1d returns 1;
        /// 100000000000000.1d returns 0;
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int CountDecimalPlaces(this double value)
        {
            if (double.IsNaN(value))
                return -1;
            else if (value == 0)
                return 0;
            
            return BitConverter.GetBytes(decimal.GetBits((decimal)Math.Abs(value))[3])[2];
        }

    }
}
