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
        

        public static void SendSync(string sender, string receiver, string password, string title, string message, bool isBodyHTML = false, bool enableSsl = true)
        {
            System.Net.Mail.MailMessage MMessage = new System.Net.Mail.MailMessage(sender, receiver, title, message);
            SmtpClient SClient = new SmtpClient("smtp.googlemail.com", 587);
            NetworkCredential NCredital = new NetworkCredential(sender, password);
            MMessage.IsBodyHtml = isBodyHTML;// true;
            SClient.EnableSsl = enableSsl;// true;
            SClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            SClient.UseDefaultCredentials = false;
            SClient.Credentials = NCredital;
            SClient.Send(MMessage);
        }


    }
}
