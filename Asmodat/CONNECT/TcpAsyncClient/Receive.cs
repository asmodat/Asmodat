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

namespace Asmodat.Connect
{

    
    public partial class TcpAsyncClient
    {
        /// <summary>
        /// Buffor for sData, that left, while reading from client socket, and was not utilizated (missing EOM)
        /// </summary>
        string sDataBuffer = ""; 


        /// <summary>
        /// This is synchronised TCP Recive method, received data will be only written into buffer if received bytes contains End Of Message string. 
        /// </summary>
        /// <param name="iTimeout">Determines, how long in [ms] method can be executed, if iTimeout is lower then 0, method will not be terminated. </param>
        private void Receive(int timeout = -1)
        {
            DateTime DTStart = DateTime.Now;
            TickTime TTStart = TickTime.Now;
            bool bTimeout = false;
            bool bDataFound = false;
            byte[] baData;

            string sData = sDataBuffer;
            string sMessage = null;
            string sMessageResidue = null;
            int iRecivedCount;

            while (!bTimeout && !bDataFound)
            {
                if (timeout > 0 && TickTime.Timeout(TTStart, timeout, TickTime.Unit.ms))
                    bTimeout = true;

                baData = new byte[1024];
                iRecivedCount = SLClient.Receive(baData);
                sData += Encoding.ASCII.GetString(baData, 0, iRecivedCount);


                sMessage = sData.ExtractTag(TcpAsyncCommon.SOM, TcpAsyncCommon.EOM, out sMessageResidue);

                if (sMessage == null) continue;
                else bDataFound = true;
            }

            if (!bDataFound)
            {
                sDataBuffer = sData;
                return;
            }

            sDataBuffer = sMessageResidue;
            sData = sMessage;

            //sData.Replace(TcpAsyncClient.SOM, "");
            //sData.Replace(TcpAsyncClient.EOM, "");
            sData.Replace(@"\\", @"\");

            DBReceive.Set(sData);
        }

        Thread ThrdReceive;
        private void ReceiveThread()
        {
            if (ThrdReceive != null && ThrdReceive.IsAlive) return;

            ThrdReceive = new Thread(() =>
            {
                this.Receive();
            });

            ThrdReceive.Start();
        }

    }
}
