using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using Asmodat.IO;
using System.IO;

using Asmodat.Types;

namespace Asmodat.Debugging
{

    public  static partial class ExceptionBufferSingletonEx
    {
        public static void WriteToExcpetionBuffer(this Exception ex)
        {
            ExceptionBufferSingleton.Instance.Buffer.Write(ex);
        }
    }
}
