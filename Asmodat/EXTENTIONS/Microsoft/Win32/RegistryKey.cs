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
using MW32 = Microsoft.Win32;
using System.Windows.Forms;

namespace Asmodat.Extensions.Microsoft.Win32
{
    public static partial class RegistryKeyEx
    {
        public static bool TrySetValue(this MW32.RegistryKey key, string name, object value)
        {
            if (key == null)
                return false;

            try
            {
                key.SetValue(name, value);
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }

            return true;
        }

        public static bool TryDeleteValue(this MW32.RegistryKey key, string name, bool throwOnMissingValue = true)
        {
            if (key == null)
                return false;

            try
            {
                key.DeleteValue(name, throwOnMissingValue);
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }

            return true;
        }


        public static bool TrySetLaunchAtStartup(bool enabled)
        {
            string name = Application.ProductName;
            string path = Application.ExecutablePath.ToString();

            if (name.IsNullOrEmpty() || path.IsNullOrEmpty())
                return false;

            path = "\"" + path + "\"";
            MW32.RegistryKey RKey_CU = MW32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            MW32.RegistryKey RKey_LM = MW32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            MW32.RegistryKey RKey_LM_wow = MW32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            MW32.RegistryKey RKey_LM_32 = MW32.RegistryKey.OpenBaseKey(MW32.RegistryHive.LocalMachine, MW32.RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            MW32.RegistryKey RKey_LM_64 = MW32.RegistryKey.OpenBaseKey(MW32.RegistryHive.LocalMachine, MW32.RegistryView.Registry32).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (enabled)
            {
                RKey_CU.TrySetValue(name, path);
                RKey_LM.TrySetValue(name, path);
                RKey_LM_wow.TrySetValue(name, path);
                RKey_LM_32.TrySetValue(name, path);
                RKey_LM_64.TrySetValue(name, path);
            }
            else
            {
                RKey_CU.TryDeleteValue(name, false);
                RKey_LM.TryDeleteValue(name, false);
                RKey_LM_wow.TryDeleteValue(name, false);
                RKey_LM_32.TryDeleteValue(name, false);
                RKey_LM_64.TryDeleteValue(name, false);
            }

            return true;
        }
    }
}
