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
    /// Interaction logic for frmAraclar.xaml
    /// </summary>
    public partial class frmAraclar : Window
    {
        public frmAraclar()
        {
            InitializeComponent();
        }
        SqlConnection baglanti;
        SqlCommand komut=new SqlCommand();
        void baglan()
        {
            if (baglanti.State == ConnectionState.Closed)
            {
                baglanti.Open();
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
            komut.CommandText = "insert into Araclar(AracNo,Marka,Model,Plaka) values(@AracNo,@Marka,@Model,@Plaka)";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@AracNo", txtAracNo.Text);
            komut.Parameters.AddWithValue("@Marka", txtMarka.Text);
            komut.Parameters.AddWithValue("@Model", txtModel.Text);
            komut.Parameters.AddWithValue("@Plaka", txtPlaka.Text);
            baglan();
            komut.ExecuteNonQuery();
            baglanti.Close();
            verileriTemizle();
        }
        //----------------------------------------------------------------------
        void verileriTemizle()
        {
            txtAracNo.Clear();
            txtMarka.Clear();
            txtModel.Clear();
            txtPlaka.Clear();
            txtAracNo.Focus();
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
                DataTable dt_Adresler = new DataTable("Araclar");
                SqlDataAdapter adaptor = new SqlDataAdapter("Select * from Araclar", baglanti);
                baglan();
                adaptor.Fill(dt_Adresler);
                DgridAraclar.ItemsSource = dt_Adresler.DefaultView;
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Oluştu.." + hata.Message);
            }
        }
        //----------------------------------------------------------------------
        String seciliKayit;
        private void DgridAraclar_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView rowview = DgridAraclar.SelectedItem as DataRowView;
            txtAracNo.Text = rowview.Row["AracNo"].ToString();
            txtMarka.Text = rowview.Row["Marka"].ToString();
            txtModel.Text = rowview.Row["Model"].ToString();
            txtPlaka.Text = rowview.Row["Plaka"].ToString();
            seciliKayit = rowview.Row["AracNo"].ToString();
        }
        //----------------------------------------------------------------------
        private void btnKytSil_Click(object sender, RoutedEventArgs e)
        {
            komut.CommandText = "Delete From Araclar Where AracNo=@AracNo";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@AracNo", txtAracNo.Text);
            baglan();
            if (komut.ExecuteNonQuery() >= 1)
                MessageBox.Show("Kayıt Silindi");
            else
                MessageBox.Show("Kayıt Bulunamadı.");
            baglanti.Close();
            VeriGetir();
            verileriTemizle();
        }

        private void btnKytGuncelle_Click(object sender, RoutedEventArgs e)
        {
            komut.CommandText = "Update Araclar set Marka=@Marka,Model=@Model,Plaka=@Plaka Where AracNo=@AracNo";
            komut.Parameters.Clear();
            komut.Parameters.AddWithValue("@Marka", txtMarka.Text);
            komut.Parameters.AddWithValue("@Model", txtModel.Text);
            komut.Parameters.AddWithValue("@Plaka", txtPlaka.Text);
            komut.Parameters.AddWithValue("@AracNo", seciliKayit);
            baglan();
            if (komut.ExecuteNonQuery() >= 1)
                MessageBox.Show("Kayıt Güncellendi");
            else
                MessageBox.Show("Kayıt  Güncellenemedi");
            baglanti.Close();
            VeriGetir();
            verileriTemizle();
            DgridAraclar.Focus();

        }

        private void btnTemizle_Click(object sender, RoutedEventArgs e)
        {
            verileriTemizle();
            seciliKayit = "0";
        }   

        private void txtFilitre_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                DataTable dt_Adresler = new DataTable("Araclar");
                SqlDataAdapter adaptor = new SqlDataAdapter("Select * from Araclar Where Marka like @Marka", baglanti);
                adaptor.SelectCommand.Parameters.AddWithValue("@Marka", txtFilitre.Text+"%");
                baglan();
                adaptor.Fill(dt_Adresler);
                DgridAraclar.ItemsSource = dt_Adresler.DefaultView;
                baglanti.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show("Hata Oluştu.." + hata.Message);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                Close();
        }
        //----------------------------------------------------------------------
    }
}
