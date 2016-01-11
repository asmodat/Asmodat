using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Windows.Forms;
using Asmodat.Debugging;

namespace Asmodat.Extensions.Windows.Forms
{


    public  static partial class ControlEx
    {
        public static void SetFontStyle(this Control control, FontStyle style)
        {
            if (control == null)
                return;
            control.Font = new Font(control.Font, style);
        }

        public static bool TryInvokeMethodAction(this Control control, Action action)
        {
            try
            {
                control.InvokeMethodAction(action);
                return true;
            }
            catch(Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }
        }

        public static T TryInvokeMethodFunction<T>(this Control control, Func<T> func)
        {
            T result;

            try
            {
                result = control.InvokeMethodFunction(func);
            }
            catch (Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return default(T);
            }


            return result;
        }

        public static void InvokeMethodAction(this Control control, Action action)
        {
        
            if (control == null || !control.InvokeRequired)
            {
                action();
            }
            else
            {
                control.Invoke((MethodInvoker)(() =>
                {
                   action();
                }));
            }
        }

        public static T InvokeMethodFunction<T>(this Control control, Func<T> func)
        {
            T result = default(T);

            if (control == null || !control.InvokeRequired)
            {
                result = func();
            }
            else
            {
                control.Invoke((MethodInvoker)(() =>
                {
                    result = func();
                }));
            }

            return result;
        }



        public static Control GetFirstParent(this Control control)
        {
            if (control == null || control.Parent == null)
                return null;

            Control result = control.Parent;

            while (result.Parent != null)
                result = result.Parent;

            return result;
        }


        public static string GetFullPathName(this Control control)
        {
            if (control == null)
                return null;

            Control result = control.Parent;
            string path = control.Name;

            if (control.Parent == null)
                return path;
            
            while (result.Parent != null)
            {
                result = result.Parent;
                path = result.Name + @"\" + path;
            }

            return path;
        }


    }
}
