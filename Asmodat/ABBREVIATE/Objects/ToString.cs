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
        /// <summary>
        /// Default custom DateTime ISO-8601 format
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dateTimeFormat">Invartian culture, default "yyyy-MM-ddTHH:mm:ss.fff", type for example: "M/d/yyyy h:mm:ss tt"</param>
        /// <returns></returns>
        public static string ToString(object value, string dateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fff")
        {
            if (value == null) return null;

            if (value.GetType() == typeof(DateTime))
            {
                //return ((DateTime)value).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                return ((DateTime)value).ToString(dateTimeFormat, CultureInfo.InvariantCulture);
            }

            return value.ToString();
        }


        

    }
}
