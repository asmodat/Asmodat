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
        //english
        /*private string[] phonems = new string[]{
    "AA", "AE", "AH", "AI", "AU", "AO", "AX", "AX RA", "EH", "EH RA",
    "EI", "ER", "I", "IH", "O + U", "OI", "U", "UH", "AX L", "AX M",
    "AX N", "B", "CH", "D", "DH", "F", "G", "H", "J", "JH", "K", "L",
    "M", "N", "NG", "P", "R", "S", "SH", "T", "TH", "V", "W", "Z", "ZH"};*/

        //German Diphthong Vowels: "IH + AX","YH + AX","EY Ing + AX","EH Ing + AX","EU Ing + AX","OE + AX","A Ing + AX","A + AX","UH + AX",
        //France Nasal Vowel Phones: "E nas","A nas","O nas","OE nas",
        //Other Phones: "WH","ESH","EZH","ET","SC","ZC","LT","SHX","HZ"
        private string[] phonems = new string[]{
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

        private Grammar CreateGrammar2D()
        { 
            SrgsDocument doc = new SrgsDocument();
            doc.PhoneticAlphabet = SrgsPhoneticAlphabet.Ups;
            SrgsRule rule = new SrgsRule("check");
            SrgsOneOf oneOf = new SrgsOneOf();
            int count = phonems.Length;
            for (int i = 0; i < count; i++)
            {
                for (int i2 = 0; i2 < count; i2++)
                {
                    string text = phonems[i] + phonems[i2];
                    string pronunciation = phonems[i] + " " + phonems[i2];

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


        private Dictionary<int, string[]> CreateNumbersUPS_2D()
        {
            Dictionary<int, string[]> ups_numbers = new Dictionary<int, string[]>();
            ups_numbers.Add(0, UPS2D_number0);
            ups_numbers.Add(1, UPS2D_number1);
            ups_numbers.Add(2, UPS2D_number2);
            ups_numbers.Add(3, UPS2D_number3);
            ups_numbers.Add(4, UPS2D_number4);
            ups_numbers.Add(5, UPS2D_number5);
            ups_numbers.Add(6, UPS2D_number6);
            ups_numbers.Add(7, UPS2D_number7);
            ups_numbers.Add(8, UPS2D_number8);
            ups_numbers.Add(9, UPS2D_number9);

            return ups_numbers;
        }


        private string[] UPS2D_number0 = {
            "BC",
            "EH + AXA + UH"
        };

        private string[] UPS2D_number1 = {
        };

        private string[] UPS2D_number2 = {
            "CAX rho",
            "DHAE"

        };

        private string[] UPS2D_number3 = {
            "ERI + AX",
            "AO + II",
            "LSHAO + I",
            "UN"

        };

        private string[] UPS2D_number4 = {
        };

        private string[] UPS2D_number5 = {
            "LSHAE",
            "ARR"
        };

        private string[] UPS2D_number6 = {
            "PHEH + AX"
        };

        private string[] UPS2D_number7 = {
            "AEE",
            "CAH",
            "TEU",
            "CAE",
            "DHEH",
            "ER rhoLSH"
        };

        private string[] UPS2D_number8 = {
            "EPH",
            "EHEH"
        };

        private string[] UPS2D_number9 = {
            "ER rhoEU"
        };

    }
}


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
