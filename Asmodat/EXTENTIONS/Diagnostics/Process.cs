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
using Asmodat.Extensions.Collections.Generic;
using Asmodat.IO;
using System.Management;

namespace Asmodat.Extensions.Diagnostics
{
    public static partial class ProcessEx
    {

        public static Process Start(string fullPath, string arguments = null)
        {
            Process process = null;

            if (!Files.Exists(fullPath))
                return process;

            return Process.Start(new ProcessStartInfo(fullPath, arguments));
        }


        
        public static string GetExecutablePath(this Process p)
        {
            if (p == null) return null;

            try
            {
                return Path.GetFullPath(p.MainModule.FileName);
            }
            catch
            {
                using (var serarch = new ManagementObjectSearcher($"SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = {p.Id}"))
                {
                    using (var results = serarch.Get())
                    {
                        ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                        if (mo != null)
                            return (string)mo["ExecutablePath"];
                    }
                }
            }

           return null;
        }

        public static bool IsRunningFromPath(string path)
        {
            if (path.IsNullOrEmpty())
                return false;

            string filter = Path.GetFullPath(path);

            Process[] arr = Process.GetProcesses();
            if (arr.IsNullOrEmpty())
                return false;

            foreach (Process p in arr)
            {
                if (p == null)
                    continue;

                if (string.Compare(filter, p.GetExecutablePath(), true) == 0)
                    return true;
            }

            return false;
        }



        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull(this Process process)
        {
            if (process == null)
                return true;
            else
                return false;
        }

        //Tries to kill process without rising exceptions
        public static void TryKill(this Process process)
        {
            if (process == null)
                return;
            try
            {
                process?.Kill();
            }
            catch
            {

            }
        }


        public static string GetStandardOutput(this Process process)
        {
            if (process.IsNull())
                return null;

            string result = string.Empty;
            using (StreamReader SReader = process.StandardOutput)
                result = SReader.ReadToEnd();

            return result;
        }

        public static string GetStandardError(this Process process)
        {
            if (process.IsNull())
                return null;

            string result = string.Empty;
            using (StreamReader SReader = process.StandardError)
                result = SReader.ReadToEnd();

            return result;
        }


        /// <summary>
        /// Reurns application standard output or error
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static string GetStandardResult(this Process process)
        {
            if (process.IsNull())
                return null;


            string result = process.GetStandardOutput();

            if (result.IsNullOrEmpty())
                result = process.GetStandardError();

            return result;
        }


    }
}
