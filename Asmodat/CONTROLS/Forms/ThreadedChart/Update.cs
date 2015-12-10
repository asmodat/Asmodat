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


        public void UpdateRange<TInvoker>(TInvoker Invoker, string name, DataPoint[] points, ChartValueType? XValueType = null, ChartValueType? YValueType = null) where TInvoker : Control
        {

            if (points.Length <= 0)
                return;

            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => UpdateRange<TInvoker>(null, name, points, XValueType, YValueType));
                UpdateTime.SetNow();
                return;
            }



            double xMin = ScaleX.ViewMinimum;
            double xMax = ScaleX.ViewMaximum;

            bool found = false;
            foreach (DataPoint point in points)
            {
                found = false;
                for (int i = ChartMain.Series[name].Points.Count - 1; i >= 0; i--)
                    if (ChartMain.Series[name].Points[i].XValue == point.XValue)
                    {
                        ChartMain.Series[name].Points[i] = point;
                        found = true;
                        break;
                    }
                    //else if (ChartMain.Series[name].Points[i].XValue < point.XValue) break;

                if (!found) ChartMain.Series[name].Points.Add(point);


                if (MinX == null || MinX.XValue > point.XValue)
                    MinX = point;
                if (MaxX == null || MaxX.XValue < point.XValue)
                    MaxX = point;
            }

            if (XValueType != null)
                ChartMain.Series[name].XValueType = (ChartValueType)XValueType;
            if (YValueType != null)
                ChartMain.Series[name].YValueType = (ChartValueType)YValueType;

            Area.AxisX.IsStartedFromZero = false;
            Area.AxisY.IsStartedFromZero = false;
            Area.RecalculateAxesScale();
            Area.RecalculateAxesScale();


            DataPoint last = ChartMain.Series[name].Points.Last();
            if (!Doubles.IsNaN(xMax, xMin) && last.XValue > xMax)
            {
                double change = (xMax - last.XValue);
                ScaleX.Zoom(xMin + change, last.XValue);
            }


            this.RescaleY();
        }
    }
}
