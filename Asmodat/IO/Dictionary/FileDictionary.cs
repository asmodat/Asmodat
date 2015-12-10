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
    public partial class FileDictionary<TKey, TValue> : IDisposable
    {
        public void Dispose()
        {
            this.PaceMaker();
        }

        public FileDictionary(string FullPath, int SaveInterval = 0)
        {
            Data = new ThreadedDictionary<TKey, TValue>();
            this.SaveInterval = SaveInterval;
            this.FullPath = FullPath;
            this.FullDirectory = Path.GetDirectoryName(FullPath);

            this.CheckPermissions();
            this.Load();

            if (SaveInterval > 0 && SaveInterval <= 0)
                Timers.Run(() => this.PaceMaker(), SaveInterval, null, true, true);

        }

        public void Delete()
        {
            Data = new ThreadedDictionary<TKey, TValue>();
            if (System.IO.File.Exists(FullPath))
                System.IO.File.Delete(FullPath);
        }


        public void Reset()
        {
            Data = new ThreadedDictionary<TKey, TValue>();
            this.Save();
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

        public void Add(TKey key, TValue value, bool save = false, bool update = true)
        {
            
                Data.Add(key, value);


            if (save)
                this.PaceMaker();
        }

        public TValue Get(TKey key)
        {
            if (Data.ContainsKey(key))
                return Data[key];

            throw new Exception("Key does not exists.");
        }


        private void PaceMaker()
        {
            if (this.IsSaved) 
                return;

            CheckPermissions();
            this.Save();
        }

        

    }
}
