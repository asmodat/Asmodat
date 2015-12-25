using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using  AForge.Video;
using  AForge.Video.DirectShow;
using AFV = AForge.Video;
using AFVDS = AForge.Video.DirectShow;

using Asmodat.Debugging;

namespace Asmodat.Extentions.AForge.Videos.DirectShow
{
    public static partial class VideoCaptureDeviceEx
    {
        public static bool IsRunning(this VideoCaptureDevice vcd)
        {
            try
            {
                if (vcd == null)
                    return false;
                else
                    return vcd.IsRunning;
            }
            catch(Exception ex)
            {
                ex.WriteToExcpetionBuffer();
                return false;
            }
        }
    }
}
