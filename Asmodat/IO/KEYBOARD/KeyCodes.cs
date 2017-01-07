using System;
using System.Collections.Generic;
 
using System.Text;
using System.Windows.Forms;

namespace Asmodat
{

    public class VirtualKeyCodes
    {
        public string[,] Codes;
        public int CodesCounter = 0;
        //public string[,] CodesBasic;
        public int CodesCounterBasic = 0;
        public string[,] CodesDx;
        public int CounterCodesDx = 0;

        public Dictionary<int, string> Data = new Dictionary<int, string>();
        //public Dictionary<int, string> DataBasic = new Dictionary<int, string>();

        public enum Key
        {
            A = 65,
            a = 97,
            I = 73,
            i = 105,
            L = 76,
            l = 108,
            m = 109,
            M = 77,
            O = 79,
            P = 80,
            S = 83,
            W = 87,
            up = 38,
            down = 40,
            left = 37,
            right = 39,
            Enter = 13,
            Space = 32,
            Esc = 27,
            F10 = 121,
            F11 = 122,
            F12 = 123,
            CMB = 4,
            MBF = 5,
            MBB = 6,
            LControlKey = 17
        }


        public VirtualKeyCodes()
        {
            Codes = this.SetCodes();
            //CodesBasic = this.SetCodesBasic();
            CodesDx = this.SetCodesDX();
            Data = this.SetData();
            //DataBasic = this.SetDataBasic();
        }

        private Dictionary<int, string> SetData()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();

            for(int i = 0; i < CodesCounter; i++)
            {
                int key = Convert.ToInt32(Codes[i, 1], 16);
                string value = Codes[i, 0];

                data.Add(key, value);
            }

            return data;
        }

        

