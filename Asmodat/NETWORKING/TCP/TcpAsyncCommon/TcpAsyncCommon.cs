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
        public enum PacketMode
        {
            Raw = 0, //used for test
            Stop = 1, //used for simple LAN transmission
            StartSizeStopCompressSum = 2 //used for long complex WAN transmission
        }


        /// <summary>
        /// Start Of Message indicator Tag
        /// </summary>
        public const string SOM = "";
        /// <summary>
        /// Enod Of Message indicator Tag
        /// </summary>
        public const string EOM = "\n";

        public static readonly byte[] EndBytes = new byte[] { 0 };//16, 19 };

        public static byte StartByte { get; private set; } = 91;
        public static byte EndByte { get; private set; } = 92;

        /// <summary>
        /// Default Uniqe Idenyfier Key
        /// </summary>
        public const string DefaultUID = "#DefaultID#";

        
    }
}
