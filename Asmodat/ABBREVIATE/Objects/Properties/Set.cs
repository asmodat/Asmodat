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

using Asmodat.Types;

namespace Asmodat.Abbreviate
{
    public static partial class Objects
    {
        public static bool SetParsedProperty(object source, string property, object value, bool canRead = true, string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff")
        {

            if (source == null) return false;
            Type TSource = source.GetType();
            if (TSource == null) return false;
            if (property == null) return true;
            PropertyInfo PInfo = TSource.GetProperty(property);
            if (PInfo == null || !PInfo.CanWrite || canRead != PInfo.CanRead) return false;

            if (!Objects.Parse(PInfo.PropertyType, ref value, dateTimeFormat))
                return false;

            PInfo.SetValue(source, value, null);

            return true;
        }

        /// <summary>
        /// This method allows to set property balue by its name
        /// </summary>
        /// <param name="destination">It is object that contains property, value will be written into. </param>
        /// <param name="property">It is name of property that will be set</param>
        /// <param name="value">It is value of property that will be set</param>
        /// <param name="canRead">If tru it is checked if property can be read, if fale if it can't . If null its not checked</param>
        /// <returns>returns true if falue was set into property</returns>
        public static bool SetProperty<TDestination>(ref TDestination destination, string property, object value, ThreeState canRead)// bool canRead = true)
        {
            Type TSource = destination.GetType();
            if (TSource == null) return false;
            PropertyInfo PInfo = TSource.GetProperty(property);
            if (PInfo == null || !PInfo.CanWrite || (canRead != ThreeState.Null && canRead != PInfo.CanRead)) return false;


            PInfo.SetValue(destination, value, null);
            return true;
        }

        /// <summary>
        /// This method allows to set multiple properties into source object by its names
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="source"></param>
        /// <param name="canRead"></param>
        /// <returns></returns>
        public static bool SetProperties<TDestination>(ref TDestination destination, Dictionary<string, object> source, ThreeState canRead)
        {
            bool success = true;
            foreach (KeyValuePair<string, object> KVP in source)
                if (!Objects.SetProperty<TDestination>(ref destination, KVP.Key, KVP.Value, canRead))
                    success = false;

            return success;
        }

        public static bool SetProperty(object source, string property, object value, bool checkCollecionsProperties = false, bool invoked = false)
        {
            Type TSource = source.GetType();
            bool bReturn = false;

            if (invoked && source is Control)
            {
                ((Control)source).Invoke((MethodInvoker)(() =>
                {
                    PropertyInfo PInfo = TSource.GetProperty(property);
                    if (PInfo != null)
                    {
                        PInfo.SetValue(source, value, null);
                        bReturn = true;
                    }
                    else bReturn = false;
                }));
            }
            else return Objects.SetProperty(source, property, value, true);

            return bReturn;
        }
    }
}
