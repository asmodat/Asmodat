using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;


using Microsoft.Speech;
using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using Microsoft.Speech.Recognition.SrgsGrammar;
using Microsoft.Speech.Synthesis.TtsEngine;
using System.Threading;
using Asmodat.Types;
using Asmodat.Debugging;

namespace Asmodat.Speech
{
    public partial class MsPhonemRecognition
    {
        SpeechRecognitionEngine Engine = null;

        public string SourceFile { get; private set; }



        Dictionary<int, string[]> numbers = new Dictionary<int, string[]>();
        Dictionary<int, string[]> numbers_UPS_2D = new Dictionary<int, string[]>();

        public System.Globalization.CultureInfo Culture { get; private set; }

        public List<Grammar> Grammars { get; private set; }

        public MsPhonemRecognition(System.Globalization.CultureInfo info = null)
        {
            if (info == null)
               info = new System.Globalization.CultureInfo("en-US");

            Culture = info;

            Grammars = CreateGrammar_Basic2DX().ToList();


            //Engine.LoadGrammar(this.CreateGrammar_Affricates());
            // Engine.LoadGrammar(this.CreateGrammar_Constant());
            // Engine.LoadGrammar(this.CreateGrammar_Diphthong());
            //Engine.LoadGrammar(this.CreateGrammar_Ejective());
            // Engine.LoadGrammar(this.CreateGrammar_Monophthong());
            //Engine.LoadGrammar(this.CreateGrammar_Total2D());
            //Engine.LoadGrammar(this.CreateGrammar_Basic2D());

            this.Initialize();
        }


        private void Initialize()
        {
            if(Engine != null)
                Engine.Dispose();

            Engine = new SpeechRecognitionEngine(Culture);

            foreach (var g in Grammars)
                Engine.LoadGrammar(g);

            Engine.SpeechHypothesized += Engine_SpeechHypothesized;
        }


        private void Engine_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            if (e == null || e.Result == null || e.Result.Words == null)
                return;

            //double position = Engine.RecognizerAudioPosition.TotalMilliseconds;
            double confidence = e.Result.Words.Last().Confidence;
            string text = e.Result.Words.Last().Text;


            Result.Add(text, confidence);

        }

        


        public bool Detected { get; private set; } = false;


        Dictionary<string, double> Result = new Dictionary<string, double>();

        public string Directory
        {
            get
            {
                string dir = System.IO.Directory.GetCurrentDirectory();
                dir += "\\AsmodatSpeech\\PhonemsRecognition";

                if (System.IO.Directory.Exists(dir))
                    System.IO.Directory.CreateDirectory(dir);

                return dir;
            }
        }

        public Dictionary<string, double> RecognizeMP3(string mp3_source, int initialSilenceTimeout_s = 30)
        {
            string wav_destination = Directory + String.Format("\\recognice_temp{0}.wav", TickTime.Now.Ticks);

            if (mp3_source == wav_destination)
                return null;

            if (File.Exists(wav_destination))
                File.Delete(wav_destination);

            Asmodat.Audio.Converter.Mp3ToWAV(mp3_source, wav_destination);
            SourceFile = wav_destination;

            
            Engine.SetInputToWaveFile(SourceFile);
            Engine.InitialSilenceTimeout = TimeSpan.FromSeconds(initialSilenceTimeout_s);

            Result = new Dictionary<string, double>();
            try
            {
                Engine.Recognize();
            }
            catch (Exception ex)
            {
                Asmodat.Debugging.Output.WriteException(ex);
            }

            Engine.SetInputToNull();
            if (File.Exists(wav_destination))
                File.Delete(wav_destination);

            return Result;
        }


        public Dictionary<string, double> RecognizeWAV(string wav_source, int initialSilenceTimeout_s = 30)
        {
            SourceFile = wav_source;
            Result = new Dictionary<string, double>();

            try
            {

                this.Initialize();
                Engine.SetInputToWaveFile(SourceFile);
                Engine.InitialSilenceTimeout = TimeSpan.FromSeconds(initialSilenceTimeout_s);

                Engine.Recognize();
            }
            catch (Exception ex)
            {
                Asmodat.Debugging.Output.WriteException(ex);
               
            }

            try
            {
                Engine.SetInputToNull();
            }
            catch (Exception ex)
            { 
                Asmodat.Debugging.Output.WriteException(ex);
            }

           

            return Result;
        }


        public Dictionary<string, double> RecognizeWAV_Async(string wav_source, int initialSilenceTimeout_s = 30)
        {
            try
            {
                SpeechRecognitionEngine engine = new SpeechRecognitionEngine(Culture);

                foreach (var g in Grammars)
                {
                    if (!engine.Grammars.Contains(g))
                    {
                        try
                        {
                            engine.LoadGrammar(g);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }


                Dictionary<string, double> result = new Dictionary<string, double>();

                engine.SpeechHypothesized += (s, e) =>
                {
                    if (e == null || e.Result == null || e.Result.Words == null)
                        return;

                    double confidence = e.Result.Words.Last().Confidence;
                    string text = e.Result.Words.Last().Text;

                    result.Add(text, confidence);
                };


                engine.SetInputToWaveFile(wav_source);
                engine.InitialSilenceTimeout = TimeSpan.FromSeconds(initialSilenceTimeout_s);


                try
                {
                    engine.Recognize();
                }
                catch (Exception ex)
                {
                    Asmodat.Debugging.Output.WriteException(ex);
                }

                try
                {
                    engine.SetInputToNull();
                }
                catch (Exception ex)
                {
                    Asmodat.Debugging.Output.WriteException(ex);
                }

                if (engine != null)
                    engine.Dispose();

                return result;
            }
            catch(Exception ex)
            {
                ex.ToOutput();
                return null;
            }
        }
    }
}
