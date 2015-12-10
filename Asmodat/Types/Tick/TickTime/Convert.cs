using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Threading;
using System.Runtime.Serialization;
using System.Globalization;
using Asmodat.Debugging;

namespace Asmodat.Types
{
    
    public partial struct TickTime : IComparable, IEquatable<TickTime>, IEquatable<long>, IComparable<TickTime>, IComparable<long>
    {
        public static readonly System.DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);

        public static TickTime FromUnixTimeStamp(double timestamp)
        {
            if (timestamp <= 0)
                return TickTime.Default;

            System.DateTime date = UnixEpoch.AddSeconds(timestamp).ToLocalTime();
            return new TickTime(date);
        }
        

        public static TickTime FromJavaTimeStamp(double timestamp)
        {
            if (timestamp <= 0)
                return TickTime.Default;

            System.DateTime date = UnixEpoch.AddSeconds(Math.Round(timestamp / 1000)).ToLocalTime();
            return new TickTime(date);
        }

        public static double ToUnixTimeStamp(DateTime date)
        {
            return (date.ToUniversalTime() - UnixEpoch.ToUniversalTime()).TotalSeconds;
        }



        public double ToUnixTimeStamp()
        {
            return (this.UTC.ToUniversalTime() - UnixEpoch.ToUniversalTime()).TotalSeconds;
        }




        public string ToRFC3339()
        {
            return XmlConvert.ToString((DateTime)this, XmlDateTimeSerializationMode.Utc);
        }

        public static TickTime FromRFC3339(string date)
        {
            if (date.IsNullOrWhiteSpace())
                return TickTime.Default;

            return new TickTime(XmlConvert.ToDateTime(date, XmlDateTimeSerializationMode.Utc));
        }

       /* public static TickTime FromRFC1123(string date)
        {
            if (date.IsNullOrWhiteSpace())
                return TickTime.Default;

            try
            {
                DateTime.Parse(date, CultureInfo.CurrentCulture.DateTimeFormat.RFC1123Pattern);

                return TickTime.Default;
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return TickTime.Default;
            }

            
        }*/


        public string ToString(string format)
        {
            return ((DateTime)(this)).ToString(format);
        }


    }
}
