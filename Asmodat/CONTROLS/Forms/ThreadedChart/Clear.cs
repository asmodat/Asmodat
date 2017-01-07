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
using Asmodat.Extensions.Windows.Forms;

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {
        /// <summary>
        /// Clears all points and keeps series attributes
        /// </summary>
        public void ClearPoints()
        {
            Invoker.TryInvokeMethodAction(() =>
            {
                foreach (Series series in ChartMain.Series)
                    series.Points.Clear();

                Reset();
            });

            UpdateTime.SetNow();
        }

        /// <summary>
        /// Clears all series
        /// </summary>
        /// <param name="invoked"></param>
        public void ClearSeries()
        {

            Invoker.TryInvokeMethodAction(() => 
            {
                ChartMain.Series.Clear();
                Reset();
            });

            UpdateTime.SetNow();
        }



        public void Reset()
        {
            ScaleX.ZoomReset();
            ScaleY.ZoomReset();
            MinX = null;
            MaxX = null;
        }

    }
}
