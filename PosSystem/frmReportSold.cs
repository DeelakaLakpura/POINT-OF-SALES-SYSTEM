using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;


namespace PosSystem
{
    public partial class frmReportSold : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        frmSoldItems f;
        

        public frmReportSold(frmSoldItems frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmReportSold_Load(object sender, EventArgs e)
        {

        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
       
        public void LoadReport()
        {
            try
            {
                ReportDataSource rptDS;


                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Bill\Report2.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                if (f.cbCashier.Text == "All Cashier") { da.SelectCommand = new SqlCommand("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc as discount, total from tblCart1 as c inner join TblProduct1 as p on c.pcode = p.pcode where status like 'sold' and sdate between '" + f.dateTimePicker1.Value + "' and '" + f.dateTimePicker2.Value +  "'", cn); }
                else { da.SelectCommand = new SqlCommand("select c.id, c.transno, c.pcode, p.pdesc, c.price, c.qty, c.disc as discount, total from tblCart1 as c inner join TblProduct1 as p on c.pcode = p.pcode where status like 'sold' and sdate between '" + f.dateTimePicker1.Value + "' and '" + f.dateTimePicker2.Value + "'and cashier like '" + f.cbCashier.Text + "'", cn); }
                da.Fill(ds.Tables["dtSoldReport"]);
                cn.Close();
                ReportParameter pDate = new ReportParameter("pDate", "Date From: " + f.dateTimePicker1.Value.ToShortDateString() + " To " + f.dateTimePicker2.Value.ToShortDateString());
                ReportParameter pCashier = new ReportParameter("pCashier", "Cashier: " + f.cbCashier.Text);
                ReportParameter pHeder = new ReportParameter("pHeder", "SALES REPORT");

                reportViewer1.LocalReport.SetParameters(pDate);
                reportViewer1.LocalReport.SetParameters(pCashier);
            
                reportViewer1.LocalReport.SetParameters(pHeder);
                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtSoldReport"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void reportViewer1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
