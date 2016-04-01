using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

using System.Data;

using System.Windows.Forms;

using System.Collections;

using System.Diagnostics;

using System.Globalization;

using System.Linq.Expressions;

using System.Numerics;



namespace Asmodat.Abbreviate
{
    public  static partial class Objects
    {
        /// <summary>
        ///  checs is one array contains the same object as in anoter one, refard of  its position
        /// </summary>
        /// <param name="oa1"></param>
        /// <param name="oa2"></param>
        /// <returns></returns>
        public static bool EqualsItems(object[] oa1, object[] oa2)
        {
            if (oa1 == oa2) return true;
            if (oa1 == null || oa2 == null) return false;
            if (oa1.Length != oa2.Length) return false;
            

            int length = oa1.Length;
            int i = 0, i2;
            bool found;

            for (; i < length; i++)
            {
                found = false;
                for (i2 = 0; i2 < length; i2++)
                    if (oa1[i] == oa2[i2])
                    {
                        found = true;
                        break;
                    }

                if (!found)
                    return false;
            }

            return true;
        }
        
        

        public static bool IsAnyNullOrEmpty(params string[] data)
        {
            foreach (string s in data)
                if (System.String.IsNullOrEmpty(s)) return true;

            return false;
        }


        public static T[] ToArray<T>(object[] array)
        {
            if (array == null) return null;
            return array.Cast<T>().ToArray();
        }

         public static void Swap<T>(ref T o1, ref T o2)
         {
             T o3 = o1;
             o1 = o2;
             o2 = o3;
         }


        /// <summary>
        /// This custom Equal method compares two objects with base Equal method of firs object and is nullably safe
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static new bool Equals(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            if (obj1 == null || obj2 == null) return false;

            return obj1.Equals(obj2);
        }
        


        public static bool IsNumber(object value)
        {
            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal
                || value is BigInteger;
        }

        
        

        public static bool Parse(Type parseType, ref object value, string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff")
        {
            if (value == null)  return true; 
            if (value.GetType() != typeof(string)) return false;

            if (parseType != typeof(string)) //parse object
            {
                string sValue = value.ToString();

                try
                {
                    if (parseType == typeof(double))
                    {

                        int indexDot = sValue.IndexOf('.');
                        int indexComma = sValue.IndexOf(',');

                        if (indexDot > 0 && indexComma > 0 && (indexDot > indexComma)) //"1,263.33" - remove dot from spread precision format
                            sValue = sValue.Replace(".", "");


                        sValue = sValue.Replace('.', ',');
                        value = double.Parse(sValue);
                    }
                    else if (parseType == typeof(int))
                    {
                        value = int.Parse(sValue);
                    }
                    else if (parseType == typeof(DateTime))
                    {
                        //value = DateTime.ParseExact(sValue,"M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None);
                        value = DateTime.ParseExact(sValue, dateTimeFormat, CultureInfo.InvariantCulture);
                    }
                    else if (parseType == typeof(bool))
                    {
                        value = bool.Parse(sValue);
                    }
                }
                catch
                {
                    if (sValue.Length > 0) throw new Exception("Unknown Format");

                    return false;
                }
            }

            return true;
        }


        

        



        //public static Dictionary<object, object> GetProperties(object source, List<string> LSProperties, bool blackList, bool canWriteCheck = true, bool canReadCheck = true)
        //{
        //    Type TSource = source.GetType();

        //    PropertyInfo[] APInfo = TSource.GetProperties();
        //    Dictionary<object, object> DSOData = new Dictionary<object, object>();
        //    List<Exception> LExceptions = new List<Exception>();

        //    foreach (PropertyInfo PI in APInfo)
        //        if (canWriteCheck && !PI.CanWrite) continue;
        //        else if (canReadCheck && !PI.CanRead) continue;
        //        else
        //        {
        //            try
        //            {
        //                string sPIName = PI.Name;
        //                if(LSProperties != null)
        //                {
        //                    if(LSProperties.Contains())
        //                }

        //                object oValue = PI.GetValue(source, null);
        //                string sName = nameof(oValue);

        //                if (sName == null)
        //                    sName = PI.Name;

        //                if (!DSOData.ContainsKey(sName))
        //                    DSOData.Add(sName, oValue);//PI.PropertyType.FullName + " " + 
        //            }
        //            catch (Exception e)
        //            {
        //                LExceptions.Add(e);
        //                continue;
        //            }
        //        }

        //    return DSOData;
        //}



    }
}
