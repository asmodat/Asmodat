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
using Asmodat.Networking.Dns;

namespace Asmodat.Networking
{
    public partial class MailsSimple
    {
        public static int SmtpPort = 25;

        public static bool Send(string from, string to, string subject, string body)
        {
            string domain = MailsSimple.GetDomain(to);
            IPAddress[] servers = GetMailExchangeServer(domain);
            foreach(IPAddress adress in servers)
            {
                try
                {
                    SmtpClient client = new SmtpClient(adress.ToString(), SmtpPort);
                    client.Send(from, to, subject, body);
                    return true;
                }
                catch
                {
                    continue;
                }

            }

            return false;
        }


        public static bool Send(MailMessage message)
        {
            string domain = MailsSimple.GetDomain(message.To[0].Address);
            IPAddress[] servers = GetMailExchangeServer(domain);
            foreach (IPAddress adress in servers)
            {
                try
                {
                    SmtpClient client = new SmtpClient(adress.ToString(), SmtpPort);
                    client.Send(message);
                    return true;
                }
                catch
                {
                    continue;
                }

            }

            return false;
        }


        public static string GetDomain(string email)
        {
            int index = email.IndexOf('@');
            if(index == -1)
                throw new ArgumentException("No a valid email adress: ", "email");

            if (email.IndexOf('<') > -1 && email.IndexOf('>') > -1)
                return email.Substring(index + 1, email.IndexOf('>') - index);
            else return email.Substring(index + 1);
        }

        public static IPAddress[] GetMailExchangeServer(string domain)
        {
            IPHostEntry entry = DomainNameUtil.GetIPHostEntryForMailExchange(domain);
            if (entry.AddressList.Length > 0)
                return entry.AddressList;
            else if (entry.Aliases.Length > 0)
                return System.Net.Dns.GetHostAddresses(entry.Aliases[0]);
            else
                return null;
        }
    }
}
