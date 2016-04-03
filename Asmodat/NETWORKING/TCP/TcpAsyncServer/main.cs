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
using Asmodat.Extensions.Net.Sockets;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Types;

namespace Asmodat.Networking
{

    public static partial class TcpAsyncServerEx
    {
        public static bool IsStarted(this TcpAsyncServer server)
        {
            if (server == null)
                return false;

            try
            {
                return server.IsStarted;
            }
            catch
            {
                return false;
            }
        }
    }

        public partial class TcpAsyncServer : IDisposable
    {

        public void Dispose()
        {
            //

            this.StopAll();

            Listener.Cleanup();
        }

        


        public int Length { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IP"></param>
        /// <param name="Port"></param>
        /// <param name="Length">defines size of the I/O buffers</param>
        public TcpAsyncServer(string IP, int Port, int Length, TcpAsyncCommon.PacketMode PacketMode)
        {
            this.IP = IP;
            this.Port = Port;
            this.Length = Length;
            this.PacketMode = PacketMode;
        }

        

        


        public void StopAll()
        {
            IsStarted = false;

            if(Timers != null) Timers.TerminateAll();
            if (Tasks != null) Tasks.RemoveAll();

            Listener.Cleanup();

            if (D2Sockets == null)
                return;

            var keys = D2Sockets.Keys;

            if (keys.IsNullOrEmpty())
                return;
            
          //  TasksManager tasks = new TasksManager(100);

            foreach (string key in keys)
                this.Stop(key);
                //tasks.Run(() => this.Stop(key), "stop" + key, true);

           // tasks.JoinStopAll(2000);
            //tasks.Dispose();
        }

        public void Stop(string key)
        {
            if (key.IsNullOrEmpty())
                return;

            try
            {
                GetHandler(key).Cleanup();
            }
            finally
            {
                if (D2Sockets != null) D2Sockets.Remove(key);
                if (D3BReceive != null) D3BReceive.Remove(key);
                if (D3BSend != null) D3BSend.Remove(key);
            }
        }

        TasksManager Tasks = new TasksManager();
        //cm sis
        
        void TimrMain_Send()
        {
            
            var keys = this.ConnectionKeys;

            if (keys.IsNullOrEmpty())
                return;

            foreach (string key in keys)
            {
                Tasks.Run(() => SendThread(key), "CommunicationSendThread" + key, false);
            }
        }

        void TimrMain_Receive()
        {
            var keys = this.ConnectionKeys;

            if (keys.IsNullOrEmpty())
                return;

            foreach (string key in keys)
            {
                Tasks.Run(() => ReceiveThread2(key), "CommunicationReceiveThread" + key, false);
            }
        }







    }
}


/*
 public void Stop(string key)
        {
            if (D2Sockets == null)
                return;

            StateObject state = D2Sockets.Get(key);
            Socket handler = state.workSocket;

            try
            {
                handler.BeginDisconnect(false, new AsyncCallback(DisconnectCallback), state);
            }
            catch(SocketException se)
            {
                ExceptionBuffer.Add(se);
            }
        }

        private void DisconnectCallback(IAsyncResult IAR)
        {
            StateObject state = (StateObject)IAR.AsyncState;
            Socket handler = state.workSocket;
            string key = state.key;

            handler.EndDisconnect(IAR);
            handler.Shutdown(SocketShutdown.Both);
            handler.Disconnect(true);
            handler.Close();
            handler.Dispose();

            D2Sockets.Remove(key);
            D3BReceive.Remove(key);
            D3BSend.Remove(key);
        }

*/
