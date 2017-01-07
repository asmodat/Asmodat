using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;
using System.Windows.Controls;
using Asmodat.Debugging;
using System.Windows;
using System.Windows.Threading;

namespace Asmodat.Extensions.Windows.Controls
{


    public  static partial class ControlEx
    {
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
            if (control == null || control.Dispatcher.CheckAccess())
                action();
            else
                Invoker.Invoke(() =>
                {
                    action();
                });
        }

        public static T InvokeMethodFunction<T>(this Control control, Func<T> func)
        {
            T result = default(T);

            if (control == null || control.Dispatcher.CheckAccess())
                result = func();
            else
                Invoker.Invoke(() =>
                {
                    result = func();
                });

            return result;
        }


        private static Dispatcher _Invoker = null;
        public static Dispatcher Invoker
        {
            get
            {
                if (_Invoker == null)
                    _Invoker = Application.Current.Dispatcher;

                return _Invoker;
            }
        }


    }
}
