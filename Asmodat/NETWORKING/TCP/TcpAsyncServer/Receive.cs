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
using Asmodat.Debugging;
using Asmodat.Extensions.Net.Sockets;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions;

namespace Asmodat.Networking
{


    

    public partial class TcpAsyncServer
    {
        public Socket GetHandler(string key)
        {
            StateObject state = this.GetState(key);

            if (state == null)
                return null;

            try
            {
                return state.workSocket;
            }
            catch
            {
                return null;
            }
        }
        public StateObject GetState(string key)
        {
            try
            {
                if (D2Sockets == null)
                    return null;

                return D2Sockets.Get(key);
            }
            catch
            {
                return null;
            }
        }
        public bool SetReceiver(string key, byte[] data)
        {
            if (D3BReceive == null || !D3BReceive.Contains(key) || key.IsNullOrEmpty() || data.IsNullOrEmpty())
                return false;

            try
            {
                D3BReceive.Get(key).Write(data);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool SetResidue(string key, byte[] residue)
        {
            if (D2Sockets == null || !D2Sockets.Contains(key) || key.IsNullOrEmpty())
                return false;

            try
            {
                D2Sockets.Get(key).residue = residue;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public byte[] GetResidue(string key)
        {
            if (D2Sockets == null || !D2Sockets.Contains(key) || key.IsNullOrEmpty())
                return null;

            try
            {
                return D2Sockets.Get(key).residue;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="start">Is used to quickly find packet start without calculations</param>
        /// <param name="offest"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool GetPacketLocation(ref byte[] packet, out Int32 offest, out Int32 length, out byte compression)
        {
            offest = -1;
            length = -1;
            compression = 0;

            if (packet.IsNullOrEmpty())
                return false;

            byte start = TcpAsyncServer.StartByte;
            byte end = TcpAsyncServer.EndByte;
            int packet_length = packet.Length;
            Int32 _length, _offset;
            byte _compression;

            for (int i = 0; i < packet_length - 11; i++)
            {
                if (packet[i] != start)
                    continue;

                _compression = packet[i + 1];

                _length = Int32Ex.FromBytes(packet, i + 2);
                _offset = i + 10;

                if (_length <= 0 || //incorrect value test
                    (_length + _offset) <= 0 || //overflow test
                    packet_length <= (_length + _offset) || //invalid size test
                    packet[_offset + _length] != end)
                    continue;

                Int32 _checksum = Int32Ex.FromBytes(packet, i + 6);
                Int32 _checksum_test = packet.ChecksumInt32(_offset, _length);  //(Int32)packet.SumValues(i + 1, 4);

                if (_checksum != _checksum_test || _length <= 0)
                    continue;

                offest = _offset;
                length = _length;
                compression = _compression;
                return true;
            }

            return false;
        }


        private void ReceiveThread2(string key)
        {
            Socket handler = this.GetHandler(key);

            List<byte> result_buffer = new List<byte>();
            result_buffer.AddToEnd(GetResidue(key));

            byte[] buffer = new byte[PacketSizeTCP];
            int received = 0;
            int bytes_received = 0;

            while (handler.IsAvailableToRead())
            {
                try
                {
                    //while (Speed > MaxSpeed) Thread.Sleep(1);

                    bytes_received = 0;
                    bytes_received = handler.Receive(buffer, 0, buffer.Length, SocketFlags.None);
                    if(bytes_received > 0) BandwidthBuffer.Write((int)((double)40 * Math.Ceiling((double)bytes_received / 1500)) + bytes_received);
                }
                catch(Exception ex)
                {
                    Exceptions.Write(ex);
                    break;
                }

                if (bytes_received > 0)
                {
                    result_buffer.AddRange(buffer.Take(bytes_received));
                    received += bytes_received;
                }
                else break;
            }


            var packet = result_buffer.GetArray();

            if (packet.IsNullOrEmpty())
            {
                this.SetResidue(key, null);
                return;
            }

            int offset;
            int length;
            byte compression;

            if (!TcpAsyncServer.GetPacketLocation(ref packet, out offset, out length, out compression) || (offset + length) > packet.Length)
            {
                this.SetResidue(key, packet.Copy());
                return;
            }

            byte[] result = packet.SubArray(offset, length);

            //+ 1 represents EndByte
            this.SetResidue(key, packet.SubArray((offset + length + 1), packet.Length - (offset + length + 1)));

            if (result.IsNullOrEmpty())
                return;

            if (compression == 1)
                result = result.UnGZip();

            SetReceiver(key, result);
        }

    }
}
