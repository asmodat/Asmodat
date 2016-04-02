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
        /// <summary>
        /// Start Of Message indicator Tag
        /// </summary>
        public const string SOM = "";
        /// <summary>
        /// Enod Of Message indicator Tag
        /// </summary>
        public const string EOM = "\n";


        /// <summary>
        /// Default Uniqe Idenyfier Key
        /// </summary>
        public const string DefaultUID = "#DefaultID#";


        public static byte[] CreatePacket(byte[] data)
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
            result.Add(TcpAsyncServer.StartByte);
            result.Add(compression);
            result.AddRange(length);
            result.AddRange(check);
            result.AddRange(data);
            result.Add(TcpAsyncServer.EndByte);
            #endregion

            return result.ToArray();
        }
    }
}
