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

namespace Asmodat.Speech
{
    public partial class MsPhonemRecognition
    {
        //english
    private string[] phonems_basic = new string[]{
    "AA", "AE", "AH", "AI", "AU", "AO", "AX", "AX RA", "EH", "EH RA",
    "EI", "ER", "I", "IH", "O + U", "OI", "U", "UH", "AX L", "AX M",
    "AX N", "B", "CH", "D", "DH", "F", "G", "H", "J", "JH", "K", "L",
    "M", "N", "NG", "P", "R", "S", "SH", "T", "TH", "V", "W", "Z", "ZH"};

        //German Diphthong Vowels: "IH + AX","YH + AX","EY Ing + AX","EH Ing + AX","EU Ing + AX","OE + AX","A Ing + AX","A + AX","UH + AX",
        //France Nasal Vowel Phones: "E nas","A nas","O nas","OE nas",
        //Other Phones: "WH","ESH","EZH","ET","SC","ZC","LT","SHX","HZ"
        private string[] phonems_total = new string[]{
            //Monophthong Vowel Phones
    "I","Y","IX","YX","UU","U","IH","YH","UH","E","EU","EX","OX","OU","O","AX","AX rho","EH","OE","ER","ER rho","UR","AH","AO","AE","AEX","A","AOE","AA","Q",
            //Diphthong Vowel Phones
    "E + I", "A + UH", "AO + I", "A + I", "I + AX", "Y + AX", "EH + AX", "U + AX", "O + AX", "AO + AX",
            //Consonant Phones
    "P","B","M","BB","PH","BH","MF","F","V","VA","TH","DH","T","D","N","RR","DX","S","Z","LSH","LH","RA","L","L vel","SH","SH pal","ZH","ZH pal","TR","DR","NR","DXR","SR","ZR","R","LR","RR rho","CT","JD","NJ","C","CJ","J","LJ","W","K","G","NG","X","GH","GA","GL","QT","QN","QQ","QH","RH","HH","HG","GT","H","WJ",
            //Affricates
    "PF","TS","CH","JH","JJ","DZ","CC","TSR","JC",
            //Click and Ejective Phones
    "PCK","TCK","NCK","CCK","LCK","BIM","DIM","QIM","QIM vls","GIM","JIM","P ejc","K ejc","S ejc","QT ejc"
        };


        private string[] phonems_monophthong = new string[]{
            //Monophthong Vowel Phones
    "I","Y","IX","YX","UU","U","IH","YH","UH","E","EU","EX","OX","OU","O","AX","AX rho","EH","OE","ER","ER rho","UR","AH","AO","AE","AEX","A","AOE","AA","Q"
        };

        private string[] phonems_diphthong = new string[]{
            //Diphthong Vowel Phones
    "E + I", "A + UH", "AO + I", "A + I", "I + AX", "Y + AX", "EH + AX", "U + AX", "O + AX", "AO + AX"
        };

        private string[] phonems_consonant = new string[]{
            //Consonant Phones
    "P","B","M","BB","PH","BH","MF","F","V","VA","TH","DH","T","D","N","RR","DX","S","Z","LSH","LH","RA","L","L vel","SH","SH pal","ZH","ZH pal","TR","DR","NR","DXR","SR","ZR","R","LR","RR rho","CT","JD","NJ","C","CJ","J","LJ","W","K","G","NG","X","GH","GA","GL","QT","QN","QQ","QH","RH","HH","HG","GT","H","WJ"
        };

        private string[] phonems_affricates = new string[]{
            //Affricates
    "PF","TS","CH","JH","JJ","DZ","CC","TSR","JC"
        };

        private string[] phonems_ejective = new string[]{
            //Click and Ejective Phones
    "PCK","TCK","NCK","CCK","LCK","BIM","DIM","QIM","QIM vls","GIM","JIM","P ejc","K ejc","S ejc","QT ejc"
        };

