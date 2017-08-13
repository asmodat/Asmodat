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

        public int MaxInputDataLength { get; set; } = 4096;
        public int MaxOutputDataLength { get; set; } = 4096;
        public int MaxErrorDataLength { get; set; } = 4096;

        public ThreadedDictionary<TickTime, string> InputData { get; private set; } = new ThreadedDictionary<TickTime, string>();
        public ThreadedDictionary<TickTime, string> OutputData { get; private set; } = new ThreadedDictionary<TickTime, string>();
        public ThreadedDictionary<TickTime, string> ErrorData { get; private set; } = new ThreadedDictionary<TickTime, string>();

        public enum DataType
        {
            None = 1,
            Output = 1 << 1,
            Error = 1 << 2,
            Input = 1 << 3,
            Any = None | Output | Error | Input
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

        public (DataType type, string value) GetData(TickTime time)
        {
            if (!time.IsDefault)
                lock (_locker)
                {
                    if (InputData.ContainsKey(time))
                        return (DataType.Input, InputData[time]);
                    if (ErrorData.ContainsKey(time))
                        return (DataType.Error, ErrorData[time]);
                    if (OutputData.ContainsKey(time))
                        return (DataType.Output, OutputData[time]);
                }

            return (DataType.None, null);
        }

       /* public KeyValuePair<TickTime, string>[] GetNextData(TickTime init)
        {
            var data = new List<KeyValuePair<TickTime, string>>();
            data.AddRange(GetNextOutputs(init));
            data.AddRange(GetNextErrors(init));
            data.AddRange(GetNextInputs(init));
            return data.ToArray();
        }*/

        public TickTime GetNextPointer(TickTime init)
        {
            var data = this.GetNextData(DataType.Any, init);

            if (data.IsNullOrEmpty())
                return TickTime.Default;

            return data.Min(x => x.Key);
        }


        public KeyValuePair<TickTime, string>[] GetNextData(DataType type, TickTime init)
        {
            ThreadedDictionary<TickTime, string> data = new ThreadedDictionary<TickTime, string>();

            if ((type & DataType.Input) > 0) data.AddRange(InputData.ToDictionary());
            if ((type & DataType.Error) > 0) data.AddRange(ErrorData.ToDictionary());
            if ((type & DataType.Output) > 0) data.AddRange(OutputData.ToDictionary());

            return data?.Where(x => x.Key > init)?.Select(x => { return new KeyValuePair<TickTime, string>(x.Key.Copy(), x.Value); })?.ToArray() ?? new KeyValuePair<TickTime, string>[0];
        }

       /* public KeyValuePair<TickTime, string>[] GetNextOutputs(TickTime init)
            => OutputData?.Where(x => x.Key > init)?.Select(x => { return new KeyValuePair<TickTime, string>(x.Key.Copy(), x.Value); })?.ToArray() ?? new KeyValuePair<TickTime, string>[0];

        public KeyValuePair<TickTime, string>[] GetNextErrors(TickTime init)
            => ErrorData?.Where(x => x.Key > init)?.Select(x => { return new KeyValuePair<TickTime, string>(x.Key.Copy(), x.Value); })?.ToArray() ?? new KeyValuePair<TickTime, string>[0];

        public KeyValuePair<TickTime, string>[] GetNextInputs(TickTime init)
            => InputData?.Where(x => x.Key > init)?.Select(x => { return new KeyValuePair<TickTime, string>(x.Key.Copy(), x.Value); })?.ToArray();
*/
        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_locker)
            {
                if (e.Data.IsNullOrEmpty())
                    return;

                if (OutputData.Count >= MaxOutputDataLength)
                    OutputData.Remove(OutputData.FirstKey);

                OutputData.Add(TickTime.Now, e.Data);
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            lock (_locker)
            {
                if (e.Data.IsNullOrEmpty())
                    return;

                if (ErrorData.Count >= MaxErrorDataLength)
                    ErrorData.Remove(OutputData.FirstKey);

                ErrorData.Add(TickTime.Now, e.Data);
            }
        }
        
        public TickTime WriteLine(string cmd) => WriteLine(cmd, 0);
        public TickTime WriteLine(string cmd, int timeout_ms)
        {
            lock (_locker)
            {
                TickTime ttOutput = TickTime.Now, ttError = TickTime.Now, ttInit = TickTime.Now;
                Process.StandardInput.WriteLine(cmd);

                if (!cmd.IsNullOrEmpty())
                {
                    if (InputData.Count >= MaxInputDataLength)
                        InputData.Remove(OutputData.FirstKey);

                    InputData.Add(ttInit, cmd);
                }

                if (timeout_ms <= 0)
                    return ttInit;

                TickTimeout timeout = new TickTimeout(timeout_ms, TickTime.Unit.ms);
                List<string> output = new List<string>(), error = new List<string>();

                while (!timeout.IsTriggered && !Process.HasExited)
                {
                    var outputKeys = OutputData.KeysArray;

                    if (ErrorData.Any(x => x.Key > ttInit) || OutputData.Any(x => x.Key > ttInit))
                        break;

                    Thread.Sleep(10);
                }

                return ttInit;
            }
        }
        
        public bool WaitForResponse(DataType responseType, TickTime init, int timeout_ms, bool waitForAnyIsCaseSensitive, params string[] waitForAny)
        {
            if (waitForAny.IsNullOrEmpty())
                return false;

            var timeout = new TickTimeout(timeout_ms, TickTime.Unit.ms);
            while (!timeout.IsTriggered)
            {
                if ((responseType & DataType.Output) > 0 && 
                    waitForAny != null && 
                    (OutputData.Any(p => p.Key > init && p.Value.ContainsAny(waitForAny, waitForAnyIsCaseSensitive == true))))
                    return true;

                if ((responseType & DataType.Error) > 0 && 
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
