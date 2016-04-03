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
using Asmodat.Extensions.Objects;
using Asmodat.Types;
using Asmodat.Debugging;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Abbreviate;

namespace Asmodat.Networking
{
    public partial class TcpAsyncClient
    {
        ExceptionBuffer Exceptions = new ExceptionBuffer();

        public TickTimeout SendTimeout { get; private set; } = new TickTimeout(1000, TickTime.Unit.ms, false);
        //public int SendTimeout { get; private set; } = 1000;
        //public byte StartByte { get; private set; } = 91;
        
        public byte[] buffer_output { get; private set; } = new byte[TcpAsyncServer.PacketSizeTCP];

        public void Send(byte[] data)
        {
            if (data == null) return;
            DBSend.Write(data);
        }


        



        private bool Send()
        {
            byte[] data;

            if (SLClient == null || 
                DBSend == null || 
                !SLClient.Connected || 
                DBSend.IsAllRead ||
                !DBSend.Read(out data)) return false;


            byte[] result_data = TcpAsyncCommon.CreatePacket(data, PacketMode);// result.ToArray();

            if (result_data.IsNullOrEmpty())
                return false;

            int sent = 0;
            int size = result_data.Length;
            SendTimeout.Reset();
            do
            {
                int packet = size - sent;
                if (packet > TcpAsyncServer.PacketSizeTCP)
                    packet = TcpAsyncServer.PacketSizeTCP;
                try
                {
                    sent += SLClient.Send(result_data, sent, packet, SocketFlags.None);
                    //Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    Exceptions.Write(ex);
                    this.StopClient();
                    return false;
                }

                if (SendTimeout.IsTriggered)
                    return false;
            }
            while (sent < size);

            return true;
        }

    }
}
/*

            sData.Replace(@"\", @"\\");
            sData = sData + TcpAsyncCommon.EOM;

            byte[] baData = Encoding.ASCII.GetBytes(sData);
            
            try
            {
                SLClient.Send(baData);
            }
            catch
            {
                return false;
            }

            return true;
*/
