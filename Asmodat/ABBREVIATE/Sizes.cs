using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Diagnostics;
using System.Drawing;

namespace Asmodat.Abbreviate
{
    public class Sizes
    {

        public static Size Add(Size size, int height, int width)
        {
            return new Size(size.Width + width, size.Height + height);
        }

        public static Size Sub(Size size, int height, int width)
        {
            return new Size(size.Width - width, size.Height - height);
        }

        public static Size Add(Size size, int value)
        {
            return new Size(size.Width + value, size.Height + value);
        }

        public static Size Sub(Size size, int value)
        {
            return new Size(size.Width - value, size.Height - value);
        }
    }
}
//ProcessStartInfo SDPSInfo = new ProcessStartInfo();
//Process SDProc;
//SDPSInfo.FileName = "ipconfig";
//SDPSInfo.Arguments = "/release";
//SDPSInfo.WindowStyle = ProcessWindowStyle.Hidden;
//SDProc = Process.Start(SDPSInfo);
//SDProc.WaitForExit();
//SDPSInfo.FileName = "ipconfig";
//SDPSInfo.Arguments = "/renew";
//SDPSInfo.WindowStyle = ProcessWindowStyle.Hidden;
//SDProc = Process.Start(SDPSInfo);
//SDProc.WaitForExit();
