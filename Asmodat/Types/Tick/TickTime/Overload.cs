using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.Xml;

using Asmodat.Abbreviate;

using System.Threading;

namespace Asmodat.Types
{
    //TODO: IFormattable,

    /// <summary>
    /// This class stores and manages DateTime as UTC tick long value, it is safe and efficient to use with serialization instead of DateTime
    /// </summary>
    public partial struct TickTime : IComparable, IEquatable<TickTime>, IEquatable<long>, IComparable<TickTime>, IComparable<long>
    {
        #region Math Operators
        public static TickTime operator +(TickTime x, long y)
        {
            return new TickTime(x.Ticks + y);
        }
        public static TickTime operator -(TickTime x, long y)
        {
            return new TickTime(x.Ticks - y);
        }
        public static TickTime operator *(TickTime x, long y)
        {
            return new TickTime(x.Ticks * y);
        }
        public static TickTime operator /(TickTime x, long y)
        {
            return new TickTime(x.Ticks * y);
        }
        public static bool operator ==(TickTime x, long y)
        {
            return x.Ticks == y;
        }
        public static bool operator !=(TickTime x, long y)
        {
            return x.Ticks == y;
        }
        public static bool operator >(TickTime x, long y)
        {
            return x.Ticks > y;
        }
        public static bool operator <(TickTime x, long y)
        {
            return x.Ticks < y;
        }
        public static bool operator >=(TickTime x, long y)
        {
            return x.Ticks >= y;
        }
        public static bool operator <=(TickTime x, long y)
        {
            return x.Ticks <= y;
        }

        public static TickTime operator ++(TickTime x)
        {
            return new TickTime(++x.Ticks);
        }
        public static TickTime operator --(TickTime x)
        {
            return new TickTime(--x.Ticks);
        }

        public static TickTime operator +(TickTime x, TickTime y)
        {
            return new TickTime(x.Ticks + y.Ticks);
        }
        public static TickTime operator -(TickTime x, TickTime y)
        {
            return new TickTime(x.Ticks - y.Ticks);
        }
        public static TickTime operator *(TickTime x, TickTime y)
        {
            return new TickTime(x.Ticks * y.Ticks);
        }
        public static TickTime operator /(TickTime x, TickTime y)
        {
            return new TickTime(x.Ticks * y.Ticks);
        }
        public static bool operator ==(TickTime x, TickTime y)
        {
            return x.Ticks == y.Ticks;
        }
        public static bool operator !=(TickTime x, TickTime y)
        {
            return x.Ticks != y.Ticks;
        }
        public static bool operator >(TickTime x, TickTime y)
        {
            return x.Ticks > y.Ticks;
        }
        public static bool operator <(TickTime x, TickTime y)
        {
            return x.Ticks < y.Ticks;
        }
        public static bool operator >=(TickTime x, TickTime y)
        {
            return x.Ticks >= y.Ticks;
        }
        public static bool operator <=(TickTime x, TickTime y)
        {
            return x.Ticks <= y.Ticks;
        }
        #endregion

        public static explicit operator DateTime(TickTime TT)
        {
            return TT.UTC;
        }
        public static explicit operator long(TickTime TT)
        {
            return TT.Ticks;
        }
        public static explicit operator TickTime(DateTime DT)
        {
            return new TickTime(DT);
        }
        public static explicit operator TickTime(long ticks)
        {
            return new TickTime(ticks);
        }
    }
}
