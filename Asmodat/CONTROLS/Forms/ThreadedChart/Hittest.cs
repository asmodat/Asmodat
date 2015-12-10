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

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {

        private DataPoint HitTestSortedDataPoints(double X)
        {
            DataPoint point = null;
            DataPoint[] points = null;

            try
            {
                points = ChartMain.Series[0].Points.ToArray();
            }
            catch { }

            if (Double.IsNaN(X) || points == null)
                return null;

            int f = 0;
            int m = points.Length / 2;
            int l = points.Length - 1;
            double mX;

            while(f < l)
            {
                m = (f + l) / 2;
                point = points[m];
                mX = point.XValue;

                if (X == point.XValue)
                    return point;
                else
                {
                    if (X > mX)
                        f = m + 1;

                    if (X < mX)
                        l = m - 1;
                }

                if(Math.Abs(f - l) == 1)
                {
                    if (Math.Abs(points[f].XValue - X) < Math.Abs(points[l].XValue - X))
                        return points[f];
                    else return points[l];
                }
            }

            return point;
        }

        

    }
}

//private DataPoint HitTestDataPoints(double X)
//{


//    DataPoint lastPoint = null;
//    double lastDistance = double.MaxValue;

//    DataPoint[] points = ChartMain.Series[0].Points.ToArray();

//    int i = 0;
//    for (; i < points.Length; i++)
//    {
//        DataPoint point = points[i];
//        double distance = Math.Abs(point.XValue - X);

//        if (lastDistance < distance)
//            return lastPoint;
//        else if (lastDistance >= distance)
//        {
//            lastDistance = distance;
//            lastPoint = point;
//        }
//    }

//    return lastPoint;
//}

///// <summary>
///// Gets the ChartArea that the mouse points
///// </summary>
///// <param name="source"></param>
///// <param name="e"></param>
///// <returns></returns>
//private ChartArea MouseInChartArea(Chart source, Point e)
//{
//    double relativeX = (double)e.X * 100 / source.Width;
//    double relativeY = (double)e.Y * 100 / source.Height;

//    foreach(ChartArea ca in source.ChartAreas)
//    {
//        if (relativeX > ca.Position.X && relativeX < ca.Position.Right && 
//            relativeY > ca.Position.Y && relativeY < ca.Position.Bottom)
//            return ca;
//    }

//    return null;
//}

//private void Find(Chart source, Point e)
//{
//    ChartArea currentArea = this.MouseInChartArea(source, e);
//    if (currentArea == null) return;

//   source.data

//}