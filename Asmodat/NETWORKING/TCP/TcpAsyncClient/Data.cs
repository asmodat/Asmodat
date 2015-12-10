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
using Asmodat.Extensions.Net.Sockets;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Networking
{
    public partial class TcpAsyncClient
    {
       


        //private IPHostEntry IPHEClient;
        private IPAddress IPAClient;
        private IPEndPoint IPEPClient;
        private Socket SLClient; //Listener

        //Asmodat.Types.
        private BufferedArray<byte[]> DBReceive { get; set; }
        private BufferedArray<byte[]> DBSend { get; set; }

        public byte[] Read()
        {
            byte[] data;
            DBReceive.Read(out data);
            return data;
        }

        public byte[][] ReadAll()
        {
            List<byte[]> result = new List<byte[]>();

            bool success = false;
            do
            {
                byte[] data;
                success = DBReceive.Read(out data);

                if(!data.IsNullOrEmpty())
                    result.Add(data);

            } while (success);

            return result.ToArray();
        }


        public int Length { get; private set; }
        public string ServerIP { get; private set; }
        public int ServerPort { get; private set; }

       
    }
}
