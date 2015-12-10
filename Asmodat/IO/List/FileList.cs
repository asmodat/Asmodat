using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Asmodat.Abbreviate;
using Asmodat.Types;

using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Asmodat.IO
{
    public partial class FileList<TValue> : IDisposable
    {

        public void Delete()
        {
            Data = new List<TValue>();
            if (System.IO.File.Exists(FullPath))
                System.IO.File.Delete(FullPath);
        }

        public void Reset()
        {
            Data = new List<TValue>();
            this.Save();
        }

        public void Dispose()
        {
            this.PaceMaker();
        }

        public FileList(string FullPath, int SaveInterval = int.MaxValue)
        {
            Data = new List<TValue>();
            this.SaveInterval = SaveInterval;
            this.FullPath = FullPath;
            this.FullDirectory = Path.GetDirectoryName(FullPath);

            this.CheckPermissions();
            this.LoadData();

            if (SaveInterval > 0 && SaveInterval < int.MaxValue)
                Timers.Run(() => this.PaceMaker(), SaveInterval, null, true, true);

        }

        public void CheckPermissions()
        {
            lock (Locker.Get("IO"))
            {
                if (!Directory.Exists(FullDirectory))
                    Directory.CreateDirectory(FullDirectory);

                if (!System.IO.File.Exists(FullPath))
                    {
                        FileStream Stream = System.IO.File.Create(FullPath);
                        Stream.Close();
                    }


                FileIOPermission Permissions = new FileIOPermission(FileIOPermissionAccess.Read, FullPath);
                Permissions.AddPathList(FileIOPermissionAccess.Write | FileIOPermissionAccess.Read, FullPath);
                try
                {
                    Permissions.Demand();
                }
                catch (Exception e)
                {
                    throw new Exception("Cannot access database ! " + e.Message);
                }
            }
        }

        public void Add(TValue value, bool save = false)
        {
            lock (Locker.Get("Data"))
            {
                Data.Add(value);
                this.UpdateTime = DateTime.Now;
            }

            if (save)
                this.SaveData();
        }

        public void Remove(TValue value, bool save = false)
        {
            lock (Locker.Get("Data"))
            {
                if (Data.Contains(value))
                    Data.Remove(value);

                this.UpdateTime = DateTime.Now;
            }

            if (save)
                this.SaveData();
        }

        



        private void PaceMaker()
        {
            if (this.IsSaved)
                return;

            CheckPermissions();
            this.SaveData();
        }

        

    }
}
