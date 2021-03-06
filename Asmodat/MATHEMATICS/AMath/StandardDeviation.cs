﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmodatMath
{
    public partial class AMath
    {
        /// <summary>
        /// confidence must be from (0 to 100)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="average"></param>
        /// <param name="confidence"></param>
        /// <param name="population"></param>
        /// <returns></returns>
        public static double StandarConfidence(double[] data, double average, double confidence, bool population = false)
        {
            if (data == null || data.Length <= 0) return double.NaN;

            double sigma = Math.Sqrt(AMath.Variance(data, average, population));
            double multiplayer = (AExcel.NORMSINV((double)confidence / 100) + 1.5) / 2;

            return sigma * multiplayer;
        }



        /// <summary>
        /// Standard deviation σ (sigma) is a measure of how spread out numbers are, it is a squer root of the variance
        /// set population to true then return sqrt(sum / (double)length); -> for standard deviation Variance
        /// set population to false then return sqrt(sum / (double)(length - 1)); -> population standard deviation variance
        /// </summary>
        /// <param name="data"></param>
        /// <param name="population"></param>
        /// <returns></returns>
        public static double StandarDeviation(double[] data, bool population = false)
        {
            if (data == null || data.Length <= 0) return double.NaN;
            return Math.Sqrt(AMath.Variance(data, data.Average(), population));
        }

        /// <summary>
        /// Standard deviation σ (sigma) is a measure of how spread out numbers are
        /// it is a squer root of the variance
        /// 
        /// set population to true then return sqrt(sum / (double)length); -> for standard deviation Variance
        /// set population to false then return sqrt(sum / (double)(length - 1)); -> population standard deviation variance
        /// </summary>
        /// <param name="data"></param>
        /// <param name="average"></param>
        /// <param name="population"></param>
        /// <returns></returns>
        public static double StandarDeviation(double[] data, double average, bool population = false)
        {
            if (data == null || data.Length <= 0) return double.NaN;
            return Math.Sqrt(AMath.Variance(data, average, population));
        }

        /// <summary>
        /// Standard deviation σ (sigma) is a measure of how spread out numbers are
        /// it is a squer root of the variance
        /// </summary>
        /// <param name="variance"></param>
        /// <returns></returns>
        public static double StandarDeviation(double variance)
        {
            return Math.Sqrt(variance);
        }

        
    }
}
