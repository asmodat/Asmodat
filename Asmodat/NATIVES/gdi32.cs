﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Natives
{


    public static class gdi32
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
    }
}
