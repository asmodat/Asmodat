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
using System.Threading;
using Asmodat.Types;

namespace Asmodat.Speech
{
    public partial class NumbersRecognition
    {
        SpeechRecognitionEngine Engine = null;

        public string SourceFile { get; private set; }

        public Grammar SpeechGrammar { get; private set; }

        


        private string[] number0 = {
            //"0",
            "zero"
        };

        private string[] number1 = {
            //"1",
            "one"
        };

        private string[] number2 = {
            //"2",
            "two",
            "to",
            "too"
            
        };

        private string[] number3 = {
            //"3",
            "three",
            "tree",
            "tee"
            
        };

        private string[] number4 = {
            //"4",
            "four",
            "our"
        };

        private string[] number5 = {
            //"5",
            "five"
        };

        private string[] number6 = {
            //"6",
            "six",
            "sex",
            "x"
        };
        private string[] number7 = {
            //"7",
            "seven",
            "even"
        };

        private string[] number8 = {
            //"8",
            "eight",
            "ate"
            
        };

        private string[] number9 = {
            //"9",
            "nine"
        };


        



        Dictionary<int, string[]> numbers = new Dictionary<int, string[]>();
        Dictionary<int, string[]> numbers_UPS_2D = new Dictionary<int, string[]>();

        public NumbersRecognition(System.Globalization.CultureInfo info = null)
        {
            //if (max > 9) throw new Exception("You can't setup recognizer above number 9, its not finished yet");

            if (info == null)
                info = new System.Globalization.CultureInfo("en-US");

            Engine = new SpeechRecognitionEngine(info);


            List<string> list = new List<string>();
            //for (int i = 0; i < 10; i++)  list.Add(i.ToString());


            numbers.Add(0, number0);
            numbers.Add(1, number1);
            numbers.Add(2, number2);
            numbers.Add(3, number3);
            numbers.Add(4, number4);
            numbers.Add(5, number5);
            numbers.Add(6, number6);
            numbers.Add(7, number7);
            numbers.Add(8, number8);
            numbers.Add(9, number9);

            numbers_UPS_2D = CreateNumbersUPS_2D();

            //foreach (var kvp in numbers) list.AddRange(kvp.Value);
            //Choices choices = new Choices(list.ToArray());
            //SpeechGrammar = new Grammar(new GrammarBuilder(choices));//,1,10));






            /* foreach(var kvp in numbers)
             {
                 string[] words = kvp.Value.ToArray();
                 Choices choice = new Choices(words);
                 Grammar grammar = new Grammar(new GrammarBuilder(choice,  0, words.Length));//,1,10));
                 Engine.LoadGrammar(grammar);
             }*/
             
             
           /* foreach (var kvp in numbers_UPS_2D)
            {
                string[] words = kvp.Value.ToArray();
                Choices choice = new Choices(words);
                Grammar grammar = new Grammar(new GrammarBuilder(choice, 1, words.Length));//,1,10));
                Engine.LoadGrammar(grammar);
            }*/



            Engine.LoadGrammar(this.CreateGrammar2D());

            Engine.SpeechDetected += Engine_SpeechDetected;
            Engine.SpeechHypothesized += Engine_SpeechHypothesized;
            Engine.SpeechRecognitionRejected += Engine_SpeechRecognitionRejected;
        }

        private void Engine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
           
        }

        private void Engine_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            

            if (e == null || e.Result == null || e.Result.Words == null)
                return;
            double position = Engine.RecognizerAudioPosition.TotalMilliseconds;
            double confidence = e.Result.Words.Last().Confidence;
            string text = e.Result.Words.Last().Text;

            //if (confidence_list.Contains(text)) confidence *= 2;

            if (confidence <= Confidence)
                return;

            RecognizedName = text;
            RecognizedNumber = Convert(RecognizedName);

            
            Confidence = confidence;
        }

        private void Engine_SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            
        }

        private string recognizedName = null;
        public string RecognizedName
        {
            get { return recognizedName; }
            private set
            {
                recognizedName = value;
                if (recognizedName == null)
                {
                    RecognizedNumber = -1;
                    Confidence = 0;
                }
                else RecognizedNumber = Convert(RecognizedName);
            }
        }
        public int RecognizedNumber { get; private set; } = -1;
        public double Confidence { get; private set; } = double.MinValue;


        public int Convert(string result)
        {
            foreach(int key in numbers.Keys)
                if (numbers[key].Contains(result))
                    return key;
            
            foreach (int key in numbers_UPS_2D.Keys)
                if (numbers_UPS_2D[key].Contains(result))
                    return key;

            return -1;
        }


        public string Directory
        {
            get
            {
                string dir = System.IO.Directory.GetCurrentDirectory();
                return dir + "\\AsmodatSpeech";
            }
        }

        public int Recognize(string mp3_source)
        {
            string wav_destination = Directory + String.Format("\\default_input{0}.wav", TickTime.Now.Ticks);

            if (mp3_source == wav_destination)
                return -1;

            if (File.Exists(wav_destination))
                File.Delete(wav_destination);

            Asmodat.Audio.Converter.Mp3ToWAV(mp3_source, wav_destination);
            SourceFile = wav_destination;

            
            Engine.SetInputToWaveFile(SourceFile);


            Engine.InitialSilenceTimeout = TimeSpan.FromSeconds(30);

            RecognizedName = null;
            Confidence = double.MinValue;

            Engine.Recognize();


            Engine.SetInputToNull();
            if (File.Exists(wav_destination))
                File.Delete(wav_destination);

            return RecognizedNumber;
        }
    }
}
