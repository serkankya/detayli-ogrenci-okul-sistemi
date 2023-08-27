using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace okulSistemi
{
    public class ogrenciKayit
    {
        public ogrenciKayit()
        {
        }

        public void kayitEKle(string ad,string soyad,string memleket,string tckimlik,int sinif,string sube,string anne,string baba)
        {
            sql.con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO tbl_ogrenciler (AD,SOYAD,MEMLEKET,TCKIMLIK,SINIF,SUBE,ANNE,BABA) VALUES (@ad,@soyad,@memleket,@tckimlik,@sinif,@sube,@anne,@baba)", sql.con);
            cmd.Parameters.AddWithValue("@ad",ad);
            cmd.Parameters.AddWithValue("@soyad", soyad);
            cmd.Parameters.AddWithValue("@memleket", memleket);
            cmd.Parameters.AddWithValue("@tckimlik", tckimlik);
            cmd.Parameters.AddWithValue("@sinif", sinif);
            cmd.Parameters.AddWithValue("@sube", sube);
            cmd.Parameters.AddWithValue("@anne", anne);
            cmd.Parameters.AddWithValue("@baba", baba);
            cmd.ExecuteNonQuery();
        }

    }
}
