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

        public static string Directory
        {
            get
            {
                string dir = System.IO.Directory.GetCurrentDirectory();
                dir += "\\AsmodatAudioConverter";

                if (!System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                return dir;
            }
        }

        public static string GetTemporaryFilePath
        {
            get
            {
                return Converter.Directory + System.String.Format("\\Temp{0}.wav", TickTime.Now.Ticks);
            }
        }
        /*
        Mp3FileReader reader = new Mp3FileReader(source);

                int duration = Converter.Mp3Duration(reader);
        */


        /// <summary>
        /// returns mp3 duration in ms
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static int GetMp3Duration(string path)
        {
            try
            {
                path = Files.GetFullPath(path);
                using (Mp3FileReader reader = new Mp3FileReader(path))
                    return (int)(reader.TotalTime).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                ex.ToOutput();
                return 0;
            }
        }

        public static int Mp3Duration(Mp3FileReader reader)
        {
            return (int)(reader.TotalTime).TotalMilliseconds;
        }

        public static void Mp3ToWAV(string mp3_source, string wav_destination)
        {
            if (string.IsNullOrEmpty(mp3_source) || string.IsNullOrEmpty(wav_destination) || !System.IO.File.Exists(mp3_source))
                return;

            using (Mp3FileReader mp3 = new Mp3FileReader(mp3_source))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    string directory = Path.GetDirectoryName(wav_destination);
                    if (!System.IO.Directory.Exists(directory))
                        System.IO.Directory.CreateDirectory(directory);

                    WaveFileWriter.CreateWaveFile(wav_destination, pcm);
                }
            }
        }


        public static  WaveStream Mp3ToWAV(Stream mp3_source)
        {
            try
            {
                mp3_source.Position = 0;
                Mp3FileReader mp3 = new Mp3FileReader(mp3_source);
                return WaveFormatConversionStream.CreatePcmStream(mp3);
            }
            catch(Exception ex)
            {
                Output.WriteException(ex);
                return null;
            }
        }

        public static WaveFormat Mp3GetWaveFormat(Stream mp3_source)
        {
            mp3_source.Position = 0;
            Mp3FileReader mp3 = new Mp3FileReader(mp3_source);
            return mp3.WaveFormat;
        }


        /*if (destination1 == null || destination2 == null)
        {
            string directory = Path.GetDirectoryName(mp3_source) + "\\";
            destination1 = directory + name + "-p1.mp3";
            destination2 = directory + name + "-p2.mp3";
        }

        if (File.Exists(destination1)) File.Delete(destination1);
        if (File.Exists(destination2)) File.Delete(destination2);

        using (Mp3FileReader reader = new Mp3FileReader(mp3_source))
        {

            double currentTime = 0;
            Mp3Frame frame = reader.ReadNextFrame();
            FileStream stream1 = new FileStream(destination1, FileMode.Create, FileAccess.Write);

            while (frame != null && currentTime < milisecond)
            {
                currentTime = (reader.CurrentTime).TotalMilliseconds;

                stream1.Write(frame.RawData, 0, frame.RawData.Length);
                frame = reader.ReadNextFrame();
            }

            stream1.Close();

            if (frame == null)
                return;

            FileStream stream2 = new FileStream(destination2, FileMode.Create, FileAccess.Write);

            while (frame != null)
            {
                stream2.Write(frame.RawData, 0, frame.RawData.Length);
                frame = reader.ReadNextFrame();
            }

            stream2.Close();
        }*/



    }
}
