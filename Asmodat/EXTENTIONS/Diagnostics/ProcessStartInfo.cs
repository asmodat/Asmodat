using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Asmodat.Extensions.Diagnostics
{
    public static partial class ProcessStartInfoEx
    {
        /*public static Process Start(string filename, string arguments)
        {
            string 
                sOutput = string.Empty, 
                sError = string.Empty;

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

            return SDProc;
        }*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Process CMD_Shutdown(string param)
        {
            ProcessStartInfo PSInfo = new ProcessStartInfo("shutdown",param);
            PSInfo.CreateNoWindow = true;
            PSInfo.UseShellExecute = true;

            return Process.Start(PSInfo);
           // return ProcessStartInfoEx.Start("cmd", "shutdown " + param);/// C 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Process CMD_Shutdown(int wait_s = 0)
        {
            return ProcessStartInfoEx.CMD_Shutdown("-s -f -t " + wait_s.ToClosedInterval(0, int.MaxValue));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Process CMD_LogOff(int wait_s = 0)
        {
            return ProcessStartInfoEx.CMD_Shutdown("-l -f -t " + wait_s.ToClosedInterval(0, int.MaxValue));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Process CMD_Restart(int wait_s = 0)
        {
            return ProcessStartInfoEx.CMD_Shutdown("-r -f -t " + wait_s.ToClosedInterval(0, int.MaxValue));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Process CMD_ShutdownAbort()
        {
            return ProcessStartInfoEx.CMD_Shutdown("-a -f");
        }
    }
}
