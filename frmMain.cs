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
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısının durumu kontrol ediliyor
                sqlCheck.check();

                // Aktif öğrenci, öğretmen ve yardımcı sayıları alınıyor ve ilgili label'lara yazılıyor
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM tbl_ogrenciler WHERE DURUM=@durum", sql.con);//Güvenlik için parametre ile yapılabilir. Bu uygulamada belirli yerlerde yaptım.
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM tbl_ogretmenler WHERE DURUM=@durum", sql.con);//Güvenlik için parametre ile yapılabilir. Bu uygulamada belirli yerlerde yaptım.
                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM tbl_yardimcilar WHERE DURUM=@durum", sql.con);   //Güvenlik için parametre ile yapılabilir. Bu uygulamada belirli yerlerde yaptım.
                int ogrenciSayisi = (int)cmd.ExecuteScalar();
                int ogretmenSayisi = (int)cmd2.ExecuteScalar();
                int yardimciSayisi = (int)cmd3.ExecuteScalar();
                lblOgrenci.Text = ogrenciSayisi.ToString();
                lblOgretmen.Text = ogretmenSayisi.ToString();
                lblYardimci.Text = yardimciSayisi.ToString();

                // Aktif müdür adı ve soyadı alınıp ilgili label'a yazılıyor
                SqlCommand cmd4 = new SqlCommand("SELECT AD ,SOYAD FROM tbl_mudurler WHERE DURUM=@durum", sql.con);
                cmd4.Parameters.AddWithValue("@durum", 1);
                SqlDataReader dr = cmd4.ExecuteReader();
                if (dr.Read())
                {
                    string ad = dr["AD"].ToString();
                    string soyad = dr["SOYAD"].ToString();

                    string adSoyad = ad + " " + soyad;
                    lblMudur.Text = adSoyad;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                // Veritabanı bağlantısı kapatılıyor
                sql.con.Close();
            }
        }

        // Müdür giriş hareketi kaydını ekleyen metot
        private void mudurHareketleri(int mudurID)
        {
            try
            {
                // Veritabanı bağlantısının durumu kontrol ediliyor
                sqlCheck.check();

                // İlgili tabloya müdür giriş hareketi kaydı ekleniyor
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_idareHareketleri (MUDURID, ISLEM, TARIH) VALUES (@mudurid, @islem, @tarih)", sql.con);
                cmd.Parameters.AddWithValue("@mudurid", mudurID);
                cmd.Parameters.AddWithValue("@islem", "Giriş Yaptı");
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);

                if (sql.con.State != ConnectionState.Open)
                {
                    sql.con.Open();
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                // Veritabanı bağlantısı kapatılıyor
                sql.con.Close();
            }
        }

        // Yardımcı giriş hareketi kaydını ekleyen metot
        private void yardimciHareketleri(int yardimciID)
        {
            try
            {
                // Veritabanı bağlantısının durumu kontrol ediliyor
                sqlCheck.check();

                // İlgili tabloya yardımcı giriş hareketi kaydı ekleniyor
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_idareHareketleri (YARDIMCIID,ISLEM,TARIH) VALUES (@yardimciid,@islem,@tarih)", sql.con);
                cmd.Parameters.AddWithValue("@yardimciid", yardimciID);
                cmd.Parameters.AddWithValue("@islem", "Giriş yaptı.");
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                // Veritabanı bağlantısı kapatılıyor
                sql.con.Close();
            }
        }

        // Öğretmen giriş hareketi kaydını ekleyen metot
        private void ogretmenHareketleri(int ogretmenID)
        {
            try
            {
                // Veritabanı bağlantısının durumu kontrol ediliyor
                sqlCheck.check();

                // İlgili tabloya öğretmen giriş hareketi kaydı ekleniyor
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_idareHareketleri (OGRETMENID,ISLEM,TARIH) VALUES (@ogretmenid,@islem,@tarih)", sql.con);
                cmd.Parameters.AddWithValue("@ogretmenid", ogretmenID);
                cmd.Parameters.AddWithValue("@islem", "Giriş yaptı.");
                cmd.Parameters.AddWithValue("@tarih", DateTime.Now);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                // Veritabanı bağlantısı kapatılıyor
                sql.con.Close();
            }
        }

        private void btnMudur_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısının durumu kontrol ediliyor
                sqlCheck.check();

                // Kullanıcı adı, şifre ve durum parametreleri ile müdür sorgusu yapılıyor
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_mudurler WHERE KULLANICIADI=@k_adi AND SIFRE=@sifre AND DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@k_adi", txtK_Adi.Text);
                cmd.Parameters.AddWithValue("@sifre", txtSifre.Text);
                cmd.Parameters.AddWithValue("@durum", 1);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("[MÜDÜR GİRİŞİ] Hoş geldiniz. Sistem yükleniyor...");

                    int mudurID = Convert.ToInt32(dr["ID"]);
                    dr.Close();

                    // Müdür giriş hareketi kaydı ekleniyor
                    mudurHareketleri(mudurID);

                    // frmMudur formuna veri aktarılıyor
                    frmMudur frm = new frmMudur();
                    ((Label)frm.Controls["lblMudurID"]).Text = mudurID.ToString();
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Lütfen kullanıcı adınızı veya şifrenizi kontrol ediniz.", "HATALI KULLANICI ADI VEYA ŞİFRE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                // Veritabanı bağlantısı kapatılıyor
                sql.con.Close();
            }
        }

        // Öğretmen Giriş Butonu
        private void btnOgretmen_Click(object sender, EventArgs e)
        {
            try
            {
                // Veritabanı bağlantısının durumu kontrol ediliyor
                sqlCheck.check();

                // Kullanıcı adı, şifre ve durum parametreleri ile öğretmen sorgusu yapılıyor
                SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_ogretmenler WHERE KULLANICIADI=@k_adi AND SIFRE=@sifre AND DURUM=@durum", sql.con);
                cmd.Parameters.AddWithValue("@k_adi", txtK_Adi.Text);
                cmd.Parameters.AddWithValue("@sifre", txtSifre.Text);
                cmd.Parameters.AddWithValue("@durum", 1);

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("[ÖĞRETMEN GİRİŞİ] Hoş geldiniz. Sistem yükleniyor...");

                    int ogretmenID = Convert.ToInt32(dr["ID"]);
                    dr.Close();

                    // Öğretmen giriş hareketi kaydı ekleniyor
                    ogretmenHareketleri(ogretmenID);

                    // frmOgretmen formuna veri aktarılıyor ve form açılıyor
                    frmOgretmen frm = new frmOgretmen();
                    ((Label)frm.Controls["lbl_ID"]).Text = ogretmenID.ToString();
                    this.Hide();
                    frm.Show();
                }
                else
                {
                    MessageBox.Show("Lütfen kullanıcı adınızı veya şifrenizi kontrol ediniz.", "HATALI KULLANICI ADI VEYA ŞİFRE", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hata " + ex);
            }
            finally
            {
                // Veritabanı bağlantısı kapatılıyor
                sql.con.Close();
            }
        }


        private void btnYardimci_Click(object sender, EventArgs e)
        {
            sql.con.Open();
            SqlCommand cmd = new SqlCommand("SELECT *FROM tbl_yardimcilar WHERE KULLANICIADI=@k_adi AND SIFRE=@sifre AND DURUM=@durum", sql.con);
            cmd.Parameters.AddWithValue("@k_adi", txtK_Adi.Text);
            cmd.Parameters.AddWithValue("@sifre", txtSifre.Text);
            cmd.Parameters.AddWithValue("@durum", 1);

            try
            {
                // Veritabanı bağlantısının durumu kontrol ediliyor
                sqlCheck.check();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("[MÜDÜR YARDIMCISI GİRİŞİ] Hoş geldiniz. Sistem yükleniyor...");

                    int yardimciID = Convert.ToInt32(dr["ID"]);
                    dr.Close();

                    // Yardımcı giriş hareketi kaydı ekleniyor
                    yardimciHareketleri(yardimciID);

                    // frmYardimci formuna veri aktarılıyor ve form açılıyor
                    frmYardimci frm = new frmYardimci();
                    ((Label)frm.Controls["lblYardimciID"]).Text = yardimciID.ToString();
                    frm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Lütfen kullanıcı adınızı veya şifrenizi kontrol ediniz.", "HATALI KULLANICI ADI VEYA ŞİFRE!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dr.Close();
                sql.con.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Hata " + ex);
            }
        }
    }
}
