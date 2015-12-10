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

    public partial class ExceptionObject
    {
        public TickTime Time { get; private set; }
        public Exception Exception { get; private set; }

        public ExceptionObject(Exception Exception, TickTime Time)
        {
            this.Time = Time;
            this.Exception = Exception;
        }
    }
}
