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

    public partial class ExceptionBuffer
    {
        public BufferedArray<ExceptionObject> Buffer { get; private set; }

        public ExceptionBuffer(int size = 128)
        {
            Buffer = new BufferedArray<ExceptionObject>(size);
        }
        
        public void Write(Exception ex)
        {
            Buffer.Write(new ExceptionObject(ex, TickTime.Now));
        }

        public ExceptionObject GetLastObject()
        {
            ExceptionObject eo;
            if (Buffer.GetLastObject(out eo))
                return eo;
            else return null;
        }

    }
}
