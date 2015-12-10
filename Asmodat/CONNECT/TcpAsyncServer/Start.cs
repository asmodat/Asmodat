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

namespace Asmodat.Connect
{

    public partial class TcpAsyncServer : IDisposable
    {

        public void StartListening()
        {
            IPHostEntry IPHEServer = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress IPAServer = null;
            IPEndPoint IPEPServer;

            foreach (IPAddress IPA in IPHEServer.AddressList)
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAServer = IPA;
                    break;
                }

            IP = IPAServer.ToString();
            IPEPServer = new IPEndPoint(IPAServer, this.Port);


            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            LingerOption LO = new LingerOption(false, 0);
            listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, LO);
            listener.Bind(IPEPServer);
            listener.Listen(100);

            try
            {
                while (true)
                {

                    allDone.Reset();
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                    allDone.WaitOne();

                }
            }
            catch (ThreadInterruptedException exc)
            {
                ExceptionBuffer.Add(exc);
                return;
            }
            finally
            {
                listener.Close();
            }
        }

        static ThreadedBuffer<Exception> ExceptionBuffer = new ThreadedBuffer<Exception>(100);
        static int iAcceptCounter = 0;

        private static void AcceptCallback(IAsyncResult IAR)
        {
            Socket Server, handler;
            try
            {
                Server = (Socket)IAR.AsyncState;
                handler = Server.EndAccept(IAR);
            }
            catch(ObjectDisposedException ode)
            {
                ExceptionBuffer.Add(ode);
                return;
            }

            allDone.Set();

            string sKey = TcpAsyncCommon.DefaultUID + ++iAcceptCounter;

            StateObject state = new StateObject();
            state.workSocket = handler;
            state.key = sKey;

            D2Sockets.Set(sKey, state);
            D2TReceive.Set(sKey, null);
            D2TSend.Set(sKey, null);
            D3BReceive.Set(sKey, new DataBuffer());
            D3BSend.Set(sKey, new DataBuffer());
        }


        public void Start()
        {
            Timers.Run(() => this.TimrMain_Elapsed(), 1, "main", true, false);
            Method.Run(() => StartListening(), null, true, false);
        }


    
      

    }
}
