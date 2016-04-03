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
using Asmodat.Extensions.Objects;
using Asmodat.Types;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions.Net.Sockets;

namespace Asmodat.Networking
{

    
    public partial class TcpAsyncClient
    {

        public int ReceiveTimeout { get; private set; } = 1000;
        public TickTimeout TimeoutReceived { get; private set; }


        public byte[] residue { get; private set; } = null;
        public byte[] buffer_input { get; private set; } = new byte[TcpAsyncServer.PacketSizeTCP];

        public TcpAsyncCommon.PacketMode PacketMode { get; set; } = TcpAsyncCommon.PacketMode.Stop;

        private bool Receive()
        {
            List<byte> result_buffer = new List<byte>();
            result_buffer.AddToEnd(residue);
            
            int received = 0;
            int received_now = 0;

            while (SLClient.IsAvailableToRead())
            {
                try
                {
                    received_now = 0;
                    received_now = SLClient.Receive(buffer_input, 0, buffer_input.Length, SocketFlags.None);
                    //Thread.Sleep(1);
                }
                catch(Exception ex)
                {
                    Exceptions.Write(ex);
                    this.StopClient();
                    break;
                }

                if (received_now > 0)
                {
                    result_buffer.AddRange(buffer_input.Take(received_now));
                    received += received_now;
                }
                else break;
            }

            var packet = result_buffer.GetArray();

            if (packet.IsNullOrEmpty())
            {
                residue = null;
                return false;
            }

            int offset;
            int length;
            byte compression;

            if (!TcpAsyncCommon.RecreatePacket(ref packet, out offset, out length, out compression, PacketMode) || (offset + length) > packet.Length)
            {
                residue = packet;
                return false;
            }

            byte[] result = packet.SubArray(offset, length);

            //+ 1 represents EndByte
            residue = packet.SubArray((offset + length + 1), packet.Length - (offset + length + 1)); 

            if (result.IsNullOrEmpty())
                return false;

            if (compression == 1)
                result = result.UnGZip();

            DBReceive.Write(result);
            return true;
        }




        
    }
}
