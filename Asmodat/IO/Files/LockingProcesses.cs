using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Asmodat.Debugging;
using System.Text.RegularExpressions;
using Asmodat.Extensions.Objects;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Asmodat.Natives;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions.Diagnostics;
using System.Threading;

namespace Asmodat.IO
{
    public static partial class Files
    {
        public static void TryKillLockingProcesses(string path, int sleepAfterKillTimeout = 500)
        {
            if (!Exists(path))
                return;

            var prosesses = TryGetLockingProcesses(path);

            if (prosesses.IsNullOrEmpty())
                return;

            bool success = false;
            foreach (Process p in prosesses)
            {
                p.TryKill();
                success = true;
            }

            if(success)
                Thread.Sleep(sleepAfterKillTimeout);
        }

        /// <summary>
        /// Find out what process(es) have a lock on the specified file.
        /// </summary>
        /// <param name="path">Path of the file.</param>
        /// <returns>Processes locking the file</returns>
        public static Process[] TryGetLockingProcesses(string path)
        {
            if (path.IsNullOrEmpty())
                return null;

            uint handle;
            string key = Guid.NewGuid().ToString();
            List<Process> processes = new List<Process>();

            int res = rstrtmgrEx.RmStartSession(out handle, 0, key);
            if (res != 0) //Could not begin restart session.  Unable to determine file locker.
                return null;

            try
            {
                const int ERROR_MORE_DATA = 234;
                uint pnProcInfoNeeded = 0,
                     pnProcInfo = 0,
                     lpdwRebootReasons = rstrtmgrEx.RmRebootReasonNone;

                string[] resources = new string[] { path }; // Just checking on one resource.

                res = rstrtmgrEx.RmRegisterResources(handle, (uint)resources.Length, resources, 0, null, 0, null);

                if (res != 0)//Could not register resource.
                    return null;

                //Note: there's a race condition here -- the first call to RmGetList() returns
                //      the total number of process. However, when we call RmGetList() again to get
                //      the actual processes this number may have increased.
                res = rstrtmgrEx.RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, null, ref lpdwRebootReasons);

                if (res == ERROR_MORE_DATA)
                {
                    // Create an array to store the process results
                    rstrtmgrEx.RM_PROCESS_INFO[] processInfo = new rstrtmgrEx.RM_PROCESS_INFO[pnProcInfoNeeded];
                    pnProcInfo = pnProcInfoNeeded;

                    // Get the list
                    res = rstrtmgrEx.RmGetList(handle, out pnProcInfoNeeded, ref pnProcInfo, processInfo, ref lpdwRebootReasons);
                    if (res == 0)
                    {
                        processes = new List<Process>((int)pnProcInfo);

                        // Enumerate all of the results and add them to the list, to be returned
                        for (int i = 0; i < pnProcInfo; i++)
                        {
                            try
                            {
                                processes.Add(Process.GetProcessById(processInfo[i].Process.dwProcessId));
                            }
                            catch (ArgumentException)
                            {
                                // the process is no longer running
                            }
                        }
                    }
                    else
                        return null; //Could not list processes locking resource.
                }
                else if (res != 0)
                    return null; //Could not list processes locking resource. Failed to get size of result.
            }
            finally
            {
                rstrtmgrEx.RmEndSession(handle);
            }

            return processes?.ToArray();
        }
    }
}
