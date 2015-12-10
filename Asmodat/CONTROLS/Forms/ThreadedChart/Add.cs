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

using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {
        /// <summary>
        /// Adds new series
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="Invoker"></param>
        public void Add(string name, SeriesChartType type)
        {
            Series series = new Series();
            series.Name = name;
            series.ChartType = type;

            Invoker.Invoke((MethodInvoker)(() =>
            {
                ChartMain.Series.Add(series);
            }));


            UpdateTime.SetNow();
        }

        /// <summary>
        /// adds new data point to series specified by name
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="name"></param>
        /// <param name="point"></param>
        /// <param name="Invoker"></param>
        public void Add(string name, DataPoint point)
        {
            Invoker.Invoke((MethodInvoker)(() =>
            {
                ChartMain.Series[name].Points.Add(point);
            }));

            UpdateTime.SetNow();
        }

        /// <summary>
        /// adds range of new data points to series specified by name
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="name"></param>
        /// <param name="points"></param>
        /// <param name="Invoker"></param>
        public void AddRange(string name, DataPoint[] points, ChartValueType? XValueType = null, ChartValueType? YValueType = null)
        {

            if (points.Length <= 0)
                return;



            Invoker.Invoke((MethodInvoker)(() =>
            {

                double xMin = ScaleX.ViewMinimum;
                double xMax = ScaleX.ViewMaximum;


                foreach (DataPoint point in points)
                {
                    ChartMain.Series[name].Points.Add(point);


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
            }));

            UpdateTime.SetNow();
        }

        public DataPoint MinX { get; private set; }
        public DataPoint MaxX { get; private set; }
    }
}
