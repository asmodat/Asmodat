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

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

namespace Asmodat.Connect
{
    public partial class TcpAsyncClient
    {
       


        //private IPHostEntry IPHEClient;
        private IPAddress IPAClient;
        private IPEndPoint IPEPClient;
        private Socket SLClient; //Listener

        private DataBuffer DBReceive = new DataBuffer();
        private DataBuffer DBSend = new DataBuffer();

        /// <summary>
        /// Main Timer, where Send and Receive operations are performed
        /// </summary>
        private System.Timers.Timer TimrMain;

        public string ServerIP { get; private set; }
        public int ServerPort { get; private set; }
        public string ClientUID { get; private set; }

        public bool IsDataAvailable
        {
            get
            {
                return DBReceive.IsDataAvalaible;
            }
        }

        public bool IsConnected
        {
            get
            {
                if (SLClient == null) return false;
                if (!SLClient.IsBound) return false;
                if (!SLClient.Connected) return false;
                //if (!SLClient.Available) return false;
                return true;
            }
        }
    }
}
