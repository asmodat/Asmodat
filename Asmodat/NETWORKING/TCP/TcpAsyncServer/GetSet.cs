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

namespace Asmodat.Networking
{
    public partial class TcpAsyncServer 
    {


        public string IP { get; private set; }
        public int Port { get; private set; }


        /// <summary>
        /// This list contains Default keys, that were returned with GetNewKey method, in order to prevent returning all the time the same default key,
        /// </summary>
        private static List<string> LSUsedKeys = new List<string>();

        /// <summary>
        /// This method returns string key of non managed (default) Connection Sockets
        /// </summary>
        /// <returns>String UID Key to all imbeded Data Dictionaries, or null if no default key exist's</returns>
        public string GetNewKey
        {
            get
            {
                foreach (string s in D2Sockets.Keys)
                {
                    if (System.String.IsNullOrEmpty(s)) continue; //Do not return null's if not neaded
                    if (!D2Sockets.Get(s).workSocket.Connected) continue; //Return only connected Keys
                    if (!s.Contains(TcpAsyncCommon.DefaultUID)) continue; //Return only not assigned default keys
                    if (LSUsedKeys.Contains(s)) continue; //Don't return the same default key if, it is not utilised

                    LSUsedKeys.Add(s);
                    return s;
                }

                if (LSUsedKeys.Count > 0)
                {
                    LSUsedKeys.Clear();
                    return GetNewKey;
                }

                return null;
            }
        }

        /// <summary>
        /// Returns true, if Connected, non managed (default) key (UID) exist in Sockets DataDictionary, else false
        /// </summary>
        public bool IsNewSocketAvailable
        {
            get
            {
                foreach (string s in D2Sockets.Keys)
                {
                    if (s == null) continue;
                    if (!D2Sockets.Get(s).workSocket.Connected) continue;
                    if (!s.Contains(TcpAsyncCommon.DefaultUID)) continue;

                    return true;
                }

                return false;
            }
        }


    }
}


//private string[] GetKeys
//{
//    get
//    {
//        return D2Sockets.Keys;
//    }
//}