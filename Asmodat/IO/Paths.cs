using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using Asmodat.Debugging;
using System.Text.RegularExpressions;
using Asmodat.Extensions.Objects;

namespace Asmodat.IO
{
    public partial class Paths
    {
        

        public static string Combine(string path1, string path2)
        {
            if (path1.IsNullOrEmpty() || path2.IsNullOrEmpty())
                return null;

            if(Path.IsPathRooted(path2))
            {
                path2 = path2.TrimStart(Path.DirectorySeparatorChar);
                path2 = path2.TrimStart(Path.AltDirectorySeparatorChar);
            }

            return Path.Combine(path1,path2);
        }
        

    }
}
