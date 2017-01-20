using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;

using System.Drawing;

using System.IO;
using System.Runtime.CompilerServices;
using Asmodat.Extensions.Collections.Generic;

namespace Asmodat.Extensions.IO
{
    

    public static class FileInfoEx
    {
        public static bool CanReadWrite(string file)
        {
            return (new FileInfo(file)).CanReadWrite();
        }

        public static bool CanReadWrite(this FileInfo fileInfo)
        {
            if (fileInfo == null || !fileInfo.Exists) return false;

            FileStream fs = null;
            try
            {
                fs = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                if (fs != null && fs.CanRead && fs.CanWrite && fs.CanSeek)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
            finally
            {
                fs?.Close();
            }
        }

        public static bool TrySetAttributes(string file, FileAttributes attributes)
        {
            return (new FileInfo(file)).TrySetAttributes(attributes);
        }

        public static bool TrySetAttributes(this FileInfo fileInfo, FileAttributes attributes)
        {
            if (fileInfo == null || !fileInfo.Exists)
                return false;

            try
            {
                File.SetAttributes(fileInfo.FullName, attributes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static FileStream TryOpen(this FileInfo fileInfo, FileMode mode)
        {
            if (fileInfo == null || !fileInfo.Exists) return null;

            try
            {
                return fileInfo.Open(mode);
            }
            catch
            {
                return null;
            }
        }
        public static FileStream TryOpen(this FileInfo fileInfo, FileMode mode, FileAccess access)
        {
            if (fileInfo == null || !fileInfo.Exists) return null;

            try
            {
                return fileInfo.Open(mode, access);
            }
            catch
            {
                return null;
            }
        }

        public static FileStream TryOpen(this FileInfo fileInfo, FileMode mode, FileAccess access, FileShare share)
        {
            if (fileInfo == null || !fileInfo.Exists) return null;

            try
            {
                return fileInfo.Open(mode, access, share);
            }
            catch
            {
                return null;
            }
        }

    }
}
