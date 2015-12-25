using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Video;
using AForge.Video.DirectShow;
using AFV = AForge.Video;
using AFVDS = AForge.Video.DirectShow;
using Asmodat.Extensions.Objects;

namespace Asmodat.Extentions.AForge.Videos.DirectShow
{
    public static partial class FilterInfoCollectionEx
    {
        public static FilterInfo[] GetVideoInputDevices()
        {
            FilterInfoCollection FIC = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            return FIC.GetFilterInfos();
        }

        public static FilterInfo[] GetFilterInfos(this FilterInfoCollection fic)
        {
            if (fic == null)
                return null;

            List<FilterInfo> list = new List<FilterInfo>();

            foreach(var v in fic)
            {
                FilterInfo fi = v.TryCast<FilterInfo>();

                if (fi == null)
                    continue;
                else
                    list.Add(fi);
            }
            
            return list.ToArray();
        }
    }
}
