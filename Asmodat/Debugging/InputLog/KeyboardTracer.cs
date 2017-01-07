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
using Asmodat.Extensions.Collections.Generic;
using System.Threading;

namespace Asmodat.Debugging
{
    public partial class InputLog
    {
        
        public string SpecialLiteralToString(string s)
        {
            if (s == "\r") s = @"\r";
            else if (s == "\b") s = @"\b";
            else if (s == "\a") s = @"\a";
            else if (s == "\f") s = @"\f";
            else if (s == "\n") s = @"\n";
            else if (s == "\r") s = @"\r";
            else if (s == "\t") s = @"\t";
            else if (s == "\v") s = @"\v";

            return s;
        }

        public bool IsSpecialLiteral(string s)
        {
            if (s == "\r") return true;
            else if (s == "\b") return true;
            else if (s == "\a") return true;
            else if (s == "\f") return true;
            else if (s == "\n") return true;
            else if (s == "\r") return true;
            else if (s == "\t") return true;
            else if (s == "\v") return true;

            return false;
        }


        public string LastSpecialLiteral = null;
        public int LastSpecialLiteralCount = 0;

        byte[] keyStatesOld = null;

        ThreadedMethod method = new ThreadedMethod(1);
        public void Beep()
        {
            Console.Beep(100, 500);
            //Thread.Sleep(100);
        }

        //string change = "";

        private void KeyboardTracer()
        {
            try
            {

                //if (Asmodat.Keyboard.IsKeyDown(80))
                    

                int lstart = this.Length;
                int i = 0;
                int state;
                bool down;
                string result;
                int key;
                byte[] keyStates = Keyboard.GetKeysStates();

                
                if ( keyStates.IsNullOrEmpty() || keyStates.EqualsSlow(keyStatesOld))
                    return;

                keyStatesOld = keyStates.Copy();

                /* if (keyStatesOld != null)
                     for (int j = 0; j < keyStates.Length; j++)
                         if (keyStates[j] != keyStatesOld[j])
                         {
                             change = "" + j;
                             break;6
                         }*/

                //Beep();
                //method.Run(() => Beep(), null, false, false);



                for (; i < Keyboard.CodesCounter; i++)
                {
                    key = Keyboard.CodeKeys[i];
                    state = Asmodat.Keyboard.GetKeyState(key);
                    down = Asmodat.Keyboard.IsKeyDown(key);

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

                        if (down)
                        {
                            result = Asmodat.Keyboard.KeyToString(key, keyStates);

                            if (result == null || result.Length != 1)
                                continue;

                            bool isspecial = this.IsSpecialLiteral(result);


                            if (isspecial && LastSpecialLiteral == result)
                            {
                                ++LastSpecialLiteralCount;
                                continue;
                            }
                            else if (LastSpecialLiteral != result)
                            {
                                if (LastSpecialLiteral != null)
                                    Data += string.Format("<{0}{1}/>", this.SpecialLiteralToString(LastSpecialLiteral), LastSpecialLiteralCount);

                                if (isspecial)
                                {
                                    LastSpecialLiteral = result;
                                    LastSpecialLiteralCount = 1;
                                    continue;
                                }
                                else
                                {
                                    LastSpecialLiteral = null;
                                    LastSpecialLiteralCount = 0;
                                }
                            }

                            char c = result[0];

                            if (char.IsLetterOrDigit(c) ||
                                char.IsSurrogate(c) ||
                                char.IsNumber(c) ||
                                char.IsPunctuation(c) ||
                                 char.IsSeparator(c) ||
                                 char.IsSymbol(c))
                            {
                                Data += c;
                            }
                        

                        }
                          
                    } 
                }

                if (lstart != this.Length)
                    TimeAction = TickTime.Now;


                if (Data != null && Data.Length > MaxLength)
                {
                    
                    Data = Data.RemoveFirst(Data.Length - MaxLength);
                    IsFull = true;
                }


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