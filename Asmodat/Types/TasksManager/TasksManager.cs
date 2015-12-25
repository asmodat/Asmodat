using Asmodat.Abbreviate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Extensions.Objects;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Extensions;

namespace Asmodat.Types
{
    public class TasksManager : IDisposable
    {
        public void Dispose()
        {
            this.RemoveAll();
        }



        private readonly object locker = new object();
        ThreadedDictionary<string, TaskObject> Data = new ThreadedDictionary<string, TaskObject>();

        public bool IsFull
        {
            get
            {
                if (Data.Count >= MaxCount)
                    return true;
                else
                    return false;
            }
        }

        public int MaxCount { get; private set; }

        public TasksManager(int MaxCount = 128)
        {
            this.MaxCount = MaxCount.ToClosedInterval(1, 1024);
        }

        /// <summary>
        /// Removes tasks, that are done and not marked for rerun (Oneitis)
        /// </summary>
        public void Cleanup()
        {
            var keys = Data.KeysArray;
            if (keys.IsNullOrEmpty())
                return;

            foreach(string key in keys)
            {
                lock(locker)
                {
                    var task = Data.TryGetValue(key, null);
                    if (task == null || (!task.Oneitis && !task.IsRunning))
                        Data.Remove(key);
                }
            }
        }

        /// <summary>
        /// Creates a longrun task, and invokes action within
        /// </summary>
        /// <param name="method"></param>
        /// <param name="ID"></param>
        /// <param name="wait"></param>
        /// <returns></returns>
        public bool Run(Expression<Action> method, string ID, bool wait)
        {
            if (method == null)
                return false;

            if (ID.IsNullOrEmpty())
                ID = Expressions.nameofFull(method);



            if (ID.IsNullOrEmpty())
                return false;

            var task = Data.TryGetValue(ID, null);

            if (task != null && task.IsRunning)
            {
                if (wait)
                    task.Wait();
                else
                    return false;
            }


            this.Cleanup();

            if (this.IsFull)
                return false;

            lock (locker)
            {
                var task_new = Data.TryGetValue(ID, null);
                if (task_new != null && task_new.IsRunning)
                    return false;

                Action action = method.Compile();
                TaskObject taskObject = new TaskObject(action);
                
                Data.Add(ID, taskObject);

                Data[ID].Run();
                return true;
            }

        }


        public void JoinStop(string ID, int timeout_ms)
        {
            if (ID.IsNullOrEmpty())
                return;

            TaskObject task = null;

            lock (locker)
            {
                task = Data.TryGetValue(ID, null);
            }
            
            if (task != null)
                task.JoinStop(timeout_ms);
        }

        public void JoinStopAll(int timeout_ms)
        {
            if (Data == null)
                return;

            var keys = Data.KeysArray;
            if (keys.IsNullOrEmpty())
                return;

            foreach (string key in keys)
                this.JoinStop(key, timeout_ms);
        }

        public void CancellAll()
        {
            if (Data == null)
                return;

            var keys = Data.KeysArray;
            if (keys.IsNullOrEmpty())
                return;

            foreach (string key in keys)
                this.Cancell(key);
        }

        public bool Cancell(string ID)
        {
            if (ID.IsNullOrEmpty())
                return false;

            lock (locker)
            {
                var task = Data.TryGetValue(ID, null);
                if (task != null)
                {
                    task.Cancell();
                    return task.IsRunning;
                }
                else return false;
            }
        }

        public void KillAll()
        {
            if (Data == null)
                return;

            var keys = Data.KeysArray;
            if (keys.IsNullOrEmpty())
                return;

            foreach (string key in keys)
                this.Kill(key);
        }

        /// <summary>
        /// tries killing thread of the task, to terminate running, better use Remove
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Kill(string ID)
        {
            if (ID.IsNullOrEmpty())
                return false;

            lock (locker)
            {
                var task = Data.TryGetValue(ID, null);
                if (task != null)
                {
                    task.Kill();
                    return task.IsRunning;
                }
                else return false;
            }
        }

        public void RemoveAll()
        {
            if (Data == null)
                return;

            var keys = Data.KeysArray;
            if (keys.IsNullOrEmpty())
                return;

            foreach (string key in keys)
                this.Remove(key);
        }

        public bool Remove(string ID)
        {
            if (ID.IsNullOrEmpty())
                return false;

            lock (locker)
            {
                var task = Data.TryGetValue(ID, null);
                if (task != null)
                {
                    task.Dispose();

                    if (task == null || !task.IsRunning)
                        return Data.Remove(ID);

                    return false;
                }
                else return false;
            }
        }


    }
}
