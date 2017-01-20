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
    

    public static class FileStreamEx
    {
        
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

        public static byte[] TryRead(this FileStream stream, long position, int count)
        {
            if (stream == null || position < 0 || count < 0 || !stream.CanRead || stream.Length < (position + count))
                return null;

            List<byte> data = new List<byte>();
            int bufferSize = 4096;
            try
            {
                stream.Position = position;

                byte[] buffer;
                if (count <= bufferSize)
                {
                    buffer = new byte[count];
                    if (stream.Read(buffer, 0, count) == count)
                        return buffer;
                    else
                        return null;
                }
                else
                    buffer = new byte[bufferSize];

                int read = 0;
                while(data.Count < count)
                {
                    read = stream.Read(buffer, 0, bufferSize);
                    if (read > 0)
                    {
                        data.AddSubArray(buffer, 0, read);
                        stream.Position = position + data.Count;
                    }
                }

                return data.GetRange(0, count)?.ToArray();
            }
            catch
            {
                return null;
            }
        }
    }
}
