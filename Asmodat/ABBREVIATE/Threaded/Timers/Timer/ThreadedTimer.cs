using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Diagnostics;
using System.Reflection;

using System.Linq.Expressions;

using System.Windows.Forms;

using System.Collections.Concurrent;

using System.Runtime.CompilerServices;

namespace Asmodat.Abbreviate
{

    //Action CompiledAction = EAMethod.Compile();
    public partial class ThreadedTimer : IDisposable
    {
        public void Dispose()
        {
            this.Stop();
            if (this.Timer != null)
            {
                this.Timer.Dispose();
                this.Timer = null;
            }
        }

        /// <summary>
        /// This constructor creates instance of ThreadedTimer, that allows to create asynchronic timer
        /// </summary>
        /// <param name="EAMethod">Method that schould be run in timer</param>
        /// <param name="interval">Interval of timer (notice here, that if method is alive, it won't be started again).</param>
        /// <param name="TPriority">Priority of timed thread.</param>
        /// <param name="autostart">Specifies if timer should be started instantly.</param>
        public ThreadedTimer(Expression<Action> EAMethod, int interval, bool autostart)
        {
            
            this.Method = EAMethod.Compile();
            this.Interval = interval;
            //this.Invoked = invoked;

            if (autostart) 
                this.Start();
        }

        

        public bool IsBusy
        {
            get
            {
                return Monitor.IsEntered(Lock);
            }
        }

        

        /// <summary>
        /// Main elapsed event that executes threaded method.
        /// </summary>
        /// <param name="sender">Defoult object sender</param>
        /// <param name="e">Defauld ElapsedEventArgs</param>
        private void Peacemaker(object sender)
        {
            if (!_Enabled)
                return;

            if (Monitor.TryEnter(Lock))
            {
                try
                {
                    Method();
                }
                finally
                {
                    Monitor.Exit(Lock);
                }
            }
        }

        /// <summary>
        /// Enables timer and sets StartTime property
        /// </summary>
        /// <param name="startThread">Defines if thread sould be started instantly.</param>
        public void Start()
        {
            TimerCallback = new TimerCallback(Peacemaker);
            this.Timer = new System.Threading.Timer(TimerCallback, null, 0, this.Interval);
            _Enabled = true;
        }

        /// <summary>
        /// Stops Timer
        /// </summary>
        public void Stop()
        {
            _Enabled = false;

            if (this.Timer != null) this.Timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            if (this.TimerCallback != null) this.TimerCallback = null;
        }

        /// <summary>
        /// Terminates and Restarts current thread tithout stopping timer
        /// </summary>

        public void RestartThread()        
        {
            this.Timer.Change(0, this.Interval);
            _Enabled = true;
        }
    }
}//



/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

using System.Diagnostics;
using System.Reflection;

using System.Linq.Expressions;

using System.Windows.Forms;

using System.Collections.Concurrent;

using System.Runtime.CompilerServices;

namespace Asmodat.Abbreviate
{

    //Action CompiledAction = EAMethod.Compile();
    public class ThreadedTimer : IDisposable
    {
        public void Dispose()
        {
            if (Thread != null)
            {
                this.Stop(true);
                Thread = null;
                Timer = null;
            }
        }

        /// <summary>
        /// This constructor creates instance of ThreadedTimer, that allows to create asynchronic timer
        /// </summary>
        /// <param name="EAMethod">Method that schould be run in timer</param>
        /// <param name="interval">Interval of timer (notice here, that if method is alive, it won't be started again).</param>
        /// <param name="TPriority">Priority of timed thread.</param>
        /// <param name="autostart">Specifies if timer should be started instantly.</param>
        public ThreadedTimer(Expression<Action> EAMethod, int interval, ThreadPriority TPriority = ThreadPriority.Normal, bool autostart = false)
        {
            this.Timer = new System.Timers.Timer();

            this.Method = EAMethod.Compile();
            this.Priority = TPriority;
            this.Interval = interval;

            if (autostart) Start(false);

        }

        private Thread Thread { get; set; }
        public System.Timers.Timer Timer { get; set; }
        public Action Method { private get; set; }

        private double _Timeout = 0;
        /// <summary>
        /// This parameter can be set in order to stop timer after specified period of time in ms
        /// Precision of this field is bounded by Interval time.
        /// Timeout event wont be executed if ithis value is smaller or equal to 0
        /// </summary>
        public double Timeout 
        { 
            get
            {
                return _Timeout;
            }
            set
            {
                _Timeout = value;
                TimeoutTime = StartTime.AddMilliseconds(value);
            }
        }

        /// <summary>
        /// This property is used to calculate timeout
        /// </summary>
        public DateTime StartTime  { get; private set; }

        /// <summary>
        /// Gets or sets time of timer expirations.
        /// </summary>
        public DateTime TimeoutTime { get; set; }

        /// <summary>
        /// This field gets and sets Interval property of threaded timer.
        /// </summary>
        public double Interval
        {
            get
            {
                return Timer.Interval;
            }
            set
            {
                Timer.Interval = value;
            }
        }

        /// <summary>
        /// This property gets and sets Priotity of thread that runs method asynchonously inside timer event
        /// </summary>
        public ThreadPriority Priority { get; set; }

        /// <summary>
        /// This Field Enables or Disables timer
        /// </summary>
        public bool Enabled
        {
            get
            {
                return Timer.Enabled;
            }
            set
            {
                Timer.Enabled = value;
            }
        }

        /// <summary>
        /// Main elapsed event that executes threaded method.
        /// </summary>
        /// <param name="sender">Defoult object sender</param>
        /// <param name="e">Defauld ElapsedEventArgs</param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
                if (Timeout > 0 && DateTime.Now >= TimeoutTime)
                    this.Stop(true);

                if (Thread == null)
                {
                    Thread = new System.Threading.Thread(() => { Method(); });
                    lock (Thread)
                    {
                        Thread.Priority = this.Priority;
                        Thread.IsBackground = true;
                        Thread.Start();
                    }
                }
                else
                {
                    lock (Thread)
                    {
                        if(Thread.IsAlive) return;
                    

                        Thread = new System.Threading.Thread(() => { Method(); });
                        Thread.Priority = this.Priority;
                        Thread.IsBackground = true;
                        Thread.Start();
                    }
                }
                //Thread Thrd = new System.Threading.Thread(() => { Method(); });
                //Thrd.Priority = this.Priority;
                //Thread = Thrd;
        }

        /// <summary>
        /// Enables timer and sets StartTime property
        /// </summary>
        /// <param name="startThread">Defines if thread sould be started instantly.</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Start(bool startThread = false)
        {
            if (Timer != null) Timer.Stop();
            this.Timer = new System.Timers.Timer();
            this.Timer.Elapsed += Timer_Elapsed;
            Timer.Start();

            if (startThread)
                RestartThread();
        }

        /// <summary>
        /// Disables Timer
        /// </summary>
        /// <param name="stopThread">Defines if therad should be stopped instantly.</param>

        public void Stop(bool stopThread = false)
        {
            Timer.Stop();

            if (stopThread && Thread != null && Thread.IsAlive) lock (Thread) 
                Thread.Abort();

        }

        /// <summary>
        /// Terminates and Restarts current thread tithout stopping timer
        /// </summary>

        public void RestartThread()        
        {
            this.Stop(true);
            this.Start(false);
        }
    }
}//
*/