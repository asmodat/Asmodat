using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;
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

        public void ProcessCheckIP()
        {
            try
            {
                if (Tracer == null || InnerRaportRequest || OuterRaportRequest || !NetTester.IsNetworkAvailable())
                    return;

                string ip = NetTester.ExternalIP;

                if (!ip.IsNullOrWhiteSpace() && ip != PublicIP)
                {
                    this.PublicIP = ip;
                    InnerRaportRequest = true;
                    ADSFile.SaveString(this.ADS_PublicIP, this.PublicIP, this.Path, true);
                }
            }
            catch(Exception ex)
            {
                Output.WriteException(ex);
            }
        }


    }
}
