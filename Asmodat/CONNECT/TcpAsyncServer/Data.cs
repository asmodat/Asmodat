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


namespace Asmodat.Connect
{
    public partial class TcpAsyncServer
    {


        private static DataDictionary<StateObject> D2Sockets = new DataDictionary<StateObject>();
        private static DataDictionary<Thread> D2TReceive = new DataDictionary<Thread>();
        private static DataDictionary<Thread> D2TSend = new DataDictionary<Thread>();
        private static DataDictionary<DataBuffer> D3BReceive = new DataDictionary<DataBuffer>();
        private static DataDictionary<DataBuffer> D3BSend = new DataDictionary<DataBuffer>();

        public DataBuffer GetBuffer(string key)
        {
            return D3BReceive.Get(key);
        }

        public bool IsDataAvailable(string sUID)
        {
            if (!D3BReceive.Contains(sUID)) return false;

            return D3BReceive.Get(sUID).IsDataAvalaible;
        }

        
    }
}
