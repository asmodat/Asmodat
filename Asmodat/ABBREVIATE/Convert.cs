using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Abbreviate
{
    public class Convert
    {
        /// <summary>
        /// Converts Enum into string. Size of characters does not matter.
        /// </summary>
        /// <typeparam name="T">Type of Enum</typeparam>
        /// <param name="TEnum">Enum value</param>
        /// <returns>String name of Enum variable or null if variable name is NULL</returns>
        public static string ToString<T>(T TEnum) where T : struct, IConvertible
        {
            string sName = Enum.GetName(typeof(T), TEnum);
            if (sName.ToUpper() == "NULL")
                return null;
            else return sName;
        }

        /// <summary>
        /// Converts String into Enum. Size of characters does not matter.
        /// </summary>
        /// <typeparam name="T">Type of Enum</typeparam>
        /// <param name="sName">String name of Enum</param>
        /// <returns>Returns Enum variable or default (0'th) element of enum if no such string exist.</returns>
        public static T ToEnum<T>(string sName) where T : struct, IConvertible
        {
            // if (!typeof(T).IsEnum)
            if (sName == null) return default(T);
            sName = sName.ToUpper();

            foreach (T TEnum in (T[])Enum.GetValues(typeof(T)))
            {
                string sTEName = Convert.ToString<T>(TEnum);
                if (sTEName == null) continue;
                else sTEName = sTEName.ToUpper();
                if (sTEName == sName) return TEnum;
            }

            return default(T);
        }


        /// <summary>
        /// Tested, and working, complementary with ToBytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static short ToShort(byte[] bytes, int index = 0, bool swapped = false)
        {
            if(swapped) return (short)((((int)bytes[index + 1]) << 8) | (int)bytes[index]);
            else  return (short)((((int)bytes[index]) << 8) | (int)bytes[index + 1]);
        }


        /// <summary>
        /// Tested, and working, complementary with ToShort
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static byte[] ToBytes(short number, bool swapped = false)
        {
            if(swapped) return new byte[2] { (byte)number, (byte)(number >> 8) };
            else return new byte[2] { (byte)(number >> 8), (byte)number };
        }
    }
}


/*
short tester
List<byte[]> output = new List<byte[]>();
            List<short> output2 = new List<short>();
            int counter = 0;
            for(int i = short.MinValue; i <= short.MaxValue ; i++)
            {
                short s = (short)i;
                byte[] bytes = Abbreviate.Convert.ToBytes(s);
                short revers = Abbreviate.Convert.ToShort(bytes);
                byte[] bytes2 = Abbreviate.Convert.ToBytes(revers);
                short revers2 = Abbreviate.Convert.ToShort(bytes);
                output.Add(bytes);
                output2.Add(s);

                if (bytes[0] != bytes2[0] || bytes[1] != bytes2[1] || revers != s || revers != revers2)
                {
                    break;
                }

            }
*/
