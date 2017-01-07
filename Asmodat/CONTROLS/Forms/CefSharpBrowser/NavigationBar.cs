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
using Asmodat.Extensions.Objects;

namespace Asmodat.CONTROLS.Forms
{
    public partial class CefSharpBrowser : UserControl
    {
        private void Browser_AddressChanged(object sender, AddressChangedEventArgs e)
        {
            if (Browser == null)
                return;



            TTbxNavigationBar.Text = Browser.Address;
        }


        private void TTbxNavigationBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (Browser == null)
                return;


            if(e.KeyCode == Keys.Enter)
            {
                this.LoadPage(TTbxNavigationBar.Text);
            }
        }


   

        public string DefaultURL = "www.google.com";

        public void LoadPage(string url)
        {
            if (Browser == null)
                return;

            if (url.IsNullOrEmpty()) url = DefaultURL;

            if (!url.ContainsAny(new string[] { "www","." }, false))
                url = "www.google.com/search?q=" + url;
            
            
            Browser.Load(url);
        }
    }
}
