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

//using System.Net.Mail;

namespace Asmodat.Networking
{
    public partial class Mails
    {
        /// <summary>
        /// Checks if email has correct format
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static bool IsValid(string address)
        {
            try
            {
                MailAddress m = new System.Net.Mail.MailAddress(address);
                return true;
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return false;
            }
        }

       

    }
}
