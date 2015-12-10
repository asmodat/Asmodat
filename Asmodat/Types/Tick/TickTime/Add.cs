using System;
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



namespace Asmodat.Types
{
    
    public partial struct TickTime : IComparable, IEquatable<TickTime>, IEquatable<long>, IComparable<TickTime>, IComparable<long>
    {
        public TickTime Copy()
        {
            return new TickTime(this.Ticks);
        }

        public TickTime Add(long value, TickTime.Unit unit)
        {
            return new TickTime(this.Ticks + (value * (long)unit));
        }

        public TickTime AddMicroseconds(long value)
        {
            return this.Add(value, Unit.us);
        }

        public TickTime AddMilliseconds(long value)
        {
            return this.Add(value, Unit.ms);
        }

        public TickTime AddSeconds(long value)
        {
            return this.Add(value, Unit.s);
        }

        public TickTime AddMinutes(long value)
        {
            return this.Add(value, Unit.m);
        }

        public TickTime AddHours(long value)
        {
            return this.Add(value, Unit.h);
        }

        public TickTime AddDays(long value)
        {
            return this.Add(value, Unit.d);
        }

        public TickTime AddWeeks(long value)
        {
            return this.Add(value, Unit.w);
        }

        //---------------------------------------------------------------------------
        // ADD NOW
        //---------------------------------------------------------------------------
        public static TickTime AddNow(long value, TickTime.Unit unit)
        {
            return new TickTime(TickTime.NowTicks + (value * (long)unit));
        }

        public static TickTime AddNowMicroseconds(long value)
        {
            return AddNow(value, Unit.us);
        }

        public static TickTime AddNowMiliseconds(long value)
        {
            return AddNow(value, Unit.ms);
        }

        public static TickTime AddNowSeconds(long value)
        {
            return AddNow(value, Unit.s);
        }

        public static TickTime AddNowMinutes(long value)
        {
            return AddNow(value, Unit.m);
        }

        public static TickTime AddNowHours(long value)
        {
            return AddNow(value, Unit.h);
        }

        public static TickTime AddNowDays(long value)
        {
            return AddNow(value, Unit.d);
        }

        public static TickTime AddNowWeeks(long value)
        {
            return AddNow(value, Unit.w);
        }
    }
}
