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
        /// This method calculates count of sparate variables
        /// data -> 1,1,1,2,2,3,4,5,6,6,6
        /// output -> 1-3,2-2,4-1,5-1,6-3
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Dictionary<double, int> Distribution(double[] data)
        {
            if (data == null || data.Length <= 0) return null;

            Dictionary<double, int> output = new Dictionary<double, int>();
            foreach (double val in data)
                if (output.ContainsKey(val)) ++output[val];
                else output.Add(val, 1);

            return output.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

    }
}
