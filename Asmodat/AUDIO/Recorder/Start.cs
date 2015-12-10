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

     
        public void Start()
        {
            
            Input = new WaveIn();

            if(Memory == null)
                Memory = new MemoryStream();

            Format = new WaveFormat((int)Converter.SampleRates.f44kHz, 16, 1);

            Provider = new BufferedWaveProvider(Format);
            Provider.DiscardOnBufferOverflow = true;

            Input.WaveFormat = Format;

            //FileProvider = new SavingWaveProvider(Provider, DefultFile);

           Writer = new WaveFileWriter(new IgnoreDisposeStream(Memory), Format);


            Input.DataAvailable += WaveInput_DataAvailable;
            Input.RecordingStopped += WaveInput_RecordingStopped;

            Input.BufferMilliseconds = 100;
            Input.NumberOfBuffers = 5;
            Input.StartRecording();
            


        }


    }
}
