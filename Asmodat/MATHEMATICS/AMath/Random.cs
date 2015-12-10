using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;

namespace AsmodatMath
{
    public partial class AMath
    {
        [ThreadStatic]
        private static Random _random;
        private static object _random_sync = new object();

        /// <summary>
        /// This method generates threadsafe random number from range (0.0, 1.0)
        /// </summary>
        /// <returns>returns double (0.0, 1.0) </returns>
        public static double Random()
        {
            double output = 0;

            lock (_random_sync)
            {
                if (_random == null)
                {
                    byte[] cryptoresult = new byte[4];
                    new RNGCryptoServiceProvider().GetBytes(cryptoresult);//RNGCryptoServiceProvider RNGCSProvider = 
                    int seed = BitConverter.ToInt32(cryptoresult, 0);
                    _random = new Random(seed);
                }

                output = _random.NextDouble();

            }

            if (output == 0 || output == 1)
                throw new Exception("Ooops this should be exclusive...");

            return output;
        }

        /// <summary>
        /// returns value from range [min, max)
        /// Methid never returns upper value !
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int Random(int min, int max)
        {
            int value = 0;

            lock (_random_sync)
            {
                if (_random == null)
                {
                    byte[] cryptoresult = new byte[4];
                    new RNGCryptoServiceProvider().GetBytes(cryptoresult);//RNGCryptoServiceProvider RNGCSProvider = 
                    int seed = BitConverter.ToInt32(cryptoresult, 0);
                    _random = new Random(seed);
                }


                value = _random.Next(min, max);
            }


            return value;
        }


        public static long Random(long min, long max)
        {
            long value = 0;

            if (min == max) return min;

            lock (_random_sync)
            {
                if (_random == null)
                {
                    byte[] cryptoresult = new byte[4];
                    new RNGCryptoServiceProvider().GetBytes(cryptoresult);//RNGCryptoServiceProvider RNGCSProvider = 
                    int seed = BitConverter.ToInt32(cryptoresult, 0);
                    _random = new Random(seed);
                }

                byte[] buf = new byte[8];
                _random.NextBytes(buf);
                long lrand = BitConverter.ToInt64(buf, 0);
                value = (Math.Abs(lrand % (max - min)) + min);
            }


            return value;
        }


        public static double Random(double min, double max, uint decimals)
        {
            if (min > max)
            {
                
                Asmodat.Debugging.Output.WriteWithMethods("AMath random max value is less then min !", 4);
                throw new Exception("AMath random max value is less then min !");
            } 
                

            double power = AMath.Pow(10, decimals);
            min *= power;
            max *= power;

            long lMin = (long)Math.Round(min, 0);
            long lMax = (long)Math.Round(max, 0);

            if (lMin < 0 || lMax < 0)
                throw new Exception("Random long overflow !");

            long value = AMath.Random(lMin, lMax);

            

            return Math.Round((double)value / power, (int)decimals);
        }

        /*
        double AMP1e8 = AMath.Pow(10, 8);
            int iAmountMin = (amountMin * AMP1e8);
            int iAmountMax = (amountMax * AMP1e8);
            */

        public static int[] Random(int min, int max, int count)
        {
            if((max - min) / 2 <= count || min >= max || count <= 0) //collisions protection
                return null;

            List<int> output = new List<int>();
            int value;

            lock (_random_sync)
            {
                if (_random == null)
                {
                    byte[] cryptoresult = new byte[4];
                    new RNGCryptoServiceProvider().GetBytes(cryptoresult);//RNGCryptoServiceProvider RNGCSProvider = 
                    int seed = BitConverter.ToInt32(cryptoresult, 0);
                    _random = new Random(seed);
                }

                
                while (output.Count < count)
                {
                    value = _random.Next(min, max);

                    if (!output.Contains(value))
                        output.Add(value);
                }
            }


            return output.ToArray();
        }


    }
}
