using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;

namespace Asmodat.IO
{
    public partial class Compression
    {
        public static void CopyTo(System.IO.Stream Source, System.IO.Stream Destination)
        {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = Source.Read(bytes, 0, bytes.Length)) != 0)
                Destination.Write(bytes, 0, cnt);
        }

        public static byte[] UnZip(byte[] bytes)
        {
            if (bytes == null) return null;

            using (MemoryStream MSI = new MemoryStream(bytes))
            using (MemoryStream MSO = new MemoryStream())
            {
                using (GZipStream GZS = new GZipStream(MSI, CompressionMode.Decompress))
                    CopyTo(GZS, MSO);

                return MSO.ToArray();
            }
        }

        public static string UnZipString(string str)
        {
            if (String.IsNullOrEmpty(str)) return null;

            byte[] bytes = Compression.GetBytes(str);
            byte[] data = Compression.UnZip(bytes);
            return Encoding.UTF8.GetString(data);
        }

        public static string UnZipString(byte[] bytes)
        {
            if (bytes == null)
                return null;

            byte[] data = Compression.UnZip(bytes);
            return Encoding.UTF8.GetString(data);
        }

        public static byte[] Zip(string str)
        {
            if (String.IsNullOrEmpty(str))
                return null;

            byte[] bytes = Compression.GetBytes(str);
            using(MemoryStream MSI = new MemoryStream(bytes))
            using (MemoryStream MSO = new MemoryStream())
            {
                using (GZipStream GZS = new GZipStream(MSO, CompressionMode.Compress))
                    CopyTo(MSI, GZS);

                return MSO.ToArray();
            }
        }

        public static string ZipString(string str)
        {
            if (String.IsNullOrEmpty(str))
                return null;

            byte[] bytes = Compression.Zip(str);
            return Compression.GetString(bytes);
        }

        public static byte[] GetBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        public static string GetString(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

    }
}
