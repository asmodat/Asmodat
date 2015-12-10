using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Asmodat.IO
{
    public partial class File : IDisposable
    {
        public static void Save(ref System.IO.FileStream Stream, string data = null, bool gzip = false)
        {
            byte[] bytes;

            if(System.String.IsNullOrEmpty(data)) bytes = new byte[0];
            else if (gzip) bytes = Compression.Zip(data);
            else bytes = Compression.GetBytes(data);

            string FullPath = Stream.Name;
            Stream.Close();
            System.IO.File.WriteAllBytes(FullPath, bytes);
            Stream = System.IO.File.OpenWrite(FullPath);
        }

        public static string Load(ref System.IO.FileStream Stream, bool gzip = false)
        {
            string FullPath = Stream.Name;
            byte[] bytes = new byte[Stream.Length];
            Stream.Flush();
            Stream.Read(bytes, 0, (int)Stream.Length);

            string data;

            if (gzip) data = Compression.UnZipString(bytes);
            else data = Compression.GetString(bytes);

            return data;
        }



        /// <summary>
        /// This method allows you to save string data into hard drive and alo zip its content witg gzip
        /// </summary>
        /// <param name="file">full path to file</param>
        /// <param name="data">data to save</param>
        /// <param name="gzip">if true compresses the data else saves plain text</param>
        public static void Save(string file, string data = null, bool gzip = false)
        {
            string FullPath = file;
            string FullDirectory = System.IO.Path.GetDirectoryName(file);

            if (!System.IO.Directory.Exists(FullDirectory))
                System.IO.Directory.CreateDirectory(FullDirectory);

            if (!System.IO.File.Exists(FullPath) || System.String.IsNullOrEmpty(data))
            {
                System.IO.FileStream Stream = System.IO.File.Create(FullPath);
                Stream.Close();
            }

            if (!System.String.IsNullOrEmpty(data))
            {
                if (gzip)
                {
                    byte[] bytes = Compression.Zip(data);
                    if (bytes != null && bytes.Length != 0)
                        System.IO.File.WriteAllBytes(FullPath, bytes);
                }
                else System.IO.File.WriteAllText(FullPath, data);
            }
        }

        /// <summary>
        /// This method loads string data from file that is compressed or not with gzop
        /// </summary>
        /// <param name="file">full file path</param>
        /// <param name="gzip">if tru method tryies to unzip string data with gzip</param>
        /// <returns>data inside file if it exists or null if it doesn't</returns>
        public static string Load(string file, bool gzip = false)
        {
            string FullPath = file;
            string FullDirectory = System.IO.Path.GetDirectoryName(file);

            if (!System.IO.Directory.Exists(FullDirectory) || !System.IO.File.Exists(FullPath))
                return null;

            string data = System.IO.File.ReadAllText(FullPath);
            if (System.String.IsNullOrEmpty(data)) return "";

            if (gzip)
                return Compression.UnZipString(data);

            return data;
        }
    }
}
