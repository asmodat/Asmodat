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

    public sealed class ExceptionBufferSingleton
    {
        private ExceptionBufferSingleton()
        {
            Buffer = new ExceptionBuffer();
        }

        public ExceptionBuffer Buffer = null;

        static readonly ExceptionBufferSingleton _instance = new ExceptionBufferSingleton();
        public static ExceptionBufferSingleton Instance
        {
            get { return _instance; }
        }
    }
}
