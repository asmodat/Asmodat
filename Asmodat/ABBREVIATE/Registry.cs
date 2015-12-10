using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;
using System.Windows.Forms;
namespace Asmodat.Abbreviate
{
    public class Registry
    {
        static RegistryKey RegBaseKey = Microsoft.Win32.Registry.LocalMachine;
        static string sRegSubKey = "SOFTWARE\\" + typeof(Asmodat.Abbreviate.Registry).Namespace;

        public Registry() { }
        public Registry(string mainFolder)
        {
            sRegSubKey += "\\" + mainFolder;

        }

        public static bool AccessTest(bool bShowException)
        {
            if (!AddKey("Access Test", "test", bShowException)) return false;
            if (GetKey("Access Test", bShowException) != "test") return false;
            if (!RemoveKey("Access Test", bShowException)) return false;

            return true;
        }


        public static bool AddKey(string sKeyName, string sValue, bool bShowException = true)
        {
            try
            {
                RegistryKey RKBase = RegBaseKey;
                RegistryKey RKSub = RKBase.CreateSubKey(sRegSubKey); //open or create
                RKSub.SetValue(sKeyName, sValue);
                return true;
            }
            catch (Exception e)
            {
                ShowErrorInfo(e, "Writing to Windows Registry Exception (AddKey [" + sKeyName +"]).", bShowException);
                return false;
            }
        }

        public static string GetKey(string sKeyName, bool bShowException = true)
        {
            try
            {
                RegistryKey RKBase = RegBaseKey;
                RegistryKey RKSub = RKBase.CreateSubKey(sRegSubKey); //open or create

                if (RKSub == null) return null;


                return (string)RKSub.GetValue(sKeyName);//sKeyName.ToUpper()
            }
            catch (Exception e)
            {
                ShowErrorInfo(e, "Reading from Windows Registry Exception (GetKey [" + sKeyName + "]).", bShowException);
                return null;
            }
        }

        public static bool RemoveKey(string sKeyName, bool bShowException = true)
        {
            try
            {
                RegistryKey RKBase = RegBaseKey;
                RegistryKey RKSub = RKBase.CreateSubKey(sRegSubKey); //open or create

                if (RKSub == null) return false;


                RKSub.DeleteValue(sKeyName);//sKeyName.ToUpper()
                return true;
            }
            catch (Exception e)
            {
                ShowErrorInfo(e, "Deleting key from Windows Registry Exception (RemoveKey [" + sKeyName + "]).", bShowException);
                return false;
            }
        }


        private static void ShowErrorInfo(Exception e, string sTittle, bool bShowException = true)
        {
            if (!bShowException) return;

            MessageBox.Show("Running program as Administratir, could solve this exception\n" + e.Message, sTittle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool LaunchAtStartup(bool removeAppFromRegistry = false, string appName = null, string patchExe = null, bool bShowException = true)
        {
            try
            {
                if (appName == null)
                    appName = Application.ProductName;
                if (patchExe == null)
                    patchExe = Application.ExecutablePath.ToString();

                RegistryKey RKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (!removeAppFromRegistry) RKey.SetValue(appName, patchExe);
                else RKey.SetValue(appName, false);

                return true;
            }
            catch (Exception e)
            {
                ShowErrorInfo(e, "Setting startup application in Windows Registry Exception (RemoveKey [" + appName + "]).", bShowException);
                return false;
            }

            
        }
    }
}
