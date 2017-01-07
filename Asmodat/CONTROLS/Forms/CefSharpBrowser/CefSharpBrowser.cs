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
        public CefSharpBrowser()
        {
            InitializeComponent();

            
        }

      
        private void CefSharpBrowser_SizeChanged(object sender, EventArgs e)
        {
            if(Controls?.Contains(Browser) == true)
                ResetBrowserApperance();
        }

        public ChromiumWebBrowser Browser { get; private set; }
        public CefSettings Settings { get; private set; }

        

       

        public void ResetBrowserApperance()
        {
            if (Browser == null)
                return;

            TTbxNavigationBar.Location = new Point(0, 0);
            TTbxNavigationBar.Width = this.Size.Width;

            Browser.Dock = DockStyle.None;
            Browser.Anchor = AnchorStyles.None;
            Browser.Location = new Point(0, TTbxNavigationBar.Height + TTbxNavigationBar.Location.Y);
            Browser.Size = new Size(this.Size.Width, this.Size.Height - (TTbxNavigationBar.Size.Height + TTbxNavigationBar.Location.Y));
            Browser.Refresh();
        }
    }
}
