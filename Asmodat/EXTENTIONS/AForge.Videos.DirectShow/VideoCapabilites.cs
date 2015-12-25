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

namespace Asmodat.Extentions.AForge.Videos.DirectShow
{
    public static partial class VideoCapabilitesEx
    {
        public static VideoCapabilities SelectBestCapability(this VideoCapabilities[] capabiliteies)
        {
            if (capabiliteies.IsNullOrEmpty())
                return null;

            decimal framerate = 0;
            decimal area = 0;
            int index = -1;
            for (int i = 0; i < capabiliteies.Length; i++)
            {
                decimal f = capabiliteies[i].MaximumFrameRate;
                decimal a = capabiliteies[i].FrameSize.Area();

                if (f < framerate || a < area)
                    continue;

                framerate = f;
                area = a;
                index = i;
            }

            if (index < 0)
                return null;
            else
                return capabiliteies[index];
        }
    }
}
