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

using Asmodat.Extensions.Objects;

namespace Asmodat.Debugging
{
    public partial class InputLog
    {

        KeyboardUsing Keyboard;
        VirtualKeyCodes KeyCodes;
        //string[,] CodesOrigin;
        


        public string Data { get; private set; } = null;


        

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

        public int MaxLength { get; private set; } = (150 * 1024) / 4;

        public bool IsFull { get; private set; } = false;

        public int Length
        {
            get
            {
                if (Data.IsNullOrEmpty())
                    return 0;

                else return Data.Length;
            }
        }
        public string Path { get; private set; }


        public string Name { get; private set; } = "DebuggInputLog";

    }
}
