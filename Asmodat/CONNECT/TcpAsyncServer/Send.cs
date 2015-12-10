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
    public partial class TcpAsyncServer
    {
        public bool Send(string sKey, string sData)
        {
            if (!D3BSend.Contains(sKey)) 
                return false;

            D3BSend.Get(sKey).Set(sData);

            return true;
        }


        private void SendThread(string sKey)
        {
            DataBuffer DBuffer = D3BSend.Get(sKey);
            Thread ThrdSend = D2TSend.Get(sKey);
            StateObject state = D2Sockets.Get(sKey);
            Socket handler = state.workSocket;


            if (!handler.Connected) 
                return;
            if (DBuffer == null) return;
            if (!DBuffer.IsDataAvalaible) return;

            string sData = DBuffer.Get(false); //Don't increment data, becouse it is not certain at this point, if it will be send
            if (sData == null) return;
            else if (sData.Length > 1024) throw new Exception("Exception in Send() method, packet size is to large.");
            DBuffer.IndexerIcrement();

            sData.Replace(@"\", @"\\");
            sData = TcpAsyncCommon.SOM + sData + TcpAsyncCommon.EOM;

            byte[] baData = Encoding.ASCII.GetBytes(sData);


            
            handler.BeginSend(baData, 0, baData.Length, 0, new AsyncCallback(SendCallback), state);
        }



        private static void SendCallback(IAsyncResult IAR)
        {
            StateObject state = (StateObject)IAR.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = handler.EndSend(IAR);
        }


    }
}
