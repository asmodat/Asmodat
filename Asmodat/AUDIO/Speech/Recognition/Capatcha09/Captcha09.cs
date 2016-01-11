using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;


using System.Threading;
using Asmodat.Types;
using Asmodat.Abbreviate;using Asmodat.Extensions.Objects;

using Asmodat.IO;
using AsmodatMath;


namespace Asmodat.Speech
{
    public partial class Captcha09
    {

        public string Directory { get; private set; } = @"C:\Captcha09";

        public string[] GetFiles(int minNameLength = 5)
        {
            string[] files = System.IO.Directory.GetFiles(Directory);

            if (files == null || files.Length <= 0)
                return null;

            List<string> list = new List<string>();
            foreach (string file in files)
            {
                if (string.IsNullOrEmpty(file))
                    continue;

                string name = Path.GetFileNameWithoutExtension(file);
                string ext = Path.GetExtension(file);

                if (ext.ToLower().Contains("mp3") && name.Length >= minNameLength)
                    list.Add(file);
            }

            return list.ToArray();
        }


        MsPhonemRecognition recognition = new MsPhonemRecognition();



        Abbreviate.ThreadedDictionary<int, ThreadedList<Dictionary<string, double>>>
            Data = new ThreadedDictionary<int, ThreadedList<Dictionary<string, double>>>();

        DatabseSimpleton Database;

        List<string> LoadedFiles = new List<string>();
        private void LoadData()
        {
            if (Database.Count <= 0)
                return;

            string[] keys = Database.Keys;
            object[] values = Database.Values;


            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                string data = (string)values[i];

                var split = key.SplitSafe('-');
                string name = split[0];
                int number = int.Parse(split[1]);

                ThreadedDictionary<string, double> buffor = new ThreadedDictionary<string, double>();
                buffor.XmlDeserialize(data);

                if (!Data.ContainsKey(number))
                    Data.Add(number, new ThreadedList<Dictionary<string, double>>());

                Data[number].Add((Dictionary<string, double>)buffor);

                if (!LoadedFiles.Contains(name))
                    LoadedFiles.Add(name);
            }


        }

        public Captcha09(string directory = null)
        {
            if (directory != null)
                Directory = Directories.Create(directory).FullName;

            for (int i = 0; i < 10; i++)
            {
                if (!Data.ContainsKey(i))
                    Data.Add(i, new ThreadedList<Dictionary<string, double>>());
            }

            Database = new DatabseSimpleton(Directory + @"\data\training.dbs", false);
            LoadData();
        }

        public void StartTraining()
        {
            string[] files = this.GetFiles(1);

            if (files == null || files.Length <= 0)
                return;

            foreach (string file in files)
            {
                
                string name = null;

                List<int> numbers = new List<int>();

                try
                {
                    name = Path.GetFileNameWithoutExtension(file);

                    if (LoadedFiles.Contains(name))
                        continue;

                    for (int i = 0; i < name.Length; i++)
                    {
                        if (!Char.IsNumber(name[i]))
                            break;

                        numbers.Add(int.Parse(name[i].ToString()));
                    }
                }
                catch
                {
                    continue;
                }

                if (numbers.Count <= 0)
                    continue;
                

                int[] words_split = Asmodat.Audio.Converter.GetMp3WordsSplit(file, numbers.Count);


                string[] temp_files = new string[numbers.Count];

                for (int i = 0; i < temp_files.Length; i++)
                    temp_files[i] = string.Format("{0}\\temp\\audio{1}-{2}.wav", Directory, (i + 1), TickTime.NowTicks);

                Asmodat.Audio.Converter.Mp3Split(file, words_split, temp_files, true);


                
                for(int i = 0; i < temp_files.Length; i++)
                {
                    Dictionary<string, double> result = recognition.RecognizeWAV(temp_files[i]);
                    this.Learn(name, numbers[i], result);
                }

                for (int i = 0; i < temp_files.Length; i++)
                    if (System.IO.File.Exists(temp_files[i]))
                        System.IO.File.Delete(temp_files[i]);

                Database.Save();

            }
        }

        public void Save()
        {
            if(Database != null)
                Database.Save();
        }

        public void Learn(string name, int number, Dictionary<string, double> result)
        {
            if (result == null || result.Count <= 0)
                return;

            Data[number].Add(result);
            TrainingStrength += result.Count;

            ThreadedDictionary<string, double> buffor = (ThreadedDictionary<string, double>)result;
            string data = buffor.XmlSerialize();


            Database.Set(name + "-" + number, data);
        }

        


        public double TrainingStrength { get; private set; } = 0;

        public int HypotesizeNumber(Dictionary<string, double> data)
        {
            if (data == null || data.Count <= 0)
                return -1;

            int[] count = new int[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] sum = new double[10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            int base_position = -1;
            double base_value = 0;
            foreach (string base_name in data.Keys)
            {
                base_value = data[base_name];
                ++base_position;

                foreach (int number in Data.Keys)
                {
                    var list = Data[number];

                    foreach (var dict in list)
                    {
                        if (dict == null || dict.Count <= 0)
                            continue;

                        int position = -1;
                        foreach (string name in dict.Keys)
                        {
                            ++position;
                            double position_divider = (double)1 / (Math.Abs(position - base_position) + 1);

                            if (name == base_name)
                            {
                                double value_divider = (double)1 / (Math.Abs(dict[name] - base_value) + 1);

                                count[number] += 1;
                                sum[number] += dict[name] * base_value * position_divider * value_divider;
                            }
                        }
                    }
                }
            }

            double[] results = new double[10];

            if (count.Max() <= 0)
                return -1;

            for (int i = 0; i < count.Length; i++)
                results[i] = sum[i] * count[i];

            int result;
            double value;

            AMath.GetFirstMax(results, out result, out value);
            

            return result;
        }
    }
}

/*
 if (result == null || result.Count <= 0)
                        continue;

                    int number = numbers[i];
                    Data[number].Add(result);
                    strength = result.Count;
                    TrainingStrength += strength;

                    ThreadedDictionary<string, double> buffor = (ThreadedDictionary<string, double>)result;
                    string data = buffor.XmlSerialize();


                    Database.Set(name + "-" + number, data);
*/
