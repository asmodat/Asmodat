using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Diagnostics;

namespace Asmodat.Abbreviate
{
    public class Console
    {
        public string Process(string filename, string arguments, bool waitForExit = true)
        {
            string sOutput = string.Empty;
            string sError = string.Empty;

            ProcessStartInfo SDPSInfo = new ProcessStartInfo();
            SDPSInfo.RedirectStandardOutput = true;
            SDPSInfo.RedirectStandardError = true;
            SDPSInfo.CreateNoWindow = true;
            SDPSInfo.UseShellExecute = false;
            SDPSInfo.WindowStyle = ProcessWindowStyle.Hidden;
            SDPSInfo.FileName = filename;
            SDPSInfo.Arguments = arguments;

            Process SDProc = new System.Diagnostics.Process();
            SDProc.StartInfo = SDPSInfo;
            SDProc.Start();

            if (!waitForExit) return null;

            using (StreamReader SReader = SDProc.StandardOutput)
                sOutput = SReader.ReadToEnd();

            if (sOutput != null && sOutput != "") return sOutput;


            using (StreamReader SReader = SDProc.StandardError)
                sOutput = SReader.ReadToEnd();

            return sError;
        }
    }
}
//ProcessStartInfo SDPSInfo = new ProcessStartInfo();
//Process SDProc;
//SDPSInfo.FileName = "ipconfig";
//SDPSInfo.Arguments = "/release";
//SDPSInfo.WindowStyle = ProcessWindowStyle.Hidden;
//SDProc = Process.Start(SDPSInfo);
//SDProc.WaitForExit();
//SDPSInfo.FileName = "ipconfig";
//SDPSInfo.Arguments = "/renew";
//SDPSInfo.WindowStyle = ProcessWindowStyle.Hidden;
//SDProc = Process.Start(SDPSInfo);
//SDProc.WaitForExit();
