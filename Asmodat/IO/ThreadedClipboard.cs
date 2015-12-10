using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.Windows.Forms;
using Asmodat.Debugging;
using System.Threading;

namespace Asmodat.IO
{
    public class ThreadedClipboard
    {
        private string GetTextResult;
        private bool ContainsTextResult;
        private void GetTextSTA(object format)
        {
            try
            {
                if (format == null)
                    GetTextResult = Clipboard.GetText();
                else
                    GetTextResult = Clipboard.GetText((TextDataFormat)format);
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                GetTextResult = string.Empty;
            }
        }
        private void ContainsTextSTA(object format)
        {
            try
            {
                if (format == null)
                    ContainsTextResult = Clipboard.ContainsText();
                else
                    ContainsTextResult = Clipboard.ContainsText((TextDataFormat)format);
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                ContainsTextResult = false;
            }
        }

        public static string GetText()
        {
            return ThreadedClipboard.GetText(null);
        }

        public static string GetText(TextDataFormat? format)
        {
            ThreadedClipboard instance = new ThreadedClipboard();
            Thread sta = new Thread(instance.GetTextSTA);
            sta.SetApartmentState(ApartmentState.STA);

            if (format != null && !format.HasValue)
                sta.Start(format.Value);
            else
                sta.Start();

            sta.Join();
            return instance.GetTextResult;
        }

        public static bool ContainsText()
        {
            return ThreadedClipboard.ContainsText(null);
        }

        public static bool ContainsText(TextDataFormat? format)
        {
            ThreadedClipboard instance = new ThreadedClipboard();
            Thread sta = new Thread(instance.ContainsTextSTA);
            sta.SetApartmentState(ApartmentState.STA);

            if (format != null && !format.HasValue)
                sta.Start(format.Value);
            else
                sta.Start();

            sta.Join();
            return instance.ContainsTextResult;
        }

    }
}
