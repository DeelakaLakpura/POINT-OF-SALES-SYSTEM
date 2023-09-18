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
    public partial class frmInventoryReport : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        string stitle = "PosSystem";

        public frmInventoryReport()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmInventoryReport_Load(object sender, EventArgs e)
        {
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        public void LoadSoldItems(string sql, string param)
        {
            try
            {
                ReportDataSource rptDS;
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Bill\ReportSold.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();


                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand(sql, cn);
                da.Fill(ds.Tables["dtSoldItem"]);
                cn.Close();

                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtSoldItem"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }




        public void LoadTopSelling(string sql, string param ,string header )
        {
            try
            {
                ReportDataSource rptDS;
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Bill\ReportTop.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();


                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand(sql, cn);
                da.Fill(ds.Tables["dtTopSelling"]);
                cn.Close();

                ReportParameter pDate = new ReportParameter("pDate", param);
                ReportParameter pHeader = new ReportParameter("pHeader", header);
                reportViewer1.LocalReport.SetParameters(pDate);
                reportViewer1.LocalReport.SetParameters(pHeader);

                rptDS = new ReportDataSource("DataSet1", ds.Tables["dtTopSelling"]);
                reportViewer1.LocalReport.DataSources.Add(rptDS);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;


            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadReport()
        {
            ReportDataSource rtpDs;
            try
            {
                  this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Bill\Report3.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();


                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("select p.pcode , p.barcode, p.pdesc,b.brand,c.category , p.price, p.qty, p.reorder from TblProduct1 as p inner join BrandTbl as b on p.bid=b.id inner join TblCatecory as c on p.cid = c.id", cn);
                da.Fill(ds.Tables["dtInventory"]);
                cn.Close();

                rtpDs = new ReportDataSource("DataSet1", ds.Tables["dtInventory"]);
                reportViewer1.LocalReport.DataSources.Add(rtpDs);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void reportViewer1_Load_1(object sender, EventArgs e)
        {

        }

        public void LoadStockReport(string psql, string param)
        {
            ReportDataSource rtpDs;
            try
            {
               
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Bill\ReportStock.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
               


                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);

                cn.Open();
                da.SelectCommand = new SqlCommand(psql, cn);
                da.Fill(ds.Tables["dtStockin"]);
                cn.Close();

                rtpDs = new ReportDataSource("DataSet1", ds.Tables["dtStockin"]);
                reportViewer1.LocalReport.DataSources.Add(rtpDs);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void LoadCancelReport(string psql, string param)
        {
            ReportDataSource rtpDs;
            try
            {
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Bill\ReportCancel.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();


                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                ReportParameter pDate = new ReportParameter("pDate", param);
                reportViewer1.LocalReport.SetParameters(pDate);

                cn.Open();
                da.SelectCommand = new SqlCommand(psql, cn);
                da.Fill(ds.Tables["dtCancel"]);
                cn.Close();

                rtpDs = new ReportDataSource("DataSet1", ds.Tables["dtCancel"]);
                reportViewer1.LocalReport.DataSources.Add(rtpDs);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;

            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
