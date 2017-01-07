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
                test1 = socket.Poll(1000, SelectMode.SelectRead);//!socket.Connected;// 
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


        public static bool TryShutdown(this Socket socket, SocketShutdown shutdown)
        {
            if (socket == null)
                return true;

            try
            {
                socket.Shutdown(shutdown);
                return true;
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }
        }

        public static bool TryDisconnect(this Socket socket, bool reuseSocket)
        {
            if (socket == null)
                return true;

            try
            {
                socket.Disconnect(reuseSocket);
                return true;
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }
        }

        public static bool TryClose(this Socket socket)
        {
            if (socket == null)
                return true;

            try
            {
                socket.Close();
                return true;
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }
        }

        public static bool TryDispose(this Socket socket)
        {
            if (socket == null)
                return true;

            try
            {
                socket.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }
        }

        public static void Cleanup(this Socket socket)
        {
            if (socket == null)
                return;

            
            socket.TryShutdown(SocketShutdown.Both);
            socket.TryDisconnect(true);
            socket.TryClose();
            //socket.TryDispose();
           // socket = null;

        }



    }
}
