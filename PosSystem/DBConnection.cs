using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PosSystem
{
    public class DBConnection
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        private double dailySales;
        private int productline;
        private int Stockonhand;
        private int craticalitems;
        private string con;

        public string MyConnection()
        {
          

        }
      
    
    
}
