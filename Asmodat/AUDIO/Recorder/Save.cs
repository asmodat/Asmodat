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
using NAudio.Utils;



namespace Asmodat.Audio
{
    public partial class Recorder
    {
        public string DefultFile { get { return Files.GetFullPath("Defaul.wav"); } }
     
        public void Save(string path = null)
        {
            path = Files.GetFullPath(path);

            if (path == null)
                path = DefultFile;

            if (Memory == null)
                return;

            Converter.SaveWAV(Memory, path);
        }

    }
}
