using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.IO;
using Asmodat.Extensions.Collections.Generic;
using System.Drawing.Imaging;
using Asmodat.Imaging;
using Asmodat.Debugging;
using System.Runtime.CompilerServices;

using System.Windows.Media.Imaging;
using Asmodat.Extensions.Drawing.Imaging;
using System.Runtime.InteropServices;
using Asmodat.Extensions.Windows.Media.Imaging;
using MW32TS = Microsoft.Win32.TaskScheduler;
using System.Windows.Forms;


namespace Asmodat.Extensions.Microsoft.Win32
{
    public static partial class TaskShedulerEx
    {
        public static void TrySetLaunchAtStartup(bool enabled)
        {
            using (MW32TS.TaskService service = new MW32TS.TaskService())
            {
                MW32TS.TaskDefinition definition = service.NewTask();
                definition.RegistrationInfo.Description = "Launches App At Startup";

                MW32TS.SessionStateChangeTrigger trigger = new MW32TS.SessionStateChangeTrigger();
                //trigger.StateChange = MW32TS.TaskSessionStateChangeType.RemoteConnect;

            }
        }

    }
}
