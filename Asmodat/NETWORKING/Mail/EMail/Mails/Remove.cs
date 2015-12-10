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
        

        public bool Remove(string subjectNotContainingSting)
        {

            this.ServerLogin();

            if (!ServerLoggedOn)
                return false;

            List<string> remove = new List<string>();
            if (Server.LoggedOn && Server.Folders != null && Server.Folders.Count > 0)
            {
                IMAPFolder[] folders = Server.Folders.ToArray();

                for (int i = 0; i < folders.Length; i++)
                    RemoveMessages(folders[i], subjectNotContainingSting);
            }





            Server.UpdateCache(false);

            return true;
        }


        public void RemoveMessages(IMAPFolder folder, string subjectNotContainingSting)
        {
            if (folder == null || folder.Messages == null || folder.Messages.Count <= 0)
                return;

            if(folder.SubFolders != null && folder.SubFolders.Count > 0)
            {
                IMAPFolder[] folders = folder.SubFolders.ToArray();

                for (int i = 0; i < folders.Length; i++)
                    this.RemoveMessages(folders[i], subjectNotContainingSting);
            }

            IMAPMessage[] messages = folder.Messages.ToArray();

            for (int i = 0; i < messages.Length; i++)
            {
                IMAPMessage message = messages[i];
                
                if (message == null)
                    continue;

                if(subjectNotContainingSting != null)
                {
                    if(message.Subject == null || !message.Subject.Contains(subjectNotContainingSting))
                    {
                        try
                        {
                            bool result = folder.Messages.Remove(message);
                            if (result)
                                continue;
                        }
                        catch(Exception ex)
                        {
                            ex.ToOutput();
                        }

                    }
                }
            }

            return;
        }





    }
}
