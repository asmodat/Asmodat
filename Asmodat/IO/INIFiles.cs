using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Asmodat.IO
{
    /// <summary>
    /// Use Example:
    /// private void btnRead_Click(object sender, EventArgs e)
    ///     {
    ///            StringBuilder returnStr = new StringBuilder(1024);
    ///
    ///            Ini iniFile = new Ini("C:\\test.ini");
    ///            iniFile.Read("Layout", "backgroundColor", returnStr);
    ///            txtBoxBackgroundColor.Text = returnStr.ToString();
    ///            iniFile.Read("Layout", "textColor", returnStr);
    ///            txtBoxTextColor.Text = returnStr.ToString();
    ///     }
    ///
    /// private void btnWrite_Click(object sender, EventArgs e)
    ///     {
    ///         Ini iniFile = new Ini();
    ///         iniFile.SetFilepath("C:\\test.ini");
    ///         iniFile.Write("Layout", "backgroundColor", txtBoxBackgroundColor.Text);
    ///         iniFile.Write("Layout", "textColor", txtBoxTextColor.Text);
    ///         txtBoxBackgroundColor.Text = "";
    ///         txtBoxTextColor.Text = "";
    ///         MessageBox.Show("Colors Saved", "IniTest");
    ///     }
    /// </summary>
    public class Ini
    {
        [DllImport("kernel32")]
        static extern UInt32 GetPrivateProfileString(String section, String keyname, String defaultName,
            StringBuilder returnValue, UInt32 size, String filepatch);

        [DllImport("kernel32")]
        static extern Boolean WritePrivateProfileString(String section, String keyname, String value,
            String filename);

        private string iniFile;

        public Ini()
        {
            this.iniFile = "";
        }

        public Ini(string iniFile)
        {
            this.SetFilepath(iniFile);
        }

        public void SetFilepath(string iniFile)
        {
            this.iniFile = iniFile;
        }

        public UInt32 Read(string section, string key, StringBuilder returnStr)
        {
            return (GetPrivateProfileString(section, key, "", returnStr, (UInt32)returnStr.MaxCapacity, this.iniFile));
        }

        public Boolean Write(string section, string key, string value)
        {
            return (WritePrivateProfileString(section, key, value, this.iniFile));
        }


        public Boolean DeleteKey(string section, string key)
        {
            return (Write(section, key, null));
        }

        public Boolean DeleteSection(string section)
        {
            return (Write(section, null, null));
        }
    }
}
