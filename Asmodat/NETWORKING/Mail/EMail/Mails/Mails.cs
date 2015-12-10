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

namespace Asmodat.Networking
{
    public partial class Mails
    {
        

        

        public Mails(string Email, string Password, bool EnableSsl = true)
        {
            this.Email = Email;
            this.Password = Password;
            this.EnableSsl = EnableSsl;
        }

        public Mails(
            string HostSMTP, int PortSMTP, 
            string HostIMAP, int PortIMAP,
            bool EnableSsl,
            UInt16 Seed, string EnEmail, string EnPassword, string EnTo)
        {
            this.Seed = Seed;
            this.EncryptedEmail = EnEmail;
            this.EncryptedPassword = EnPassword;
            this.EncryptedTo = EnTo;
            this.EnableSsl = EnableSsl;
            this.HostSMTP = HostSMTP;
            this.HostIMAP = HostIMAP;
            this.PortSMTP = PortSMTP;
            this.PortIMAP = PortIMAP;
            
        }
        

        public void InitializeClient()
        {
            NetworkCredential Creditals = new NetworkCredential();
            Creditals.UserName = this.Email;
            Creditals.SecurePassword = this.SecurePassword;
            Client = new SmtpClient(HostSMTP, PortSMTP);
            Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            Client.EnableSsl = this.EnableSsl;
            Client.UseDefaultCredentials = false;
            Client.Credentials = Creditals;
            Client.SendCompleted += SClient_SendCompleted;
        }

        public bool Compleated { get; private set; } = false;

        public bool Delivered
        {
            get
            {
                if (this.Compleated && this.Error == null && !this.Canceled)
                    return true;
                else return false;
            }
        }

        public void WaitForResponse(int timeout, TickTime.Unit unit = TickTime.Unit.ms)
        {
            TickTime start = TickTime.Now;
            while (!TickTime.Timeout(start, timeout, unit) && !Compleated)
                Thread.Sleep(100);
        }




       

    }
}
