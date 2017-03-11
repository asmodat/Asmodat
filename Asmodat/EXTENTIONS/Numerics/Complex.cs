using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Extensions.Objects;
using System.Numerics;

namespace Asmodat.Extensions.Numerics
{
    

    public static class ComplexEx
    {
        public static bool IsNaN(this Complex c)
        {
            return double.IsNaN(c.Real) || double.IsNaN(c.Imaginary);
        }

        /// <summary>
        /// Turns complex into one by one complex, with signs
        /// example: 0.5 -0.1i => 1 - i
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Complex GetSign(this Complex c)
        {
            return new Complex(c.Real.GetSign(), c.Imaginary.GetSign());
        }
    }
}
