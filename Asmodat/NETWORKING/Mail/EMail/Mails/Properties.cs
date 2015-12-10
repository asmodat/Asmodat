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

using Asmodat.Cryptography;
using System.Security;

namespace Asmodat.Networking
{
    public partial class Mails
    {
        public bool EnableSsl { get; private set; } = true;
        public bool EnableSslIMAP { get; private set; } = true;

        public SmtpClient Client { get; private set; }


        public System.Net.Mail.MailMessage Message { get; private set; }

        public Exception Error { get; private set; } = null;

        public bool Canceled { get; private set; } = false;

        public object UserState { get; private set; } = null;

        public string HostSMTP { get; private set; } = "smtp.googlemail.com";
        public string HostIMAP { get; private set; } = "imap.googlemail.com";

        public int PortSMTP { get; private set; } = 587;
        public int PortIMAP { get; private set; } = 993;


        private UInt16 Seed { get; set; } = 25;

        private SecureString email = null;

        private SecureString to = null;

        private SecureString password = null;

        public string To
        {
            get
            {
                return AED0x1.Decrypt(to, Seed);

            }
            private set
            {
                to = AED0x1.EncryptSecure(value, Seed);
            }
        }
        public string Email
        {
            get
            {
                return AED0x1.Decrypt(email, Seed);

            }
            private set
            {
                email = AED0x1.EncryptSecure(value, Seed);
            }
        }
        public string Password
        {
            get
            {
                return AED0x1.Decrypt(password, Seed);

            }
            private set
            {
                password = AED0x1.EncryptSecure(value, Seed);
            }
        }

        public string EncryptedTo
        {
            get
            {
                return to.Release();

            }
            private set
            {
                to = value.Secure();
            }
        }
        public string EncryptedEmail
        {
            get
            {
                return email.Release();

            }
            private set
            {
                email = value.Secure();
            }
        }
        public string EncryptedPassword
        {
            get
            {
                return password.Release();

            }
            private set
            {
                password = value.Secure();
            }
        }

        public SecureString SecureTo
        {
            get
            {
                return AED0x1.DecryptSecure(to, Seed);

            }
            private set
            {
                to = AED0x1.EncryptSecure(value, Seed);
            }
        }
        public SecureString SecurePassword
        {
            get
            {
                return AED0x1.DecryptSecure(password, Seed);

            }
            private set
            {
                password = AED0x1.EncryptSecure(value, Seed);
            }
        }
        public SecureString SecureEmail
        {
            get
            {
                return AED0x1.DecryptSecure(email, Seed);

            }
            private set
            {
                email = AED0x1.EncryptSecure(value, Seed);
            }
        }
        

    }
}