        private string[,] SetCodes()
        {
            string[,] KeyCodes = new string[500, 2];

            int i = -1;
            KeyCodes[++i, 0] = "Cancel"; KeyCodes[i, 1] = "03";
            KeyCodes[++i, 0] = "Back"; KeyCodes[i, 1] = "08";
            KeyCodes[++i, 0] = "Tab"; KeyCodes[i, 1] = "09";
            KeyCodes[++i, 0] = "Clear"; KeyCodes[i, 1] = "0C";
            KeyCodes[++i, 0] = "Enter"; KeyCodes[i, 1] = "0D";
            //KeyCodes[++i, 0] = "Return"; KeyCodes[i, 1] = "0E"; & 0F are ubndefined
            KeyCodes[++i, 0] = "Shift"; KeyCodes[i, 1] = "10";
            KeyCodes[++i, 0] = "Control"; KeyCodes[i, 1] = "11";
            KeyCodes[++i, 0] = "Menu"; KeyCodes[i, 1] = "12";
            KeyCodes[++i, 0] = "Pause"; KeyCodes[i, 1] = "13";
            KeyCodes[++i, 0] = "CapsLock"; KeyCodes[i, 1] = "14";
            //KeyCodes[++i, 0] = "Capital"; KeyCodes[i, 1] = "14"; diffrent name but its capslock
            KeyCodes[++i, 0] = "Escape"; KeyCodes[i, 1] = "1B";
            KeyCodes[++i, 0] = "IMEConvert"; KeyCodes[i, 1] = "1C";
            KeyCodes[++i, 0] = "IMEAccept"; KeyCodes[i, 1] = "1E";
            KeyCodes[++i, 0] = "Space"; KeyCodes[i, 1] = "20";
            KeyCodes[++i, 0] = "PageUp"; KeyCodes[i, 1] = "21";
            KeyCodes[++i, 0] = "Next"; KeyCodes[i, 1] = "22";
            KeyCodes[++i, 0] = "End"; KeyCodes[i, 1] = "23";
            KeyCodes[++i, 0] = "Home"; KeyCodes[i, 1] = "24";
            KeyCodes[++i, 0] = "Left"; KeyCodes[i, 1] = "25";
            KeyCodes[++i, 0] = "Up"; KeyCodes[i, 1] = "26";
            KeyCodes[++i, 0] = "Right"; KeyCodes[i, 1] = "27";
            KeyCodes[++i, 0] = "Down"; KeyCodes[i, 1] = "28";
            KeyCodes[++i, 0] = "Select"; KeyCodes[i, 1] = "29";
            KeyCodes[++i, 0] = "Print"; KeyCodes[i, 1] = "2A";
            KeyCodes[++i, 0] = "Execute"; KeyCodes[i, 1] = "2B";
            KeyCodes[++i, 0] = "SnapShot"; KeyCodes[i, 1] = "2C";
            KeyCodes[++i, 0] = "Insert"; KeyCodes[i, 1] = "2D";
            KeyCodes[++i, 0] = "Delete"; KeyCodes[i, 1] = "2E";
            KeyCodes[++i, 0] = "Help"; KeyCodes[i, 1] = "2F";

            KeyCodes[++i, 0] = "D0"; KeyCodes[i, 1] = "30";
            KeyCodes[++i, 0] = "D1"; KeyCodes[i, 1] = "31";
            KeyCodes[++i, 0] = "D2"; KeyCodes[i, 1] = "32";
            KeyCodes[++i, 0] = "D3"; KeyCodes[i, 1] = "33";
            KeyCodes[++i, 0] = "D4"; KeyCodes[i, 1] = "34";
            KeyCodes[++i, 0] = "D5"; KeyCodes[i, 1] = "35";
            KeyCodes[++i, 0] = "D6"; KeyCodes[i, 1] = "36";
            KeyCodes[++i, 0] = "D7"; KeyCodes[i, 1] = "37";
            KeyCodes[++i, 0] = "D8"; KeyCodes[i, 1] = "38";
            KeyCodes[++i, 0] = "D9"; KeyCodes[i, 1] = "39";

            KeyCodes[++i, 0] = "A"; KeyCodes[i, 1] = "41";
            KeyCodes[++i, 0] = "B"; KeyCodes[i, 1] = "42";
            KeyCodes[++i, 0] = "C"; KeyCodes[i, 1] = "43";
            KeyCodes[++i, 0] = "D"; KeyCodes[i, 1] = "44";
            KeyCodes[++i, 0] = "E"; KeyCodes[i, 1] = "45";
            KeyCodes[++i, 0] = "F"; KeyCodes[i, 1] = "46";
            KeyCodes[++i, 0] = "G"; KeyCodes[i, 1] = "47";
            KeyCodes[++i, 0] = "H"; KeyCodes[i, 1] = "48";
            KeyCodes[++i, 0] = "I"; KeyCodes[i, 1] = "49";
            KeyCodes[++i, 0] = "J"; KeyCodes[i, 1] = "4A";
            KeyCodes[++i, 0] = "K"; KeyCodes[i, 1] = "4B";
            KeyCodes[++i, 0] = "L"; KeyCodes[i, 1] = "4C";
            KeyCodes[++i, 0] = "M"; KeyCodes[i, 1] = "4D";
            KeyCodes[++i, 0] = "N"; KeyCodes[i, 1] = "4E";
            KeyCodes[++i, 0] = "O"; KeyCodes[i, 1] = "4F";
            KeyCodes[++i, 0] = "P"; KeyCodes[i, 1] = "50";
            KeyCodes[++i, 0] = "Q"; KeyCodes[i, 1] = "51";
            KeyCodes[++i, 0] = "R"; KeyCodes[i, 1] = "52";
            KeyCodes[++i, 0] = "S"; KeyCodes[i, 1] = "53";
            KeyCodes[++i, 0] = "T"; KeyCodes[i, 1] = "54";
            KeyCodes[++i, 0] = "U"; KeyCodes[i, 1] = "55";
            KeyCodes[++i, 0] = "V"; KeyCodes[i, 1] = "56";
            KeyCodes[++i, 0] = "W"; KeyCodes[i, 1] = "57";
            KeyCodes[++i, 0] = "X"; KeyCodes[i, 1] = "58";
            KeyCodes[++i, 0] = "Y"; KeyCodes[i, 1] = "59";
            KeyCodes[++i, 0] = "Z"; KeyCodes[i, 1] = "5A";

            KeyCodes[++i, 0] = "LWin"; KeyCodes[i, 1] = "5B";
            KeyCodes[++i, 0] = "RWin"; KeyCodes[i, 1] = "5C";
            KeyCodes[++i, 0] = "Apps"; KeyCodes[i, 1] = "5D";
            KeyCodes[++i, 0] = "Sleep"; KeyCodes[i, 1] = "5F";

            KeyCodes[++i, 0] = "Numpad0"; KeyCodes[i, 1] = "60";
            KeyCodes[++i, 0] = "Numpad1"; KeyCodes[i, 1] = "61";
            KeyCodes[++i, 0] = "Numpad2"; KeyCodes[i, 1] = "62";
            KeyCodes[++i, 0] = "Numpad3"; KeyCodes[i, 1] = "63";
            KeyCodes[++i, 0] = "Numpad4"; KeyCodes[i, 1] = "64";
            KeyCodes[++i, 0] = "Numpad5"; KeyCodes[i, 1] = "65";
            KeyCodes[++i, 0] = "Numpad6"; KeyCodes[i, 1] = "66";
            KeyCodes[++i, 0] = "Numpad7"; KeyCodes[i, 1] = "67";
            KeyCodes[++i, 0] = "Numpad8"; KeyCodes[i, 1] = "68";
            KeyCodes[++i, 0] = "Numpad9"; KeyCodes[i, 1] = "69";

            KeyCodes[++i, 0] = "Multiply"; KeyCodes[i, 1] = "6A";
            KeyCodes[++i, 0] = "Add"; KeyCodes[i, 1] = "6B";
            KeyCodes[++i, 0] = "Separator"; KeyCodes[i, 1] = "6C";
            KeyCodes[++i, 0] = "Subtract"; KeyCodes[i, 1] = "6D";
            KeyCodes[++i, 0] = "Decimal"; KeyCodes[i, 1] = "6E";
            KeyCodes[++i, 0] = "Divide"; KeyCodes[i, 1] = "6F";

            KeyCodes[++i, 0] = "F1"; KeyCodes[i, 1] = "70";
            KeyCodes[++i, 0] = "F2"; KeyCodes[i, 1] = "71";
            KeyCodes[++i, 0] = "F3"; KeyCodes[i, 1] = "72";
            KeyCodes[++i, 0] = "F4"; KeyCodes[i, 1] = "73";
            KeyCodes[++i, 0] = "F5"; KeyCodes[i, 1] = "74";
            KeyCodes[++i, 0] = "F6"; KeyCodes[i, 1] = "75";
            KeyCodes[++i, 0] = "F7"; KeyCodes[i, 1] = "76";
            KeyCodes[++i, 0] = "F8"; KeyCodes[i, 1] = "77";
            KeyCodes[++i, 0] = "F9"; KeyCodes[i, 1] = "78";
            KeyCodes[++i, 0] = "F10"; KeyCodes[i, 1] = "79";
            KeyCodes[++i, 0] = "F11"; KeyCodes[i, 1] = "7A";
            KeyCodes[++i, 0] = "F12"; KeyCodes[i, 1] = "7B";
            KeyCodes[++i, 0] = "F13"; KeyCodes[i, 1] = "7C";
            KeyCodes[++i, 0] = "F14"; KeyCodes[i, 1] = "7D";
            KeyCodes[++i, 0] = "F15"; KeyCodes[i, 1] = "7E";
            KeyCodes[++i, 0] = "F16"; KeyCodes[i, 1] = "7F";
            KeyCodes[++i, 0] = "F17"; KeyCodes[i, 1] = "80";
            KeyCodes[++i, 0] = "F18"; KeyCodes[i, 1] = "81";
            KeyCodes[++i, 0] = "F19"; KeyCodes[i, 1] = "82";
            KeyCodes[++i, 0] = "F20"; KeyCodes[i, 1] = "83";
            KeyCodes[++i, 0] = "F21"; KeyCodes[i, 1] = "84";
            KeyCodes[++i, 0] = "F22"; KeyCodes[i, 1] = "85";
            KeyCodes[++i, 0] = "F23"; KeyCodes[i, 1] = "86";
            KeyCodes[++i, 0] = "F24"; KeyCodes[i, 1] = "87";

            KeyCodes[++i, 0] = "NumLock"; KeyCodes[i, 1] = "90";
            KeyCodes[++i, 0] = "Scroll"; KeyCodes[i, 1] = "91";
            KeyCodes[++i, 0] = "LShiftKey"; KeyCodes[i, 1] = "A0";
            KeyCodes[++i, 0] = "RShiftKey"; KeyCodes[i, 1] = "A1";
            KeyCodes[++i, 0] = "LControlKey"; KeyCodes[i, 1] = "A2";
            KeyCodes[++i, 0] = "RControlKey"; KeyCodes[i, 1] = "A3";
            KeyCodes[++i, 0] = "LMenu"; KeyCodes[i, 1] = "A4";
            //KeyCodes[++i, 0] = "Menu, Alt"; KeyCodes[i, 1] = "A4";
            KeyCodes[++i, 0] = "RMenu"; KeyCodes[i, 1] = "A5";
            KeyCodes[++i, 0] = "BrowserBack"; KeyCodes[i, 1] = "A6";
            KeyCodes[++i, 0] = "BrowserForward"; KeyCodes[i, 1] = "A7";
            KeyCodes[++i, 0] = "BrowserRefresh"; KeyCodes[i, 1] = "A8";
            KeyCodes[++i, 0] = "BrowserStop"; KeyCodes[i, 1] = "A9";
            KeyCodes[++i, 0] = "BrowserSearch"; KeyCodes[i, 1] = "AA";
            KeyCodes[++i, 0] = "BrowserFavorites"; KeyCodes[i, 1] = "AB";
            KeyCodes[++i, 0] = "BrowserHome"; KeyCodes[i, 1] = "AC";
            KeyCodes[++i, 0] = "VolumeMute"; KeyCodes[i, 1] = "AD";
            KeyCodes[++i, 0] = "VolumeDown"; KeyCodes[i, 1] = "AE";
            KeyCodes[++i, 0] = "VolumeUp"; KeyCodes[i, 1] = "AF";
            KeyCodes[++i, 0] = "MediaNextTrack"; KeyCodes[i, 1] = "B0";
            KeyCodes[++i, 0] = "MediaPrevTrack"; KeyCodes[i, 1] = "B1";
            KeyCodes[++i, 0] = "MediaStop"; KeyCodes[i, 1] = "B2";
            KeyCodes[++i, 0] = "MediaPlayPause"; KeyCodes[i, 1] = "B3";
            KeyCodes[++i, 0] = "LaunchMail"; KeyCodes[i, 1] = "B4";
            KeyCodes[++i, 0] = "Oem1"; KeyCodes[i, 1] = "BA";
            KeyCodes[++i, 0] = "Oemplus"; KeyCodes[i, 1] = "BB";
            KeyCodes[++i, 0] = "Oemcomma"; KeyCodes[i, 1] = "BC";
            KeyCodes[++i, 0] = "OemMinus"; KeyCodes[i, 1] = "BD";
            KeyCodes[++i, 0] = "OemPeriod"; KeyCodes[i, 1] = "BE";

            KeyCodes[++i, 0] = "Oem2"; KeyCodes[i, 1] = "BF";
            KeyCodes[++i, 0] = "Oem3"; KeyCodes[i, 1] = "C0";
            KeyCodes[++i, 0] = "Oem4"; KeyCodes[i, 1] = "DB";
            KeyCodes[++i, 0] = "Oem5"; KeyCodes[i, 1] = "DC";
            KeyCodes[++i, 0] = "Oem6"; KeyCodes[i, 1] = "DD";
            KeyCodes[++i, 0] = "Oem7"; KeyCodes[i, 1] = "DE";
            KeyCodes[++i, 0] = "Oem8"; KeyCodes[i, 1] = "DF";
            KeyCodes[++i, 0] = "Oem102"; KeyCodes[i, 1] = "E2";

            KeyCodes[++i, 0] = "ProcessKey"; KeyCodes[i, 1] = "E5";
            KeyCodes[++i, 0] = "Packet"; KeyCodes[i, 1] = "E7";
            KeyCodes[++i, 0] = "Crsel"; KeyCodes[i, 1] = "F7";
            KeyCodes[++i, 0] = "Exsel"; KeyCodes[i, 1] = "F8";
            KeyCodes[++i, 0] = "Play"; KeyCodes[i, 1] = "FA";
            KeyCodes[++i, 0] = "Zoom"; KeyCodes[i, 1] = "FB";
            KeyCodes[++i, 0] = "Noname"; KeyCodes[i, 1] = "FC";
            KeyCodes[++i, 0] = "Pa1"; KeyCodes[i, 1] = "FD";
            KeyCodes[++i, 0] = "OemClear"; KeyCodes[i, 1] = "FE";
            CodesCounter = i;

            return KeyCodes;
        }

