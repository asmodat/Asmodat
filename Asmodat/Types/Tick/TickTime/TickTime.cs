﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

using Asmodat.Abbreviate;

using System.Threading;
using System.Runtime.Serialization;
using System.Globalization;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions;
using Asmodat.Types;

namespace Asmodat.Types
{
    //TODO: IFormattable,

    /// <summary>
    /// This class stores and manages DateTime as UTC tick long value, it is safe and efficient to use with serialization instead of DateTime
    /// </summary>
    [Serializable]
    [DataContract(Name = "tick_time")]
    public partial struct TickTime : IComparable, IEquatable<TickTime>, IEquatable<long>, IComparable<TickTime>, IComparable<long>
    {

        public bool IsUndefined
        {
            get
            {
                if (Ticks <= 0)
                    return true;
                else
                    return false;
            }
        }

        public double Microseconds { get { return this.TotalMicroseconds; } }
        public long Miliseconds { get { return (long)this.TotalMiliseconds; } }
        public long Seconds { get { return (long)this.TotalSeconds; } }
        public long Minutes { get { return (long)this.TotalMinutes; } }
        public long Hours { get { return (long)this.TotalHours; } }
        public long Days { get { return (long)this.TotalDays; } }
        public long Weeks { get { return (long)this.TotalWeeks; } }

        public double TotalMicroseconds { get { return this.Total(Unit.us); } }
        public double TotalMiliseconds { get { return this.Total(Unit.ms); } }
        public double TotalSeconds { get { return this.Total(Unit.s); } }
        public double TotalMinutes { get { return this.Total(Unit.m); } }
        public double TotalHours { get { return this.Total(Unit.h); } }
        public double TotalDays { get { return this.Total(Unit.d); } }
        public double TotalWeeks { get { return this.Total(Unit.w); } }


        /// <summary>
        /// Determines how many units of time passed since begining of time
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public double Total(Unit unit) { return Ticks / (double)unit; }


        public string TotalString(bool addPrefixZeros, params Unit[] units)
        {
            units = units?.Distinct()?.ToArray();
            if (units.IsNullOrEmpty())
                return null;

            string result = "";
            long value;
            foreach (var u in units)
            {
                value = (long)Total(u);

                if (!result.IsNullOrEmpty()) //first value should not be limited
                    value %= (long)u.GetBase();

                if (!addPrefixZeros && value == 0 && result.IsNullOrEmpty()) //remove prefix zeros if specified
                    continue;

                result += $"{value}{u.ToString()} ";
            }
            
            return result.Trim();
        }


        public bool IsDefault
        {
            get
            {
                if (Ticks == DefaultTicks)
                    return true;
                else return false;
            }
        }

        public static long DefaultTicks = 0;

        public static TickTime Default
        { 
            get
            {
                return new TickTime(DefaultTicks);
            }
        }
    


        public TickTime(long Ticks) //: this()
        {
            this.Ticks = Ticks;
        }
        public TickTime(DateTime DateTime)
        {
            this.Ticks = DateTime.ToUniversalTime().Ticks;
        }

        [DataMember(Name = "ticks")]
        [XmlElement("ticks")]
        public long Ticks;