        private string[] letters = new string[]{
    "a","b","c","d","e","f","g","i","j","k","l","m","n","o","p","q","r","s","t","u","w","v","x","y","z"};

        private Grammar CreateGrammar_Monophthong()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("monophthong");

            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_monophthong.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_monophthong[i];
                string pronunciation = phonems_monophthong[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Diphthong()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("diphthong");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_diphthong.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_diphthong[i];
                string pronunciation = phonems_diphthong[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Constant()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("constant");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_consonant.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_consonant[i];
                string pronunciation = phonems_consonant[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Affricates()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("affricates");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_affricates.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_affricates[i];
                string pronunciation = phonems_affricates[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Ejective()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("ejective");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_ejective.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_ejective[i];
                string pronunciation = phonems_ejective[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }

        private Grammar CreateGrammar_Total2D()
        { 
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("total2D");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_total.Length;
            for (int i = 0; i < count; i++)
            {
                for (int i2 = 0; i2 < count; i2++)
                {
                    string text = phonems_total[i] + phonems_total[i2];
                    string pronunciation = phonems_total[i] + " " + phonems_total[i2];

                    SrgsToken token = new SrgsToken(text);
                    token.Pronunciation = pronunciation;
                    SrgsItem item = new SrgsItem(token);
                    oneOf.Add(item);
                }
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Basic2D()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("basic2D");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_basic.Length;
            for (int i = 0; i < count; i++)
            {
                for (int i2 = 0; i2 < count; i2++)
                {
                    string text = phonems_basic[i] + phonems_basic[i2];
                    string pronunciation = phonems_basic[i] + " " + phonems_basic[i2];

                    SrgsToken token = new SrgsToken(text);
                    token.Pronunciation = pronunciation;
                    SrgsItem item = new SrgsItem(token);
                    oneOf.Add(item);
                }
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }


        public Grammar[] CreateGrammar_Basic2DX()
        {
            List<Grammar> grammars = new List<Grammar>();
            List<List<string>> words = new List<List<string>>();

            int count = phonems_basic.Length;
            for (int i = 0; i < count; i++)
            {
                SrgsDocument doc = new SrgsDocument();
                doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
                SrgsRule rule = new SrgsRule("basic2D" + i);
                SrgsOneOf oneOf = new SrgsOneOf();

                for (int i2 = 0; i2 < count; i2++)
                {
                    string text = phonems_basic[i] + phonems_basic[i2];
                    string pronunciation = phonems_basic[i] + " " + phonems_basic[i2];

                    SrgsToken token = new SrgsToken(text);
                    token.Pronunciation = pronunciation;
                    SrgsItem item = new SrgsItem(token);
                    oneOf.Add(item);
                }

                rule.Add(oneOf);
                doc.Rules.Add(rule);
                doc.Root = rule;
                doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                Grammar grammar = new Grammar(doc);
                grammars.Add(grammar);
            }

            return grammars.ToArray();
        }

        public Grammar[] CreateGrammar_Alphabetic()
        {
            List<Grammar> grammars = new List<Grammar>();
            List<string[]> alphabet = new List<string[]>();
            //consonants
            alphabet.Add(new string[] { "b", "bb" });
            alphabet.Add(new string[] { "d", "dd","ed" });
            alphabet.Add(new string[] { "f", "ff","ph","gh","lf","ft" });
            alphabet.Add(new string[] { "g", "gg","gh","gu","gue" });
            alphabet.Add(new string[] { "h", "wh" });
            alphabet.Add(new string[] { "j", "ge","g","dge","di","gg" });
            alphabet.Add(new string[] { "k", "c","ch","cc","lk","qu","ck","X" });
            alphabet.Add(new string[] { "l", "ll" });
            alphabet.Add(new string[] { "m", "mm","mb","mn","lm" });
            alphabet.Add(new string[] { "n", "nn","kn","gn","pn","ng","ngue" });
            alphabet.Add(new string[] { "p", "pp" });
            alphabet.Add(new string[] { "r", "rr","wr","rh" });
            alphabet.Add(new string[] { "s", "ss", "c","sc","ps","st","ce","se" });
            alphabet.Add(new string[] { "t", "tt","th","ed" });
            alphabet.Add(new string[] { "v", "f","ph","ve" });
            alphabet.Add(new string[] { "w", "wh","u","o" });
            alphabet.Add(new string[] { "y", "i","j" });
            alphabet.Add(new string[] { "z", "zz","s","ss","x","ze","se" });
            //digraphs
            alphabet.Add(new string[] { "zh", "s","si","z" });
            alphabet.Add(new string[] { "ch", "tch","tu","ti","te" });
            alphabet.Add(new string[] { "sh", "ce","s","ci","si","ch","sci","ti" });
            alphabet.Add(new string[] { "th" });
            //short vowels
            alphabet.Add(new string[] { "a", "ai" });
            alphabet.Add(new string[] { "e", "ea","u","ie","ai","a","eo","ei","ai","a","eo","ei","ae","ay" });
            alphabet.Add(new string[] { "i", "e","o","u","ui","y","ie" });
            alphabet.Add(new string[] { "o", "a","ho" });
            alphabet.Add(new string[] { "u", "o","oo","ou" });
            alphabet.Add(new string[] { "oo", "u","ou","o" });
            //long vowels
            alphabet.Add(new string[] { "ai", "a","eigh","aigh","ay","et","ei","et","ei","au","a-e","ea","ey" });
            alphabet.Add(new string[] { "ee", "e","ea","y","ey","oe","ie","i","ei","eo","ay" });
            alphabet.Add(new string[] { "i", "y","igh","ie","uy","ye","ai","is","eigh","i-e" });
            alphabet.Add(new string[] { "oa", "o-e","o","oe","ow","ough","eau","oo","ew" });
            alphabet.Add(new string[] { "oo", "ew", "ue", "u-e", "oe", "ough", "ui", "o", "oeu", "ou" });
                alphabet.Add(new string[] { "u", "you","ew","iew","yu","eue","eau","ieu","eu" });
            alphabet.Add(new string[] { "oi", "oy","uoy" });
            alphabet.Add(new string[] { "ow", "ou","ough" });
            alphabet.Add(new string[] { "er", "ar","our","or","i","e","u","ur","re","eur" });
            //R' controlled vowels
            alphabet.Add(new string[] { "air", "are","ear","ere","eir","ayer" });
            alphabet.Add(new string[] { "ar", "a","au","er","ear" });
            alphabet.Add(new string[] { "ir", "er","ur","ear","or","our","ur" });
            alphabet.Add(new string[] { "aw", "a","or","oor","ore","oar","our","augh","ar","ough","au" });
            alphabet.Add(new string[] { "ear", "eer","ere","ier" });
            alphabet.Add(new string[] { "ure", "our" });

            foreach (var l in alphabet)
            {
                string[] words = l.ToArray();
                Choices choice = new Choices(words);
                GrammarBuilder builder = new GrammarBuilder(choice, 0, 1);
              
                Grammar grammar = new Grammar(builder);
                grammars.Add(grammar);
            }

            return grammars.ToArray();
        }


        public Grammar[] CreateGrammar_AlphabeticBasic()
        {
            List<Grammar> grammars = new List<Grammar>();
            List<string[]> alphabet = new List<string[]>();
            //consonants
            alphabet.Add(new string[] { "a", "ai", "eig", "aig", "au", "ae", "ea", "ey" });
            alphabet.Add(new string[] { "b", "bb" });
            alphabet.Add(new string[] { "c", "sh", "ce", "s", "ci", "si", "ch", "sci", "ti", "te" });
            alphabet.Add(new string[] { "d", "dd", "ed" });
            alphabet.Add(new string[] { "e", "ee", "ea", "eo", "ei", "eo", "ei", "et", "ay" });
            alphabet.Add(new string[] { "f", "ff", "ph", "gh", "lf", "ft" });
            alphabet.Add(new string[] { "g", "gg", "gh", "gu", "gue" });
            alphabet.Add(new string[] { "h", "wh", "ch" });
            alphabet.Add(new string[] { "i", "ui", "ie", "igh", "ie", "uy", "ye", "is", "eigh" });
            alphabet.Add(new string[] { "j", "ge", "g", "dge", "di", "gg" });
            alphabet.Add(new string[] { "k", "cc", "lk", "qu", "ck", "X" });
            alphabet.Add(new string[] { "l", "ll" });
            alphabet.Add(new string[] { "m", "mm", "mb", "mn", "lm" });
            alphabet.Add(new string[] { "n", "nn", "kn", "gn", "pn", "ng", "ngue" });
            alphabet.Add(new string[] { "o", "ho","oo","ou","oa", "oe", "ow", "ough", "eau", "ew", "oi", "oy", "uoy","aw" });
            alphabet.Add(new string[] { "p", "pp" });
            alphabet.Add(new string[] { "r", "rr", "wr", "rh", "er", "ar", "our", "or", "ur", "re", "eur", "air", "are", "ear", "ere", "eir", "ayer" });
            alphabet.Add(new string[] { "s", "ss", "c", "sc", "ps", "st", "ce", "se" });
            alphabet.Add(new string[] { "t", "tt", "th", "ed", "th" });
            alphabet.Add(new string[] { "u", "you", "ew", "iew", "yu", "eue", "eau", "ieu", "eu" });
            alphabet.Add(new string[] { "v", "f", "ph", "ve" });
            alphabet.Add(new string[] { "w", "wh", "u", "o" });
            alphabet.Add(new string[] { "x", "ex" });
            alphabet.Add(new string[] { "y" });
            alphabet.Add(new string[] { "z", "zz", "si", "ze", "se","zh" });
            
 

            foreach (var l in alphabet)
            {
                string[] words = l.ToArray();
                Choices choice = new Choices(words);
                GrammarBuilder builder = new GrammarBuilder(choice, 0, 1);

                Grammar grammar = new Grammar(builder);
                grammars.Add(grammar);
            }

            return grammars.ToArray();
        }

        public Grammar[] CreateGrammar_Alphabetic3D()
        {
            List<Grammar> grammars = new List<Grammar>();
            List<List<string>> words = new List<List<string>>();

            int count = letters.Length;
            string text1, text2;
            List<string> letter1D = new List<string>();


            for (int i = 0; i < count; i++)
            {
                List<string> letter2D = new List<string>();
                List<string> letter3D = new List<string>();
                letter1D.Add(letters[i]);
                for (int i2 = 0; i2 < count; i2++)
                {
                    text1 = letters[i] + letters[i2];
                    letter2D.Add(text1);

                    for (int i3 = 0; i3 < count; i3++)
                    {
                        text2 = letters[i] + letters[i2] + letters[i3];
                        letter3D.Add(text2);
                    }

                    
                }

                words.Add(letter3D);
                words.Add(letter2D);
            }

            words.Add(letter1D);



            foreach (var w in words)
            {
                Choices choice = new Choices(w.ToArray());
                GrammarBuilder builder = new GrammarBuilder(choice, 0, 1);

                Grammar grammar = new Grammar(builder);
                grammars.Add(grammar);
            }

            return grammars.ToArray();
        }

        public Grammar[] CreateGrammar_Alphabetic2D()
        {
            List<Grammar> grammars = new List<Grammar>();
            List<List<string>> words = new List<List<string>>();

            int count = letters.Length;
            string text1;
            //List<string> letter1D = new List<string>();


            for (int i = 0; i < count; i++)
            {
                List<string> letter2D = new List<string>();
                //letter1D.Add(letters[i]);
                for (int i2 = 0; i2 < count; i2++)
                {
                    text1 = letters[i] + letters[i2];
                    letter2D.Add(text1);
                }

                words.Add(letter2D);
            }

            //words.Add(letter1D);



            foreach (var w in words)
            {
                Choices choice = new Choices(w.ToArray());
                GrammarBuilder builder = new GrammarBuilder(choice, 0, 5);

                Grammar grammar = new Grammar(builder);
                grammars.Add(grammar);
            }

            return grammars.ToArray();
        }
    }
}

/*
private Grammar CreateGrammar_Monophthong()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("check");

            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_monophthong.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_monophthong[i];
                string pronunciation = phonems_monophthong[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Diphthong()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("check");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_diphthong.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_diphthong[i];
                string pronunciation = phonems_diphthong[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Constant()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("check");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_consonant.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_consonant[i];
                string pronunciation = phonems_consonant[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Affricates()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("check");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_affricates.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_affricates[i];
                string pronunciation = phonems_affricates[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Ejective()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("check");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_ejective.Length;
            for (int i = 0; i < count; i++)
            {
                string text = phonems_ejective[i];
                string pronunciation = phonems_ejective[i];

                SrgsToken token = new SrgsToken(text);
                token.Pronunciation = pronunciation;
                SrgsItem item = new SrgsItem(token);
                oneOf.Add(item);
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }

        private Grammar CreateGrammar_Total2D()
        { 
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("check");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_total.Length;
            for (int i = 0; i < count; i++)
            {
                for (int i2 = 0; i2 < count; i2++)
                {
                    string text = phonems_total[i] + phonems_total[i2];
                    string pronunciation = phonems_total[i] + " " + phonems_total[i2];

                    SrgsToken token = new SrgsToken(text);
                    token.Pronunciation = pronunciation;
                    SrgsItem item = new SrgsItem(token);
                    oneOf.Add(item);
                }
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
        private Grammar CreateGrammar_Basic2D()
        {
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("check");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems_basic.Length;
            for (int i = 0; i < count; i++)
            {
                for (int i2 = 0; i2 < count; i2++)
                {
                    string text = phonems_basic[i] + phonems_basic[i2];
                    string pronunciation = phonems_basic[i] + " " + phonems_basic[i2];

                    SrgsToken token = new SrgsToken(text);
                    token.Pronunciation = pronunciation;
                    SrgsItem item = new SrgsItem(token);
                    oneOf.Add(item);
                }
            }

            rule.Add(oneOf);
            doc.Rules.Add(rule);
            doc.Root = rule;
            doc.Culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            Grammar grammar = new Grammar(doc);
            return grammar;
        }
*/

/*
private string[] UPS2D_number0 = {
            "DER",
            "DAX RA",
            "DHER",
            "EH RAAU",
            "BH",
            "EIAX L"
        };

        private string[] UPS2D_number1 = {
            "WW",
        };

        private string[] UPS2D_number2 = {
            "HH",
            "O + UU",
            "EH RAAO",
            "DHAE",
            "HAX RA"

        };

        private string[] UPS2D_number3 = {
            "AX LI",
            "AX ND",
            "AX LOI",
            "DHIH",
            "EID",
            "AXER"

        };

        private string[] UPS2D_number4 = {
            "AX LAI",
            "AX LAX", //8?
            "AX LAA"
        };

        private string[] UPS2D_number5 = {
            "VH",
            "AX LAE"
        };

        private string[] UPS2D_number6 = {
            "FEH"
        };
        private string[] UPS2D_number7 = {
            "DAX M",
            "GAX M",
            "DHEH",
            "THAE",
            "HAH",
            "TAX",
            "HAE"
        };

        private string[] UPS2D_number8 = {
            "EHEH"
        };

        private string[] UPS2D_number9 = {
            "AIAX M",
            "AX NH",
            "ERAA",
            "AAR"
        };
*/
