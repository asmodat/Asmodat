using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using System.IO;
using Asmodat.Debugging;


using System.Text.RegularExpressions;

using Asmodat.Extensions.Objects;

namespace Asmodat.IO
{
    public partial class Files
    {
        /// <summary>
        /// Windows
        /// </summary>
        public static string[] InvalidNameStrings { get; } = { "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9" };
        /// <summary>
        /// NTFS
        /// </summary>
        public const int MaximumNameLength = 255;
        /// <summary>
        /// NTFS
        /// </summary>
        public const int PreferedPathLength = 260;

        public static bool IsValidFilename(string filename)
        {
            //filename cannot be empty string or longer then 255 chars
            if (filename.IsNullOrWhiteSpace() || filename.Length > Files.MaximumNameLength)
                return false;

            char[] list = System.IO.Path.GetInvalidFileNameChars();

            //filename cannot contains invalid characters
            if (filename.ContainsAny(list, false))
                return false;

            //name must contain extension
            if (filename.Contains("."))
            {
                string extention = Files.GetExtension(filename);
                filename = filename.ReplaceLast(extention, "");
            }

            filename = filename.Trim();

            if (filename.EqualsAny(InvalidNameStrings))
                return false;
            
            return true;
        }

        public static string RemoveInvalidFilenameCharacters(string filename)
        {
            if (filename.IsNullOrEmpty()) return null;

            string result = filename;

            char[] list = System.IO.Path.GetInvalidFileNameChars();

            foreach (char c in list)
                result = result.Replace(c + "", "");

            if (result.IsNullOrEmpty()) return null;

            string upper = result.ToUpper();

            foreach (string s in InvalidNameStrings)
                if (upper == s)
                    return null;

            if (result.Length > Files.MaximumNameLength)
                result = result.Substring(0, Files.MaximumNameLength);

            return result;
        }

        /// <summary>
        /// Returns extension with dot: .extentionname, or empty string in case there is no extention
        /// </summary>
        /// <param name="path">path to file</param>
        /// <returns></returns>
        public static string GetExtension(string path)
        {
            if (path.IsNullOrWhiteSpace())
                return null;
            else if (!path.Contains("."))
                return "";

            FileInfo info = new FileInfo(path);
            return info.Extension;
        }

        public static bool HasExtension(string path)
        {
            path = Files.GetFullPath(path);

            if (path.IsNullOrWhiteSpace())
                return false;

            return Path.HasExtension(path);
        }

        public static bool Exists(string path)
        {
            path = GetFullPath(path);

            if (path.IsNullOrEmpty())
                return false;

            return System.IO.File.Exists(path);
        }

        public static void Create(string path)
        {
            if (!Files.Exists(path))
            {
                path = Files.GetFullPath(path);
                System.IO.File.Create(path).Dispose();
            }
        }

        public static bool TryCopy(string source, string destination)
        {
            if (!Exists(source) || Exists(destination))
                return false;

            try
            {
                System.IO.File.Copy(source, destination);
            }
            catch
            {
                return false;
            }

            return true;
        }



        public static void SaveText(string path, string data, bool gzip = true)
        {
            if (!Files.Exists(path))
                Files.Create(path);

            path = Files.GetFullPath(path);

            Asmodat.IO.File file = new File(path, gzip);
            file.Save(data);
            file.Close();
            file.Dispose();
        }

        public static string LoadText(string path, bool gzip = true)
        {
            if (!Files.Exists(path))
                return null;

            path = Files.GetFullPath(path);
            string data = "";

            Asmodat.IO.File file = new File(path, gzip);
            data = file.Load();
            file.Close();
            file.Dispose();

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if file does not exist or was deleted without exception</returns>
        public static bool Delete(string path)
        {
            path = GetFullPath(path);

            if (path == null)
                return false;
            else if (!Files.Exists(path))
                return true;

            try
            {
                System.IO.File.Delete(path);
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return false;
            }

            return true;
        }


        public static string GetFullPath(string path)
        {
            if (path.IsNullOrEmpty())
                return null;

            if (!path.Contains(":"))
                path = Paths.Combine(Directories.Current,path);

            return path;
        }

        public static string GetName(string path, bool extention = true)
        {
            path = Files.GetFullPath(path);
            if (path.IsNullOrWhiteSpace())
                return null;

            FileInfo info = new FileInfo(path);

            string name = info.Name;
            if (!extention)
            {
                string ext = Files.GetExtension(path);
                name = name.ReplaceLast(ext, "", true);
            }
            
            return name;
        }

        public static string GetNameWithoutExtention(string path)
        {
            return GetName(path, false);
        }


        public static string ExeFullName { get { return Files.GetName(Files.ExePath, true);  } }
        public static string ExeName { get { return Files.GetName(Files.ExePath, false); } }
        public static string ExePath {  get {  return System.Reflection.Assembly.GetEntryAssembly().Location; } }
        public static string ExeRoot { get  { return Path.GetPathRoot(Files.ExePath); } }
        public static string ExeDirectory { get { return GetDirectory(ExePath); } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetDirectory(string path)
        {
            path = Files.GetFullPath(path);

            if (string.IsNullOrEmpty(path))
                return null;

            return Path.GetDirectoryName(path);
        }



        private static object _AppendText = new object();
        public static bool AppendText(string path, string text)
        {
            bool success = true;

            lock(_AppendText)
            {
                try
                {
                    Files.Create(path);
                    path = Files.GetFullPath(path);

                    TextWriter writer = new StreamWriter(path, true);
                    writer.WriteLine(text);
                    writer.Close();
                    writer.Dispose();
                }
                catch (Exception ex)
                {
                    ex.ToOutput();
                    success = false;
                }
            }

            return success;
        }



        public static List<string> ReadAllLines(string path)
        {
            if (path.IsNullOrEmpty())
                return null;

            path = Files.GetFullPath(path);

            if (!Files.Exists(path))
                return null;

            List<string> result = new List<string>();
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(path, System.Text.Encoding.UTF8, true, 128))
                {
                    string line;
                    while((line = reader.ReadLine()) != null)
                    {
                        result.Add(line);
                    }

                    reader.Close();
                }
                stream.Flush();
                stream.Close();
           }

            return result;
        }


        public static bool TryMove(string oldFile, string newFile)
        {
            if (!Files.Exists(oldFile) || newFile.IsNullOrEmpty())
                return false;

            try
            {
                System.IO.File.Move(oldFile, newFile);
                return true;
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return false;
            }
        }

    }
}
