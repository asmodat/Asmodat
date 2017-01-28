using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asmodat.WPFControls
{
    
    public partial class DonateButtonPaypal : UserControl
    {
        public DonateButtonPaypal()
        {
            InitializeComponent();
            TBtnDonate.Click += TBtnDonate_Click;
        }




        private void TBtnDonate_Click(object sender, RoutedEventArgs e)
        {
            string sURL = "";
            string business = "asmodat@gmail.com";
            string description = "Support, Development funding and Donation Appreciation for Asmodat Software";
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
