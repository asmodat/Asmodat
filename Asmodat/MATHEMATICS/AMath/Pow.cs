using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatMath
{
    public partial class AMath
    {
        /// <summary>
        /// if (exp less then 0) throw new Exception("Pow only positive exp. can be used");
        /// This version is fixed, tested and working !
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static double Pow(double num, uint exp)
        {
            double result = 1.0;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= num;
                exp >>= 1;
                num *= num;
            }

            return result;
        }

        /*
        /// <summary>
        /// Carefoul not tested
        /// </summary>
        /// <param name="num"></param>
        /// <param name="exp"></param>
        /// <returns></returns>
        public static double PowApproximation(double num, double exp)
        {
            int tmp = (int)(BitConverter.DoubleToInt64Bits(num) >> 32);
            int tmp2 = (int)(exp * (tmp - 1072632447) + 1072632447);

            return BitConverter.DoubleToInt64Bits(((long)tmp2) << 32);
        }
        */

        public static double Pow2(double value) { return value * value; }
        public static double Pow3(double value) { return value * value * value; }
        public static double Pow4(double value) { return value * value * value * value; }
        public static double Pow5(double value) { return value * value * value * value * value; }
        public static double Pow6(double value) { return value * value * value * value * value * value; }
        public static double Pow7(double value) { return value * value * value * value * value * value * value; }
    }
}
