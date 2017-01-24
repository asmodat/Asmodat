using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;

namespace Asmodat.IO
{
    /// <summary>
    /// This class can aquire and maintin access to the file
    /// </summary>
    public partial class File : IDisposable
    {

        //Directory.GetCurrentDirectory()
        public static string AppDirectory
        {
            get
            {
                return System.IO.Directory.GetCurrentDirectory() + @"\Asmodat.IO\Data";
            }
        }


        public void Dispose()
        {
            FullPath = null;
            FullDirectory = null;
            if (Stream != null)
                this.Close();
        }

        private ThreadedLocker Locker = new ThreadedLocker(10);
        public string FullDirectory { get; private set; }
        public string FullPath { get; private set; }
        public bool GZip { get; private set; }

        private System.IO.FileStream _Stream;
        public System.IO.FileStream Stream { get { return _Stream; } private set { _Stream = value; } }


        public File(string file, bool gzip)
        {
            string directory = System.IO.Path.GetDirectoryName(file);

            if (System.String.IsNullOrEmpty(directory))
                FullDirectory = File.AppDirectory;
            else this.FullDirectory = directory;

            string name = System.IO.Path.GetFileName(file);
            if(!name.Contains("."))
                name += ".aio";

            FullPath = System.IO.Path.Combine(FullDirectory, name);
            this.GZip = gzip;

            if (!System.IO.Directory.Exists(this.FullDirectory))
                System.IO.Directory.CreateDirectory(this.FullDirectory);

            //Opens or creates File
            Stream = System.IO.File.Open(this.FullPath, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
        }

        public void Close()
        {
            lock (Locker.Get("IO"))
            {
                Stream.Close();
            }
        }

        public void Save(string data)
        {
            lock(Locker.Get("IO"))
            {
                Asmodat.IO.File.Save(ref _Stream, data, GZip);
            }
        }

        public string Load()
        {
            lock (Locker.Get("IO"))
            {
                return Asmodat.IO.File.Load(ref _Stream, GZip);
            }
        }
    }
}

/*
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
*/