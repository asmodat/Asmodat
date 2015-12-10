using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Types;

using System.Resources;

using Asmodat.IO;
using Asmodat.Cryptography;
using System.Security;


namespace Asmodat.Debugging
{
    public partial class InputTracer
    {

        public void Clear()
        {
            Data.Clear();
            IsFull = false;
            TimeStart = TickTime.Now;
            TimeAction = TickTime.Now;

            for (int i = 0; i < CodeStates.Length; i++)
            {
                CodeStates[i] = int.MinValue;
                CodeDown[i] = false;
            }

        }



        public void Delete()
        {
            this.Clear();

            try
            {
                ADSFile.Write("", this.Path, this.Name);
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
            }
        }

        public void Load()
        {
            try
            {
                string data = ADSFile.Read(this.Path, this.Name);

                if (data.IsNullOrWhiteSpace())
                {
                    Data = new SecureString();
                    return;
                }

                DataRawEncryptedCompressed = data;
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
            }
        }


        public void Save()
        {
            try
            {
                if (Data.IsNullOrWhiteSpace())
                    return;
                
                
                ADSFile.Write(DataRawEncryptedCompressed, this.Path, this.Name);
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
            }
        }

        private void Saver()
        {
            this.Save();
        }


    }
}
