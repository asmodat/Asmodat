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

        public static byte[] TryRead(this FileStream stream, long position, int count)
        {
            if (stream == null || position < 0 || count < 0 || !stream.CanRead || stream.Length < (position + count))
                return null;

            List<byte> data = new List<byte>();
            int bufferSize = 4096;
            try
            {
                stream.Position = position;

                byte[] buffer = new byte[bufferSize];
                int read = 0;
                while(data.Count < count)
                {
                    read = stream.Read(buffer, 0, bufferSize);
                    if (read > 0)
                        data.AddSubArray(buffer, 0, read);
                }

                return data.ToArray();
            }
            catch
            {
                return null;
            }
        }
    }
}
