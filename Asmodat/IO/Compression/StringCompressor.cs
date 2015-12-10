using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;

namespace Asmodat.IO
{
    public partial class StringCompressor
    {

        public static string Zip(string str)
        {
            if (str == null)
                return null;

            byte[] buffer = Encoding.UTF8.GetBytes(str);
            MemoryStream memory = new MemoryStream();
            using (GZipStream stream = new GZipStream(memory, CompressionMode.Compress, true))
            {
                stream.Write(buffer, 0, buffer.Length);
            }

            memory.Position = 0;
            byte[] data = new byte[memory.Length];
            memory.Read(data, 0, data.Length);

            byte[] zipbuffer = new byte[data.Length + 4];
            Buffer.BlockCopy(data, 0, zipbuffer, 4, data.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, zipbuffer, 0, 4);

            string result = Convert.ToBase64String(zipbuffer);
            return result;
        }

        public static string UnZip(string str)
        {
            if (str == null)
                return null;

            byte[] zipbuffer = Convert.FromBase64String(str);

            using (MemoryStream memory = new MemoryStream())
            {
                int length = BitConverter.ToInt32(zipbuffer, 0);
                memory.Write(zipbuffer, 4, zipbuffer.Length - 4);

                byte[] buffer = new byte[length];

                memory.Position = 0;
                using (GZipStream stream = new GZipStream(memory, CompressionMode.Decompress))
                {
                    stream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        

    }
}
