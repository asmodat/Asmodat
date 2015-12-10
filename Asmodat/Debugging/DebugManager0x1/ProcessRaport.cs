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
using Asmodat.Resources;

namespace Asmodat.Debugging
{
    public partial class DebugMnager0x1
    {

        public void ProcessRaport()
        {
            try
            {
                if (Tracer == null || !NetTester.IsNetworkAvailable())
                    return;

                if (InnerRaportRequest || OuterRaportRequest || Tracer.IsFull)
                {
                    string user = Environment.UserName;
                    string machine = Environment.MachineName;
                    string ippublic;
                    string iplocal = NetTester.LocalIP;

                    if (this.PublicIP.IsNullOrWhiteSpace())
                        ippublic = NetTester.ExternalIP;
                    else ippublic = this.PublicIP;

                    Microsoft.VisualBasic.Devices.ComputerInfo info = new Microsoft.VisualBasic.Devices.ComputerInfo();
                    


                    string title = string.Format("BotRaport0x1: <ID:{0}/>", this.ID);

                    string message = string.Format(
    @"
<ID:{0}/>
<User:{1}/>
<Name:{2}/>
<PublicIP:{3}/>
<LocalIP:{4}/>
<InnerRequest:{5}/>
<OuterRequest:{6}/>
<InstalledUICulture:{7}/>
<OSFullName:{8}/>
<OSPlatform:{9}/>
<OSVersion:{10}/>
<Is64BitOperatingSystem:{11}/>
<AvailablePhysicalMemory:{12}/>
<AvailableVirtualMemory:{13}/>
<MaxClockSpeed:{14}/>
<NumberOfCores:{15}/>
<DriveAvailableFreeSpace:{16}/>",
                        this.ID,
                        user,
                        machine,
                        ippublic,
                        iplocal,
                        InnerRaportRequest, 
                        OuterRaportRequest,
                        info.InstalledUICulture,
                        info.OSFullName,
                        info.OSPlatform,
                        info.OSVersion,
                        Environment.Is64BitOperatingSystem,
                        info.AvailablePhysicalMemory,
                        info.AvailableVirtualMemory,
                        ProcessorsInfo.MaxClockSpeed(),
                        ProcessorsInfo.NumberOfCores(),
                        DrivesInfo.AvailableFreeSpace(Files.ExeRoot));

                    EMail.SendAsync(title, message, false, Tracer.DataRawEncryptedCompressed.ToMemory(),"BotRaport0x1-loggs.abr");
                    EMail.WaitForResponse(3, TickTime.Unit.m);

                    if (EMail.Delivered)
                    {
                        InnerRaportRequest = false;
                        OuterRaportRequest = false;
                        Tracer.Delete();
                    }
                    else
                    {
                        EMail.StopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
            }
        }
    }
}
