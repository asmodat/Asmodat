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

using Asmodat.Types;

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {

        /// <summary>
        /// Gets or sets Series based on string key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Series this[string key]
        {
            get
            {
                return this.ChartMain.Series[key];
            }
            set
            {
                this.ChartMain.Series[key] = value;
            }
        }

        /// <summary>
        /// Gets or sets Chart Area based on intefer index
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ChartArea this[int index]
        {
            get
            {
                return this.ChartMain.ChartAreas[index];
            }
            set
            {
                this.ChartMain.ChartAreas[index] = value;
            }
        }


        private bool _Cursors = false;
        /// <summary>
        /// Enables or disables cursors on chart
        /// </summary>
        public bool Cursors 
        { 
            get
            {
                return _Cursors;
            }

            set
            {
                _Cursors = value;
                LblCursorX.Visible = value;
                LblCursorY.Visible = value;
            }
        }


        /// <summary>
        /// Returns plot location as 2D vector
        /// ----------End
        /// |           |
        /// |           |
        /// start--------
        /// </summary>
        public Vector2D PlotVector
        {
            get
            {
                Axis axisX = ChartMain.ChartAreas[0].AxisX;
                Axis axisY = ChartMain.ChartAreas[0].AxisY;

                if (Doubles.IsNaN(axisX.Minimum, axisY.Minimum, axisX.Maximum, axisY.Maximum))
                    return Vector2D.Default;

                Point2D Start = new Point2D(
                    axisX.ValueToPixelPosition(axisX.Minimum), 
                    axisY.ValueToPixelPosition(axisY.Minimum));

                Point2D End = new Point2D(
                    axisX.ValueToPixelPosition(axisX.Maximum),
                    axisY.ValueToPixelPosition(axisY.Maximum));

                return new Vector2D(Start, End);
            }
        }


        //ChartArea area = ;
        //Axis axisX = area.AxisX;
        //Axis axisY = area.AxisY;
        //AxisScaleView scaleX = axisX.ScaleView;
        //AxisScaleView scaleY = axisY.ScaleView;


        private ChartArea Area
        {
            get
            {
                return ChartMain.ChartAreas[0];
            }
        }

        private Axis AxisX
        {
            get
            {
                return Area.AxisX;
            }
        }
        private Axis AxisY
        {
            get
            {
                return Area.AxisY;
            }
        }

        private AxisScaleView ScaleX
        {
            get
            {
                return AxisX.ScaleView;
            }
        }
        private AxisScaleView ScaleY
        {
            get
            {
                return AxisY.ScaleView;
            }
        }


    }
}
