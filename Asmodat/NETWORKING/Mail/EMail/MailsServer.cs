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

//using netDumbster.smtp;

namespace Asmodat.Networking
{
    public partial class MailsServer
    {
        public int Port { get; private set; } = 1992;


        //for encoding proiblems
        //message.Subject = "=?UTF-8?B?" & Convert.ToBase64String(Encoding.UTF8.GetBytes(outboxMessage.Title)) & "?="
        public MailsServer(int Port = 1992)
        {
            
        }

      

        public string Username { get; private set; }
        public string Password { get; private set; }

        public string Host { get; private set; }
        

        public void Send(string from, string to, string subject, string body, bool HTML)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = HTML;
            message.To.Add(to);

            SmtpClient Client = new SmtpClient();
            Client.Credentials = new NetworkCredential(Username, Password);
            Client.EnableSsl = false;
            Client.Host = NetTester.LocalIP; //Host;
            Client.Port = Port;

            Client.Send(message);
        }
    }
}


/*
 public partial class MailsServer
    {
        public int Port { get; private set; } = 1992;


        //for encoding proiblems
        //message.Subject = "=?UTF-8?B?" & Convert.ToBase64String(Encoding.UTF8.GetBytes(outboxMessage.Title)) & "?="
        public MailsServer(int Port = 1992)
        {
            this.Port = Port;
            SimpleSmtpServer Server = SimpleSmtpServer.Start(Port);
            Server.MessageReceived += Server_MessageReceived;
        }

        private void Server_MessageReceived(object sender, MessageReceivedArgs e)
        {
            
        }

        public string Username { get; private set; }
        public string Password { get; private set; }

        public string Host { get; private set; }
        

        public void Send(string from, string to, string subject, string body, bool HTML)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = HTML;
            message.To.Add(to);

            SmtpClient Client = new SmtpClient();
            Client.Credentials = new NetworkCredential(Username, Password);
            Client.EnableSsl = false;
            Client.Host = NetTester.LocalIP; //Host;
            Client.Port = Port;

            Client.Send(message);
        }
    }   
    */
