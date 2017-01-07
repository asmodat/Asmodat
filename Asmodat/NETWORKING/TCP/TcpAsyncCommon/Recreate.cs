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
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Networking
{
    public partial class TcpAsyncCommon
    {
        public static bool RecreatePacket(
            ref byte[] packet, 
            out Int32 offest, 
            out Int32 length, 
            out byte compression, 
            PacketMode mode)
        {
            if (mode == PacketMode.Raw)
            {
                offest = 0;
                compression = 0;
                return RecreatePacket_Raw(ref packet, out length);
            }
            else if (mode == PacketMode.Stop)
            {
                offest = 0;
                compression = 0;
                return RecreatePacket_Stop(ref packet, out length);
            }
            else if (mode == PacketMode.StartSizeStopCompressSum)
            {
                return RecreatePacket_StartSizeStopCompressSum(ref packet, out offest, out length, out compression);
            }
            else
            {
                offest = -1;
                length = -1;
                compression = 0;
                return false;
            }
        }


        public static bool RecreatePacket_Raw(ref byte[] packet, out Int32 length)
        {
            length = -1;

            if (packet.IsNullOrEmpty())
                return false;
            
            length = packet.Length;

            return true;
        }

        public static bool RecreatePacket_Stop(ref byte[] packet, out Int32 length)
        {
            length = -1;

            if (packet.IsNullOrEmpty())
                return false;

            int eom_length = TcpAsyncCommon.EndBytes.Length;
            bool found;
            for (int i = 0; i <= packet.Length - eom_length; i++)
            {
                found = true;
                for (int i2 = 0; i2 < eom_length; i2++)
                {
                    if (packet[i + i2] != TcpAsyncCommon.EndBytes[i2])
                    {
                        found = false;
                        break;
                    }
                }

                if (!found)
                    continue;
                
                length = i;

                List<byte> result = new List<byte>();
                result.AddRange(packet.SubArray(0, i));
                result.AddRange(packet.SubArray(i + eom_length));
                packet = result.ToArray();

                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="start">Is used to quickly find packet start without calculations</param>
        /// <param name="offest"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool RecreatePacket_StartSizeStopCompressSum(ref byte[] packet, out Int32 offest, out Int32 length, out byte compression)
        {
            offest = -1;
            length = -1;
            compression = 0;

            if (packet.IsNullOrEmpty())
                return false;

            byte start = TcpAsyncCommon.StartByte;
            byte end = TcpAsyncCommon.EndByte;
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

        
    }
}
