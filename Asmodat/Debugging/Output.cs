using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;

namespace Asmodat.Debugging
{

    public class Throw
    {
        public static void Exception(string message = "Asmodat.Debugging: default exception.")
        {
            throw new System.Exception(message);
        }

    }



    public static partial class Output
    {

        public static void WriteLine(string data, int callerSearchDeep = 3)
        {
            string caller = Objects.nameofmethod(callerSearchDeep);

            if (string.IsNullOrEmpty(caller)) caller = "";
            if (string.IsNullOrEmpty(data)) data = "";

            string format = string.Format("|{0}| {1}", caller, data);
            System.Diagnostics.Debug.WriteLine(format);

        }


        public static void WriteWithMethods(string data, int callerSearchDeep = 3)
        {
            string callersFormat = "";
            List<string> callers = new List<string>();
            for(int i = callerSearchDeep; i  > 1; i--)
            {
                string caller = Objects.nameofmethod(i);

                if (caller == null)
                    continue;

                callersFormat += string.Format("| ()=> {0} \n", caller);
            }

      
            System.Diagnostics.Debug.WriteLine(callersFormat + data);

        }

        public static void WriteException(Exception ex, int callerSearchDeep = 4)
        {
            Output.WriteWithMethods(ex.Message, callerSearchDeep);
        }

        public static void ToOutput(this Exception ex, int callerSearchDeep = 4)
        {
            Output.WriteException(ex, callerSearchDeep);

        }
    }
}
