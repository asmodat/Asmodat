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

        public SeriesCollection SeriesCollection
        {
            get
            { return this.ChartMain.Series;
            }
        }

        public ChartAreaCollection ChartAreaCollection
        {
            get
            {
                return this.ChartMain.ChartAreas;
            }
        }

        public int CountPoints(string name)
        {
            int result = 0;
            Invoker.Invoke((MethodInvoker)(() =>
            {
                if (this.SeriesContainsName(name))
                    result = ChartMain.Series[name].Points.Count;
            }));

           

            return result;
        }

        public bool SeriesContainsName(string name)
        {
            if (name == null || ChartMain == null || ChartMain.Series == null)
                return false;

            foreach (var s in ChartMain.Series)
                if (s.Name == name)
                    return true;

            return false;
        }



        public Series GetSeries<TInvoker>(string name, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<Series, TInvoker>(Invoker, () => GetSeries<TInvoker>(name, null));

      
            return ChartMain.Series[name];
        }

        public ChartArea GetChartArea<TInvoker>(int index, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<ChartArea, TInvoker>(Invoker, () => GetChartArea<TInvoker>(index, null));

            return ChartMain.ChartAreas[index];
        }


        public Axis GetAreaAxisX<TInvoker>(int index, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<Axis, TInvoker>(Invoker, () => GetAreaAxisX<TInvoker>(index, null));

            return Objects.CloneJson(ChartMain.ChartAreas[index].AxisX, Newtonsoft.Json.ReferenceLoopHandling.Ignore);//ChartMain.ChartAreas[index].AxisX;
        }



        public ThreadedChart Clone<TInvoker>(TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
                lock (Locker[ChartMain])
                return Abbreviate.FormsControls.Invoke<ThreadedChart, TInvoker>(Invoker, () => Clone<TInvoker>(null));


            ThreadedChart copy = Objects.CloneJson(this, Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //ThreadedChart copy = (ThreadedChart)this.MemberwiseClone();
            return copy;
        }
        

    }
}