        private string[,] SetCodesDX()
        {
            string[,] KeyCodes = new string[500, 2];



            int i = -1;
            /*KeyCodes[++i, 0] = "Cancel"; KeyCodes[i, 1] = "03";
            KeyCodes[++i, 0] = "Back"; KeyCodes[i, 1] = "08";
            KeyCodes[++i, 0] = "Tab"; KeyCodes[i, 1] = "09";
            KeyCodes[++i, 0] = "Clear"; KeyCodes[i, 1] = "0C";
            KeyCodes[++i, 0] = "Return"; KeyCodes[i, 1] = "0D";
            KeyCodes[++i, 0] = "Menu"; KeyCodes[i, 1] = "12";
            KeyCodes[++i, 0] = "Pause"; KeyCodes[i, 1] = "13";
            KeyCodes[++i, 0] = "IMEConvert"; KeyCodes[i, 1] = "1C";
            KeyCodes[++i, 0] = "IMEAccept"; KeyCodes[i, 1] = "1E";
            KeyCodes[++i, 0] = "Space"; KeyCodes[i, 1] = "20";
            KeyCodes[++i, 0] = "PageUp"; KeyCodes[i, 1] = "21";
            KeyCodes[++i, 0] = "Next"; KeyCodes[i, 1] = "22";
            KeyCodes[++i, 0] = "Home"; KeyCodes[i, 1] = "24";
            KeyCodes[++i, 0] = "Select"; KeyCodes[i, 1] = "29";
            KeyCodes[++i, 0] = "Print"; KeyCodes[i, 1] = "2A";
            KeyCodes[++i, 0] = "PrintScreen"; KeyCodes[i, 1] = "2B";
            KeyCodes[++i, 0] = "Execute"; KeyCodes[i, 1] = "2B";
            KeyCodes[++i, 0] = "SnapShot"; KeyCodes[i, 1] = "2C";
            KeyCodes[++i, 0] = "Insert"; KeyCodes[i, 1] = "2D";
            KeyCodes[++i, 0] = "Delete"; KeyCodes[i, 1] = "2E";
            KeyCodes[++i, 0] = "Help"; KeyCodes[i, 1] = "2F";*/

            KeyCodes[++i, 0] = "D0"; KeyCodes[i, 1] = "11";  //DX-ed
            KeyCodes[++i, 0] = "D1"; KeyCodes[i, 1] = "2";
            KeyCodes[++i, 0] = "D2"; KeyCodes[i, 1] = "3";
            KeyCodes[++i, 0] = "D3"; KeyCodes[i, 1] = "4";
            KeyCodes[++i, 0] = "D4"; KeyCodes[i, 1] = "5";
            KeyCodes[++i, 0] = "D5"; KeyCodes[i, 1] = "6";
            KeyCodes[++i, 0] = "D6"; KeyCodes[i, 1] = "7";
            KeyCodes[++i, 0] = "D7"; KeyCodes[i, 1] = "8";
            KeyCodes[++i, 0] = "D8"; KeyCodes[i, 1] = "9";
            KeyCodes[++i, 0] = "D9"; KeyCodes[i, 1] = "10";

            KeyCodes[++i, 0] = "A"; KeyCodes[i, 1] = "30"; //DX-ed
            KeyCodes[++i, 0] = "B"; KeyCodes[i, 1] = "48";
            KeyCodes[++i, 0] = "C"; KeyCodes[i, 1] = "46";
            KeyCodes[++i, 0] = "D"; KeyCodes[i, 1] = "32";
            KeyCodes[++i, 0] = "E"; KeyCodes[i, 1] = "18";
            KeyCodes[++i, 0] = "F"; KeyCodes[i, 1] = "33";
            KeyCodes[++i, 0] = "G"; KeyCodes[i, 1] = "34";
            KeyCodes[++i, 0] = "H"; KeyCodes[i, 1] = "35";
            KeyCodes[++i, 0] = "I"; KeyCodes[i, 1] = "23";
            KeyCodes[++i, 0] = "J"; KeyCodes[i, 1] = "36";
            KeyCodes[++i, 0] = "K"; KeyCodes[i, 1] = "37";
            KeyCodes[++i, 0] = "L"; KeyCodes[i, 1] = "38";
            KeyCodes[++i, 0] = "M"; KeyCodes[i, 1] = "50";
            KeyCodes[++i, 0] = "N"; KeyCodes[i, 1] = "49";
            KeyCodes[++i, 0] = "O"; KeyCodes[i, 1] = "24";
            KeyCodes[++i, 0] = "P"; KeyCodes[i, 1] = "25";
            KeyCodes[++i, 0] = "Q"; KeyCodes[i, 1] = "16";
            KeyCodes[++i, 0] = "R"; KeyCodes[i, 1] = "19";
            KeyCodes[++i, 0] = "S"; KeyCodes[i, 1] = "31";
            KeyCodes[++i, 0] = "T"; KeyCodes[i, 1] = "20";
            KeyCodes[++i, 0] = "U"; KeyCodes[i, 1] = "22";
            KeyCodes[++i, 0] = "V"; KeyCodes[i, 1] = "47";
            KeyCodes[++i, 0] = "W"; KeyCodes[i, 1] = "17";
            KeyCodes[++i, 0] = "X"; KeyCodes[i, 1] = "45";
            KeyCodes[++i, 0] = "Y"; KeyCodes[i, 1] = "21";
            KeyCodes[++i, 0] = "Z"; KeyCodes[i, 1] = "44";

            KeyCodes[++i, 0] = "Escape"; KeyCodes[i, 1] = "1"; //DX-ed
            //KeyCodes[++i, 0] = "End"; KeyCodes[i, 1] = "207";
            //KeyCodes[++i, 0] = "Control"; KeyCodes[i, 1] = "29";
            //KeyCodes[++i, 0] = "LControlKey"; KeyCodes[i, 1] = "29";
            //KeyCodes[++i, 0] = "RControlKey"; KeyCodes[i, 1] = "157";
            //KeyCodes[++i, 0] = "Menu, Alt"; KeyCodes[i, 1] = "56"; // R 184
            //KeyCodes[++i, 0] = "CapsLock"; KeyCodes[i, 1] = "58";
            //KeyCodes[++i, 0] = "Capital"; KeyCodes[i, 1] = "58";
            //KeyCodes[++i, 0] = "Shift"; KeyCodes[i, 1] = "42";
            //KeyCodes[++i, 0] = "LShiftKey"; KeyCodes[i, 1] = "42";
            //KeyCodes[++i, 0] = "RShiftKey"; KeyCodes[i, 1] = "54";
            //KeyCodes[++i, 0] = "Enter"; KeyCodes[i, 1] = "156";
            //KeyCodes[++i, 0] = "Space"; KeyCodes[i, 1] = "57";
            //KeyCodes[++i, 0] = "Separator"; KeyCodes[i, 1] = "57";

            KeyCodes[++i, 0] = "F1"; KeyCodes[i, 1] = "59"; //DX-ed
            KeyCodes[++i, 0] = "F2"; KeyCodes[i, 1] = "60";
            KeyCodes[++i, 0] = "F3"; KeyCodes[i, 1] = "61";
            KeyCodes[++i, 0] = "F4"; KeyCodes[i, 1] = "62";
            KeyCodes[++i, 0] = "F5"; KeyCodes[i, 1] = "63";
            KeyCodes[++i, 0] = "F6"; KeyCodes[i, 1] = "64";
            KeyCodes[++i, 0] = "F7"; KeyCodes[i, 1] = "65";
            KeyCodes[++i, 0] = "F8"; KeyCodes[i, 1] = "66";
            KeyCodes[++i, 0] = "F9"; KeyCodes[i, 1] = "67";
            KeyCodes[++i, 0] = "F10"; KeyCodes[i, 1] = "68";
            KeyCodes[++i, 0] = "F11"; KeyCodes[i, 1] = "87";
            KeyCodes[++i, 0] = "F12"; KeyCodes[i, 1] = "88";

            KeyCodes[++i, 0] = "Left"; KeyCodes[i, 1] = "203"; //DX-ed
            KeyCodes[++i, 0] = "Up"; KeyCodes[i, 1] = "200";
            KeyCodes[++i, 0] = "Right"; KeyCodes[i, 1] = "205";
            KeyCodes[++i, 0] = "Down"; KeyCodes[i, 1] = "208";


            /*KeyCodes[++i, 0] = "LWin"; KeyCodes[i, 1] = "5B";
            KeyCodes[++i, 0] = "RWin"; KeyCodes[i, 1] = "5C";
            KeyCodes[++i, 0] = "Apps"; KeyCodes[i, 1] = "5D";
            KeyCodes[++i, 0] = "Sleep"; KeyCodes[i, 1] = "5F";

            KeyCodes[++i, 0] = "Numpad0"; KeyCodes[i, 1] = "60";
            KeyCodes[++i, 0] = "Numpad1"; KeyCodes[i, 1] = "61";
            KeyCodes[++i, 0] = "Numpad2"; KeyCodes[i, 1] = "62";
            KeyCodes[++i, 0] = "Numpad3"; KeyCodes[i, 1] = "63";
            KeyCodes[++i, 0] = "Numpad4"; KeyCodes[i, 1] = "64";
            KeyCodes[++i, 0] = "Numpad5"; KeyCodes[i, 1] = "65";
            KeyCodes[++i, 0] = "Numpad6"; KeyCodes[i, 1] = "66";
            KeyCodes[++i, 0] = "Numpad7"; KeyCodes[i, 1] = "67";
            KeyCodes[++i, 0] = "Numpad8"; KeyCodes[i, 1] = "68";
            KeyCodes[++i, 0] = "Numpad9"; KeyCodes[i, 1] = "69";

            KeyCodes[++i, 0] = "Multiply"; KeyCodes[i, 1] = "6A";
            KeyCodes[++i, 0] = "Add"; KeyCodes[i, 1] = "6B";
            
            KeyCodes[++i, 0] = "Subtract"; KeyCodes[i, 1] = "6D";
            KeyCodes[++i, 0] = "Decimal"; KeyCodes[i, 1] = "6E";
            KeyCodes[++i, 0] = "Divide"; KeyCodes[i, 1] = "6F";

            
            KeyCodes[++i, 0] = "F13"; KeyCodes[i, 1] = "7C";
            KeyCodes[++i, 0] = "F14"; KeyCodes[i, 1] = "7D";
            KeyCodes[++i, 0] = "F15"; KeyCodes[i, 1] = "7E";
            KeyCodes[++i, 0] = "F16"; KeyCodes[i, 1] = "7F";
            KeyCodes[++i, 0] = "F17"; KeyCodes[i, 1] = "80";
            KeyCodes[++i, 0] = "F18"; KeyCodes[i, 1] = "81";
            KeyCodes[++i, 0] = "F19"; KeyCodes[i, 1] = "82";
            KeyCodes[++i, 0] = "F20"; KeyCodes[i, 1] = "83";
            KeyCodes[++i, 0] = "F21"; KeyCodes[i, 1] = "84";
            KeyCodes[++i, 0] = "F22"; KeyCodes[i, 1] = "85";
            KeyCodes[++i, 0] = "F23"; KeyCodes[i, 1] = "86";
            KeyCodes[++i, 0] = "F24"; KeyCodes[i, 1] = "87";

            KeyCodes[++i, 0] = "NumLock"; KeyCodes[i, 1] = "90";
            KeyCodes[++i, 0] = "Scroll"; KeyCodes[i, 1] = "91";
            KeyCodes[++i, 0] = "LMenu"; KeyCodes[i, 1] = "A4";
            KeyCodes[++i, 0] = "Menu, Alt"; KeyCodes[i, 1] = "A4";
            KeyCodes[++i, 0] = "RMenu"; KeyCodes[i, 1] = "A5";
            KeyCodes[++i, 0] = "BrowserBack"; KeyCodes[i, 1] = "A6";
            KeyCodes[++i, 0] = "BrowserForward"; KeyCodes[i, 1] = "A7";
            KeyCodes[++i, 0] = "BrowserRefresh"; KeyCodes[i, 1] = "A8";
            KeyCodes[++i, 0] = "BrowserStop"; KeyCodes[i, 1] = "A9";
            KeyCodes[++i, 0] = "BrowserSearch"; KeyCodes[i, 1] = "AA";
            KeyCodes[++i, 0] = "BrowserFavorites"; KeyCodes[i, 1] = "AB";
            KeyCodes[++i, 0] = "BrowserHome"; KeyCodes[i, 1] = "AC";
            KeyCodes[++i, 0] = "VolumeMute"; KeyCodes[i, 1] = "AD";
            KeyCodes[++i, 0] = "VolumeDown"; KeyCodes[i, 1] = "AE";
            KeyCodes[++i, 0] = "VolumeUp"; KeyCodes[i, 1] = "AF";
            KeyCodes[++i, 0] = "MediaNextTrack"; KeyCodes[i, 1] = "B0";
            KeyCodes[++i, 0] = "MediaPrevTrack"; KeyCodes[i, 1] = "B1";
            KeyCodes[++i, 0] = "MediaStop"; KeyCodes[i, 1] = "B2";
            KeyCodes[++i, 0] = "MediaPlayPause"; KeyCodes[i, 1] = "B3";
            KeyCodes[++i, 0] = "LaunchMail"; KeyCodes[i, 1] = "B4";
            KeyCodes[++i, 0] = "Oem1"; KeyCodes[i, 1] = "BA";
            KeyCodes[++i, 0] = "Oemplus"; KeyCodes[i, 1] = "BB";
            KeyCodes[++i, 0] = "Oemcomma"; KeyCodes[i, 1] = "BC";
            KeyCodes[++i, 0] = "OemMinus"; KeyCodes[i, 1] = "BD";
            KeyCodes[++i, 0] = "OemPeriod"; KeyCodes[i, 1] = "BE";

            KeyCodes[++i, 0] = "Oem2"; KeyCodes[i, 1] = "BF";
            KeyCodes[++i, 0] = "Oem3"; KeyCodes[i, 1] = "C0";
            KeyCodes[++i, 0] = "Oemtilde"; KeyCodes[i, 1] = "C0";
            KeyCodes[++i, 0] = "Oem4"; KeyCodes[i, 1] = "DB";
            KeyCodes[++i, 0] = "Oem5"; KeyCodes[i, 1] = "DC";
            KeyCodes[++i, 0] = "Oem6"; KeyCodes[i, 1] = "DD";
            KeyCodes[++i, 0] = "Oem7"; KeyCodes[i, 1] = "DE";
            KeyCodes[++i, 0] = "Oem8"; KeyCodes[i, 1] = "DF";
            KeyCodes[++i, 0] = "Oem102"; KeyCodes[i, 1] = "E2";

            KeyCodes[++i, 0] = "ProcessKey"; KeyCodes[i, 1] = "E5";
            KeyCodes[++i, 0] = "Packet"; KeyCodes[i, 1] = "E7";
            KeyCodes[++i, 0] = "Crsel"; KeyCodes[i, 1] = "F7";
            KeyCodes[++i, 0] = "Exsel"; KeyCodes[i, 1] = "F8";
            KeyCodes[++i, 0] = "Play"; KeyCodes[i, 1] = "FA";
            KeyCodes[++i, 0] = "Zoom"; KeyCodes[i, 1] = "FB";
            KeyCodes[++i, 0] = "Noname"; KeyCodes[i, 1] = "FC";
            KeyCodes[++i, 0] = "Pa1"; KeyCodes[i, 1] = "FD";
            KeyCodes[++i, 0] = "OemClear"; KeyCodes[i, 1] = "FE";*/
            CounterCodesDx = i;

            return KeyCodes;
        }

    }

}


