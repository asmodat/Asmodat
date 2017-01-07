using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;

namespace Asmodat.CONTROLS.Forms
{
    public partial class CefSharpBrowser : UserControl
    {
        public void Initialize(string startupPage = "www.google.com")
        {
            if (Browser != null)
                return;

            this.Settings = new CefSettings();
            this.Settings.WindowlessRenderingEnabled = true;
            this.Settings.CefCommandLineArgs.Add("disable-gpu", "1");
            Cef.Initialize(this.Settings);
            Browser = new ChromiumWebBrowser(startupPage);
            Browser.AddressChanged += Browser_AddressChanged;
            Browser.LoadingStateChanged += Browser_LoadingStateChanged;

            this.Controls.Add(Browser);
            ResetBrowserApperance();

            this.SizeChanged += CefSharpBrowser_SizeChanged;
            this.TTbxNavigationBar.KeyDown += TTbxNavigationBar_KeyDown;
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading)
                TTbxNavigationBar.BackColor = Color.LightPink;
            else
                TTbxNavigationBar.BackColor = Color.White;
        }
    }
}
