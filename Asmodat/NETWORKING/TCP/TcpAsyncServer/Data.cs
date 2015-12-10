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

namespace Asmodat.Networking
{
    public partial class TcpAsyncServer
    {
        private DataDictionary<StateObject> D2Sockets = new DataDictionary<StateObject>();
        private DataDictionary<BufferedArray<byte[]>> D3BReceive = new DataDictionary<BufferedArray<byte[]>>();
        private DataDictionary<BufferedArray<byte[]>> D3BSend = new DataDictionary<BufferedArray<byte[]>>();




        public bool IsDataAvailable(string sUID)
        {
            if (!D3BReceive.Contains(sUID)) return false;

            return !D3BReceive.Get(sUID).IsAllRead;
        }

        
    }
}
