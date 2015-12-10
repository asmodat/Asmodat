using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

namespace AsmodatMath
{
    public partial class MonteCarloSimulation
    {



        /// <summary>
        /// this is faster for short span
        /// Simulates next value of double set
        /// </summary>
        /// <param name="data"></param>
        /// <returns>future random output</returns>
        public static double Next(double[] data)
        {
            if (data == null || data.Length <= 0) return double.NaN;

            double[] PFR = MonteCarloSimulation.PeriodicChange(data);
            double Averange = AMath.Average(PFR);
            double Variance = AMath.Variance(PFR, Averange, false);
            double Drift = Averange - (Variance / 2.0);
            double StandardDeviation = Math.Sqrt(Variance); //AMath.StandarDeviation(Variance);
            return data.Last() * Math.Exp(Drift + StandardDeviation * AExcel.NORMSINV(AMath.Random()));
        }

        /// <summary>
        /// This is faster for longer span
        /// </summary>
        /// <param name="data"></param>
        /// <param name="average"></param>
        /// <param name="periodic"></param>
        /// <param name="variance"></param>
        /// <returns></returns>
        public static double Next(double[] data, ref double average, ref List<double> periodic, ref double variance)
        {
            double[] PDR = MonteCarloSimulation.PeriodicChange(data, ref periodic);
            average = AMath.Average(PDR, average);
            variance = AMath.Variance(PDR, average, variance, false);
            double Drift = average - (variance / 2.0);
            double StandardDeviation = Math.Sqrt(variance); //AMath.StandarDeviation(Variance);
            return data.Last() * Math.Exp(Drift + StandardDeviation * AExcel.NORMSINV(AMath.Random()));
        }

        /// <summary>
        /// Simulates N next values
        /// </summary>
        /// <param name="data"></param>
        /// <param name="N">count of values in generated sequence</param>
        /// <returns></returns>
        public static List<double> Next(double[] data, int N)
        {
            if (data == null || data.Length <= 0) return null;

            List<double> input = data.ToList();
            double[] output = new double[N];
            double average = double.NaN;
            double variance = double.NaN;
            List<double> periodic = new List<double>();

            int i = 0;
            for (; i < N; i++)
            {
                output[i] = MonteCarloSimulation.Next(input.ToArray(), ref average, ref periodic, ref variance);
                input.Add(output[i]);
            }

            return output.ToList();
        }



        /// <summary>
        /// Calculates periodic change of data
        /// proof:
        /// TodayPrice = YesterdayPrice*e^r -> TP/YP = e^r
        /// ln(TP/YP) = ln(e^r) -> ln(TP/YP) = r;
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double[] PeriodicChange(double[] data)
        {
            int length = data.Length;
            double[] output = new double[length - 1];

            int i = 1;
            for (; i < length; i++)
                output[i - 1] = Math.Log(data[i] / data[i - 1]);

            return output;
        }


        public static double[] PeriodicChange(double[] data, ref List<double> last)
        {
            int length = data.Length;
            double[] output = new double[length - 1];

            if (last.Count == 0)
                output = MonteCarloSimulation.PeriodicChange(data);
            else
            {
                int LN1 = data.Length - 1;
                last.Add(Math.Log(data[LN1] / data[LN1 - 1]));
                output = last.ToArray();
            }


            return output;
        }
    }
}



///// <summary>
//        /// Simulates N next values K times
//        /// </summary>
//        /// <param name="data"></param>
//        /// <param name="N">count of values in generated sequence</param>
//        /// <param name="K">count of sequence simulations</param>
//        /// <returns></returns>
//        public static List<List<double>> Next(double[] data, int N, int K, bool parrarel = false)
//        {
//            if (data == null || data.Length <= 0) return null;

//             List<List<double>> output = new List<List<double>>();
//             object lock_obj = new object();

//             if (parrarel)
//                 Parallel.For(0, K, i => 
//                 {
//                     List<double> part = MonteCarloSimulation.Next(data.ToArray(), N);

//                     lock(lock_obj)
//                         output.Add(part); 
//                 });
//             else
//             {
//                 int i = 0;
//                 for (; i < K; i++)
//                     output.Add(MonteCarloSimulation.Next(data.ToArray(), N));
//             }

//            return output;
//        }