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
    public partial class DonateButtonBitcoin : UserControl
    {
        public DonateButtonBitcoin()
        {
            InitializeComponent();
            TBtnDonate.Click += TBtnDonate_Click;
        }
        
        private void TBtnDonate_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
@"Do you want to copy this bitcoin donation addres to your clipboard ?

1G8txscQs54oLDXBzDtxDDhZ2P5noiYvfd

Thank you for your support, evry single satoshi helps, and motivates me to create this applications big time.

If you have a great idea, how to improve this program, or you wish for a new feature, please send me an email: asmodat@gmail.com",
"Bitcoin (BTC) donation wallet.", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                Clipboard.SetText("1G8txscQs54oLDXBzDtxDDhZ2P5noiYvfd");
        }
    }
}
