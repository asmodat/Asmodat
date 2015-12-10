using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;

using AsmodatMath;
using Asmodat.Abbreviate;

using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System.Net.Mail;
using System.Threading;
using System.IO;
using Asmodat.IO;
using System.Security.Cryptography.X509Certificates;

namespace Asmodat.Networking
{

    public class Gmail
    {

        public GmailService Service { get; private set; }

        public ClientSecrets Secrets { get; private set; }

        public BaseClientService.Initializer Initializer { get; private set; }

        public UserCredential Credentials { get; private set; }

        public string Name { get; private set; }

        public string OAuth2ID { get; private set; }

        public string OAuth2Secret { get; private set; }

        public string User { get; private set; }

        //BotV0x1
        //504304721897-1ef8devll02lc732dg2aukte3fjtq7a3.apps.googleusercontent.com
        //R_g2-feEwk1jorX4GF3oDkzo
        public Gmail(string User,string Name, string OAuth2ID, string OAuth2Secret)
        {
            this.User = User;
            this.Name = Name;
            this.OAuth2ID = OAuth2ID;
            this.OAuth2Secret = OAuth2Secret;
            Secrets = new ClientSecrets();
            Secrets.ClientId = OAuth2ID;
            Secrets.ClientSecret = OAuth2Secret;

            this.Initialize();
        }

        public void Initialize()
        {
            //string path = Files.GetFullPath("AsmodatBot.json");

            /*using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Credentials = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                    new[] { GmailService.Scope.GmailReadonly },
                    Name, CancellationToken.None).Result;
            }*/

            X509Certificate2 Certificate = new X509Certificate2(OAuth2ID, OAuth2Secret, X509KeyStorageFlags.Exportable);

            ServiceAccountCredential.Initializer SACInitializer = new ServiceAccountCredential.Initializer(OAuth2ID);
            SACInitializer.Scopes = new[] { GmailService.Scope.GmailReadonly, GmailService.Scope.GmailSend };
            SACInitializer.User = this.User;

            ServiceAccountCredential SACCredentials = new ServiceAccountCredential(SACInitializer.FromCertificate(Certificate));
            //ServiceAccountCredential SACredentials = new ServiceAccountCredential(new ServiceAccountCredential.Initializer)



            //GmailService.Scope.GmailSend,


            Initializer =  new BaseClientService.Initializer();
            Initializer.HttpClientInitializer = Credentials;
            Initializer.ApplicationName = Name;
           

            Service = new GmailService(Initializer);
            
        }
        
        public void Send(string to, string subject, string body, bool HTML = false)
        {
            MailMessage m2essage = new MailMessage();
            m2essage.Subject = subject;
            m2essage.Body = body;
            m2essage.IsBodyHtml = HTML;
            m2essage.To.Add(to);

            StringWriter writer = new StringWriter();
            //m2essage.Sa

            Message message = new Message();

            message.Raw = m2essage.ToString();

            Service.Users.Messages.Send(message, to);

           // UsersResource.MessagesResource.SendRequest request = new UsersResource.MessagesResource.SendRequest(Service, message, OAuth2ID);

        }


        private static string Base64UrlEncode(string input)
        {
            var inputbytes = System.Text.Encoding.UTF8.GetBytes(input);
            return System.Convert.ToBase64String(inputbytes).Replace('+', '-').Replace('/', '-').Replace("=", "");
        }
    }
}
