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
using Asmodat.Extensions.Net.Sockets;
using Asmodat.Extensions.Objects;

namespace Asmodat.Connect
{
    public class StateObject
    {
        public string key;
        public Socket workSocket = null;
        public const int BufferSize = 1024*1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    public partial class TcpAsyncServer
    {
        private void ReceiveThread(string sKey)
        {
            try
            {
                if (D3BSend == null)
                    return;

                DataBuffer DBuffer = D3BSend.Get(sKey);
                StateObject state = D2Sockets.Get(sKey);

                if (state == null)
                    return;

                Socket handler = state.workSocket;

                if (!handler.IsConnected())
                    return;

                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                string message = e.Message;
            }
        }

        private static void ReadCallback(IAsyncResult IAR)
        {
            StateObject state = (StateObject)IAR.AsyncState;
            Socket handler = state.workSocket;
            int bytesRead = 0;

            if (!handler.Connected) return;
            try
            {
                bytesRead = handler.EndReceive(IAR);
            }
            catch
            {
                return;
            }

            if (bytesRead <= 0) return;

            System.String content = System.String.Empty;
            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
            content = state.sb.ToString();


            if (content.IndexOf(TcpAsyncCommon.EOM) < 0)
            {
                if (content.Length < StateObject.BufferSize / 2)
                {
                    try
                    {
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                    }catch(Exception e)
                    {
                        ExceptionBuffer.Add(e);
                    }
                }
                else
                {
                    state.sb.Clear();
                    //state.buffer = new byte[StateObject.BufferSize];
                    return;
                }
            }
            
            state.sb.Clear();
            //state.buffer = new byte[StateObject.BufferSize];
            List<string> Packets = content.SplitSafe(TcpAsyncCommon.EOM).ToList();

            if (Packets.Count > 0)
            {
                foreach (string packet in Packets)
                {
                    string subPacket = packet;
                    if (subPacket == null)
                        subPacket = "";
                    else subPacket = subPacket.Replace(@"\\", @"\");

                    if (!D3BReceive.Contains(state.key)) return;
                    else D3BReceive.Get(state.key).Set(subPacket);
                    //Thread.Sleep(1);
                }
            }

        }


    }
}

/*
 private static void ReadCallback(IAsyncResult IAR)
        {
            


            System.String content = System.String.Empty;
            StateObject state = (StateObject)IAR.AsyncState;
            Socket handler = state.workSocket;

            if (!handler.Connected) return;

            int bytesRead = handler.EndReceive(IAR);

            if (bytesRead <= 0) return;

            state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
            content = state.sb.ToString();

            if (content.IndexOf(TcpAsyncCommon.EOM) < 0) handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

            List<string> Packets = Asmodat.Abbreviate.String.ToList(content, TcpAsyncCommon.EOM).ToList();

            if (Packets.Count > 0)
            {
                foreach (string packet in Packets)
                {
                    string subPacket = packet;
                    if (subPacket == null)
                        subPacket = "";
                    else subPacket = subPacket.Replace(@"\\", @"\");

                    if (!D3BReceive.Contains(state.key)) return;
                    else D3BReceive.Get(state.key).Set(subPacket);
                    //Thread.Sleep(1);
                }

                state.sb.Clear();
            }

        }


    }
 */
