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
using Asmodat.Debugging;

namespace Asmodat.Extensions.IO
{
    

    public static class FileStreamEx
    {
        public static byte[] TryReadAll(string file)
        {
            byte[] data = null;
            FileStream stream = null;
            try
            {
                stream = FileInfoEx.TryOpen(file, FileMode.Open, FileAccess.Read, FileShare.Read);

                if (stream == null || !stream.CanRead)
                    return null;
                else if (stream.Length == 0)
                    return new byte[0];

                 data = TryRead(stream, 0, stream.Length);
            }
            finally
            {
                stream?.Flush();
                stream?.Close();
            }

            return data;
        }


        public static bool TryWrite(this FileStream stream, byte[] data, long? position = null, int offset = 0, int? count = null)
        {
            if (stream == null || 
                data == null ||
                position < 0 || offset < 0 ||
                (count != null && (count < 0 || (offset + count) > data.Length)) ||
                (position != null && position.Value > stream.Length))
                return false;

            try
            {
                if(position != null)
                    stream.Position = position.Value;

                if (data.Length == 0)
                    return true;
                else
                {
                    if(count == null)
                        stream.Write(data, offset, data.Length);
                    else
                        stream.Write(data, offset, count.Value);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static byte[] TryRead(this FileStream stream, long position, long count)
        {
            if (stream == null || position < 0 || count < 0 || !stream.CanRead || stream.Length < (position + count))
                return null;
            else if (count == 0)
                return new byte[0];

            int bufferSize = 4096;
            try
            {
                stream.Position = position;

                byte[] buffer;
                if (count <= bufferSize)
                {
                    buffer = new byte[count];
                    if (stream.Read(buffer, 0, (int)count) == count)
                        return buffer;
                    else
                        return null;
                }
                else
                    buffer = new byte[bufferSize];

                byte[] data = new byte[count];
                int leng;
                long read = 0;
                while(read < count)
                {
                    if (read + bufferSize > count)
                        leng = (int)(count - read);
                    else
                        leng = bufferSize;

                    if (stream.Read(buffer, 0, leng) > 0)
                    {
                        Array.Copy(buffer, 0, data, read, leng);
                        read += leng;
                    }
                    else
                        stream.Position = read;
                }

                return data;
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return null;
            }
        }

        public static bool TryFlush(this FileStream stream)
        {
            if (stream == null)
                return false;

            try
            {
                stream.Flush();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static bool TryClose(this FileStream stream)
        {
            if (stream == null)
                return false;

            try
            {
                stream.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

    }
}
