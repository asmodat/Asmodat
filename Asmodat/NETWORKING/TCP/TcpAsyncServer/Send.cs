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
        

        public bool Send(string key, byte[] data)
        {
            if (!D3BSend.Contains(key)) 
                return false;

            D3BSend.Get(key).Write(data);

            return true;
        }

        /// <summary>
        /// Sends data to all clients
        /// </summary>
        /// <param name="sData"></param>
        /// <returns></returns>
        public bool SendAll(byte[] data)
        {
            if (data.IsNullOrEmpty())
                return false;

            var keys = this.ConnectionKeys;

            if (keys.IsNullOrEmpty())
                return false;

            foreach (var k in keys)
                this.Send(k, data);

            return true;
        }

        public bool SendAllIfRead(byte[] data, byte[] messageRead)
        {
            if (data.IsNullOrEmpty() || messageRead.IsNullOrEmpty())
                return false;

            var keys = this.ConnectionKeys;

            if (keys.IsNullOrEmpty())
                return false;

            foreach (var k in keys)
            {
                byte[] read = this.Read(k);

                if (Bytes.Compare(read, messageRead))
                    this.Send(k, data);
            }

            return true;
        }

        public byte[] Read(string key)
        {
            if (D3BReceive == null || !D3BReceive.Contains(key))
                return null;
            try
            {
                byte[] read;
                D3BReceive.Get(key).Read(out read);
                return read;
            }
            catch
            {
                return null;
            }
        }

        public byte[][] ReadAll(string key)
        {
            if (D3BReceive == null || !D3BReceive.Contains(key))
                return null;
            try
            { 
                return D3BReceive.Get(key).ReadAll();
            }
            catch
            {
                return null;
            }
        }

        public Dictionary<string, byte[][]> ReadAll()
        {
            if (D3BReceive == null)
                return null;

            var keys = D3BReceive.Keys;

            if (keys.IsNullOrEmpty())
                return null;

            Dictionary<string, byte[][]> result = new Dictionary<string, byte[][]>();

            foreach(var key in keys)
                result.Add(key, this.ReadAll(key));
            
            return result;
        }



        ThreadedLocker TLocker = new ThreadedLocker(1024);


        private bool SendThread(string key)
        {
            if (D3BSend == null || key.IsNullOrEmpty())
                return false;
           
            BufferedArray<byte[]> DBuffer = D3BSend.Get(key);

            if (DBuffer == null || DBuffer.IsAllRead)
                return false;

            StateObject state = this.GetState(key);
            Socket handler = this.GetHandler(key);

            byte[] data;

            if (!handler.IsConnected() || DBuffer == null || DBuffer.IsAllRead || !DBuffer.Read(out data))
                return false;
            
            byte[] result_data = TcpAsyncCommon.CreatePacket(data, PacketMode);

            if (data.IsNullOrEmpty())
                return false;

            int sent = 0;
            int size = result_data.Length;

            do
            {
                int packet = size - sent;
                if (packet > PacketSizeTCP)
                    packet = PacketSizeTCP;
                try
                {
                    while (Speed > MaxSpeed)
                        Thread.Sleep(1);

                    int bytes_send = handler.Send(result_data, sent, packet, SocketFlags.None);
                    sent += bytes_send;
                    if (bytes_send > 0)
                        BandwidthBuffer.Write((int)((double)40 * Math.Ceiling((double)bytes_send/1500)) + bytes_send);
                }
                catch(Exception ex)
                {
                    Exceptions.Write(ex);
                    return false;
                }
            }
            while (sent < size);


            return true;
        }




    }
}
