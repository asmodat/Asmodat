using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace Asmodat.Extensions.Net.Sockets
{
    

    public static class SocketEx
    {
        public static bool IsAvailableToRead(this Socket socket)
        {
            try
            {
                if (socket == null || socket.Available <= 0)
                    return false;
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool IsConnected(this Socket socket)
        {
            if (socket == null || !socket.Connected)
                return false;

            bool test1 = false, test2 = false;
            try
            {
                test1 = !socket.Connected;// socket.Poll(1000, SelectMode.SelectRead);
                test2 = (socket.Available == 0);
            }
            catch
            {
                test1 = false;
                test2 = false;
            }

            if (test1 && test2)
                return false;
            else
                return true;
        }


        public static bool Cleanup(this Socket socket)
        {
            if (socket == null)
                return true;

            try
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Disconnect(true);
                socket.Close();
                socket.Dispose();

                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}
