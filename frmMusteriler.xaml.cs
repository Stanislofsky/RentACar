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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;

namespace AracKiralama
{
    /// <summary>
    /// Interaction logic for frmMusteriler.xaml
    /// </summary>
    public partial class frmMusteriler : Window
    {
        public frmMusteriler()
        {
            InitializeComponent();
        }
        SqlConnection baglanti;
        SqlCommand komut = new SqlCommand();
        void baglan()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
            }
        }
        //----------------------------------------------------------------------
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            baglanti = new SqlConnection();
            baglanti.ConnectionString = AnaMenu.baglantiBilgisi;
            komut.Connection = baglanti;
            VeriGetir();
        }
        //----------------------------------------------------------------------
        void VeriGetir()
        {
            try
            {
                DataTable dt_Musteriler = new DataTable("MUSTERI");
                SqlDataAdapter adaptor = new SqlDataAdapter("Select MKod,Tcno,Ad,Soyad,Tel,Adres from MUSTERI", baglanti);
                baglan();
                adaptor.Fill(dt_Musteriler);
                DgridMusteriler.ItemsSource = dt_Musteriler.DefaultView;
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Oluştu.." + hata.Message);
            }
        }
        //----------------------------------------------------------------------
        private void btnKytEkle_Click(object sender, RoutedEventArgs e)
        {
            veriEkle();
            VeriGetir();
        }
        //----------------------------------------------------------------------
        void veriEkle()
        {
            komut.Connection = baglanti;
            komut.CommandText = "insert into MUSTERI(MKod,Tcno,Ad,Soyad,Adres,Tel) values(@MKod,@Tcno,@Ad,@Soyad,@Adres,@Tel)";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@MKod", txtMusKod.Text);
            komut.Parameters.AddWithValue("@Tcno", txtTcNo.Text);
            komut.Parameters.AddWithValue("@Ad", txtAd.Text);
            komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@Tel", txtTelefon.Text);
            baglan();
            komut.ExecuteNonQuery();
            baglanti.Close();
            verileriTemizle();
        }
        //----------------------------------------------------------------------
        void verileriTemizle()
        {
            txtMusKod.Clear();
            txtTcNo.Clear();
            txtAd.Clear();
            txtSoyad.Clear();
            txtTelefon.Clear();
            txtAdres.Clear();
            txtMusKod.Focus();
        }
        //----------------------------------------------------------------------
        private void btnKytGuncelle_Click(object sender, RoutedEventArgs e)
        {
            komut.CommandText = "Update MUSTERI set TcNo=@TcNo,Ad=@Ad,Soyad=@Soyad,Adres=@Adres,Tel=@Tel Where MKod=@MKod";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@TcNo", txtTcNo.Text);
            komut.Parameters.AddWithValue("@Ad", txtAd.Text);
            komut.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
            komut.Parameters.AddWithValue("@Adres", txtAdres.Text);
            komut.Parameters.AddWithValue("@Tel", txtTelefon.Text);
            komut.Parameters.AddWithValue("@MKod", seciliKayit);
            baglan();
            if (komut.ExecuteNonQuery() >= 1)
                MessageBox.Show("Kayıt Güncellendi");
            else
                MessageBox.Show("Kayıt  Güncellenemedi");
            baglanti.Close();
            VeriGetir();
            verileriTemizle();
            DgridMusteriler.Focus();
        }

        private void btnKytSil_Click(object sender, RoutedEventArgs e)
        {
            komut.CommandText = "Delete From MUSTERI Where Mkod=@Mkod";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@Mkod", txtMusKod.Text);
            baglan();
            if (komut.ExecuteNonQuery() >= 1)
                MessageBox.Show("Kayıt Silindi");
            else
                MessageBox.Show("Kayıt Bulunamadı.");
            baglanti.Close();
            VeriGetir();
            verileriTemizle();
        }

        private void btnTemizle_Click(object sender, RoutedEventArgs e)
        {
            verileriTemizle();
            seciliKayit = "0";
        }
        String seciliKayit;
        private void DgridMusteriler_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView rowview = DgridMusteriler.SelectedItem as DataRowView;
            txtMusKod.Text = rowview.Row["MKod"].ToString();
            txtTcNo.Text = rowview.Row["Tcno"].ToString();
            txtAd.Text = rowview.Row["Ad"].ToString();
            txtSoyad.Text = rowview.Row["Soyad"].ToString();
            txtTelefon.Text = rowview.Row["Tel"].ToString();
            txtAdres.Text = rowview.Row["Adres"].ToString();
            seciliKayit = rowview.Row["MKod"].ToString();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }

        private void txtFilitre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DataTable dt_Musteriler = new DataTable("MUSTERI");
                SqlDataAdapter adaptor = new SqlDataAdapter("Select * from MUSTERI Where Ad like @Ad", baglanti);
                adaptor.SelectCommand.Parameters.AddWithValue("@Ad", txtFilitre.Text + "%");
                baglan();
                adaptor.Fill(dt_Musteriler);
                DgridMusteriler.ItemsSource = dt_Musteriler.DefaultView;
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Oluştu.." + hata.Message);
            }
        }

       
    }
}
