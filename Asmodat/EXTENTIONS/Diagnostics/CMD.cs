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
using System.Collections;

namespace Asmodat.Extensions.Diagnostics
{
    public static partial class ProcessEx
    {
        public static Process StartCMD(bool createNoWindow = true)
         => StartCMD("cmd.exe", createNoWindow);

        public static Process StartCMD(string fileName = "cmd.exe", bool createNoWindow = true)
        {
            var pi = new ProcessStartInfo
            {
                FileName = fileName,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = createNoWindow
            };

            Process process = new Process { StartInfo = pi };
            process.Start();
            return process;
        }


        public static (List<string> output, List<string> error) ExecuteSingleCommand(string command)
        {
            var cmd = StartCMD("cmd.exe", true);

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.WriteLine("exit");
            cmd.StandardInput.Flush();//testr
            cmd.StandardInput.Close();//test
            cmd.WaitForExit();

            var result = cmd.ReadOutputs();

            cmd.Dispose();

            return result;
        }


        public static (List<string> output, List<string> error) ReadOutputs(this Process cmd)
        {
            if (cmd?.StandardOutput == null || cmd?.StandardError == null)
                return (null, null);

            var output = new List<string>();
            var error = new List<string>();

            while (cmd?.StandardOutput?.EndOfStream == false && cmd.StandardOutput.Peek() > -1)
            {
                var line = cmd.StandardOutput.ReadLine();

                if (line.IsNullOrEmpty())
                    continue;

                output.Add(line);
            }

            while (cmd?.StandardError?.EndOfStream == false && cmd.StandardError.Peek() > -1)
            {
                var line = cmd.StandardError.ReadLine();

                if (line.IsNullOrEmpty())
                    continue;

                error.Add(line);
            }

            return (output, error);
        }

        public static (List<string> output, List<string> error) ExeCMD(this Process cmd, string command)
        {
            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            //cmd.StandardInput.Close();
            return cmd.ReadOutputs();
        }

        public static void ExitCMD(this Process cmd, string exitCMD = "exit")
        {
            cmd.StandardInput.WriteLine(exitCMD);
            cmd.WaitForExit();
        }

    }
}
