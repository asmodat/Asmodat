using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

using System.Reflection;

using System.Data;

using System.Windows.Forms;

using System.Collections;

using System.Diagnostics;

using System.Globalization;

using System.Linq.Expressions;

using Asmodat.Types;

using System.IO;
using Asmodat.IO;

namespace Asmodat.Abbreviate
{
    public static partial class Streams
    {

        public static string ToString(this StreamReader stream)
        {
            if (stream == null)
                return null;
           
            return stream.ReadToEnd();
        }

        public static byte[] ToArray(Stream stream)
        {
            if (stream == null)
                return null;

            byte[] buf = null;

            if(stream.CanSeek) stream.Position = 0;

            using (MemoryStream memory = new MemoryStream())
            {
                stream.CopyTo(memory);
                buf = memory.ToArray();
            }
            return buf;
        }

        public static MemoryStream WriteToMemory(byte[] data)
        {
            if (data == null)
                return null;
            if (data.Length <= 0)
                return new MemoryStream();

            MemoryStream memory = new MemoryStream();
            memory.Write(data, 0, data.Length);

            return memory;
        }


        public static MemoryStream ToMemory(byte[] data)
        {
            if (data == null)
                return null;
            if (data.Length <= 0)
                return new MemoryStream();

            MemoryStream memory = new MemoryStream();
            memory.Write(data, 0, data.Length);

            return memory;
        }



        public static MemoryStream ToMemory(this string data)
        {
            if (data == null)
                return null;
            MemoryStream memory = new MemoryStream();
            StreamWriter writer = new StreamWriter(memory);
            writer.Write(data);
            writer.Flush();
            return memory;
        }

        public static MemoryStream ToMemory(this Stream stream)
        {
            byte[] data = Streams.ToArray(stream);
            return Streams.ToMemory(data);
        }

        public static string ToFile(MemoryStream stream, string path)
        {
            if (stream == null)
                return null;

            path = Files.GetFullPath(path);
            if (path == null )
                return null;

            stream.Position = 0;
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, (int)stream.Length);
                file.Write(data, 0, data.Length);
                stream.Close();
            }

            return path;
        }

        public static string ToFile(Stream stream, string path)
        {
            MemoryStream memory = Streams.ToMemory(stream);
            return Streams.ToFile(memory, path);
        }


      
        public static MemoryStream CopyToMemory(this Stream stream)
        {
            if (stream == null)
                return null;

            if (stream.Length <= 0)
                return new MemoryStream();

            byte[] buffer = new byte[stream.Length];
            stream.Write(buffer, 0, buffer.Length);

            return new MemoryStream(buffer);

        }
    }
}
