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
    public partial class DonateButtonMonero : UserControl
    {
        public DonateButtonMonero()
        {
            InitializeComponent();
            TBtnDonate.Click += TBtnDonate_Click;
        }
        
        private void TBtnDonate_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
@"Do you want to copy this monero donation addres to your clipboard ?

4AsrE9PyZJ68bEYgArLJg13xceLyPJqMk4n9LVYBNaRMXcwS3o3p87ZUgAyjGpksyVAuEDxe4q9rcfJCVrv5GNzB2VzUcSV

Thank you for your support, evry single decinero helps, and motivates me to create this applications big time.",
"Monero (XMR) donation wallet.", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
                Clipboard.SetText("4AsrE9PyZJ68bEYgArLJg13xceLyPJqMk4n9LVYBNaRMXcwS3o3p87ZUgAyjGpksyVAuEDxe4q9rcfJCVrv5GNzB2VzUcSV");
        }
    }
}
