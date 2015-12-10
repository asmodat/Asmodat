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
    public partial class SavingWaveProvider : IWaveProvider, IDisposable
    {
        private readonly IWaveProvider Provider;
        private readonly WaveFileWriter Writer;
        private bool IsWriterDisposed;

        public WaveFormat WaveFormat {
            get { return Provider.WaveFormat; }
        }

        public SavingWaveProvider(IWaveProvider Provider, string path)
        {
            this.Provider = Provider;
            Writer = new WaveFileWriter(path, Provider.WaveFormat);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var read = Provider.Read(buffer, offset, count);
            if(count > 0 && !IsWriterDisposed)
            {
                Writer.Write(buffer, offset, read);
            }
            if(count == 0)
            {
                Dispose();
            }
            return read;
        }

        public void Dispose()
        {
            if(!IsWriterDisposed)
            {
                IsWriterDisposed = true;
                Writer.Dispose();
            }
        }

    }
}
