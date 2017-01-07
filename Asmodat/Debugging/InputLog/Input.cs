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

namespace Asmodat.Debugging
{
    public partial class InputLog
    {
        


        


        public int StartSpan(TickTime.Unit unit = TickTime.Unit.ms)
        {
            
                if (TimeStart.IsUndefined)
                    return -1;

            return (int)(TimeStart.Span() / (long)unit);

        }

        public bool TraceClipboard { get; private set; } = false;
        public int TraceClipboardMinLength { get; private set; } = 4;
        public int TraceClipboardMaxLength { get; private set; } = 64;

        
        public InputLog()
        {
            this.TraceClipboard = TraceClipboard;
            this.Path = Files.ExePath;

            this.Load();
   
            this.MaxLength = MaxLength;
            Keyboard = new KeyboardUsing();
            Timers = new ThreadedTimers(10);
            KeyCodes = new VirtualKeyCodes();
            CodeStates = new int[Keyboard.CodesCounter];
            CodeDown = new bool[Keyboard.CodesCounter];

            TimeStart = TickTime.Now;
        }




    }
}
