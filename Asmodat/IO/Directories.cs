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

        public static string[] GetFiles(string path, string searchPattern = "*.*", bool includeSubdirectories = true)
        {
            return includeSubdirectories ?
                 Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories):
                 Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] GetDirectories(string path, string searchPattern = "*", bool includeSubdirectories = true)
        {
            return includeSubdirectories ?
                 Directory.GetDirectories(path, searchPattern, SearchOption.AllDirectories) :
                 Directory.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
        }

        public static string[] TryGetDirectories(ThreadedList<string> paths, string searchPattern = "*", bool includeSubdirectories = true)
        {
            if (paths.IsNullOrEmpty())
                return new string[0];

            var _paths = paths.ToArray();

            ThreadedList<string> subPaths = new ThreadedList<string>();

            if (!_paths.IsNullOrEmpty())
                Parallel.ForEach(_paths, (path) =>
                {
                    if (!path.IsNullOrEmpty())
                        try
                        {
                            subPaths.AddRange(Directory.GetDirectories(path, searchPattern, SearchOption.TopDirectoryOnly));
                        }
                        catch
                        {

                        }
                });

            if (includeSubdirectories)
                paths.AddRange(TryGetDirectories(subPaths.Clone(), searchPattern, true));
            else
                paths.AddRange(subPaths);

            return paths.ToArray();
        }

        public static string[] TryGetDirectories(string path, string searchPattern = "*", bool includeSubdirectories = true)
        {
            return TryGetDirectories(new ThreadedList<string>() { path }, searchPattern, includeSubdirectories);
        }


        public static string[] TryGetFiles(string path, string directorySearchPattern = "*", string fileSearchPattern = "*", bool includeSubdirectories = true)
        {

            var directories = TryGetDirectories(path, directorySearchPattern, includeSubdirectories);
            ThreadedList<string> files = new ThreadedList<string>();

            Parallel.ForEach(directories, (directory) =>
            {
                if (!path.IsNullOrEmpty())
                    try
                    {
                        files.AddRange(Directory.GetFiles(directory, fileSearchPattern, SearchOption.TopDirectoryOnly).ToList());
                    }
                    catch
                    {

                    }
            });

            return files.ToArray();
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
