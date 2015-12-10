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

namespace Asmodat.Abbreviate
{
    public static partial class Objects
    {

        public static object GetProperty(object source, string property, bool checkCollecionsProperties = false, bool invoked = false)
        {
            if (source == null) return null;

            object value = null;
            Type TSource = source.GetType();

            if (invoked && source is Control)
                ((Control)source).Invoke((MethodInvoker)(() =>
                {
                    PropertyInfo PInfo = TSource.GetProperty(property);
                    if (PInfo != null) value = PInfo.GetValue(source, null);
                }));
            else
            {
                PropertyInfo PInfo = TSource.GetProperty(property);
                if (PInfo != null) value = PInfo.GetValue(source, null);
            }

            return value;
        }

        /// <summary>
        /// Gets property ny its name that Can(be)Read and Can(be)Write
        /// </summary>
        /// <param name="source">Object that contains property</param>
        /// <param name="property">Property name.</param>
        /// <returns>Property value.</returns>
        public static object GetProperty(object source, string property)
        {
            if (source == null) return null;

            object value = null;
            Type TSource = source.GetType();

            PropertyInfo PInfo = TSource.GetProperty(property);
            if (PInfo != null && PInfo.CanRead && PInfo.CanWrite) value = PInfo.GetValue(source, null);
            else throw new Exception("GetProperty Excpetion, Property cant be null and must be Read and Write accesible");

            return value;
        }


        public static Dictionary<object, object> GetProperties(object source, bool canWriteCheck = true, bool canReadCheck = true)
        {
            Type TSource = source.GetType();

            PropertyInfo[] APInfo = TSource.GetProperties();
            Dictionary<object, object> DSOData = new Dictionary<object, object>();
            List<Exception> LExceptions = new List<Exception>();

            foreach (PropertyInfo PI in APInfo)
                if (canWriteCheck && !PI.CanWrite) continue;
                else if (canReadCheck && !PI.CanRead) continue;
                else
                {
                    try
                    {
                        object oValue = PI.GetValue(source, null);
                        string sName = nameof(oValue);

                        if (sName == null)
                            sName = PI.Name;

                        if (!DSOData.ContainsKey(sName))
                            DSOData.Add(sName, oValue);//PI.PropertyType.FullName + " " + 
                    }
                    catch (Exception e)
                    {
                        LExceptions.Add(e);
                        continue;
                    }
                }

            return DSOData;
        }


        
    }
}