        [IgnoreDataMember]
        [XmlIgnore]
        public DateTime UTC
        {
            get
            {
                return new DateTime(Ticks, DateTimeKind.Utc);
            }
            set
            {
                if (value.Kind != DateTimeKind.Utc) throw new ArgumentException("Passed Date is not UTC");
                Ticks = value.ToUniversalTime().Ticks;//you cannot be ceratin that passes value is UTC
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public const string FormatDateTime = "yyyy-MM-dd HH:mm:ss";

        [IgnoreDataMember]
        [XmlIgnore]
        public const string FormatLongDateTime = "yyyy-MM-dd HH:mm:ss.fff";

        [IgnoreDataMember]
        [XmlIgnore]
        public const string FormatLongTime = "HH:mm:ss.fff";

        [IgnoreDataMember]
        [XmlIgnore]
        public const string FormatTime = "HH:mm:ss";
        //hh:mm:ss.fff

        public string ToDateTimeString()
        {
            return this.UTC.ToString(FormatDateTime, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts UTC tick time into LongFormat string: "yy-MM-dd HH:mm:ss.fff"
        /// </summary>
        /// <returns>Returns formated string "yy-MM-dd HH:mm:ss.fff"</returns>
        public string ToLongDateTimeString()
        {
            return this.UTC.ToString(FormatLongDateTime, CultureInfo.InvariantCulture);
        }

        public string ToLongTimeString()
        {
            return this.UTC.ToString(FormatLongTime, CultureInfo.InvariantCulture);
        }

        public string ToTimeString()
        {
            return this.UTC.ToString(FormatTime, CultureInfo.InvariantCulture);
        }

        public string ToStringExact(string format)
        {
            return this.UTC.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses string date using specified format from const object's of TickTime class
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static TickTime ParseExact(string date, string format)
        {
            return new TickTime(DateTime.ParseExact(date, format, CultureInfo.InvariantCulture));
        }

        

        


        public enum Unit : long
        {
            us = 10,
            ms = 1000 * us,
            s =  1000 * ms,
            m =  60 * s,
            h = 60 * m,
            d = 24 * h,
            w = 7 * d
        }


        /// <summary>
        ///  returns true if time is out else false
        /// </summary>
        /// <param name="start"></param>
        /// <param name="timeout"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static bool Timeout(TickTime start, long timeout, Unit unit = Unit.ms)
        {
            TickTime end = start + ((long)unit * timeout);

            if (TickTime.Now >= end) return true;
            else return false;
        }

        public static decimal TimeoutSpan(TickTime start, long timeout, Unit unit, Unit unit_result)
        {
            TickTime end = start + ((long)unit * timeout);
            return end.Span(unit_result);
        }


        [IgnoreDataMember]
        [XmlIgnore]
        public DateTime Local
        {
            get
            {
                return new DateTime(Ticks, DateTimeKind.Utc).ToLocalTime();
            }
            set
            {
                if (value.Kind != DateTimeKind.Local) throw new ArgumentException("Passed Date is not a local time");
                Ticks = value.ToUniversalTime().Ticks;
            }
        }

        /// <summary>
        /// This property returns current span between now and stored tick value
        /// </summary>
        // [IgnoreDataMember]
        // [XmlIgnore]
        public long Span()
        {
            return (TickTime.Now.Ticks - this.Ticks);
        }


        public decimal Span(TickTime.Unit unit)
        {
            return (decimal)(TickTime.Now.Ticks - this.Ticks) / (decimal)unit;
        }


        public TickTime TickSpan
        {
            get
            {
                return (TickTime.Now - this.Ticks);
            }
        }

        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime MinValue { get { return new TickTime(0); } }

        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime MaxValue { get { return new TickTime(long.MaxValue); } }

        [IgnoreDataMember]
        [XmlIgnore]
#pragma warning disable IDE1006 // Naming Styles
        public static TickTime us { get { return new TickTime(10); } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime ms { get { return TickTime.us * (long)1000; } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime s { get { return TickTime.ms * (long)1000; ; } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime m { get { return TickTime.s * (long)60; ; } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime h { get { return TickTime.m * (long)60; ; } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime d { get { return TickTime.h * (long)24; ; } }
        [IgnoreDataMember]
        [XmlIgnore]
        public static TickTime w { get { return TickTime.d * (long)7; ; } }
#pragma warning restore IDE1006 // Naming Styles

        public int CompareTo(object obj)
        {
            TickTime that = (TickTime)obj;

            return this.Ticks.CompareTo(that.Ticks);
        }

        public int CompareTo(TickTime that)
        {
            return this.Ticks.CompareTo(that.Ticks);
        }

        public int CompareTo(long that)
        {
            return this.Ticks.CompareTo(that);
        }

        public override string ToString()
        {
            return Ticks.ToString();
        }
        public static TickTime Parse(string value)
        {
            return new TickTime(long.Parse(value));
        }

        public bool Equals(TickTime that)
        {
            return this.Ticks == that.Ticks;
        }
        public bool Equals(long that)
        {
            return this.Ticks == that;
        }
        public override bool Equals(object obj)
        {
            if (obj is TickTime) return Equals((TickTime)obj);
            else if (!Objects.IsNumber(obj)) return false;

            return Ticks == (long)obj;
        }

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }

    }
}
///// <summary>
///// Returns UTC current tick time
///// </summary>
//[XmlIgnore]
//public static long Now
//{
//    get
//    {
//        return Asmodat.Abbreviate.UTC.Now; //System.Diagnostics.Stopwatch.GetTimestamp(); //DateTime.Now.ToUniversalTime().Ticks;
//    }
//}


///// <summary>
///// Returns UTC date time
///// </summary>
//[XmlIgnore]
//public static DateTime Now
//{
//    get
//    {
//        return new DateTime(TickTime.Stamp, DateTimeKind.Utc);
//    }
//}



        //[XmlIgnore]
        //public DateTime Unspecified
        //{
        //    set
        //    {
        //        Ticks = value.ToUniversalTime().Ticks;//you cannot be ceratin that passes value is UTC
        //    }
        //}

//public long Total(DateTime DT)
//{
//    return (DT.ToUniversalTime().Ticks - this.Ticks);
//}


//


//public int CompareTo(object obj)
//{
//    return 1;
//}

//public int CompareTo(TickTime TT)
//{
//    if (this.Ticks < TT.Ticks) return 1;
//    else if (this.Ticks > TT.Ticks) return -1;
//    else return 0;
//}

//public static bool operator ==(TickTime x, TickTime y)
//{
//    return x.Ticks == y.Ticks ? true : false;
//}
//public static TickTime operator !=(TickTime x, TickTime y)
//{
//    if (x.Value == 0 || y.Value == 0) return Null;
//    return x.Value != y.Value ? True : False;
//}