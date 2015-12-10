using Asmodat.Debugging;
using Asmodat.Extensions.Collections.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using AsmodatMath;
using Asmodat.Abbreviate;


//using ICSharpCode.SharpZipLib.Zip;
//using ICSharpCode.SharpZipLib.Core;
//using ICSharpCode.SharpZipLib.Checksums;



namespace Asmodat.Extensions.Objects
{
    

    public  static partial class byteEx
    {
        public static byte[] IZip(this byte[] data, int level = 9)
        {
            if (data == null) return null;
            if (data.Length == 0) return new byte[0];

            level = level.ToClosedInterval(0, 9);

            using (MemoryStream memory = new MemoryStream())
            {
                using (Ionic.Zip.ZipOutputStream stream = new Ionic.Zip.ZipOutputStream(memory))
                {
                    stream.CompressionLevel = (Ionic.Zlib.CompressionLevel)level;
                    stream.PutNextEntry("default");
                    stream.Write(data, 0, data.Length);
                }

                return memory.ToArray();
            }
        }

        public static byte[] UnIZip(this byte[] data)
        {
            if (data == null) return null;
            if (data.Length == 0) return new byte[0];

            using (MemoryStream output = new MemoryStream())
            {
                using (MemoryStream input = new MemoryStream(data))
                {
                    using (Ionic.Zip.ZipInputStream stream = new Ionic.Zip.ZipInputStream(input))
                    {
                        var Entry = stream.GetNextEntry();
                        stream.CopyTo(output);
                        return output.ToArray();
                    }
                }
            }
        }

        public static byte[] Zip(this byte[] data, int level = 9)
        {
            if (data == null) return null;
            if (data.Length == 0) return new byte[0];

            level = level.ToClosedInterval(0, 9);
           
            using (MemoryStream memory = new MemoryStream())
            {
                using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream stream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(memory))
                {
                    stream.SetLevel(level);
                    ICSharpCode.SharpZipLib.Zip.ZipEntry Entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry("");

                    Entry.Size = data.Length;
                    stream.PutNextEntry(Entry);
                    stream.Write(data, 0, data.Length);
                }

                return memory.ToArray();
            }
        }

        public static byte[] UnZip(this byte[] data)
        {
            if (data == null) return null;
            if (data.Length == 0) return new byte[0];

            using (MemoryStream output = new MemoryStream())
            {
                using (MemoryStream input = new MemoryStream(data))
                {
                    ZipArchive archive = new ZipArchive(input);
                    if (archive.Entries == null || archive.Entries.Count <= 0)
                        return null;

                    using (Stream stream = archive.Entries[0].Open())
                    {
                        stream.CopyTo(output);
                        return output.ToArray();
                    }
                }
            }


        }

        public static byte[] GZip(this byte[] data)
        {
            if (data == null) return null;
            if (data.Length == 0) return new byte[0];

            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream stream = new GZipStream(memory, System.IO.Compression.CompressionMode.Compress, true))
                {
                    stream.Write(data, 0, data.Length);
                }

                return memory.ToArray();
            }
        }

        public static byte[] UnGZip(this byte[] data)
        {
            if (data == null)  return null;
            if (data.Length == 0)  return new byte[0];

            try
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    using (MemoryStream buffer = new MemoryStream(data))
                    {
                        using (GZipStream stream = new GZipStream(buffer, System.IO.Compression.CompressionMode.Decompress))
                        {
                            stream.CopyTo(memory);
                            return memory.ToArray();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Exceptions.Write(ex);
                return null;
            }
        }


    }
}
