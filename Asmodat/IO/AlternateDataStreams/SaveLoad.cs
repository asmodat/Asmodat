using Asmodat.Debugging;
using System;
using System.IO;

using Asmodat.Abbreviate;
using Asmodat.Extensions.Objects;


namespace Asmodat.IO
{
    public partial class ADSFile
    {

        public static string GetFullPatch(string path)
        {
            try
            {
                if (path.IsNullOrWhiteSpace())
                    path = Files.ExePath;
                else
                    path = Files.GetFullPath(path);
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
            }

            return path;
        }


        public static string LoadString(string name, string path = null, bool unzip = false)
        {
            path = ADSFile.GetFullPatch(path);
            string data = null;

            if (path.IsNullOrWhiteSpace())
                return data;

            try
            {
                if (unzip)
                {
                    data = StringCompressor.UnZip(ADSFile.Read(path, name));
                }
                else
                {
                    data = ADSFile.Read(path, name);
                }
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                data = null;
            }

            return data;
        }

        public static bool SaveString(string name, string data, string path, bool zip = false)
        {
            path = ADSFile.GetFullPatch(path);

            if (path.IsNullOrWhiteSpace())
                return false;

            try
            {
                if (zip)
                {
                    ADSFile.Write(StringCompressor.Zip(data), path, name);
                }
                else
                {
                    ADSFile.Write(data, path, name);
                }
            }
            catch (Exception ex)
            {
                Output.WriteException(ex);
                return false;
            }

            return true;
        }


    }
}