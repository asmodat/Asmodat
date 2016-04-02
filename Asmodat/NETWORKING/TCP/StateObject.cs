using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using System.Collections.Concurrent;

using System.Timers;
using System.IO;

using System.Net;
using System.Net.Sockets;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Types;
using Asmodat.Extensions.Net.Sockets;

namespace Asmodat.Networking
{
    public class StateObject : IDisposable
    {
        public TickTime Time = TickTime.Default;
        public string key = null;
        public Socket workSocket = null;
        public byte[] residue = null;

        public void Dispose()
        {
            workSocket.Cleanup();
        }
    }
}
