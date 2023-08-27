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
    public partial class frmOgretmen : Form
    {
        public frmOgretmen()
        {
            InitializeComponent();
        }

        // Form yüklendiğinde çalışacak event handler
        private void frmOgretmen_Load(object sender, EventArgs e)
        {
            try
            {
                sqlCheck.check();
                // Ders adlarını veritabanından çekip ComboBox'a ekliyor
                SqlCommand cmd = new SqlCommand("SELECT DERSLER FROM tbl_dersler", sql.con);
                SqlCommand cmd2 = new SqlCommand("SELECT *FROM tbl_ogrenciNotlar", sql.con);

                SqlDataAdapter da = new SqlDataAdapter(cmd2);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cbDersler.Items.Add(dr["DERSLER"].ToString());
                }
                dr.Close();
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

        // "Kaydet" butonuna tıklandığında çalışacak event handler
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                sqlCheck.check();

                // Öğrenci notlarını veritabanına ekliyor
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_ogrenciNotlar (OGRNUMARA,OGRETMENID,DERSID,SINAV1,SINAV2,SINAV3,SOZLU1,PROJE1) VALUES (@ogrNo,@ogretmenID,@dersID,@S1,@S2,@S3,@sozlu1,@proje1)", sql.con);
                // Parametreler dolduruluyor...
                cmd.ExecuteNonQuery();
                MessageBox.Show("Notlar eklendi.");

                // İdare hareketi ekleniyor
                SqlCommand cmd2 = new SqlCommand("INSERT INTO tbl_idareHareketleri (OGRETMENID,ISLEM,TARIH) VALUES (@ogretmenID,@islem,@tarih)", sql.con);
                // Parametreler dolduruluyor...
                cmd2.ExecuteNonQuery();
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

        // Dersler ComboBox'ının seçimi değiştiğinde çalışacak event handler
        private void cbDersler_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                sqlCheck.check();

                // Seçilen ders adına göre ders ID'sini veritabanından alıp label'a yazıyor
                SqlCommand cmd = new SqlCommand("SELECT DERSID FROM tbl_dersler WHERE DERSLER=@dersad", sql.con);
                cmd.Parameters.AddWithValue("@dersad", cbDersler.SelectedItem.ToString());

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    lblDersID.Text = dr["DERSID"].ToString();
                }

                dr.Close();
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
            // Seçilen öğrencinin not bilgileri ilgili alanlara aktarılıyor
            int secilenOgrenci = dataGridView1.SelectedCells[0].RowIndex;
            txtOgrNO.Text = dataGridView1.Rows[secilenOgrenci].Cells[1].Value.ToString();
            cbDersler.Text = dataGridView1.Rows[secilenOgrenci].Cells[3].Value.ToString();
            // Diğer bilgiler de benzer şekilde alınıyor...
        }

        // "Güncelle" butonuna tıklandığında çalışacak event handler
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sqlCheck.check();

                // Öğrenci notları güncelleniyor
                SqlCommand cmd = new SqlCommand("UPDATE tbl_ogrenciNotlar SET OGRNUMARA=@no,DERSID=@ders,SINAV1=@S1,SINAV2=@S2,SINAV3=@S3,PROJE1=@proje,SOZLU1=@sozlu WHERE ID=@id", sql.con);
                // Parametreler dolduruluyor...
                cmd.ExecuteNonQuery();
                MessageBox.Show("Öğrenci notları güncellendi.");

                // İdare hareketi ekleniyor
                SqlCommand cmd2 = new SqlCommand("INSERT INTO tbl_idareHareketleri (OGRETMENID,ISLEM,TARIH) VALUES (@ogrID,@islem,@tarih)", sql.con);
                // Parametreler dolduruluyor...
                cmd2.ExecuteNonQuery();
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
