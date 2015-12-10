using Asmodat.Debugging;
using Asmodat.Extensions.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Asmodat.Types
{
    public static class TaskObjectEx
    {
        
    }

    public class TaskObject : IDisposable
    {
        public void Dispose()
        {
            this.Cancell();
            //this.Kill();
        }

        public ExceptionBuffer Exceptions { get; private set; }

        public Action Action { get; private set; } = null;

        public Thread Thread { get; private set; } = null;

        public Task Task { get; private set; } = null;

        /// <summary>
        /// Creation time of Task Object
        /// </summary>
        public TickTime CreationTime { get; private set; } = TickTime.Default;

        public CancellationTokenSource TokenSource { get; private set; } = null;

        public CancellationToken Token { get; private set; }

        public System.Threading.Tasks.TaskCreationOptions CancellationOptions { get; private set; }

        public TaskScheduler Scheduler { get; private set; }

        /// <summary>
        /// Defines if object can be rerun (true), or can be removed after it's done
        /// </summary>
        public bool Oneitis { get; set; } = false;

        public TaskObject(Action Action)
        {
            Exceptions = new ExceptionBuffer(8);

            this.Action = Action;
            CreationTime.SetNow();
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;
            CancellationOptions = TaskCreationOptions.None; 
            //CancellationOptions = TaskCreationOptions.DenyChildAttach;
            Scheduler = TaskScheduler.Default;

          
        }

        public void Cancell()
        {
            if (TokenSource != null && !TokenSource.IsCancellationRequested)
                TokenSource.Cancel();
        }

        public void Kill()
        {
            this.Thread.KillInstantly();
        }

        public void Wait()
        {
            if (this.IsRunning)
                this.Task.Wait();
        }

        public decimal JoinStop(int timeout_ms)
        {
            TickTimeout timeout = new TickTimeout(timeout_ms, TickTime.Unit.ms);

            while (!this.Stopped && !timeout.IsTriggered)
                Thread.Sleep(1);

            return timeout.Span;
        }

        public bool Started { get; private set; } = false;
        public bool Stopped { get; private set; } = false;

        public bool Run()
        {
            this.Started = false;
            this.Stopped = false;

            if (Action == null)
                return false;

           

            this.Task = Task.Factory.StartNew(
                () => 
                {
                    try
                    {
                        this.Started = true;
                        this.Thread = Thread.CurrentThread;

                        try
                        {
                            if (Token != null)
                                Token.ThrowIfCancellationRequested();

                            using (Token.Register(Thread.CurrentThread.Abort))
                            {
                                Action.Invoke();
                            }
                        }
                        catch (TaskCanceledException ex_tc)
                        {
                            Exceptions.Write(ex_tc);
                        }
                    }
                    finally
                    {
                        this.Stopped = true;
                    }
                    
                }, Token, CancellationOptions, Scheduler);

            return true;
        }

        public bool IsRunning
        {
            get
            {
                if (this.Task == null || this.Task == null)
                    return false;

                if (this.Task.IsCompleted)
                    return false;

                return true;
            }
        }
    }
}
