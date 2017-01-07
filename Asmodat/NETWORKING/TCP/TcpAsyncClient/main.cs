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
using Asmodat.Debugging;
using Asmodat.Types;
using Asmodat.Extensions.Net.Sockets;

namespace Asmodat.Networking
{
    public partial class TcpAsyncClient
    {
        ThreadedTimers Timers = new ThreadedTimers(100);

        public TcpAsyncClient(string sIP, int iPORT, int Length, TcpAsyncCommon.PacketMode PacketMode, long timeout_receiver = 0, int SendTimeout_ms = 0)
        {
            this.PacketMode = PacketMode;
            this.TimeoutReceived = new TickTimeout(timeout_receiver, TickTime.Unit.ms, timeout_receiver > 0);
            this.Length = Length;
            ServerIP = sIP;
            ServerPort = iPORT;
            this.SendTimeout = new TickTimeout(SendTimeout_ms, TickTime.Unit.ms, SendTimeout_ms > 0);

            DBReceive = new Types.BufferedArray<byte[]>(this.Length);
            DBSend = new Types.BufferedArray<byte[]>(this.Length);
        }

        //Thread ThrdStart;
        public void Start()
        {
            //TimeoutReceived.Reset();
            Timers.Run(() => Timer_Restart(), 1000);//, null, true, false);
        }

        


        public void Timer_Restart()
        {
            bool connected = this.IsConnected();
            bool triggered = TimeoutReceived.IsTriggered;
            if (connected && !triggered)
                return;

            Timers.Terminate(() => TimrMain_Send());
            Timers.Terminate(() => TimrMain_Receive());
            this.StopClient();

            try
            {
                IPAClient = IPAddress.Parse(ServerIP);

                IPEPClient = new IPEndPoint(IPAClient, ServerPort);

                SLClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SLClient.Connect(IPEPClient);
                SLClient.NoDelay = TcpAsyncServer.NoDelay;
                SLClient.SendBufferSize = TcpAsyncServer.PacketSizeTCP;
                SLClient.ReceiveBufferSize = TcpAsyncServer.PacketSizeTCP;
                SLClient.SendTimeout = TcpAsyncServer.SendTimeout;
                SLClient.ReceiveTimeout = TcpAsyncServer.ReceiveTimeout;
                SLClient.Ttl = TcpAsyncServer.Ttl;
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                this.StopClient();
                return;
            }

            
            Timers.Run(() => TimrMain_Send(), 1);
            Timers.Run(() => TimrMain_Receive(), 1);
            this.TimeoutReceived.Reset();
        }


        public void StopClient()
        {
            try
            {
                if (SLClient == null)
                    return;

                SLClient.Shutdown(SocketShutdown.Both);
                SLClient.Close();
                SLClient = null;
            }
            catch(Exception ex)
            {
                ex.ToOutput();
            }
        }

        
        public void Stop()
        {
            Timers.Terminate(() => Timer_Restart());
            Timers.TerminateAll();

            this.StopClient();
            
            Thread.Sleep(100);
        }

        public bool IsConnected()
        {
            return SLClient.IsConnected();
        }

        
        void TimrMain_Send()
        {
            this.Send();
        }

        void TimrMain_Receive()
        {
            if(this.Receive())
                this.TimeoutReceived.Reset();
        }




    }
}

/*
void TimrMain_Elapsed()
        {
            if (Methods == null)
                return;

            try
            {
                Methods.Terminate("Receive");
                Methods.Terminate("Send");
                Methods.Run(() => this.Receive2(), "Receive", true, false);
                Methods.Run(() => this.Send(), "Send", true, false);
                Methods.Join("Receive");
                Methods.Join("Send");
            }
            catch(Exception ex)
            {
                Exceptions.Write(ex);
                return;
            }
        }
*/
