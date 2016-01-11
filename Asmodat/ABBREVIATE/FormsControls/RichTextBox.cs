using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using System.Drawing;

using System.Reflection;

using System.Data;
using Asmodat.Types;
using Asmodat.Extensions.Objects;

namespace Asmodat.Abbreviate
{
    public partial class FormsControls
    {
        public static void AppendText(RichTextBox RTBox, string text, Color color, int maxLines, bool scroll = false, int timeout = 1000)
        {

            RTBox.Invoke((MethodInvoker)(() =>
            {
                bool readonlystate = RTBox.ReadOnly;
                try
                {
                    RTBox.ReadOnly = false;
                    RTBox.SelectionStart = RTBox.TextLength;
                    RTBox.SelectionLength = 0;
                    RTBox.SelectionColor = color;
                    RTBox.AppendText(text);
                    RTBox.SelectionColor = RTBox.ForeColor;

                    int iSelectionSave = RTBox.SelectionStart;
                    TickTime start = TickTime.Now;
                    while (RTBox.Lines.Length > maxLines && !TickTime.Timeout(start, timeout, TickTime.Unit.ms))
                    {
                        RTBox.SelectionStart = 0;
                        RTBox.SelectionLength = RTBox.Text.IndexOf("\n", 0) + 1;
                        RTBox.SelectedText = "";
                    }

                    if (scroll && RTBox.Text != null)
                    {
                        RTBox.SelectionStart = RTBox.Text.Length;
                        RTBox.ScrollToCaret();
                    }

                    if (TickTime.Timeout(start, timeout, TickTime.Unit.ms))
                        Asmodat.Debugging.Output.WriteLine("Rtbx timeout.");

                }
                catch (Exception ex)
                {
                    Asmodat.Debugging.Output.WriteException(ex);
                }

                RTBox.ReadOnly = readonlystate;
            }));

        }

        public static void AppendTextToStart(RichTextBox RTBox, string text, Color color, int maxLines, int timeout = 1000)
        {

            RTBox.Invoke((MethodInvoker)(() =>
            {
                bool readonlystate = RTBox.ReadOnly;

                try
                {
                    
                    RTBox.ReadOnly = false;
                    RTBox.SelectionStart = 0;
                    RTBox.SelectionLength = 0;// text.Length - 1;
                    RTBox.ScrollToCaret();
                    RTBox.SelectionColor = color;
                    RTBox.SelectedText = text;

                    TickTime start = TickTime.Now;
                    while (RTBox.Lines.Length >= maxLines && !TickTime.Timeout(start, timeout, TickTime.Unit.ms))
                    {
                        string rtext = RTBox.Text; 
                        int last1 = rtext.IndexOfByCount('\n', -1);
                        int last2 = rtext.IndexOfByCount('\n', -2);

                        if (last1 < 0 || last2 < 0)
                        {
                            Asmodat.Debugging.Output.WriteLine("Not managed outcom in  RichTextBox Abbreviate class !");
                            return;
                        }
                            // throw new Exception("TODO: finish"); 

                        RTBox.SelectionStart = (int)last2 + 1;
                        RTBox.SelectionLength = ((int)last1 - (int)last2) + 1;
                        RTBox.SelectedText = "";
                    }

                    RTBox.SelectionStart = 0;
                    RTBox.SelectionLength = 0;
                    RTBox.ScrollToCaret();

                    if (TickTime.Timeout(start, timeout, TickTime.Unit.ms))
                        Asmodat.Debugging.Output.WriteLine("Rtbx timeout.");

                }
                catch (Exception ex)
                {
                    Asmodat.Debugging.Output.WriteException(ex);
                }

                RTBox.ReadOnly = readonlystate;
            }));


        }


        public static bool LineContains(RichTextBox RTBox, string text, bool? last = null)
        {
            bool result = false;
            if (RTBox == null || RTBox.Lines.Length <= 0)
                return result;

            lock (RTBox)
            RTBox.Invoke((MethodInvoker)(() =>
            {
                if (RTBox == null || RTBox.Lines == null || RTBox.Lines.Length <= 0)
                    return;

                var lines = RTBox.Lines.ToArray();

                if (last == null)
                {
                    for (int i = 0; i < lines.Length; i++)
                        if (lines[i].Contains(text))
                        {
                            result = true;
                            break;
                        }
                }
                else if (last.Value)
                {
                    if (lines.Last().Contains(text))
                        result = true;
                }
                else
                {
                    if (lines.First().Contains(text))
                        result = true;
                }



            }));

            return result;
        }


        public static string GetFirstLine(RichTextBox RTBox)
        {
            string line = null;
            int iLinesLength = 0;

            RTBox.Invoke((MethodInvoker)(() =>
            {
                iLinesLength = RTBox.Lines.Length;


                if (iLinesLength <= 0) return;
                else if (iLinesLength == 1)
                {
                    line = RTBox.Text;
                    iLinesLength = 0;
                    RTBox.Text = "";
                }
                else
                {
                    RTBox.Select(0, RTBox.GetFirstCharIndexFromLine(1));
                    line = RTBox.SelectedText;
                    RTBox.SelectedText = "";
                }
            }));

            return line;
        }
    }
}
