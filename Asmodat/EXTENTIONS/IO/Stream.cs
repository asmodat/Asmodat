using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;

using System.IO;
using System.Runtime.CompilerServices;

namespace Asmodat.Extensions.IO
{
    

    public static class StreamEx
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this Stream stream)
        {
            if (stream == null || stream.Length <= 0)
                return true;
            else 
                return false;
        }
    }
}
