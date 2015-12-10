using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

using System.Drawing;

using System.Reflection;

using System.Data;


using System.Collections;

using System.Diagnostics;

using System.Globalization;

using System.Linq.Expressions;
using System.Runtime.InteropServices;

namespace Asmodat.FormsControls
{
    public partial class FormsControls
    {
        public const UInt32 SB_TOP = 0x6;
        public const UInt32 SB_LEFT = 0x6;
        public const UInt32 WM_VSCROLL = 0x115;
        public const UInt32 WM_HSCROLL = 0x115;

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);


        
    }
}

/*
public static TResult Invoke<TResult>(Expression<Func<TResult>> expression)
        {
            //TResult value = default(TResult);
            //Control reference = null;

            //try
            //{
            //    reference = Expressions.GetReference<TResult, Control>(expression);
            //}
            //finally { }


            //reference.Invoke((MethodInvoker)(() =>
            //{
            //    value = expression.Compile()();
            //}));

            return expression.Compile().Invoke();

            //return value;
        }
*/