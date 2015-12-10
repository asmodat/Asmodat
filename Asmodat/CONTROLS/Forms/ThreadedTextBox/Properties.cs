﻿using System;
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
using Asmodat.Debugging;

namespace Asmodat.FormsControls
{
    public partial class ThreadedTextBox : TextBox
    {


        public Control Invoker { get; private set; }
        public bool AutoscrollTop { get; set; } = false;
        public bool AutoscrollLeft { get; set; } = false;

        public bool AutoscrollFocusDisable { get; set; } = true;

        public new bool ReadOnly
        {
            get
            {
                bool var = false;
                if (Invoker == null) return base.ReadOnly;
                else Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.ReadOnly;
                }));

                return var;
            }
            set
            {
                if(Invoker == null) base.ReadOnly = value;
                else Invoker.Invoke((MethodInvoker)(() =>
                {
                    base.ReadOnly = value;
                }));
            }
        }

        public new string[] Lines
        {
            get
            {
                string[] var = null;
                if (Invoker == null) return base.Lines;
                else Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.Lines;
                }));

                return var;
            }
            set
            {
                if (Invoker == null) base.Lines = value;
                else Invoker.Invoke((MethodInvoker)(() =>
                {
                    base.Lines = value;
                }));
            }
        }
        

        public new int SelectionStart
        {
            get
            {
                int var = -1;
                Invoker.Invoke((MethodInvoker)(() =>
                {
                   
                    var = base.SelectionStart;
                }));

                return var;
            }
            set
            {
                if (Invoker == null) base.SelectionStart = value;
                else Invoker.Invoke((MethodInvoker)(() =>
                {
                    base.SelectionStart = value;
                }));
            }
        }

        public new int SelectionLength
        {
            get
            {
                int var = -1;
                Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.SelectionLength;
                }));

                return var;
            }
            set
            {
                if (Invoker == null) base.SelectionLength = value;
                else Invoker.Invoke((MethodInvoker)(() =>
                {
                    base.SelectionLength = value;
                }));
            }
        }


       

        public new string SelectedText
        {
            get
            {
                if (Invoker == null) return base.SelectedText;

                string var = null;
                Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.SelectedText;
                }));

                return var;
            }
            set
            {
                if (Invoker == null) base.SelectedText = value;
                else Invoker.Invoke((MethodInvoker)(() =>
                {
                    base.SelectedText = value;
                }));
            }
        }
        ExceptionBuffer Exceptions = new ExceptionBuffer();
        public new string Text
        {
            get
            {
                if (Invoker == null || !this.IsHandleCreated) return base.Text;

                string var = null;
                
                Invoker.Invoke((MethodInvoker)(() =>
                {
                    var = base.Text;
                }));

                return var;
            }
            set
            {
                if (Invoker == null || !this.IsHandleCreated) base.Text = value;
                else
                {
                    try
                    {
                        Invoker.Invoke((MethodInvoker)(() =>
                        {
                            base.Text = value;
                        }));
                    }
                    catch(Exception ex)
                    {
                        Exceptions.Write(ex);
                    }
                }
            }
        }
    }
}