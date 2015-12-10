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
            if (SLClient == null || DBSend == null || !SLClient.Connected || DBSend.IsAllRead) return false;

            byte[] data;
            if (!DBSend.Read(out data))
                return false;

            if (data.IsNullOrEmpty())
                return false;

            byte[] data_compressed = data.GZip();
            byte compression = 0;
            if (data_compressed.Length < data.Length - 1)
            {
                compression = 1;
                data = data_compressed;
            }

            byte[] length = Int32Ex.ToBytes(data.Length);
            Int32 checksum = data.ChecksumInt32();
            byte[] check = Int32Ex.ToBytes(checksum);

            List<byte> result = new List<byte>();
            #region packet
            result.Add(TcpAsyncServer.StartByte);
            result.Add(compression);
            result.AddRange(length);
            result.AddRange(check);
            result.AddRange(data);
            result.Add(TcpAsyncServer.EndByte);
            #endregion
            byte[] result_data = result.ToArray();


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
