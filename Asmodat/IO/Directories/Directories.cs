using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.IO;
using Asmodat.Debugging;
using Asmodat.Extensions.Collections.Generic;
using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;
using System.Diagnostics;

using SHDocVw;
using System.Web;
using System.Threading;

namespace Asmodat.IO
{
    public partial class Directories
    {
        /// <summary>
        /// Returns directory in format:
        /// Drive:\\Patch
        /// </summary>
        /// <returns></returns>
        public static string Current
        {
            get
            {
                try
                {
                    return Directory.GetCurrentDirectory();
                }
                catch (Exception ex)
                {
                    ex.ToOutput();
                    return null;
                }
            }
        }


        /// <summary>
        /// Format: X:\\
        /// </summary>
        public static string SystemDrive
        {
            get
            {
                try
                {
                    return Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
                }
                catch (Exception ex)
                {
                    ex.ToOutput();
                    return null;
                }
            }
        }

        /// <summary>
        /// Creates directory if it does not exists, 
        /// partial path format: @"folder\folder" == GetCurrent() + @"\folder\folder"
        /// full patch format: drive:\folder\folder
        /// </summary>
        /// <returns>returns true if success, else false</returns>
        public static DirectoryInfo Create(string path)
        {
            
            if (string.IsNullOrEmpty(path))
                return null;

            if(!path.Contains(":"))
                path = Directories.Current + "\\" + path;

            DirectoryInfo info = null;
            try
            {
                //if (Directory.Exists(path)) info = new DirectoryInfo(path); - not neaded

                info = System.IO.Directory.CreateDirectory(path);
               

            }
            catch
            {
                return null;
            }

            return info;
        }


        public static string CreateSimple(string path)
        {
            DirectoryInfo di = Directories.Create(path);
            if (di == null)
                return null;
            else return di.FullName;
        }

        public static string GetFullPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (!path.Contains(":"))
                path = Directories.Current + "\\" + path;

            return path;
        }

        /// <summary>
        /// Checks if given path is a root drive
        /// </summary>
        /// <param name="path"></param>
        /// <returns>null if can't say / invalid path, true if is root directory, false is has a parent</returns>
        public static bool? IsRoot(string path)
        {
            if (path.IsNullOrEmpty() || !Directory.Exists(path))
                return null;

            DirectoryInfo di = new DirectoryInfo(path);
            return di.Parent == null ? true : false;
        }

        public static string GetParent(string path)
        {
            return (path.IsNullOrEmpty() || !Directory.Exists(path)) ? null : new DirectoryInfo(path)?.Parent?.FullName;
        }


        public static string GetName(string folder)
        {
            if (folder.IsNullOrEmpty())
                return null;

            return Path.GetFileName(folder);
        }

        /// <summary>
        /// This method tries to close all oppened explorer instanes that contain folder path
        /// </summary>
        /// <param name="folder"></param>
        /// <returns>Returns false if error occured, or true if there were no explorer instances or instanses were closed</returns>
        public static bool TryCloseExplorerInstances(string folder)
        {
            bool success = false;
            try
            {
                string origin = new System.Uri(folder).AbsoluteUri.ToLower();
                ShellWindows sw = new SHDocVw.ShellWindows();
                foreach (InternetExplorer ie in sw)
                {
                    if (ie == null || ie.LocationURL.IsNullOrEmpty())
                        continue;

                    string location = new System.Uri(ie.LocationURL).AbsoluteUri.ToLower();
                    if (location.Contains(origin))//type.Equals("explorer") && 
                    {
                        ie.Quit();
                        Thread.Sleep(50);
                    }
                }

                success = true;
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                success = false;
            }

            return success;
        }

        public static bool TryMove(string sourceDirName, string destDirName, bool force = false)
        {
            if (sourceDirName.IsNullOrEmpty() || destDirName.IsNullOrEmpty() || !Directory.Exists(sourceDirName))
                return false;
            try
            {
                Directory.Move(sourceDirName, destDirName);
            }
            catch
            {
                if (force)
                    try
                    {
                        if (TryCloseExplorerInstances(sourceDirName))
                            Thread.Sleep(50);
                        Directory.Move(sourceDirName, destDirName);
                        return true;

                    }
                    catch (Exception ex)
                    {
                        ex.ToOutput();
                        return false;
                    }


                return false;
            }

            return true;
        }


        public static void TryEmpty(string path)
        {

            path = Directories.GetFullPath(path);

            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                return;

            DirectoryInfo info = new DirectoryInfo(path);

            foreach (FileInfo file in info.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch(Exception ex)
                {
                    Output.WriteException(ex);
                }
            }

            foreach (DirectoryInfo directory in info.GetDirectories())
            {
                Directories.TryEmpty(directory.FullName);

                try
                {
                    directory.Delete(false);
                }
                catch (Exception ex)
                {
                    Output.WriteException(ex);
                }
            }
        }

        


    }
}
