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
using Asmodat.Extensions.Net.Sockets;
using Asmodat.Debugging;

using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Connect
{
    
    public partial class TcpAsyncServer : IDisposable
    {

        public void Dispose()
        {
            this.StopAll();
        }


        public void StopAll()
        {
            Method.Terminate("main");


            var keys = D2Sockets.Keys;

            try
            {
                ThreadedMethod methods = new ThreadedMethod(keys.Length);

                foreach (string key in keys)
                    methods.Run(() => this.Stop(key), "stop" + key, true, false);

                methods.JoinAll();
            }
            finally
            {
                Method.Terminate(() => StartListening());
            }
        }

        public TcpAsyncServer(int iPort)
        {
            this.Port = iPort;
        }

        ThreadedTimers Timers = new ThreadedTimers(100);
        ThreadedMethod Method = new ThreadedMethod(100, ThreadPriority.Normal, 1);
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        

        
        public void Stop(string key)
        {
            StateObject state = D2Sockets.Get(key);

            try
            {
                if (state != null && state.workSocket != null)
                {
                    Socket handler = state.workSocket;
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Disconnect(true);
                    handler.Close();
                    handler.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                if (D2Sockets != null) D2Sockets.Remove(key);
                if (D2TReceive != null) D2TReceive.Remove(key);
                if (D2TSend != null) D2TSend.Remove(key);
                if (D3BReceive != null) D3BReceive.Remove(key);
                if (D3BSend != null) D3BSend.Remove(key);
            }
        }
        
        
        void TimrMain_Elapsed()
        {
            if (D2Sockets == null)
                return;

            var keys = D2Sockets.Keys;

            if (keys.IsNullOrEmpty())
                return;

            foreach (string key in keys)
            {
                this.ReceiveThread(key);
                this.SendThread(key);
            }
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
