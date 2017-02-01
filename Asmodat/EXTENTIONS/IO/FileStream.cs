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
            byte[] data;
            FileStream stream = null;
            try
            {
                stream = FileInfoEx.TryOpen(file, FileMode.Open, FileAccess.Read, FileShare.Read);

                if (stream == null || !stream.CanRead)
                    return null;
                else if (stream.Length == 0)
                    return new byte[0];

                TryRead(stream, out data, 0, stream.Length);
            }
            finally
            {
                stream?.Flush();
                stream?.Close();
            }

            return data;
        }

        public static bool TryWrite(this FileStream stream, List<byte> data, long? position = null, long offset = 0, long? count = null)
        {
            if (data == null)
                return false;

            return TryWrite(stream, data.ToArray(), position, offset, count);
        }

        public static bool TryWrite(this FileStream stream, byte[] data, long? position = null, long offset = 0, long? count = null)
        {
            return TryWrite(stream, ref data, position, offset, count);
        }

        public static bool TryWrite(this FileStream stream, ref byte[] data, long? position = null, long offset = 0, long? count = null)
        {
            if (stream == null ||
                data == null ||
                position < 0 || offset < 0 ||
                (count != null && (count < 0 || (offset + count) > data.Length)) ||
                (position != null && position.Value > stream.Length))
                return false;

            try
            {
                if (position != null)
                    stream.Position = position.Value;

                if (data.Length == 0) return true;

                if (count == null)
                    count = data.Length;

                //stream.Write(data, (int)offset, (int)count.Value);
                int bufferSize = 4096; long cnt;

                if (offset + count.Value < int.MaxValue) //buffor not neaded simply write
                {
                    while (offset < count.Value)
                    {
                        cnt = (count.Value - offset) > bufferSize ? bufferSize : (count.Value - offset);
                        stream.Write(data, (int)offset, (int)cnt);
                        offset += cnt;
                    }
                }
                else //first copy data to buffer then to stream
                {
                    byte[] buffer = new byte[bufferSize];
                    while (offset < count.Value)
                    {
                        cnt = (count.Value - offset) > bufferSize ? bufferSize : (count.Value - offset);
                        Array.Copy(data, offset, buffer, 0, cnt);
                        stream.Write(buffer, 0, (int)cnt);
                        offset += cnt;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads entire stream form 0 to stream.Length
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] TryReadAll(this FileStream stream)
        {
            if (stream == null)
                return null;

            byte[] data;
            if (TryReadAll(stream, out data))
                return data;
            else
                return null;
        }

        /// <summary>
        /// Reads entire stream form 0 to stream.Length
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryReadAll(this FileStream stream, out byte[] data)
        {
            if (stream == null)
            {
                data = null;
                return false;
            }

            return TryRead(stream, out data, 0, stream.Length);
        }

        public static byte[] TryRead(this FileStream stream, long position, long count)
        {
            byte[] data;
            if (TryRead(stream, out data, position, count))
                return data;
            else
                return null;
        }

        public static bool TryRead(this FileStream stream, out byte[] data,  long position, long count)
        {
            if (stream == null || position < 0 || count < 0 || !stream.CanRead || stream.Length < (position + count))
            {
                data = null;
                return false;
            }
            else if (count == 0)
            {
                data = new byte[0];
                return true; //must be true - it is success
            }

            int bufferSize = 4096;
            try
            {
                stream.Position = position;

                if (count <= bufferSize)
                {
                    data = new byte[count];
                    if (stream.Read(data, 0, (int)count) == count)
                        return true;
                    else
                    {
                        data = null;
                        return false;
                    }
                }

                byte[] buffer = new byte[bufferSize];
                data = new byte[count];
                int leng;
                long read = 0;
                while (read < count)
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

                return true;
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                data = null;
                return false;
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
