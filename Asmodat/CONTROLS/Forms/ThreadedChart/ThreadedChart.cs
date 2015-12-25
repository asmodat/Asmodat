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

using System.Threading;

namespace Asmodat.FormsControls
{
    public partial class ThreadedChart : UserControl
    {
        ThreadedTimers Timers = new ThreadedTimers(10);
        ThreadedMethod Threads = new ThreadedMethod(100, System.Threading.ThreadPriority.Lowest, 10);
        ThreadedLocker Locker = new ThreadedLocker(100);
        ThreadedThreeStates States = new ThreadedThreeStates(100);

        public string ID { get; set; }


        public ThreadedChart()
        {
            InitializeComponent();
        }

        private bool _Started = false;
        public bool Started { get { return _Started; } private set { _Started = value; } }
        public void Start()
        {
            this.Init();

            ChartMain.MouseMove += ChartMain_MouseMove;
            ChartMain.MouseWheel += ChartMain_MouseWheel;
            ChartMain.MouseEnter += ChartMain_MouseEnter;
            ChartMain.MouseLeave += ChartMain_MouseLeave;
            ChartMain.AxisViewChanged += ChartMain_AxisViewChanged;
            ChartMain.AxisScrollBarClicked += ChartMain_AxisScrollBarClicked;
            ChartMain.MouseDown += ChartMain_MouseDown;
            ChartMain.MouseUp += ChartMain_MouseUp;

            Timers.Run(() => this.PeacemakerCursor(), 10, 1000, null, true);//
            Timers.Run(() => this.PeacemakerLabels(), 100, 1000, null, true);

            Started = true;
        }

        private TickTime MouseTime = TickTime.MinValue;
        /// <summary>
        /// Returns time between last move of the mouse cursor
        /// </summary>
        public double MouseSpan
        {
            get
            {
                return ((DateTime)TickTime.Now - (DateTime)MouseTime).TotalMilliseconds;
            }
        }

        private TickTime UpdateTime = TickTime.MinValue;
        /// <summary>
        /// Returns time between last update of chart
        /// </summary>
        public double UpdateSpan
        {
            get
            {
                return ((DateTime)TickTime.Now - (DateTime)UpdateTime).TotalMilliseconds;
            }
        }


        public new Point MousePosition
        {
            get;
            private set;
        }

        public Point2D CursorPosition { get; private set; }



        /// <summary>
        /// Enables or disables displaying cursor coordinates
        /// </summary>
        public bool Cooridinates { get; set; }

        /// <summary>
        /// Displays cursor collisions
        /// </summary>
        public bool Collisions { get; set; }

        
        public SeriesCollection Series
        {
            get
            {
                lock (Locker[ChartMain])
                return ChartMain.Series;
            }
        }

    }
}
//DataPoint DPoint = new DataPoint();
//            DPoint.SetValueXY(time, high, low, open, close);
//            ChartMain.Series[name].Points.Add(DPoint);
//            ChartMain.Series[name].Points.AddXY(time, high, low, open, close);




