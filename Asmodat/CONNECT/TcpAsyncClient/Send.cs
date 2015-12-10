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

       public void Send(string sData)
        {
            if (sData == null) return;
            DBSend.Set(sData);
        }


        private bool Send()
        {
            if (!SLClient.Connected || !DBSend.IsDataAvalaible) return false;

            string sData = DBSend.Get(false); //Don't increment data, becouse it is not certain at this point, if it will be send

            if (sData == null) return false;

            sData.Replace(@"\", @"\\");
            sData = sData + TcpAsyncCommon.EOM;

            byte[] baData = Encoding.ASCII.GetBytes(sData);

            if (baData.Length > 1024) throw new Exception("Exception in Send() method, packet size is to large.");

            try
            {
                SLClient.Send(baData);
            }
            catch
            {
                return false;
            }

            DBSend.IndexerIcrement(); //Increment indexer, becouse data was successfully send.
            return true;
        }


        Thread ThrdSend;
        private void SendThread()
        {
            if (!DBSend.IsDataAvalaible) return;
            if (ThrdSend != null && ThrdSend.IsAlive) return;


            ThrdSend = new Thread(() =>
            {
                this.Send();
            });

            
            ThrdSend.Start();
        }
    }
}
