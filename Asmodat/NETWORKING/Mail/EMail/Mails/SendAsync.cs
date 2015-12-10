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
using Asmodat.Extensions.Objects;

using Asmodat.InterIMAP;

//using AE.Net.Mail;
using Asmodat.Cryptography;
using System.Collections;
using Asmodat.Debugging;


namespace Asmodat.Networking
{
    public partial class Mails
    {

        public bool SendAsync(string title, string message, bool HTML = false)
        {
            return this.SendAsync(this.To, title, message, HTML, null, null);
        }

        public bool SendAsync(string title, string message, bool HTML, MemoryStream file, string filename)
        {
            MemoryStream[] memory = new MemoryStream[1] { file };
            string[] name = new string[1] { filename };
            return this.SendAsync(this.To, title, message, HTML, memory, name);
        }



        public bool SendAsync(string recipient, string title, string message, bool HTML, MemoryStream[] attachments = null, string[] filenames = null)
        {
            Compleated = false;
            Error = null;
            Canceled = false;
            UserState = null;
            Message = new MailMessage(this.Email, recipient, title, message);
            Message.IsBodyHtml = HTML;


            if(attachments != null && attachments.Length == filenames.Length)
            {
                for(int i = 0; i < attachments.Length; i++)
                {
                    if (attachments[i] == null || filenames[i].IsNullOrEmpty())
                        continue;

                    attachments[i].Position = 0;
                    Message.Attachments.Add(new Attachment(attachments[i], filenames[i]));
                }

            }

            InitializeClient();

            try
            {
                Client.SendAsync(Message, null);
            }
            catch (Exception ex)
            {
                Compleated = true;
                Error = ex;
                Canceled = true;
                Client.Dispose();
                Message.Dispose();
                return false;
            }

            return true;
        }




        private void SClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                Compleated = true;
                Error = e.Error;
                Canceled = e.Cancelled;
                UserState = e.UserState;
            }
            finally
            {
                if(Client != null)
                    Client.Dispose();
                if (Message != null)
                    Message.Dispose();
            }
        }


        public bool StopAsync()
        {
            try
            {
                if (Client != null)
                    Client.SendAsyncCancel();

            }
            catch(Exception ex)
            {
                Output.WriteException(ex);
                return false;
            }

            return true;
        }



    }
}
