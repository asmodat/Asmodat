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

using IWshRuntimeLibrary;

using Asmodat.IO;
using Asmodat.Extensions.COM.Shell32;

namespace Asmodat.Extensions.COM.IWshRuntimeLibrary
{
    public static partial class WshShellClassEx
    {
       /* public static void CreateShortcut(string target, string path, string description, string arguments)
        {
            if (target.IsNullOrEmpty() || path.IsNullOrEmpty())
                return;

            WshShellClass WSClass = new WshShellClass();
            IWshShortcut IWShortcut;
            IWShortcut = (IWshShortcut)WSClass.CreateShortcut(path);

            string target_dir = Asmodat.IO.Files.GetDirectory(target);

            IWShortcut.TargetPath = target;
            IWShortcut.WorkingDirectory = target_dir;// Application.StartupPath;

            if(!description.IsNullOrEmpty()) IWShortcut.Description = description;
            if (!arguments.IsNullOrEmpty()) IWShortcut.Arguments = arguments;

            Asmodat.IO.Files.Delete(path);
            IWShortcut.Save();
        }*/

        public static void CreateStartupFolderShortcut()
        {
            WshShellClass WSClass = new WshShellClass();
            IWshShortcut IWShortcut;
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
            string path = directory + @"\" + Application.ProductName + ".lnk";
            IWShortcut = (IWshShortcut)WSClass.CreateShortcut(path);

            IWShortcut.TargetPath = Application.ExecutablePath;
            IWShortcut.WorkingDirectory = Application.StartupPath;
            IWShortcut.Description = "This is a startup shortcut.";
            IWShortcut.Arguments = "/a /c";

            Asmodat.IO.Files.Delete(path);
            IWShortcut.Save();
        }

        
        
    }
}
