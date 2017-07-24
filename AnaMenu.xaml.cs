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

namespace AracKiralama
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AnaMenu : Window
    {
        public AnaMenu()
        {
            InitializeComponent();
        }

        private void _cik(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void aracAc(object sender, RoutedEventArgs e)
        {
            frmAraclar Araclar = new frmAraclar();
            Araclar.ShowDialog();
        }
        public static string baglantiBilgisi;
        private void ilkDegerler(object sender, RoutedEventArgs e)  
        {
            baglantiBilgisi = "Data Source=.;Initial Catalog=ARACKIRALAMA;uid=sa;pwd=123";
        }

        private void musteriAc(object sender, RoutedEventArgs e)
        {
            frmMusteriler MusteriKart = new frmMusteriler();
            MusteriKart.ShowDialog();
        }
    }
}
