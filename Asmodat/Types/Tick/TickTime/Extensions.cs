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

    public static class TickTimeEx
    {
        public static bool Timeout(this TickTime start, long timeout_ms)
        {
            return TickTime.Timeout(start, timeout_ms, TickTime.Unit.ms);
        }

        public static bool Timeout(this TickTime start, long timeout, TickTime.Unit unit = TickTime.Unit.ms)
        {
            return TickTime.Timeout(start, timeout, unit);
        }

        public static decimal TimeoutSpan(this TickTime start, long timeout, TickTime.Unit unit = TickTime.Unit.ms)
        {
            return TickTime.TimeoutSpan(start, timeout, unit, unit);
        }

        public static decimal TimeoutSpan(this TickTime start, long timeout, TickTime.Unit unit, TickTime.Unit unit_result)
        {
            return TickTime.TimeoutSpan(start, timeout, unit, unit_result);
        }
    }
}