using Asmodat.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions;
using System.Diagnostics;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;
using System.Threading;

namespace Asmodat.Types
{
    public partial class CMD
    {
        private readonly static object _locker = new object();

        public string FileName { get; }
        public bool CreateNoWindow { get; }
        private Process Process { get; set; }

        public int MaxOutputDataLength { get; set; } = 4096;
        public int MaxErrorDataLength { get; set; } = 4096;

        public ThreadedDictionary<TickTime, string> OutputData { get; private set; } = new ThreadedDictionary<TickTime, string>();
        public ThreadedDictionary<TickTime, string> ErrorData { get; private set; } = new ThreadedDictionary<TickTime, string>();

        public enum ResponseType
        {
            Data = 1,
            Error = 1 << 1,
            Any = Data | Error
        }

        public CMD(string fileName = "cmd.exe", bool createNoWindow = true)
        {

            FileName = fileName;
            CreateNoWindow = createNoWindow;

            var pi = new ProcessStartInfo
            {
                FileName = this.FileName,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false, //must be false in order to redirect IO streams
                CreateNoWindow = this.CreateNoWindow
            };

            Process = new Process { StartInfo = pi };
            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();

            Process.OutputDataReceived += Process_OutputDataReceived;
            Process.ErrorDataReceived += Process_ErrorDataReceived;
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.IsNullOrEmpty())
                return;

            if (OutputData.Count >= MaxOutputDataLength)
                OutputData.Remove(OutputData.FirstKey);

            OutputData.Add(TickTime.Now, e.Data);
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data.IsNullOrEmpty())
                return;

            if (ErrorData.Count >= MaxErrorDataLength)
                ErrorData.Remove(OutputData.FirstKey);

            ErrorData.Add(TickTime.Now, e.Data);
        }


        public (string[] output, string[] error, TickTime start) WriteLine(string cmd) => WriteLine(cmd, 0);
        public (string[] output, string[] error, TickTime start) WriteLine(string cmd, int timeout_ms)
        {
            lock (_locker)
            {
                TickTime ttOutput = TickTime.Now, ttError = TickTime.Now, ttInit = TickTime.Now;
                Process.StandardInput.WriteLine(cmd);

                if (timeout_ms <= 0)
                    return (null, null, ttInit);

                TickTimeout timeout = new TickTimeout(timeout_ms, TickTime.Unit.ms);
                List<string> output = new List<string>(), error = new List<string>();

                while (!timeout.IsTriggered && !Process.HasExited)
                {
                    var outputKeys = OutputData.KeysArray;

                    foreach (var key in outputKeys)
                    {
                        var val = OutputData.TryGetValue(key);
                        if (val.IsNullOrEmpty())
                            continue;

                        if (key > ttOutput)
                        {
                            output.Add(val);
                            ttOutput = key;
                        }
                    }

                    var errorKeys = ErrorData.KeysArray;

                    foreach (var key in errorKeys)
                    {
                        var val = ErrorData.TryGetValue(key);
                        if (val.IsNullOrEmpty())
                            continue;

                        if (key > ttError)
                        {
                            error.Add(val);
                            ttError = key;
                        }
                    }
                }

                return (output.ToArray(), error.ToArray(), ttInit);
            }
        }

        

        public bool WaitForAnyResponse(ResponseType type, TickTime init, int timeout_ms, bool waitForAnyIsCaseSensitive, params string[] waitForAny)
        {
            if (waitForAny.IsNullOrEmpty())
                return false;

            var timeout = new TickTimeout(timeout_ms, TickTime.Unit.ms);
            while (!timeout.IsTriggered)
            {
                if ((type & ResponseType.Data) > 0 && 
                    waitForAny != null && 
                    (OutputData.Any(p => p.Key > init && p.Value.ContainsAny(waitForAny, waitForAnyIsCaseSensitive == true))))
                    return true;

                if ((type & ResponseType.Error) > 0 && 
                    waitForAny != null && 
                    (ErrorData.Any(p => p.Key > init && p.Value.ContainsAny(waitForAny, waitForAnyIsCaseSensitive == true))))
                    return true;

                Thread.Sleep(10);
            }

            return false;
        }

        public void Exit(string exitCommand = "exit", int timeout_ms = 200)
        {
            this.WriteLine(exitCommand, timeout_ms);
            Process.Kill();
        }
    }
}
