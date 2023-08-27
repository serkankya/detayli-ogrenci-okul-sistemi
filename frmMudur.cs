using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace okulSistemi
{
    public partial class frmMudur : Form
    {
        public frmMudur()
        {
            InitializeComponent();
        }

        private void verileriListele(string veriler)
        {
            // Veritabanından verileri çekmek için SqlDataAdapter ve DataSet kullanılıyor
            SqlDataAdapter da = new SqlDataAdapter(veriler, sql.con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            // DataGridView kontrolüne veriler bağlanıyor
            dataGridView1.DataSource = ds.Tables[0];
        }

        // Öğrenci Listele Butonu
        private void btnOgrenciListele_Click(object sender, EventArgs e)
        {
            // Arama kutusu görünür hale getiriliyor
            txtOgrenci.Visible = true;

            // VerileriListele metoduna öğrenci verilerini çekme sorgusu gönderiliyor
            verileriListele("SELECT *FROM tbl_ogrenciler WHERE DURUM=1");
        }

        // Yardımcı Listele Butonu
        private void btnYardimciListele_Click(object sender, EventArgs e)
        {
            // Arama kutusu görünür hale getiriliyor
            txtYardimci.Visible = true;

            // VerileriListele metoduna yardımcı verilerini çekme sorgusu gönderiliyor
            verileriListele("SELECT *FROM tbl_yardimcilar WHERE DURUM=1");
        }

        // Öğretmen Listele Butonu
        private void btnOgretmenListele_Click(object sender, EventArgs e)
        {
            // Arama kutusu görünür hale getiriliyor
            txtOgretmen.Visible = true;

            // VerileriListele metoduna öğretmen verilerini çekme sorgusu gönderiliyor
            verileriListele("SELECT *FROM tbl_ogretmenler WHERE DURUM=1");
        }

        // Öğrenci Arama Kutusu Değiştiğinde
        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Öğrenci bilgilerini arama sorgusu ile çekme
                SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_ogrenciler WHERE (NUMARA LIKE @ogrBilgi OR AD LIKE @ogrBilgi OR SOYAD LIKE @ogrBilgi) AND DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@ogrBilgi", "%" + txtOgrenci.Text + "%");
                cmd.Parameters.AddWithValue("@durum", true);

                // Verileri DataGridView kontrolüne aktarma
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                sql.con.Close();
            }
        }

        // Form Yüklenme Olayı
        private void frmMudur_Load(object sender, EventArgs e)
        {
            try
            {
                sqlCheck.check();

                // Arama kutularını gizle
                txtOgrenci.Visible = false;
                txtYardimci.Visible = false;
                txtOgretmen.Visible = false;

                // Toplam öğrenci sayısı ve tüm öğrenci sayısını hesaplama ve gösterme
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_ogrenciler WHERE DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@durum", true);
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM tbl_ogrenciler", sql.con);
                int toplamOgrenci = (int)cmd.ExecuteScalar();
                int tumOgrenci = (int)cmd2.ExecuteScalar();
                lblToplamOgr.Text = toplamOgrenci.ToString();
                lblButunZamanlar.Text = tumOgrenci.ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                sql.con.Close();
            }
        }

        // Yardımcı Arama Kutusu Değiştiğinde
        private void txtYardimci_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Yardımcı bilgilerini arama sorgusu ile çekme
                sql.con.Open();
                SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_yardimcilar WHERE (AD LIKE @yardimciBilgi OR ID LIKE @yardimciBilgi OR SOYAD LIKE @yardimciBilgi) AND DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@yardimciBilgi", "%" + txtYardimci.Text + "%");
                cmd.Parameters.AddWithValue("@durum", true);

                // Verileri DataGridView kontrolüne aktarma
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                sql.con.Close();
            }
        }

        // Öğretmen Arama Kutusu Değiştiğinde
        private void txtOgretmen_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Öğretmen bilgilerini arama sorgusu ile çekme
                sql.con.Open();
                SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_ogretmenler WHERE (ID LIKE @ogretmenBilgi OR AD LIKE @ogretmenBilgi OR SOYAD LIKE @ogretmenBilgi) AND DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@ogretmenBilgi", "%" + txtOgretmen.Text + "%");
                cmd.Parameters.AddWithValue("@durum", true);

                // Verileri DataGridView kontrolüne aktarma
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                sql.con.Close();
            }
        }

        // Kayıt Ekle Butonu
        private void btnKayitEkle_Click(object sender, EventArgs e)
        {
            // Yeni öğrenci kaydı eklemek için frmOgrenciYonetim formu açılıyor
            frmOgrenciYonetim frm = new frmOgrenciYonetim();
            ((Label)frm.Controls["lbl_ID"]).Text = lblMudurID.Text;
            ((Label)frm.Controls["lblYetki"]).Text = "Müdür";
            frm.Show();
        }
    }
}
