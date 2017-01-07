using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asmodat.Donate
{
    public partial class FormsPaypalButton : UserControl
    {
        public FormsPaypalButton()
        {
            InitializeComponent();

            //BtnPayPalDonate.Image = Asmodat.Properties.Resources.paypal_donate_button_100;
            //BtnPayPalDonate.BackgroundImageLayout = ImageLayout.Stretch;
            //BtnPayPalDonate.Width = Asmodat.Properties.Resources.paypal_donate_button_100.Width + 1;
            //BtnPayPalDonate.Height = Asmodat.Properties.Resources.paypal_donate_button_100.Height + 1;
            //BtnPayPalDonate.Text = "";
        }

        private void BtnPayPalDonate_Click(object sender, EventArgs e)
        {
            string sURL = "";
            string business = "asmodat@gmail.com";
            string description = "Support and Donation Appreciation for Asmodat Software";
            string country = "US";
            string currency = "USD";

            sURL += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                //"&amount=" + 1 +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=PP%2dDonationsBF";

            sURL = sURL.Replace(" ", "%20");

            System.Diagnostics.Process.Start(sURL);
        }
    }
}
