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

namespace Asmodat.Audio
{
    public class RawSourceWaveStream : WaveStream
    {
        private Stream source;
        private WaveFormat format;

        public RawSourceWaveStream(Stream source, WaveFormat format)
        {
            this.source = source;
            this.format = format;

        }

        public override WaveFormat WaveFormat
        {
            get
            {
                return this.format;
            }
        }

        public override long Length
        {
            get
            {
                return this.source.Length;
            }
        }

        public override long Position
        {
            get
            {
                return this.source.Position;
            }

            set
            {
                this.source.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return source.Read(buffer, offset, count);
        }
    }
}
