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

namespace Asmodat.Audio
{
    public partial class Converter
    {

        public static byte[] WaveStreamToArray(WaveStream stream)
        {
            if (stream == null)
                return null;
            else if (stream.Length == 0)
                return new byte[0];

            byte[] buff = new byte[stream.Length];

            MemoryStream memory = new MemoryStream();
            int reader = 0;
            stream.Position = 0;
            while ((reader = stream.Read(buff, 0, buff.Length)) != 0)
                memory.Write(buff, 0, reader);

            stream.Position = 0;

            return memory.ToArray();
        }

        public static double[] WaveStreamToDoubleArray(WaveStream stream, bool swap = true)
        {
            byte[] bytes = Converter.WaveStreamToArray(stream);
            double[] doubles = Converter.WavToDoubleArray(bytes, swap);
            return doubles;
        }

        public static WaveStream ArrayToWaveStream(byte[] data, WaveFormat format)
        {
            if (data == null)
                return null;

            Stream memory = Streams.WriteToMemory(data);
            memory.Position = 0;

            return new RawSourceWaveStream(memory, format);
        }

        public static WaveStream ArrayToWaveStream(short[] data, WaveFormat format, bool swap = true)
        {
            if (data == null)
                return null;


            byte[] bytes = Converter.WavToByteArray(data, swap);
            Stream memory = Streams.WriteToMemory(bytes);
            memory.Position = 0;

            return new RawSourceWaveStream(memory, format);
        }

        public static WaveStream ArrayToWaveStream(double[] data, WaveFormat format, bool swap = true)
        {
            if (data == null)
                return null;


            byte[] bytes = Converter.WavToByteArray(data, swap);
            Stream memory = Streams.WriteToMemory(bytes);
            memory.Position = 0;

            return new RawSourceWaveStream(memory, format);
        }







    }
}
