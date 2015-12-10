using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

using System.Speech;
using System.Speech.AudioFormat;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Speech.Recognition.SrgsGrammar;
using System.Speech.Synthesis.TtsEngine;

using NAudio.Wave;
using Asmodat.Types;
using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;
using Asmodat.Debugging;
using Asmodat.IO;

namespace Asmodat.Audio
{
    public partial class Converter
    {

      
        public static  void SaveWAV(Stream stream, string path)
        {

            path = Files.GetFullPath(path);

            if (stream == null || path == null)
                return;

            

            try
            {
                Files.Delete(path);

                stream.Position = 0;
                using (WaveFileReader reader = new WaveFileReader(stream))
                {
                    WaveFileWriter.CreateWaveFile(path, reader);
                }
            }
            catch(Exception ex)
            {
                ex.ToOutput();
            }
        }




    }
}
