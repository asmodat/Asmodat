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
        public static byte[] CreatePacket(byte[] data, PacketMode mode)
        {
            if (mode == PacketMode.Raw)
            {
                return CreatePacket_Raw(data);
            }
            else if (mode == PacketMode.Stop)
            {
                return CreatePacket_Stop(data);
            }
            else if (mode == PacketMode.StartSizeStopCompressSum)
            {
                return CreatePacket_StartSizeStopCompressSum(data);
            }
            else
            {
                
                return null;
            }
        }

        public static byte[] CreatePacket_Raw(byte[] data)
        {
            if (data.IsNullOrEmpty())
                return null;

            return data;
        }

        /// <summary>
        /// For packet to be accepted must end with \n\r - 0x10 0x13 - 16 19
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CreatePacket_Stop(byte[] data)
        {
            if (data.IsNullOrEmpty())
                return null;

            List<byte> result = new List<byte>();
            #region packet
            result.AddRange(data);
            result.AddRange(TcpAsyncCommon.EndBytes);
            #endregion

            return result.ToArray();
        }

        public static byte[] CreatePacket_StartSizeStopCompressSum(byte[] data)
        {
            if (data.IsNullOrEmpty())
                return null;

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
            result.Add(TcpAsyncCommon.StartByte);
            result.Add(compression);
            result.AddRange(length);
            result.AddRange(check);
            result.AddRange(data);
            result.Add(TcpAsyncCommon.EndByte);
            #endregion

            return result.ToArray();
        }
    }
}
