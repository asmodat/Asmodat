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
    public static partial class TaskSchedulerEx
    {
        public static bool TryDeleteTasksByName(string name)
        {
            if (name.IsNullOrEmpty())
                return false;

            try
            {
                using (MW32TS.TaskService service = new MW32TS.TaskService())
                {
                    if (service.RootFolder == null || service.RootFolder.Tasks == null)
                        return true;

                    var tasks = service.RootFolder.Tasks.Where(t => t.Name == name);
                    
                    foreach (var task in tasks)
                        service.RootFolder.DeleteTask(task.Name, false);
                    
                }
            }
            catch(Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }

            return true;
        }


        public static void TrySetLaunchAtStartup(bool enabled, string username = null, string userpassword = null)
        {
            string name = Application.ProductName;

            using (MW32TS.TaskService service = new MW32TS.TaskService())
            {
                TryDeleteTasksByName(name);

                if (!enabled)
                    return;

                MW32TS.LogonTrigger trigger = new MW32TS.LogonTrigger();
               

               // MW32TS.BootTrigger trigger = new MW32TS.BootTrigger();
               // trigger.Delay = new TimeSpan(0, 0, 5);
               //trigger.Enabled = true;

                MW32TS.ExecAction action = new MW32TS.ExecAction(Application.ExecutablePath, null, null);
                action.WorkingDirectory = Application.StartupPath;

                MW32TS.TaskDefinition definition = service.NewTask();
                definition.RegistrationInfo.Description = "Launches App At Startup";
                definition.Triggers.Add(trigger);
                definition.Actions.Add(action);

                definition.Principal.RunLevel = MW32TS.TaskRunLevel.Highest;
                //definition.Principal.
                if (!username.IsNullOrEmpty())
                    service.UserName = username;

                if (!userpassword.IsNullOrEmpty())
                    service.UserPassword = userpassword;

                service.RootFolder.RegisterTaskDefinition(name, definition);
            }
        }

    }
}
