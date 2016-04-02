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
using Asmodat.Types;

using Asmodat.Extensions.Objects;
using Asmodat.Debugging;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions.Net.Sockets;

namespace Asmodat.Networking
{

    public partial class TcpAsyncServer
    {

        public Socket Listener { get; private set; } = null;

        public void StartListening()
        {
            if (Listener == null)
                return;
            Listener.Listen(128);
            try
            {
                while (IsStarted)
                {
                    this.Accept();
                    Thread.Sleep(1000);
                }
            }
            catch (ThreadInterruptedException exc)
            {
                Exceptions.Write(exc);
                return;
            }
            finally
            {
                Listener.Cleanup();
            }
        }

       

        private void AcceptCallback(IAsyncResult IAR)
        {
            Socket Server, handler;
            try
            {
                Server = (Socket)IAR.AsyncState;
                handler = Server.EndAccept(IAR);
            }
            catch(ObjectDisposedException ode)
            {
                Exceptions.Write(ode);
                return;
            }

            allDone.Set();

            string sKey = TcpAsyncCommon.DefaultUID + TickTime.NowTicks;

            StateObject state = new StateObject();
            state.workSocket = handler;
            state.key = sKey;

            D2Sockets.Set(sKey, state);
            D3BReceive.Set(sKey, new BufferedArray<byte[]>(this.Length));
            D3BSend.Set(sKey, new BufferedArray<byte[]>(this.Length));
        }


        private void Accept()
        {
            Socket handler = Listener.Accept();
            string key = TcpAsyncCommon.DefaultUID + TickTime.NowTicks;

            StateObject state = new StateObject();
            state.workSocket = handler;
            state.workSocket.NoDelay = TcpAsyncServer.NoDelay;
            state.workSocket.SendBufferSize = TcpAsyncServer.PacketSizeTCP;
            state.workSocket.ReceiveBufferSize = TcpAsyncServer.PacketSizeTCP;
            state.workSocket.SendTimeout = TcpAsyncServer.SendTimeout;
            state.workSocket.ReceiveTimeout = TcpAsyncServer.ReceiveTimeout;
            state.workSocket.Ttl = TcpAsyncServer.Ttl;
            state.key = key;
            state.Time = TickTime.Now;

            D2Sockets.Set(key, state);
            D3BReceive.Set(key, new BufferedArray<byte[]>(this.Length));
            D3BSend.Set(key, new BufferedArray<byte[]>(this.Length));

            //this.Send(key, "#START".GetBytes());
        }


        public void Start()
        {
            IsStarted = true;
            Timers.Run(() => Timer_Restart(), 1000, null, true, false);
        }



        public void Timer_Cleanup()
        {
            if (D2Sockets == null)
                return;

            var keys = D2Sockets.Keys;

            if (keys.IsNullOrEmpty())
                return;

            foreach(string key in keys)
            {
                if(!GetHandler(key).IsConnected())
                    this.Stop(key);
            }


        }


        public void Timer_Restart()
        {
            Timers.Terminate("TimrMain_Send");
            Timers.Terminate("TimrMain_Receive");

            Timers.Run(() => TimrMain_Send(), 1, "TimrMain_Send", true, false);
            Timers.Run(() => TimrMain_Receive(), 1, "TimrMain_Receive", true, false);
            Timers.Run(() => Timer_Cleanup(), 1000, "Cleanup", true, false);
            try
            {
                IPHostEntry IPHEServer = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                IPAddress IPAServer = null;
                IPEndPoint IPEPServer;

                if (IP.IsNullOrEmpty())
                {
                    foreach (IPAddress IPA in IPHEServer.AddressList)
                        if (IPA.AddressFamily == AddressFamily.InterNetwork)
                        {
                            IPAServer = IPA;
                            break;
                        }
                }
                else
                {
                    IPAServer = IPAddress.Parse(IP);
                }

                IP = IPAServer.ToString();
                IPEPServer = new IPEndPoint(IPAServer, this.Port);


                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                LingerOption LO = new LingerOption(false, 0);
                Listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, LO);
                Listener.Bind(IPEPServer);

                StartListening();
            }
            catch (Exception ex)
            {
                Exceptions.Write(ex);
                Listener.Cleanup();
                return;
            }
        }




    }
}
