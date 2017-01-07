using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Windows.Forms;
using System.ComponentModel;
using Asmodat;
using System.Runtime.InteropServices;
using System.Drawing;
using Asmodat.Types;
using Asmodat.Extensions.Windows.Forms;
using Asmodat.Debugging;

namespace Asmodat.FormsControls
{
    public partial class ThreadedRichTextBox : RichTextBox
    {
        public ThreadedRichTextBox() : base()
        {
            this.VScroll += ThreadedRichTextBox_VScroll;
            this.HScroll += ThreadedRichTextBox_HScroll;
            this.KeyDown += ThreadedTextBox_KeyDown;
        }
        

        /*public void Initialize(Control Invoker)
        {
            this.Invoker = Invoker;

            if (this.Focused) return;
        }*/

        public bool ScrollTop()
        {
            return Invoker.TryInvokeMethodFunction(() => { return FormsControls.PostMessage(this.Handle, FormsControls.WM_VSCROLL, (IntPtr)FormsControls.SB_TOP, IntPtr.Zero); });
            //Invoker.Invoke((MethodInvoker)(() => { FormsControls.PostMessage(this.Handle, FormsControls.WM_VSCROLL, (IntPtr)FormsControls.SB_TOP, IntPtr.Zero); }));
        }

        public bool ScrolLeft()
        {
            return Invoker.TryInvokeMethodFunction(() => { return FormsControls.PostMessage(this.Handle, FormsControls.WM_HSCROLL, (IntPtr)FormsControls.SB_LEFT, IntPtr.Zero); });
            //Invoker.Invoke((MethodInvoker)(() =>{ FormsControls.PostMessage(this.Handle, FormsControls.WM_HSCROLL, (IntPtr)FormsControls.SB_LEFT, IntPtr.Zero);}));
        }

        private void ThreadedRichTextBox_VScroll(object sender, EventArgs e)
        {
            if (!AutoscrollTop) return;
            if (AutoscrollFocusDisable && this.Focused) return;

            this.ScrollTop();

        }

        private void ThreadedRichTextBox_HScroll(object sender, EventArgs e)
        {
            if (!AutoscrollLeft) return;
            if (AutoscrollFocusDisable && this.Focused) return;

            this.ScrolLeft();
        }


        public new void ScrollToCaret()
        {
            Invoker.TryInvokeMethodAction(() =>{ base.ScrollToCaret(); });
        }

        public void ScrollToEnd()
        {
            Invoker.TryInvokeMethodAction(() => {
                base.SelectionStart = base.Text.Length;
                base.ScrollToCaret();
                base.Refresh();
            });

            this.AppendText("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <param name="maxLines"></param>
        /// <param name="deleteMultiplayer">Reduces flicker of scrollbar</param>
        /// <param name="timeout"></param>
        public void AppendTextToStart(string text, Color color, int maxLines, double deleteMultiplayer = 2, int timeout = 1000)
        {
            try
            {
                bool readonlystate = this.ReadOnly;
                this.ReadOnly = false;

                this.SelectionStart = 0;
                this.SelectionLength = 0;// text.Length - 1;
                this.ScrollToCaret();
                this.SelectionColor = color;
                this.SelectedText = text;

                TickTimeout ttimeout = new TickTimeout(timeout, TickTime.Unit.ms);

                if (this.Lines.Length >= maxLines)
                    while (this.Lines.Length >= (maxLines * deleteMultiplayer) && !ttimeout.IsTriggered)
                    {
                        string rtext = this.Text;
                        int last1 = rtext.IndexOfByCount('\n', -1);
                        int last2 = rtext.IndexOfByCount('\n', -2);

                        if (last1 < 0 || last2 < 0)
                        {
                            Asmodat.Debugging.Output.WriteLine("Not managed outcom in  RichTextBox Abbreviate class !");
                            this.ScrollTop();
                            return;
                        }

                        this.SelectionStart = (int)last2 + 1;
                        this.SelectionLength = ((int)last1 - (int)last2) + 1;
                        this.SelectedText = "";

                        this.ScrollTop();
                    }
                

                this.ScrollTop();

                if (ttimeout.IsTriggered)
                    Asmodat.Debugging.Output.WriteLine("Rtbx timeout.");

                this.ReadOnly = readonlystate;

            }
            catch (Exception ex)
            {
                ex.ToOutput();
            }
        }
        

        public new void AppendText(string text)
        {
            Invoker.TryInvokeMethodAction(() => { base.AppendText(text); });
        }

        public new void Select(int start, int length)
        {
            Invoker.TryInvokeMethodAction(() => { base.Select(start, length); });
        }

        public new int GetFirstCharIndexFromLine(int lineNumber)
        {
            return Invoker.TryInvokeMethodFunction(() => { return base.GetFirstCharIndexFromLine(lineNumber); });
        }

        public void AppendText(string text, Color color, int maxLines, double deleteDivider = 2)
        {
            bool readonlystate = this.ReadOnly;
            this.ReadOnly = false;

            this.SelectionStart = this.TextLength;
            this.SelectionLength = 0;
            this.SelectionColor = color;
            this.AppendText(text);
            this.SelectionColor = this.ForeColor;

            int lines = this.Lines.Length;

            if (lines > maxLines)
            {
                this.Select(0, this.GetFirstCharIndexFromLine((int)(lines/deleteDivider)));
                this.SelectedText = "";
            }

            this.ReadOnly = readonlystate;
        }



        public bool ContainsLine(string text, bool? last = null)
        {
            bool result = false;

            try
            {
                if (this.Lines == null || this.Lines.Length <= 0)
                    return result;

                var lines = this.Lines.ToArray();


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

                
            }
            catch (Exception ex)
            {
                Asmodat.Debugging.Output.WriteException(ex);
            }

            return result;
        }


    }
}