/*
private Dictionary<int, string> SetDataBasic()
        {
            Dictionary<int, string> data = new Dictionary<int, string>();

            for (int i = 0; i < CodesCounterBasic; i++)
            {
                int key = Convert.ToInt32(CodesBasic[i, 1], 16);
                string value = CodesBasic[i, 0];

                data.Add(key, value);
            }

            return data;
        }

        private string[,] SetCodesBasic()
        {
            string[,] KeyCodes = new string[500, 2];

            int i = -1;
            KeyCodes[++i, 0] = "Cancel"; KeyCodes[i, 1] = "03";
            KeyCodes[++i, 0] = "Back"; KeyCodes[i, 1] = "08";
            KeyCodes[++i, 0] = "Tab"; KeyCodes[i, 1] = "09";
            KeyCodes[++i, 0] = "Clear"; KeyCodes[i, 1] = "0C";
            KeyCodes[++i, 0] = "Enter"; KeyCodes[i, 1] = "0D";
            //KeyCodes[++i, 0] = "Shift"; KeyCodes[i, 1] = "10";
            //KeyCodes[++i, 0] = "Ctrl"; KeyCodes[i, 1] = "11";
            //KeyCodes[++i, 0] = "Alt"; KeyCodes[i, 1] = "12";
            KeyCodes[++i, 0] = "Pause"; KeyCodes[i, 1] = "13";
            KeyCodes[++i, 0] = "CapsLock"; KeyCodes[i, 1] = "14";
            KeyCodes[++i, 0] = "Esc"; KeyCodes[i, 1] = "1B";
            KeyCodes[++i, 0] = " "; KeyCodes[i, 1] = "20";
            KeyCodes[++i, 0] = "PageUp"; KeyCodes[i, 1] = "21";
            KeyCodes[++i, 0] = "PageDn"; KeyCodes[i, 1] = "22";
            KeyCodes[++i, 0] = "End"; KeyCodes[i, 1] = "23";
            KeyCodes[++i, 0] = "Home"; KeyCodes[i, 1] = "24";
            KeyCodes[++i, 0] = "Left"; KeyCodes[i, 1] = "25";
            KeyCodes[++i, 0] = "Up"; KeyCodes[i, 1] = "26";
            KeyCodes[++i, 0] = "Right"; KeyCodes[i, 1] = "27";
            KeyCodes[++i, 0] = "Down"; KeyCodes[i, 1] = "28";
            //KeyCodes[++i, 0] = "Select"; KeyCodes[i, 1] = "29";
            //KeyCodes[++i, 0] = "Print"; KeyCodes[i, 1] = "2A";
            //KeyCodes[++i, 0] = "Execute"; KeyCodes[i, 1] = "2B";
            KeyCodes[++i, 0] = "PrtScn"; KeyCodes[i, 1] = "2C";
            KeyCodes[++i, 0] = "Ins"; KeyCodes[i, 1] = "2D";
            KeyCodes[++i, 0] = "Del"; KeyCodes[i, 1] = "2E";
            KeyCodes[++i, 0] = "Help"; KeyCodes[i, 1] = "2F";

            KeyCodes[++i, 0] = "0"; KeyCodes[i, 1] = "30";
            KeyCodes[++i, 0] = "1"; KeyCodes[i, 1] = "31";
            KeyCodes[++i, 0] = "2"; KeyCodes[i, 1] = "32";
            KeyCodes[++i, 0] = "3"; KeyCodes[i, 1] = "33";
            KeyCodes[++i, 0] = "4"; KeyCodes[i, 1] = "34";
            KeyCodes[++i, 0] = "5"; KeyCodes[i, 1] = "35";
            KeyCodes[++i, 0] = "6"; KeyCodes[i, 1] = "36";
            KeyCodes[++i, 0] = "7"; KeyCodes[i, 1] = "37";
            KeyCodes[++i, 0] = "8"; KeyCodes[i, 1] = "38";
            KeyCodes[++i, 0] = "9"; KeyCodes[i, 1] = "39";

            KeyCodes[++i, 0] = "A"; KeyCodes[i, 1] = "41";
            KeyCodes[++i, 0] = "B"; KeyCodes[i, 1] = "42";
            KeyCodes[++i, 0] = "C"; KeyCodes[i, 1] = "43";
            KeyCodes[++i, 0] = "D"; KeyCodes[i, 1] = "44";
            KeyCodes[++i, 0] = "E"; KeyCodes[i, 1] = "45";
            KeyCodes[++i, 0] = "F"; KeyCodes[i, 1] = "46";
            KeyCodes[++i, 0] = "G"; KeyCodes[i, 1] = "47";
            KeyCodes[++i, 0] = "H"; KeyCodes[i, 1] = "48";
            KeyCodes[++i, 0] = "I"; KeyCodes[i, 1] = "49";
            KeyCodes[++i, 0] = "J"; KeyCodes[i, 1] = "4A";
            KeyCodes[++i, 0] = "K"; KeyCodes[i, 1] = "4B";
            KeyCodes[++i, 0] = "L"; KeyCodes[i, 1] = "4C";
            KeyCodes[++i, 0] = "M"; KeyCodes[i, 1] = "4D";
            KeyCodes[++i, 0] = "N"; KeyCodes[i, 1] = "4E";
            KeyCodes[++i, 0] = "O"; KeyCodes[i, 1] = "4F";
            KeyCodes[++i, 0] = "P"; KeyCodes[i, 1] = "50";
            KeyCodes[++i, 0] = "Q"; KeyCodes[i, 1] = "51";
            KeyCodes[++i, 0] = "R"; KeyCodes[i, 1] = "52";
            KeyCodes[++i, 0] = "S"; KeyCodes[i, 1] = "53";
            KeyCodes[++i, 0] = "T"; KeyCodes[i, 1] = "54";
            KeyCodes[++i, 0] = "U"; KeyCodes[i, 1] = "55";
            KeyCodes[++i, 0] = "V"; KeyCodes[i, 1] = "56";
            KeyCodes[++i, 0] = "W"; KeyCodes[i, 1] = "57";
            KeyCodes[++i, 0] = "X"; KeyCodes[i, 1] = "58";
            KeyCodes[++i, 0] = "Y"; KeyCodes[i, 1] = "59";
            KeyCodes[++i, 0] = "Z"; KeyCodes[i, 1] = "5A";

            KeyCodes[++i, 0] = "LWin"; KeyCodes[i, 1] = "5B";
            KeyCodes[++i, 0] = "RWin"; KeyCodes[i, 1] = "5C";
            //KeyCodes[++i, 0] = "Apps"; KeyCodes[i, 1] = "5D";
            //KeyCodes[++i, 0] = "Sleep"; KeyCodes[i, 1] = "5F";

            KeyCodes[++i, 0] = "Num0"; KeyCodes[i, 1] = "60";
            KeyCodes[++i, 0] = "Num1"; KeyCodes[i, 1] = "61";
            KeyCodes[++i, 0] = "Num2"; KeyCodes[i, 1] = "62";
            KeyCodes[++i, 0] = "Num3"; KeyCodes[i, 1] = "63";
            KeyCodes[++i, 0] = "Num4"; KeyCodes[i, 1] = "64";
            KeyCodes[++i, 0] = "Num5"; KeyCodes[i, 1] = "65";
            KeyCodes[++i, 0] = "Num6"; KeyCodes[i, 1] = "66";
            KeyCodes[++i, 0] = "Num7"; KeyCodes[i, 1] = "67";
            KeyCodes[++i, 0] = "Num8"; KeyCodes[i, 1] = "68";
            KeyCodes[++i, 0] = "Num9"; KeyCodes[i, 1] = "69";

            KeyCodes[++i, 0] = "*"; KeyCodes[i, 1] = "6A";
            KeyCodes[++i, 0] = "+"; KeyCodes[i, 1] = "6B";
            KeyCodes[++i, 0] = "-"; KeyCodes[i, 1] = "6C";
            KeyCodes[++i, 0] = "-"; KeyCodes[i, 1] = "6D";
            KeyCodes[++i, 0] = "."; KeyCodes[i, 1] = "6E";
            KeyCodes[++i, 0] = "/"; KeyCodes[i, 1] = "6F";

            KeyCodes[++i, 0] = "F1"; KeyCodes[i, 1] = "70";
            KeyCodes[++i, 0] = "F2"; KeyCodes[i, 1] = "71";
            KeyCodes[++i, 0] = "F3"; KeyCodes[i, 1] = "72";
            KeyCodes[++i, 0] = "F4"; KeyCodes[i, 1] = "73";
            KeyCodes[++i, 0] = "F5"; KeyCodes[i, 1] = "74";
            KeyCodes[++i, 0] = "F6"; KeyCodes[i, 1] = "75";
            KeyCodes[++i, 0] = "F7"; KeyCodes[i, 1] = "76";
            KeyCodes[++i, 0] = "F8"; KeyCodes[i, 1] = "77";
            KeyCodes[++i, 0] = "F9"; KeyCodes[i, 1] = "78";
            KeyCodes[++i, 0] = "F10"; KeyCodes[i, 1] = "79";
            KeyCodes[++i, 0] = "F11"; KeyCodes[i, 1] = "7A";
            KeyCodes[++i, 0] = "F12"; KeyCodes[i, 1] = "7B";
            KeyCodes[++i, 0] = "F13"; KeyCodes[i, 1] = "7C";
            KeyCodes[++i, 0] = "F14"; KeyCodes[i, 1] = "7D";
            KeyCodes[++i, 0] = "F15"; KeyCodes[i, 1] = "7E";
            KeyCodes[++i, 0] = "F16"; KeyCodes[i, 1] = "7F";
            KeyCodes[++i, 0] = "F17"; KeyCodes[i, 1] = "80";
            KeyCodes[++i, 0] = "F18"; KeyCodes[i, 1] = "81";
            KeyCodes[++i, 0] = "F19"; KeyCodes[i, 1] = "82";
            KeyCodes[++i, 0] = "F20"; KeyCodes[i, 1] = "83";
            KeyCodes[++i, 0] = "F21"; KeyCodes[i, 1] = "84";
            KeyCodes[++i, 0] = "F22"; KeyCodes[i, 1] = "85";
            KeyCodes[++i, 0] = "F23"; KeyCodes[i, 1] = "86";
            KeyCodes[++i, 0] = "F24"; KeyCodes[i, 1] = "87";

            KeyCodes[++i, 0] = "NumLock"; KeyCodes[i, 1] = "90";
            //KeyCodes[++i, 0] = "Scroll"; KeyCodes[i, 1] = "91";
            KeyCodes[++i, 0] = "LShift"; KeyCodes[i, 1] = "A0";
            KeyCodes[++i, 0] = "RShift"; KeyCodes[i, 1] = "A1";
            KeyCodes[++i, 0] = "LCtrl"; KeyCodes[i, 1] = "A2";
            KeyCodes[++i, 0] = "RCtrl"; KeyCodes[i, 1] = "A3";
            KeyCodes[++i, 0] = "LAlt"; KeyCodes[i, 1] = "A4";
            KeyCodes[++i, 0] = "RAlt"; KeyCodes[i, 1] = "A5";

            KeyCodes[++i, 0] = ";"; KeyCodes[i, 1] = "BA";
            KeyCodes[++i, 0] = "+"; KeyCodes[i, 1] = "BB";
            KeyCodes[++i, 0] = ","; KeyCodes[i, 1] = "BC";
            KeyCodes[++i, 0] = "-"; KeyCodes[i, 1] = "BD";
            KeyCodes[++i, 0] = "."; KeyCodes[i, 1] = "BE";

            KeyCodes[++i, 0] = "/"; KeyCodes[i, 1] = "BF";
            KeyCodes[++i, 0] = "`"; KeyCodes[i, 1] = "C0";
            KeyCodes[++i, 0] = "["; KeyCodes[i, 1] = "DB";
            KeyCodes[++i, 0] = "\\"; KeyCodes[i, 1] = "DC";
            KeyCodes[++i, 0] = "]"; KeyCodes[i, 1] = "DD";
            KeyCodes[++i, 0] = "\""; KeyCodes[i, 1] = "DE";
            //KeyCodes[++i, 0] = "Oem8"; KeyCodes[i, 1] = "DF";
            //KeyCodes[++i, 0] = "\\"; KeyCodes[i, 1] = "E2";
            
           // KeyCodes[++i, 0] = "Packet"; KeyCodes[i, 1] = "E7";
            //KeyCodes[++i, 0] = "Crsel"; KeyCodes[i, 1] = "F7";
            //KeyCodes[++i, 0] = "Exsel"; KeyCodes[i, 1] = "F8";
            //KeyCodes[++i, 0] = "Play"; KeyCodes[i, 1] = "FA";
            //KeyCodes[++i, 0] = "Zoom"; KeyCodes[i, 1] = "FB";
            //KeyCodes[++i, 0] = "Noname"; KeyCodes[i, 1] = "FC";
            //KeyCodes[++i, 0] = "Pa1"; KeyCodes[i, 1] = "FD";

            KeyCodes[++i, 0] = "Clear"; KeyCodes[i, 1] = "FE";
            CodesCounterBasic = i;

            return KeyCodes;
        }*/
