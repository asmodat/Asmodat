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

using AsmodatMath;
using System.Threading;

using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {
        private void ChartMain_MouseMove(object sender, MouseEventArgs e)
        {
            MouseTime.SetNow();

            Point point = new Point(e.X, e.Y);
            if (MousePosition != point)
            {
                ZoomPointOrigin = new Point2D(0, 0); //This property must be zeroed to diffrent value then ZoomPointNow
                ZoomPointNow = new Point2D(0, 1); //This property must be zeroed to diffrent value then ZoomPointOrigin
                CursorMoved = true;
            }

            MousePosition = point;

            this.Threads.Run(() => _ChartMain_MouseMove(sender, e), null, false, true);


            MoveX(e);
        }

        /// <summary>
        /// This property is used to determine if mose was moved while zooming, so new zoom origin pont can be choosed
        /// </summary>
        private Point2D ZoomPointOrigin { get; set; } = new Point2D(0, 0);

        /// <summary>
        /// This property is usen in conjunction with ZoomPointOrigin to define wheter or not mouse was move wile zooming.
        /// </summary>
        private Point2D ZoomPointNow { get; set; } = new Point2D(0, 1);

        void ChartMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (ChartMain.Series.IsNullOrEmpty())
                return;

            ZoomPointNow = (Point2D)MousePosition;


            Area.RecalculateAxesScale();
            double position = ChartMain.ChartAreas[0].AxisX.ScaleView.Position;
            double xMin = ScaleX.ViewMinimum;
            double xMax = ScaleX.ViewMaximum;
            double size = xMax - xMin;// ChartMain.ChartAreas[0].AxisX.ScaleView.Size;

            //if (e.Location.X < 0 || e.Location.Y < 0)// || e.Location.X > 100 || e.Location.Y > 100)
             //   return;

            double cursor = 0;
            try
            {
                cursor = AxisX.PixelPositionToValue(e.Location.X);
            }
            catch { return; }

            double change = size / 3;

            double pXStart = double.NaN;
            double pXEnd = double.NaN;

            int locationX = e.Location.X;
            double positionValue = AxisX.PixelPositionToValue(locationX);

            //If mouse is not moved zoom toward origin cursor
            if (ZoomPointOrigin == ZoomPointNow)
                positionValue = CursorPosition.X;




            if (e.Delta < 0 && RescaledPointsCount < ChartMain.Series[0].Points.Count)
            {

                pXStart = positionValue - change * 3;
                pXEnd = positionValue + change * 3;

                ScaleX.Zoom(pXStart, pXEnd);

            }
            else if (e.Delta > 0 && RescaledPointsCount > 30)
            {

                pXStart = positionValue - change;
                pXEnd = positionValue + change;


            }

            if (!Doubles.IsNaN(pXEnd, pXStart))
            {

                if (pXStart < MinX.XValue)
                    pXStart = MinX.XValue;
                if (pXEnd > MaxX.XValue)
                    pXEnd = MaxX.XValue;


                ScaleX.Zoom(pXStart, pXEnd);
                this.RescaleY();
            }

            ZoomPointOrigin = (Point2D)MousePosition;
        }


        void ChartMain_MouseLeave(object sender, EventArgs e)
        {
            if (ChartMain.Focused)
                ChartMain.Parent.Focus();

            MouseVector.Reset();
        }

        void ChartMain_MouseEnter(object sender, EventArgs e)
        {
            if (!ChartMain.Focused)
                ChartMain.Focus();
        }

        void ChartMain_AxisScrollBarClicked(object sender, ScrollBarEventArgs e)
        {
            this.RescaleY();
        }

        void ChartMain_AxisViewChanged(object sender, ViewEventArgs e)
        {
            ChartMain.ChartAreas[0].RecalculateAxesScale();
            this.RescaleY();
        }

        void ChartMain_MouseDown(object sender, MouseEventArgs e)
        {
            MouseVector.Reset();
            MouseVector.Start = (Point2D)e.Location;
        }

        void ChartMain_MouseUp(object sender, MouseEventArgs e)
        {
            MouseVector.Reset();
        }



        /// <summary>
        /// Moves chart around depending on 
        /// </summary>
        /// <param name="e"></param>
        private void MoveX(MouseEventArgs e)
        {
            if (MouseVector.Start.IsInvalid) 
                return;

            MouseVector.End = (Point2D)e.Location;

            if (MouseVector.IsInvalid ||
                MouseVector.Min < 0 ||
                MouseVector.MaxX >= ChartMain.Width ||
                MouseVector.MaxY >= ChartMain.Height ||
                MouseVector.Width < 1
                ) 
                return;


            double position = ChartMain.ChartAreas[0].AxisX.ScaleView.Position;
            double size = ChartMain.ChartAreas[0].AxisX.ScaleView.Size;

            double x1 = AxisX.PixelPositionToValue(MouseVector.Start.X);
            double x2 = AxisX.PixelPositionToValue(MouseVector.End.X);
            double move = x2 - x1;

            double pXStart = position - move;
            double pXEnd = position + size;


            if (MinX == null || MaxX == null)
                return;

            if ((pXStart < MinX.XValue && move > 0) || (pXEnd > MaxX.XValue && move < 0))
            {
                MouseVector.Reset();
                return;
            }
            

            //ScaleX.Zoom(pXStart, pXEnd);

            ChartMain.ChartAreas[0].AxisX.ScaleView.Position = pXStart;
            //ChartMain.ChartAreas[0].AxisX.ScaleView.Size = pXEnd - pXStart;

            
            
            this.RescaleY();
            MouseVector.Start = MouseVector.End;
        }


      


        private Vector2D _MouseVector = new Vector2D();
        /// <summary>
        /// This vector describes value movement of mouse from mouse up to mouse dn
        /// </summary>
        public Vector2D MouseVector { get { return _MouseVector; } private set { _MouseVector = value; } }


        private double RescaledPointsCount { get; set; }
        private void RescaleY()
        {
            Area.RecalculateAxesScale();

            double start = ScaleX.ViewMinimum;
            double end = ScaleX.ViewMaximum;

            var points = ChartMain.Series[0].Points;
            double[] tempH = points.Where((x, i) => AMath.InArea(points[i].XValue, start, end)).Select(x => x.YValues.Max()).ToArray();
            double[] tempL = points.Where((x, i) => AMath.InArea(points[i].XValue, start, end)).Select(x => x.YValues.Min()).ToArray();

            if (tempL.Length <= 0)// && tempH.Length <= 0)
                return;

            RescaledPointsCount = tempL.Length;

            double ymin = tempL.Min();
            double ymax = tempH.Max();

            double scale = (ymax - ymin) * 0.05;

            ymin -= scale;
            ymax += scale;

            ChartMain.ChartAreas[0].AxisY.ScaleView.Position = ymin;
            ChartMain.ChartAreas[0].AxisY.ScaleView.Size = ymax - ymin;
        }

    }
}


//void ChartMain_MouseWheel(object sender, MouseEventArgs e)
//        {
//            if(e.Delta < 0)
//            {
//                ScaleX.ZoomReset();
//                ScaleY.ZoomReset();
//                RescaledPointsCount = double.MaxValue;
//            }

//            if (e.Delta > 0 && RescaledPointsCount > 30)
//            {
//                double xMin = ScaleX.ViewMinimum;
//                double xMax = ScaleX.ViewMaximum;

//                double pXStart = AxisX.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 3;
//                double pXEnd = AxisX.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 3;

//                ScaleX.Zoom(pXStart, pXEnd);
//            }

//            //ChartMain.ChartAreas[0].AxisX.ScaleView.Position = pXStart;
//            //ChartMain.ChartAreas[0].AxisX.ScaleView.Size = pXEnd - pXStart;
            
//            this.RescaleY();
//        }