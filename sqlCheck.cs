using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace okulSistemi
{
    public class sqlCheck
    {

        public static void check()
        {
            if (sql.con.State != ConnectionState.Open)
            {
                sql.con.Open();
            }
        }

    }
}
