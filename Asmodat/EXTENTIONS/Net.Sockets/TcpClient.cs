using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using Asmodat.Debugging;

namespace Asmodat.Extensions.Net.Sockets
{
    

    public static class TcpClientEx
    {
      

        public static bool IsConnected(this TcpClient client)
        {
            if (client?.Connected != true || client?.Client?.Connected != true)
                return false;

            try
            {
                return client.Client.Poll(0, SelectMode.SelectRead) ? 
                    !(client.Client.Receive(new byte[1], SocketFlags.Peek) == 0) : 
                    true;
            }
            catch
            {
                return false;
            }
        }

        


    }
}
