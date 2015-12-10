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
using Asmodat.Types;

using System.Threading;

using AsmodatMath;

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {

        private void _ChartMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (!Cursors) return;

            while (UpdateSpan < 100)
                Thread.Sleep(1);

            //cursor position schould not be changed while zooming, if mouse have not moved during this operation
            if (ZoomPointOrigin == ZoomPointNow)
                return;

                ChartMain.Invoke((MethodInvoker)(() =>
            {
                ChartArea area = ChartMain.ChartAreas[0];
                area.CursorX.SetCursorPixelPosition(MousePosition, false);
                area.CursorY.SetCursorPixelPosition(MousePosition, false);
                CursorPosition = new Point2D(area.CursorX.Position, area.CursorY.Position);
            }));
        }


        private void CursorsLabelsUpdate()
        {
            if (CursorPosition.IsInvalid || !Cursors)
                return;

            int XX = MousePosition.X - LblCursorX.Width / 2;
            int YY = MousePosition.Y - LblCursorY.Height / 2;

 

            ChartArea areas = ChartMain.ChartAreas[0];



            if (PlotVector.IsInvalid || !AMath.InArea(PlotVector, (Point2D)MousePosition))
                return;
            

            DateTime time = DateTime.FromOADate(CursorPosition.X);
            string sX = time.ToString("yyyy-MM-dd HH:mm:ss");
            string sY = System.String.Format("{0:N5}", CursorPosition.Y);

            ChartMain.Invoke((MethodInvoker)(() =>
            {
                if (sX != LblCursorX.Text)
                {
                    LblCursorX.Text = sX;
                    LblCursorX.Location = new Point(XX, LblCursorX.Location.Y);
                }

                if (sY != LblCursorY.Text)
                {
                    LblCursorY.Text = sY;
                    LblCursorY.Location = new Point(LblCursorY.Location.X, YY);
                }


                ChartMain.Update();
            }));
        }





        /// <summary>
        /// This property defines if cursor was moved, and position wasn't decoded by Peacemaker
        /// </summary>
        private bool CursorMoved { get; set; }


        private void PeacemakerLabels()
        {
            if (CursorPosition.IsInvalid || !Cursors)
                return;

            this.CursorsLabelsUpdate();
        }
        
        private void PeacemakerCursor()
        {

            if (!Collisions || !CursorMoved || UpdateSpan < 100 || MouseSpan < 1) return;

            DataPoint point = null;


            if (!CursorPosition.IsInvalid)
                 Threads.RunF<DataPoint>(() => this.HitTestSortedDataPoints(CursorPosition.X), "HitTestSortedDataPoints", true, true);



            point = Threads.JoinF<DataPoint>("HitTestSortedDataPoints");

            if (point != null)
                this.Collision = point;


            CursorMoved = false;
            
        }



        DataPoint _Collision = null;
        /// <summary>
        /// Last Collision of cursor with DataPoint
        /// </summary>
        public DataPoint Collision
        {
            get
            {
                return _Collision;
            }
            private set
            {
                _Collision = value;
            }
        }
    }
}

//if (PlotLocation.Invalid || !AMath.InArea(PlotLocation, (Point2D)MousePosition))
//            {
                

//                if (LblCursorX.Visible)
//                    Asmodat.Abbreviate.FormsControls.SetProperty(LblCursorX, "Visible", false, true);
//                if (LblCursorY.Visible)
//                    Asmodat.Abbreviate.FormsControls.SetProperty(LblCursorY, "Visible", false, true);
//                return;
//            }
//            else
//            {
//                if (!LblCursorX.Visible)
//                    Asmodat.Abbreviate.FormsControls.SetProperty(LblCursorX, "Visible", true, true);
//                if (!LblCursorY.Visible)
//                    Asmodat.Abbreviate.FormsControls.SetProperty(LblCursorY, "Visible", true, true);
//            }


//point = this.HitTestSortedDataPoints(CursorPosition.X);// Abbreviate.FormsControls.Invoke<DataPoint, Chart>(ChartMain, () => this.HitTestSortedDataPoints()); //

//HTResult = Abbreviate.FormsControls.Invoke<HitTestResult, ThreadedChart>(this, () => ChartMain.HitTest(MousePosition.X, MousePosition.Y, ChartElementType.DataPoint));




//// ChartMain.HitTest(,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,)

// Threads.Run(() => HitTest(), null, true, true);
// Threads.Join(() => HitTest());

// if (HTResult != null && HTResult.ChartElementType == ChartElementType.DataPoint && HTResult.PointIndex >= 0)
// {
//     this.Collision = ChartMain.Series[0].Points[HTResult.PointIndex];
//     //string text = "";
//     //DateTime time = DateTime.FromOADate(DPoint.XValue);
//     //text = Series[0].Name + "\n\n";
//     //text += "\n" + time.ToLongDateString() + "\n" + time.ToLongTimeString();
//     //text += "\n\n";
//     ////text += nametag + "\n";
//     //foreach (double value in DPoint.YValues)
//     //    text += value + "\n";
//     //ChartMain.Series[0].LegendText = text;
// }
