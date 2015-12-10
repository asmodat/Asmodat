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
        public enum AreaAxis
        {
            X = 1,
            X2 = 2,
            Y = 3,
            Y2 = 4,
        }

        public enum SeriesColors
        {
            Green,
            Red,
            black,
            White
        }

        /// <summary>
        /// , , , , , , , , 
        /// </summary>
        public enum SeriesAttributes
        {
            PriceUpColor,
            PriceDownColor,
            LabelValueType,
            MaxPixelPointWidth,
            MinPixelPointWidth,
            PixelPointDepth,
            PixelPointGapDepth,
            PixelPointWidth,
            PointWidth
        }



        public void SetCandlestickSettings<TInvoker>(
            TInvoker Invoker,
            string series,
            Color PriceUpColor,
            Color PriceDownColor,
            Color SticksColor,
            Color BorderColor
            ) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => SetCandlestickSettings<TInvoker>(null, series, PriceUpColor, PriceDownColor, SticksColor, BorderColor));
                UpdateTime.SetNow();
                return;
            }

            ChartMain.Series[series]["PriceUpColor"] = PriceUpColor.ToArgb().ToString();
            ChartMain.Series[series]["PriceDownColor"] = PriceDownColor.ToArgb().ToString();
            ChartMain.Series[series].Color = SticksColor;
            ChartMain.Series[series].BorderColor = BorderColor;
        }


        public void SetAreaAxisLabelStyle<TInvoker>(
            TInvoker Invoker,
            int index,
            AreaAxis aaxis,
            string Format = null
            ) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => SetAreaAxisLabelStyle<TInvoker>(null, index, aaxis, Format));
                UpdateTime.SetNow();
                return;
            }

            Axis axis = null;

            
            
                if (aaxis == AreaAxis.X)
                    axis = ChartMain.ChartAreas[index].AxisX;
                else if (aaxis == AreaAxis.Y)
                    axis = ChartMain.ChartAreas[index].AxisY;
                else if (aaxis == AreaAxis.X2)
                    axis = ChartMain.ChartAreas[index].AxisX2;
                else if (aaxis == AreaAxis.Y2)
                    axis = ChartMain.ChartAreas[index].AxisY2;

 
                if (Format != null)
                    axis.LabelStyle.Format = Format;
            
        }


        /// <summary>
        /// public void SetAreaAxis TInvoker>(
        /// TInvoker Invoker,
        /// int index, 
        /// AreaAxis aaxis, 
        /// string title = null,
        /// StringAlignment? TitleAlignment = null,
        /// TextOrientation? TxtOrientation = null,
        /// LabelStyle LblStyle = null
        ///  ) where TInvoker : Control
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="Invoker"></param>
        /// <param name="index"></param>
        /// <param name="aaxis"></param>
        /// <param name="title"></param>
        /// <param name="TitleAlignment"></param>
        /// <param name="TxtOrientation"></param>
        /// <param name="LblStyle"></param>
        public void SetAreaAxis<TInvoker>(
            TInvoker Invoker,
            int index, 
            AreaAxis aaxis, 
            string title = null,
            StringAlignment? TitleAlignment = null,
            TextOrientation? TxtOrientation = null,
            LabelStyle LblStyle = null
            ) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => SetAreaAxis<TInvoker>(null, index, aaxis, title, TitleAlignment, TxtOrientation, LblStyle));
                UpdateTime.SetNow();
                return;
            }

            Axis axis = null;

            if (aaxis == AreaAxis.X)
                axis = ChartMain.ChartAreas[index].AxisX;
            else if (aaxis == AreaAxis.Y)
                axis = ChartMain.ChartAreas[index].AxisY;
            else if (aaxis == AreaAxis.X2)
                axis = ChartMain.ChartAreas[index].AxisX2;
            else if (aaxis == AreaAxis.Y2)
                axis = ChartMain.ChartAreas[index].AxisY2;

         
                if (title != null)
                    axis.Title = title;
                if (TitleAlignment != null)
                    axis.TitleAlignment = (StringAlignment)TitleAlignment;
                if (TxtOrientation != null)
                    axis.TextOrientation = (TextOrientation)TxtOrientation;
                if (LblStyle != null)
                    axis.LabelStyle = LblStyle;
            
        }

        /// <summary>
        /// public void SetSeriesX TInvoker>(string title, 
        /// ChartValueType? cvtype = null, 
        /// AxisType? atype = null, 
        /// string ValueMeber = null, 
        /// TInvoker Invoker = null) where TInvoker : Control
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="title"></param>
        /// <param name="cvtype"></param>
        /// <param name="atype"></param>
        /// <param name="ValueMeber"></param>
        /// <param name="Invoker"></param>
        public void SetSeriesX<TInvoker>(string title, 
            ChartValueType? cvtype = null, 
            AxisType? atype = null, 
            string ValueMeber = null, 
            TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => SetSeriesX<TInvoker>(title, cvtype, atype, ValueMeber, null));
                UpdateTime.SetNow();
                return;
            }

           
                if (cvtype != null)
                    ChartMain.Series[title].XValueType = (ChartValueType)cvtype;
                if (ValueMeber != null)
                    ChartMain.Series[title].XValueMember = ValueMeber;
                if (atype != null)
                    ChartMain.Series[title].XAxisType = (AxisType)atype;
            
        }

        /// <summary>
        /// public void SetSeriesY TInvoker>(
        ///    string title,
        ///     ChartValueType? cvtype = null,
        ///     AxisType? atype = null,
        ///     string ValueMebers = null,
        ///     int? ValuesPerPoint = null,
        ///     TInvoker Invoker = null) where TInvoker : Control
        /// </summary>
        /// <typeparam name="TInvoker"></typeparam>
        /// <param name="title"></param>
        /// <param name="cvtype"></param>
        /// <param name="atype"></param>
        /// <param name="ValueMebers"></param>
        /// <param name="ValuesPerPoint"></param>
        /// <param name="Invoker"></param>
        public void SetSeriesY<TInvoker>(
            string title,
            ChartValueType? cvtype = null,
            AxisType? atype = null,
            string ValueMebers = null,
            int? ValuesPerPoint = null,
            TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => SetSeriesY<TInvoker>(title, cvtype, atype, ValueMebers, ValuesPerPoint, null));
                UpdateTime.SetNow();
                return;
            }

            
                if (cvtype != null)
                    ChartMain.Series[title].YValueType = (ChartValueType)cvtype;
                if (ValueMebers != null)
                    ChartMain.Series[title].YValueMembers = ValueMebers;
                if (atype != null)
                    ChartMain.Series[title].YAxisType = (AxisType)atype;
                if (ValuesPerPoint != null)
                    ChartMain.Series[title].YValuesPerPoint = (int)ValuesPerPoint;
            

        }






        public void SetSeries<TInvoker>(string name, Series value, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => SetSeries<TInvoker>(name, value, null));
                UpdateTime.SetNow();
                return;
            }

            ChartMain.Series[name] = value;
        }

        public void SetArea<TInvoker>(int index, ChartArea value, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => SetArea<TInvoker>(index, value, null));
                UpdateTime.SetNow();
                return;
            }

            ChartMain.ChartAreas[index] = value;
        }

        public void SetAreaAxisX<TInvoker>(int index, Axis value, TInvoker Invoker = null) where TInvoker : Control
        {
            if (Invoker != null)
            {
                //lock (Locker[ChartMain])
                Abbreviate.FormsControls.Invoke(Invoker, () => SetAreaAxisX<TInvoker>(index, value, null));
                UpdateTime.SetNow();
                return;
            }

            ChartMain.ChartAreas[index].AxisX = value;
        }






        
    }
}
