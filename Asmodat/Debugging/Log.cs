using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using Asmodat.IO;
using System.IO;


namespace Asmodat.Debugging
{


    public static partial class Log
    {
        public static string DefaultFile
        {
            get
            {
                return Directories.Current + @"\" + "DefaultAsmodatDebuggLog.adl";
            }
        }

        public static void Delete(string file = null)
        {
            try
            {
                if (file.IsNullOrEmpty())
                    file = Log.DefaultFile;

                Files.Delete(file);
            }
            catch(Exception ex)
            {
                ex.ToOutput();
            }
        }

        public static void AppendText(string data, string file = null)
        {
            if (file.IsNullOrEmpty())
                file = Log.DefaultFile;
            else Files.GetFullPath(file);

            Files.AppendText(file, data);
        }


        public static void WriteWithMethods(string data, string file = null, int callerSearchStart = 3, int callerSearchEnd = 2)
        {
            string format = "";
            List<string> callers = new List<string>();
            for(int i = callerSearchStart; i  >= callerSearchEnd; i--)
            {
                string caller = Objects.nameofmethod(i);

                if (caller == null)
                    continue;

                format += string.Format("| ({0})=> {1} \r\n", i, caller);
            }

            format += string.Format(">>{0}\r\n", data);
            Log.AppendText(format, file);

        }

        public static void WriteException(Exception ex, string file = null, bool stackTrace = false, int callerSearchStart = 4, int callerSearchEnd = 2)
        {
            if (ex == null)
                return;

            string format;

            if (stackTrace)
                format = string.Format("{0}\r\n{1}", ex.Message, ex.StackTrace);
            else
                format = ex.Message;

            Log.WriteWithMethods(format, file, callerSearchStart, callerSearchEnd);
        }

        public static void ToLog(this Exception ex, bool stackTrace, string file = null, int callerSearchStart = 6, int callerSearchEnd = 4)
        {
            Log.WriteException(ex, file, stackTrace, callerSearchStart, callerSearchEnd);
        }

        public static void ToLog(this Exception ex, bool stackTrace = false, int callerSearchStart = 6, int callerSearchEnd = 4)
        {
            try
            {
                Log.WriteException(ex, null, stackTrace, callerSearchStart, callerSearchEnd);
                Log.WriteException(ex.InnerException, null, stackTrace, callerSearchStart, callerSearchEnd);
            }
            catch(Exception x)
            {
                x.ToOutput();
            }
        }
    }
}
