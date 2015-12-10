using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

namespace AsmodatMath
{
    public partial class AMath
    {

        /// <summary>
        /// | n |
        /// | k |
        /// </summary>
        /// <param name="n"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double BinomialCoefficient(uint n, uint k)
        {
            if (n < k)
                return 0;

            double numerator = 1, i = n, denominator;
            for (; i > k; i--)
                numerator *= i;


            denominator = Factorial(n - k);
            return (double)numerator / denominator;
        }


        public static uint Factorial(uint n)
        {
            if (n < 0)
                throw new Exception("Factorial can't be negative !");

            uint result = 1, i = 1;
            for (; i <= n; i++)
                result *= i;

            return result;
        }



        public static double BinomialDistribution(uint nTrials, uint kSuccesses, double successPropability)
        {
            double BC = BinomialCoefficient(nTrials, kSuccesses);
            double P1 = AMath.Pow(successPropability, kSuccesses);
            double P2 = AMath.Pow(1 - successPropability, (nTrials - kSuccesses));

            return BC * P1 * P2;
        }
    }
}
