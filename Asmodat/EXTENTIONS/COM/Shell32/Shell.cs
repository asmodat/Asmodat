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
using S32 = Shell32;

using Asmodat.IO;

namespace Asmodat.Extensions.COM.Shell32
{
    public static partial class ShellEx
    {
        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string directory = Path.GetDirectoryName(shortcutFilename);
            string filename = Path.GetFileName(shortcutFilename);

            S32.Shell shell = new S32.ShellClass();
            S32.Folder folder = shell.NameSpace(directory);
            S32.FolderItem item = folder.ParseName(filename);

            if (item == null)
                return String.Empty;

            S32.ShellLinkObject link = (S32.ShellLinkObject)item.GetLink;
            return link.Path;
        }

        public static void DeleteStartupFolderShortcuts(string targetExeName)
        {
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Startup);

            DirectoryInfo info = new DirectoryInfo(directory);
            FileInfo[] files = info.GetFiles("*.lnk");

            foreach (FileInfo file in files)
            {
                string target = ShellEx.GetShortcutTargetFile(file.FullName);

                if (target.EndsWith(targetExeName, StringComparison.InvariantCultureIgnoreCase))
                    Asmodat.IO.Files.Delete(file.FullName);
            }
        }

    }
}
