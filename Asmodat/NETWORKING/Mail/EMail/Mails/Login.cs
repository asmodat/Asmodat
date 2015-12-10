using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.Mail;
using Asmodat.Types;
using System.Threading;

using Asmodat.Abbreviate;

using Asmodat.InterIMAP;

//using AE.Net.Mail;
using Asmodat.Cryptography;
using System.Collections;
using Asmodat.Debugging;

namespace Asmodat.Networking
{
    public partial class Mails
    {


        public IMAPClient Server { get; private set; }


        public bool ServerLoggedOn
        {
            get
            {
                if (Server != null && Server.LoggedOn)
                    return true;
                else return false;
            }
        }


        public bool ServerLogin(string folder = null)
        {
            try
            {
                if (Server != null && Server.LoggedOn)
                    return true;

                IMAPConfig config = new IMAPConfig(HostIMAP, PortIMAP, Email, Password, EnableSslIMAP, false, folder);
                IMAPClient client = new IMAPClient(config, null, 1);
                client.Logon();
                Server = client;

                return true;
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return false;
            }
        }


       

       

    }
}
