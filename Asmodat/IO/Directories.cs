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

        public static List<string> GetFiles(string path, string searchPattern = "*.*", bool includeSubdirectories = true)
        {
            return includeSubdirectories ?
                 Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories).ToList<string>() :
                 Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly).ToList<string>();
        }

        public static List<string> GetDirectories(string path, string searchPattern = "*", bool includeSubdirectories = true)
        {
            return includeSubdirectories ?
                 Directory.GetDirectories(path, searchPattern, SearchOption.AllDirectories).ToList<string>() :
                 Directory.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly).ToList<string>();
        }

        public static ThreadedList<string> TryGetDirectories(ThreadedList<string> paths, string searchPattern, bool includeSubdirectories)
        {
            paths = paths.IsNullOrEmpty() == true ? new ThreadedList<string>() : paths;

            var _paths = paths.ToArray();

            ThreadedList<string> subPaths = new ThreadedList<string>();

            if (!_paths.IsNullOrEmpty())
                Parallel.ForEach(_paths, (path) =>
                {
                    if (!path.IsNullOrEmpty())
                        try
                        {
                            subPaths.AddRange(Directory.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly).ToList<string>());
                        }
                        catch
                        {

                        }
                });

            if (includeSubdirectories)
                paths.AddRange(TryGetDirectories((ThreadedList<string>)subPaths.Clone().ToList(), searchPattern, true));
            else
                paths.AddRange(subPaths);

            return paths;
        }

        public static List<string> TryGetDirectories(string path, string searchPattern, bool includeSubdirectories)
        {
            return TryGetDirectories(new ThreadedList<string>() { path }, searchPattern, includeSubdirectories);
        }


        public static List<string> TryGetFiles(string path, string searchPattern, bool includeSubdirectories)
        {
            var directories = TryGetDirectories(path, searchPattern, includeSubdirectories);
            ThreadedList<string> files = new ThreadedList<string>();

            Parallel.ForEach(directories, (directory) =>
            {
                if (!path.IsNullOrEmpty())
                    try
                    {
                        files.AddRange(Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly).ToList<string>());
                    }
                    catch
                    {

                    }
            });

            return files.ToList();
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
