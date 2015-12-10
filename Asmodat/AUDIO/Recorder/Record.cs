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

        public WaveIn Input { get; private set; } = null;
        public WaveFileWriter Writer { get; private set; } = null;
        public MemoryStream Memory { get; private set; } = null;

        public BufferedWaveProvider Provider { get; private set; } = null;

        public SavingWaveProvider FileProvider { get; private set; } = null;

        public WaveFormat Format { get; private set; } = null;

        public void Stop()
        {
             

            if (Input == null)
                return;

            Input.StopRecording();
        }

        private void WaveInput_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if(Input != null)
            {
                Input.Dispose();
                Input = null;
            }

            if(Writer != null)
            {
                Writer.Close();
                Writer = null;
            }
        }

        private void WaveInput_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (e == null || e.Buffer.Length <= 0 || Writer == null)
                return;

            Provider.AddSamples(e.Buffer, 0, e.BytesRecorded);
            Writer.Write(e.Buffer, 0, e.BytesRecorded);

            Player.Play(e.Buffer, Format);
            
        }

        public void Play()
        {
            Player.Play(Provider);
        }
    }
}
