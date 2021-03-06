﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics; //Complex32.Log((Complex32)x).Real;
using System.Numerics;

//using cdrnet.Lib.MathLib; //(new Scalar.Logarithm.ScalarNaturalLogarithm(new Scalar.ScalarExpressionValue(x))).Calculate();

//using MathLib;

using Asmodat.Abbreviate;

namespace AsmodatMath
{
    public partial class AMath
    {
        const double E = 2.71828182845904523536028747135266249775724709369995;
        public static ThreadedDictionary<double, double> ThreadedNaturalLogarithm = new ThreadedDictionary<double, double>();


        public static double Ln(double x)
        {
            double var;

            if (ThreadedNaturalLogarithm.ContainsKey(x))
                var = ThreadedNaturalLogarithm[x];
            else
            {
                var = Math.Log(x);
                ThreadedNaturalLogarithm.Add(x, var);
            }

            return var;
        }

    }
}


//public static double Ln(double x)
//        {


//            uint i;
//            double taylor, ex;

//            ex = x - E;
//            taylor = ex;
//            i = 2;

//            for (; i < 10; i++)
//            {
//                if (i % 2 == 0) taylor -= Math.Pow(ex, i) * (1.0 / i);
//                else taylor += Math.Pow(ex, i) * (1.0 / i);
//            }

//            return taylor;
//        }

//       private static int[] tab64 = new int[64]
//    {
//        63,  0, 58,  1, 59, 47, 53,  2,
//        60, 39, 48, 27, 54, 33, 42,  3,
//    61, 51, 37, 40, 49, 18, 28, 20,
//    55, 30, 34, 11, 43, 14, 22,  4,
//    62, 57, 46, 52, 38, 26, 32, 41,
//    50, 36, 17, 19, 29, 10, 13, 21,
//    56, 45, 25, 31, 35, 16,  9, 12,
//    44, 24, 15,  8, 23,  7,  6,  5
//    };

//        public static int Log2_64(UInt64 value)
//        {
//            value |= value >> 1;
//            value |= value >> 2;
//            value |= value >> 4;
//            value |= value >> 8;
//            value |= value >> 16;
//            value |= value >> 32;

//            return tab64[((UInt64)((value - (value >> 1)) * 0x07EDD5E59A4E28C2)) >> 58];
//        }


//public static double Exp(double exponent)
//{
//    //if (exponent > 700 || exponent < -700)
//    //    return double.NaN;

//    double X, P, Frac, I, L, Iterations;
//    Iterations = 10;
//    X = exponent;
//    Frac = X;
//    P = (1.0 + X);
//    I = 1.0;

//    do
//    {
//        I++;
//        Frac *= (X / I);
//        L = P;
//        P += Frac;
//        if (--Iterations == 0) break;
//    } while (L != P);

//    return P;
//}


//public static double Ln(double power)
//{
//    double N, P, L, R, A, E, Iterations1, Iterations2;
//    E = 2.71828182845905;
//    Iterations1 = 10;
//    Iterations2 = 10;
//    P = power;
//    N = 0.0;

//    while (P >= E)
//    {
//        P /= E;
//        N++;
//        if (--Iterations1 == 0) break;
//    }

//    N += (P / E);
//    P = power;

//    do
//    {
//        A = N;
//        L = (P / (AMath.Exp(N - 1.0)));
//        R = ((N - 1.0) * E);
//        N = ((L + R) / E);

//        if (--Iterations2 == 0) break;
//    } while (N != A);

//    return N;
//}

//double d = Math.Exp(725);
//            double d1 = Math.Exp(-725);
//            double d2 = AMath.Exp(725);
//            double d3 = AMath.Exp(-725);
//            if (d != d1 || d2 != d3)
//                return;