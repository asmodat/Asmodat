using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using Asmodat.Abbreviate;
using Asmodat.Types;

using System.Resources;

using Asmodat.IO;
using System.Security;
using Asmodat.Cryptography;

namespace Asmodat.Debugging
{
    public partial class InputTracer
    {

        KeyboardUsing Keyboard;
        VirtualKeyCodes KeyCodes;
        //string[,] CodesOrigin;

        private UInt16 Seed { get; set; }  = 2991;


        public SecureString Data { get; private set; } = new SecureString();

        public SecureString DataEncrypted
        {
            get
            {
                return AED0x1.EncryptSecure(Data, Seed);
            }
            set
            {
                Data = AED0x1.DecryptSecure(value, Seed);
            }
        }

        public string DataRaw
        {
            get
            {
                return Data.Release();
            }
            set
            {
                Data = value.Secure();
            }
        }

        public string DataRawCompressed
        {
            get
            {
                try
                {
                    return StringCompressor.Zip(DataRaw);
                }
                catch(Exception ex)
                {
                    Output.WriteException(ex);
                    return null;
                }
            }
            set
            {
                try
                {
                    DataRaw = StringCompressor.UnZip(value);
                }
                catch (Exception ex)
                {
                    Output.WriteException(ex);
                    Data = new SecureString();
                }
            }
        }


        public string DataRawEncryptedCompressed
        {
            get
            {
                try
                {
                    return StringCompressor.Zip(DataRawEncrypted);
                }
                catch (Exception ex)
                {
                    Output.WriteException(ex);
                    return null;
                }
            }
            set
            {
                try
                {
                    DataRawEncrypted = StringCompressor.UnZip(value);
                }
                catch (Exception ex)
                {
                    Output.WriteException(ex);
                    Data = new SecureString();
                }
            }
        }


        public string DataRawEncrypted
        {
            get
            {
                return DataEncrypted.Release();
            }
            set
            {
                DataEncrypted = value.Secure();
            }
        }

        // ThreadedDictionary<int, string> Codes;
        ThreadedTimers Timers;

        TickTime TimeStart = TickTime.MinValue;
        TickTime TimeStop = TickTime.MinValue;
        TickTime TimeAction = TickTime.MinValue;

        private bool[] CodeDown;
        private int[] CodeStates;
        //private int[] CodeKeys;
        //private string[] CodeValues;
        //private int CodesCounter;

        public int MaxLength { get; private set; }

        public bool IsFull { get; private set; } = false;

        public int Length
        {
            get
            {
                if (Data.IsNull())
                    return 0;

                else return Data.Length;
            }
        }
        public string Path { get; private set; }


        public string Name { get; private set; } = "DebuggInputTracer";

    }
}
