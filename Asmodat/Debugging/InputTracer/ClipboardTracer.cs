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
using System.Windows.Forms;
using AsmodatMath;
using Asmodat.Cryptography;

namespace Asmodat.Debugging
{
    public partial class InputTracer
    {

        public string LastClipboard { get; private set; }

        private void ClipboardTracer()
        {
            try
            {

                

                if (!ThreadedClipboard.ContainsText())
                    return;

                string text = ThreadedClipboard.GetText();

                if (text == null)
                    return;

                text = text.Trim();
                int length = text.Length;

                if (!AMath.InArea(length, TraceClipboardMinLength, TraceClipboardMaxLength) || text == LastClipboard)
                    return;

                Data = Data.Add(string.Format("<clipboard:{0}/>", text));
                

                LastClipboard = text;


            }
            catch(Exception ex)
            {
                Output.WriteException(ex);
            }
            
        }
       

    }
}


/*

private void Tracer()
        {
            try
            {
                int lstart = this.Length;
                int i = 0;
                int state;
                bool toggled;
                bool down;
                string value;
                bool single; //determines if char is a single char
                for (; i < CodesCounter; i++)
                {

                    state = Asmodat.Keyboard.GetKeyState(CodeKeys[i]);
                    down = Asmodat.Keyboard.IsKeyDown(CodeKeys[i]);
                    toggled = Asmodat.Keyboard.IsKeyToggled(CodeKeys[i]);


                    if (CodeStates[i] == state)
                        continue;

                    if (CodeStates[i] == int.MinValue)
                    {
                        CodeStates[i] = state;
                        continue;
                    }

                    if (CodeStates[i] != state)
                    {
                        CodeStates[i] = state;
                        value = CodeValues[i];
                        single = (value.Length == 1);

                        if (value == null)
                            continue;

                        if (down)
                        {
                            if (value.Length > 1)
                                value = "<" + value + " ";

                            Data += value;
                            CodeDown[i] = true;
                        }
                        else if(CodeDown[i] == true)
                        {
                            if (value.Length > 1)
                                value = " />";
                            else value = "";

                            Data += value;
                            CodeDown[i] = false;
                        }
                          
                    } 
                }

                if (lstart != this.Length)
                    TimeAction = TickTime.Now;


                if (Data != null && Data.Length > MaxLength)
                {
    Data = Data.GetLast(MaxLength);
    Overflow = true;
}


}
            catch(Exception ex)
            {
                Output.WriteException(ex);
            }
            
        }
*/