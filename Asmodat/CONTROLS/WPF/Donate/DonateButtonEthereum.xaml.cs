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
    public partial class DonateButtonEthereum : UserControl
    {
        public DonateButtonEthereum()
        {
            InitializeComponent();
            TBtnDonate.Click += TBtnDonate_Click;
        }
        
        private void TBtnDonate_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
@"Do you want to copy this ethereum donation addres to your clipboard ?

0x5A946c1259E707c4B58a5fa4E44a26eAE9CacC2E

Thank you for your support, evry single finney helps, and motivates me to create this applications big time.",
"Ethereum (ETH) donation wallet.", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                Clipboard.SetText("0x5A946c1259E707c4B58a5fa4E44a26eAE9CacC2E");
        }
    }
}
