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
    public partial class InputTracer
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

        /// <summary>
        /// 150KB == 150 * 1024 = 76800
        /// </summary>
        /// <param name="MaxLength"></param>
        public InputTracer(int MaxLength, string path = null)
        {
            this.TraceClipboard = TraceClipboard;

            if (path == null)
                path = Files.ExePath;
            else path = Files.GetFullPath(path);

            this.Path = path;

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
