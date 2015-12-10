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
    public partial class Player
    {
        
        public static void PlayRaw(MemoryStream Memory)
        {
            if (Memory == null || Memory.Length <= 0)
                return;
            try
            {


                WaveFormat Format = new WaveFormat((int)Converter.SampleRates.f44kHz,16,1);

                Memory.Position = 0;
                using (WaveOut wout = new WaveOut())
                {
                    using (RawSourceWaveStream raw = new RawSourceWaveStream(Memory, Format))
                    {
                        wout.Volume = 1;
                        wout.Init(raw);
                        wout.Play();
                        wout.Stop();
                    }
                }
                
            }
            catch(Exception ex)
            {
                ex.ToOutput();
            }
        }


        public static void Play(BufferedWaveProvider Provider)
        {
            if (Provider == null)
                return;
            try
            {

                using (WaveOut wout = new WaveOut())
                {
                    wout.Volume = 1;
                    wout.Init(Provider);
                    wout.Play();
                    wout.Stop();
                }
            }
            catch (Exception ex)
            {
                ex.ToOutput();
            }
        }


        public static void Play(byte[] buffer, WaveFormat Format)
        {
            if (buffer == null || buffer.Length <= 0 || Format == null)
                return;
            try
            {
                using (WaveOut wout = new WaveOut())
                {
                    using (MemoryStream Memory = new MemoryStream(buffer))
                    {
                        using (RawSourceWaveStream raw = new RawSourceWaveStream(Memory, Format))
                        {
                            wout.Volume = 1;
                            wout.Init(raw);
                            wout.Play();
                            wout.Stop();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToOutput();
            }
        }

    }
}
