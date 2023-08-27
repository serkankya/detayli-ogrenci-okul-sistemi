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
    public partial class frmOgrenciYonetim : Form
    {
        public frmOgrenciYonetim()
        {
            InitializeComponent();
        }

        // "Kaydet" butonuna tıklandığında çalışacak event handler
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                // Yeni bir ogrenciKayit nesnesi oluşturuluyor
                ogrenciKayit kayit = new ogrenciKayit();

                // Form üzerindeki metin kutularından gerekli veriler alınıyor
                string ad = txtAd.Text;
                string soyad = txtSoyad.Text;
                string memleket = txtMemleket.Text;
                string tckimlik = txtTc.Text;
                int sinif = Convert.ToInt32(txtSinif.Text);
                string sube = txtSube.Text;
                string anne = txtAnne.Text;
                string baba = txtBaba.Text;

                // Veritabanına öğrenci kaydı ekleniyor
                kayit.kayitEKle(ad, soyad, memleket, tckimlik, sinif, sube, anne, baba);
                MessageBox.Show("Kayıt eklendi");

                // Kullanıcının yetkisine bağlı olarak idare hareketi ekleniyor
                if (lblYetki.Text == "Müdür")
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbl_idareHareketleri (MUDURID,ISLEM,TARIH) VALUES (@mudurid,@islem,@tarih)", sql.con);
                    cmd.Parameters.AddWithValue("@mudurid", lbl_ID.Text);
                    cmd.Parameters.AddWithValue("@islem", "Öğrenci kaydedildi.");
                    cmd.Parameters.AddWithValue("@tarih", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
                else if (lblYetki.Text == "Müdür Yardımcısı")
                {
                    SqlCommand cmd3 = new SqlCommand("INSERT INTO tbl_idareHareketleri (YARDIMCIID,ISLEM,TARIH) VALUES (@yardimciid,@islem,@tarih)", sql.con);
                    cmd3.Parameters.AddWithValue("@yardimciid", lbl_ID.Text);
                    cmd3.Parameters.AddWithValue("@islem", "Öğrenci kaydedildi.");
                    cmd3.Parameters.AddWithValue("@tarih", DateTime.Now);

                    cmd3.ExecuteNonQuery();
                }
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

        // Form yüklendiğinde çalışacak event handler
        private void frmOgrenciYonetim_Load(object sender, EventArgs e)
        {
            // Gerekli kontrollerin görünürlüğü ayarlanıyor
            gbKayit.Visible = false;
            gbGuncelle.Visible = false;
            btnSil.Visible = false;
            txtSilinecekNo.Visible = false;
            lblSilmeBilgisi.Visible = false;

            try
            {
                // Veritabanından öğrenci bilgileri çekiliyor ve DataGridView'e yükleniyor
                sqlCheck.check();
                SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_ogrenciler WHERE DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@durum", true);
                cmd.ExecuteNonQuery();
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

        // "Kayıt Seçildi" butonuna tıklandığında çalışacak event handler
        private void btnKayitSecildi_Click(object sender, EventArgs e)
        {
            // İlgili kontrollerin görünürlüğü ayarlanıyor
            gbKayit.Visible = true;
            gbGuncelle.Visible = false;
        }

        // "Güncelleme Seçildi" butonuna tıklandığında çalışacak event handler
        private void btnGuncellemeSecildi_Click(object sender, EventArgs e)
        {
            // İlgili kontrollerin görünürlüğü ayarlanıyor
            gbGuncelle.Visible = true;
            gbKayit.Visible = false;
        }

        // Arama metin kutusunun içeriği değiştiğinde çalışacak event handler
        private void txtArama_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Arama sonucunu veritabanından çekip DataGridView'e yüklüyor
                sqlCheck.check();
                SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_ogrenciler WHERE (NUMARA LIKE @ogrBilgi OR AD LIKE @ogrBilgi OR SOYAD LIKE @ogrBilgi) AND DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@ogrBilgi", "%" + txtArama.Text + "%");
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

        // DataGridView'da hücreye tıklandığında çalışacak event handler
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Güncelleme işlemi için ilgili kontroller dolduruluyor
            gbGuncelle.Visible = true;
            int secilenOgrenci = dataGridView1.SelectedCells[0].RowIndex;
            gtxtNumara.Text = dataGridView1.Rows[secilenOgrenci].Cells[0].Value.ToString();
            gtxtAd.Text = dataGridView1.Rows[secilenOgrenci].Cells[1].Value.ToString();
            // Diğer bilgiler de benzer şekilde alınıyor...
        }

        // "Güncelle" butonuna tıklandığında çalışacak event handler
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            try
            {
                // Öğrenci bilgileri güncelleniyor
                sqlCheck.check();
                SqlCommand cmd = new SqlCommand("UPDATE tbl_ogrenciler SET AD=@ad,SOYAD=@soyad,MEMLEKET=@memleket,TCKIMLIK=@tc,SINIF=@sinif,SUBE=@sube,ANNE=@anne,BABA=@baba WHERE NUMARA=@numara ", sql.con);
                // Parametreler dolduruluyor...
                cmd.ExecuteNonQuery();
                MessageBox.Show("Öğrenci bilgileri güncellendi.");

                // İdare hareketi ekleniyor
                if (lblYetki.Text == "Müdür")
                {
                    SqlCommand cmd2 = new SqlCommand("INSERT INTO tbl_idareHareketleri (MUDURID,ISLEM,TARIH) VALUES (@mudurid,@islem,@tarih)", sql.con);
                    // Parametreler dolduruluyor...
                    cmd2.ExecuteNonQuery();
                }
                else if (lblYetki.Text == "Müdür Yardımcısı")
                {
                    SqlCommand cmd3 = new SqlCommand("INSERT INTO tbl_idareHareketleri (YARDIMCIID,ISLEM,TARIH) VALUES (@yardimciid,@islem,@tarih)", sql.con);
                    // Parametreler dolduruluyor...
                    cmd3.ExecuteNonQuery();
                }
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

        // "Silme Seçildi" butonuna tıklandığında çalışacak event handler
        private void btnSilmeSecildi_Click(object sender, EventArgs e)
        {
            // Silme işlemi için ilgili kontroller görünür hale getiriliyor
            txtSilinecekNo.Visible = true;
            btnSil.Visible = true;
            lblSilmeBilgisi.Visible = true;
        }

        // "Sil" butonuna tıklandığında çalışacak event handler
        private void btnSil_Click(object sender, EventArgs e)
        {
            // Silme işlemi onaylandığında gerçekleşecek işlemler
            DialogResult onay = MessageBox.Show("Öğrencinin kaydını silmek istediğinizden emin misiniz ? ", "UYARI", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (onay == DialogResult.Yes)
            {
                try
                {
                    sql.con.Open();

                    // Öğrencinin durumu pasif olarak güncelleniyor
                    SqlCommand cmd = new SqlCommand("UPDATE tbl_ogrenciler SET DURUM=@durum WHERE NUMARA=@numara", sql.con);
                    cmd.Parameters.AddWithValue("@durum", false);
                    cmd.Parameters.AddWithValue("@numara", txtSilinecekNo.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Öğrenci kaydı silindi.");

                    // İdare hareketi ekleniyor
                    if (lblYetki.Text == "Müdür")
                    {
                        SqlCommand cmd2 = new SqlCommand("INSERT INTO tbl_idareHareketleri (MUDURID,ISLEM,TARIH) VALUES (@mudurid,@islem,@tarih)", sql.con);
                        // Parametreler dolduruluyor...
                        cmd2.ExecuteNonQuery();
                    }
                    else if (lblYetki.Text == "Müdür Yardımcısı")
                    {
                        SqlCommand cmd3 = new SqlCommand("INSERT INTO tbl_idareHareketleri (YARDIMCIID,ISLEM,TARIH) VALUES (@yardimciid,@islem,@tarih)", sql.con);
                        // Parametreler dolduruluyor...
                        cmd3.ExecuteNonQuery();
                    }
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
}
