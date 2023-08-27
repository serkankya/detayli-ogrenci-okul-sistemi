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
    public partial class frmYardimci : Form
    {
        public frmYardimci()
        {
            InitializeComponent();
        }

        // "İşlemler" butonuna tıklandığında çalışacak event handler
        private void btnIslemler_Click(object sender, EventArgs e)
        {
            // Öğrenci Yönetimi formu oluşturuluyor ve gerekli bilgiler aktarılıyor
            frmOgrenciYonetim frm = new frmOgrenciYonetim();
            ((Label)frm.Controls["lbl_ID"]).Text = lblYardimciID.Text;
            ((Label)frm.Controls["lblYetki"]).Text = "Müdür Yardımcısı";
            frm.Show();
        }

        // Verileri DataGridView'da listelemek için kullanılan metod
        private void verileriListele(string veriler)
        {
            // SQL sorgusu ile veriler çekilip DataGridView'a aktarılıyor
            SqlDataAdapter da = new SqlDataAdapter(veriler, sql.con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
        }

        // "Öğrenci Listele" butonuna tıklandığında çalışacak event handler
        private void btnOgrenciListele_Click(object sender, EventArgs e)
        {
            // Öğrenci verilerini DataGridView'da listeleme metodu çağrılıyor
            verileriListele("SELECT *FROM tbl_ogrenciler WHERE DURUM=1");
        }

        // "Öğretmen Listele" butonuna tıklandığında çalışacak event handler
        private void btnOgretmenListele_Click(object sender, EventArgs e)
        {
            // Öğretmen verilerini DataGridView'da listeleme metodu çağrılıyor
            verileriListele("SELECT *FROM tbl_ogretmenler WHERE DURUM=1");
        }

        // Öğrenci arama metni değiştiğinde çalışacak event handler
        private void txtOgrenci_TextChanged(object sender, EventArgs e)
        {
            try
            {
                sqlCheck.check();

                // Arama metniyle öğrenci verilerini filtreleyip DataGridView'da listeleme
                SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_ogrenciler WHERE (NUMARA LIKE @ogrBilgi OR AD LIKE @ogrBilgi OR SOYAD LIKE @ogrBilgi) AND DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@ogrBilgi", "%" + txtOgrenci.Text + "%");
                cmd.Parameters.AddWithValue("@durum", true);

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

        // Öğretmen arama metni değiştiğinde çalışacak event handler
        private void txtOgretmen_TextChanged(object sender, EventArgs e)
        {
            try
            {
                sqlCheck.check();

                // Arama metniyle öğretmen verilerini filtreleyip DataGridView'da listeleme
                SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_ogretmenler WHERE (NUMARA LIKE @ogrBilgi OR AD LIKE @ogrBilgi OR SOYAD LIKE @ogrBilgi) AND DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@ogrBilgi", "%" + txtOgretmen.Text + "%");
                cmd.Parameters.AddWithValue("@durum", true);

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
    }
}
