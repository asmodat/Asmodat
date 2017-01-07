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


        public static string[] TryGetFiles(string path, string directorySearchPattern = "*", string fileSearchPattern = "*.*", bool includeSubdirectories = true)
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
        /// This method can be used to "track" files count and currentPaths using threaded lists while iterating thourg big directory.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="paths"></param>
        /// <param name="fileSearchPattern"></param>
        /// <param name="directorySearchPattern"></param>
        /// <param name="includeSubdirectories"></param>
        /// <returns>Item1: file list, Item 2: directory list</returns>
        public static Tuple<string[], string[]> TryGetFilesAndDirectories(ThreadedList<string> files, ThreadedList<string> paths, string fileSearchPattern = "*", string directorySearchPattern = "*", bool includeSubdirectories = true, ThreadedList<string> currentPaths = null)
        {
            if (files == null || paths == null)
                throw new ArgumentNullException("Files and paths can't be null.");

            if (paths.IsNullOrEmpty())
                return new Tuple<string[], string[]>(files.ToArray(), new string[0]);

            var _paths = paths.ToArray();

            ThreadedList<string> subPaths = new ThreadedList<string>();

            if (!_paths.IsNullOrEmpty())
                Parallel.ForEach(_paths, (path) =>
                {
                    if (path.IsNullOrEmpty()) return;
                    try { subPaths.AddRange(Directory.GetDirectories(path, directorySearchPattern, SearchOption.TopDirectoryOnly)); } catch  { }
                    try { files.AddRange(Directory.GetFiles(path, fileSearchPattern, SearchOption.TopDirectoryOnly).ToList()); } catch { }
                });


            currentPaths?.AddRange(subPaths);

            if (includeSubdirectories)
                paths.AddRange(TryGetFilesAndDirectories(files, subPaths.Clone(), fileSearchPattern, directorySearchPattern, true, currentPaths).Item2);
            else
                paths.AddRange(subPaths);

            return new Tuple<string[], string[]>(files.ToArray(), paths.ToArray());
        }
    }
}
