using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using Asmodat.Types;

using System.Threading;
using Asmodat.Extensions.Windows.Forms;

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {
        private Control _Invoker = null;
        public Control Invoker
        {
            get
            {
                if (_Invoker == null)
                    _Invoker = this.GetFirstParent();

                return _Invoker;
            }
        }

        private void Init()
        {
            this.ClearSeries();

            ChartArea area = ChartMain.ChartAreas[0];

            area.CursorX.LineDashStyle = ChartDashStyle.Dot;
            area.CursorY.LineDashStyle = ChartDashStyle.Dot;

            area.AxisX.TitleAlignment = StringAlignment.Center;
            area.AxisX.TextOrientation = TextOrientation.Horizontal;

            area.AxisY.TitleAlignment = StringAlignment.Center;
            area.AxisY.TextOrientation = TextOrientation.Horizontal;

            area.CursorX.Interval = 0.000000001;
            area.CursorY.Interval = 0.000000001;

            area.AxisX.ScrollBar.Enabled = false;
            area.AxisY.ScrollBar.Enabled = false;


            Cursors = true;
            Collisions = true;
            Cooridinates = false;
            CursorMoved = false; //Cursor des not mov on start, it can be set only by ChartMain_MouseMove event

            LblCursorX.UseCompatibleTextRendering = true;
            LblCursorY.UseCompatibleTextRendering = true;
            LblCursorX.Text = "";
            LblCursorY.Text = "";
        }
    }
}
