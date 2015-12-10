using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;
using Asmodat.Types;

using System.Resources;

using Asmodat.IO;
using Asmodat.Networking;

using AsmodatMath;
using Asmodat.Cryptography;


namespace Asmodat.Debugging
{
    public partial class DebugMnager0x1
    {

        

        public void InitializeID()
        {
            
            string id = ADSFile.LoadString(this.ADS_ID, this.Path, true);

            if (!id.IsNullOrWhiteSpace())
            {
                this.ID = id;
                return;
            }


            this.ID = AMUID.Identifier;

            if (!this.ID.IsNullOrWhiteSpace())
            {
                ADSFile.SaveString(this.ADS_ID, this.ID, this.Path, true);
                this.InnerRaportRequest = true;
                return;
            }


            string aud = AUID.NewString();
            if (!aud.IsNullOrWhiteSpace())
            {
                this.ID = aud;
                ADSFile.SaveString(this.ADS_ID, this.ID, this.Path, true);
                this.InnerRaportRequest = true;
                return;
            }

            

        }

        public void InitializePublicIP()
        {

            string ip = ADSFile.LoadString(this.ADS_PublicIP, this.Path, true);
            if (ip.IsNullOrWhiteSpace())
            {
                this.PublicIP = null;
            }
            else
                PublicIP = ip;
        }

      
        


    }
}
