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

namespace Asmodat.Connect
{
    public partial class TcpAsyncClient
    {
        public TcpAsyncClient(string sIP, int iPORT)
        {
            ServerIP = sIP;
            ServerPort = iPORT;

            TimrMain = new System.Timers.Timer(1);
            TimrMain.Elapsed += TimrMain_Elapsed;
            TimrMain.Enabled = false;
        }

        Thread ThrdStart;
        public void Start()
        {
            ThrdStart = new Thread(() =>
            {
                IPAClient = IPAddress.Parse(ServerIP);

                IPEPClient = new IPEndPoint(IPAClient, ServerPort);

                SLClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SLClient.Connect(IPEPClient);
                
                
                //IPHEClient = Dns.GetHostEntry(Dns.GetHostName());
                //IPAClient = IPAddress.Parse(ServerIP);

                //IPEPClient = new IPEndPoint(IPAClient, ServerPort);

                //SLClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //SLClient.Connect(IPEPClient);
            });

            ThrdStart.Start();
            TimrMain.Enabled = true;
        }

        
        public void Stop()
        {
            TimrMain.Enabled = false;

            try
            {
                if (SLClient == null) return;
                SLClient.Shutdown(SocketShutdown.Both);
                SLClient.Close();
            }
            catch
            {

            }

            SLClient = null;
        }



        
        void TimrMain_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!this.IsConnected) return;

            this.ReceiveThread();
            this.SendThread();
        }

       


    }
}


/*
Thread ThrdMain; //Main Timer Thread

if (ThrdMain != null && ThrdMain.IsAlive) return;

            ThrdMain = new Thread(() =>
                {


                    
                });

            ThrdMain.Start();

*/
