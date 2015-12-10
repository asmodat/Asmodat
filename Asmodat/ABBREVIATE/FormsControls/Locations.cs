using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Diagnostics;
using System.Drawing;

using System.Windows.Forms;

namespace Asmodat.Abbreviate
{
    public class Locations
    {
        public enum FitType
        {
            Center = 0
        }

        public static Point Fit(Control parent, Size control, FitType type = FitType.Center)
        {
            Point pp = parent.Location;
            Point pc = new Point();// control.Location;
            int wp = parent.Width;
            int hp = parent.Height;
            int wc = control.Width;
            int hc = control.Height;

            int dw = wp - wc;
            int dh = hp - hc;

            pc.X = dw / 2;
            pc.Y = dh / 2;
            return pc;
        }

        public static void Fit(Control parent, ref Control control, FitType type = FitType.Center)
        {
            control.Location = Locations.Fit(parent, control.Size, type);

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
