using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



using Asmodat.Abbreviate;
using Asmodat.Types;

using Asmodat.Networking;
using System.ServiceModel.Dispatcher;
using mshtml;
using Asmodat.Debugging;

using System.Threading;
using System.Drawing.Imaging;
using Asmodat.IO;
using Asmodat;

namespace Asmodat.Imaging
{
    public partial class VisualLocator
    {
        public VisualLocator(Point2D location, Size size, double certainity, string path)
        {
            Location = location;
            Size = size;
            Certainity = certainity;
            Path = path;
        }

        public VisualLocator(string path)
        {
            Path = path;
            Certainity = 0;
        }

        public string Path { get; private set; }

        public Point2D Location { get; private set;  }
        public Size Size { get; private set; }
        public double Certainity { get; private set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((Point)Location, Size);
            }
        }

        public Point2D Center
        {
            get
            {
                return ((Vector2D)this.Rectangle).Center;
            }
        }

        /// <summary>
        /// 0,0 is top left
        /// </summary>
        /// <param name="xPercentage"></param>
        /// <param name="yPercentage"></param>
        /// <returns></returns>
        public Point2D Locate(double xPercentage, double yPercentage)
        {
            double w = (double)this.Rectangle.Width * (xPercentage / 100);
            double h = (double)this.Rectangle.Height * (yPercentage / 100);
            Point2D p = this.Location;
            p.Move(w, h);

            return p;
        }
    }
}
