using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asmodat.Abbreviate
{
    public static class Rand
    {
        private static Random RGenerator = new Random();

        public static string UniqueSID()
        {
            
            DateTime DTime = DateTime.Now;

            string sID = "" +
            DTime.Year +
            DTime.Month +
            DTime.Day +
            DTime.Hour +
            DTime.Second +
            DTime.Millisecond +
            RGenerator.Next(0, int.MaxValue);
            
            return sID;
        }

        

    }
}
