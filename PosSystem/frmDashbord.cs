using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PosSystem
{
    public partial class frmDashbord : Form
    {

        SqlConnection cn;
        SqlCommand cm;
        DBConnection db = new DBConnection();

        public frmDashbord()
        {
            InitializeComponent();
            cn = new SqlConnection();
            cn.ConnectionString = db.MyConnection();
            LoadChart();
        }

        private void frmDashbord_Resize(object sender, EventArgs e)
        {
            panel1.Left = (this.Width = panel1.Width) / 2;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        public void LoadChart()
        {
            cn.Open();
            SqlDataAdapter da = new SqlDataAdapter("select month(sdate) as month, isnull(sum(total), 0.0) as total from tblCart1 where status like 'Sold' group by Month(sdate)", cn);
            DataSet ds =new DataSet();

            da.Fill(ds, "Sales");
            chart1.DataSource = ds.Tables["Sales"];
            Series series1 = chart1.Series["Series1"];
            series1.ChartType = SeriesChartType.Doughnut;

            series1.Name = "SALES";

            var chart = chart1;
            chart1.Series[series1.Name].XValueMember = "month";
            chart1.Series[series1.Name].YValueMembers = "total";
            chart1.Series[0].IsValueShownAsLabel = true;


            cn.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmDashbord_Load(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }
    }
}
