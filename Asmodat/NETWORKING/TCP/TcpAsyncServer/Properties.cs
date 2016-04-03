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
using Asmodat.Debugging;
using Asmodat.Extensions.Net.Sockets;
using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Types;

namespace Asmodat.Networking
{
   
    public partial class TcpAsyncServer
    {
        ThreadedTimers Timers = new ThreadedTimers(100);
        //ThreadedMethod Methods = new ThreadedMethod(100);

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        ExceptionBuffer Exceptions = new ExceptionBuffer();
        

        public static int PacketSizeTCP { get; private set; } = 8192;//100 * 1024 * 1024;// 1400;//8192;//163884;//32768;//65536;//;

        public static int SendTimeout { get; private set; } = 1000;
        public static int ReceiveTimeout { get; private set; } = 1000;
        public static short Ttl { get; private set; } = 42;
        public static bool NoDelay { get; private set; } = true;

        /// <summary>
        /// Maximum allowed speed in bits per second
        /// </summary>
        public static int MaxSpeed { get; private set; } = 50 * 1024 * 1024; //50Mb/s

        public static int MaxSpeedAbsolute { get; set; } = 1024 * 1024 * 1024; //1Gb/s

        public int SetMaxSpeed(int value)
        {
            if (value < 1024)
                value = 1024;

            if (value > TcpAsyncServer.MaxSpeedAbsolute)
                value = TcpAsyncServer.MaxSpeedAbsolute;

            MaxSpeed = value;
            return value;
        }


        public bool IsStarted { get; private set; } = false;

        public int ConnectionsCount
        {
            get
            {
                if (D2Sockets == null)
                    return 0;
                else
                {
                    var keys = D2Sockets.Keys;
                    if (keys.IsNullOrEmpty())
                        return 0;

                    return keys.Length;
                }
            }
        }

        public string[] ConnectionKeys
        {
            get
            {
                if (D2Sockets == null)
                    return null;
                else
                {
                    return D2Sockets.Keys;
                }
            }
        }

        /// <summary>
        /// 1.5GB/s max capacity for 1500B frame
        /// </summary>
        public TickBuffer<int> BandwidthBuffer = new TickBuffer<int>(1024*1024, 1000, TickTime.Unit.ms);

        /// <summary>
        /// Speed in bits per second, both send and received
        /// </summary>
        public double Speed
        {
            get
            {
                var all = BandwidthBuffer.ReadAllValues();
                if (all.IsNullOrEmpty())
                    return 0;

                var sum = all.SumValues();
                try
                {
                    sum *= 8;

                    if (sum < 0 || sum > int.MaxValue)
                        return 0;

                    return (double)sum;
                }
                catch
                {
                    return 0;
                }
            }
        }

    }
}
