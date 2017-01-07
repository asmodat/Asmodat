using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using System.Numerics;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Types;

namespace Asmodat.Extensions.Objects
{
    

    public static class DateTimeEx
    {
        public static TickTime ToTickTime(this DateTime dt)
        {
            return new TickTime(dt);
        }

        public static int ToInt(this DayOfWeek dow)
        {
            int result = 0;
            switch(dow)
            {
                case DayOfWeek.Monday : result = 1; break;
                case DayOfWeek.Tuesday: result = 2;  break;
                case DayOfWeek.Wednesday: result = 3; break;
                case DayOfWeek.Thursday: result = 4; break;
                case DayOfWeek.Friday: result = 5; break;
                case DayOfWeek.Saturday: result = 6; break;
                case DayOfWeek.Sunday: result = 7; break;
            }

            return result;
        }

    }
}
