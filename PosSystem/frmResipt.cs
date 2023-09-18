using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace PosSystem
{
    public partial class frmResipt : Form
    {
       
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        // string store = "Ds Studio Software Solution";
        // string address = "258/B Colombo9,Srilanka";
        frmPOS f;


        public frmResipt(frmPOS frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;

        }

        private void frmResipt_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
        public void LoadReport(string pcash, string pchange)
        {
            ReportDataSource rtpDataSource;
            try
            {
                LocalReport report = new LocalReport();
                this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Bill\Report1.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();
                



                DataSet1 ds = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("select c.id, c.transno, c.pcode, c.price, c.qty, c.disc, c.total, c.sdate, c.status , p.pdesc from tblCart1 as c inner join TblProduct1 as p on p.pcode = c.pcode where transno like '" + f.lblTransno.Text + "'", cn);
                da.Fill(ds.Tables["dtSold"]);
                cn.Close();

                // ReportParameter pItems = new ReportParameter("pItems");
                ReportParameter pPhone = new ReportParameter("pPhone", f.lblPhone.Text);
                ReportParameter pTotal = new ReportParameter("pTotal", f.lblTotal.Text);
                ReportParameter pCash = new ReportParameter("pCash", pcash);
                ReportParameter pDiscount = new ReportParameter("pDiscount", f.lblDiscount.Text);
                ReportParameter pChange = new ReportParameter("pChange", pchange);
                ReportParameter pStore = new ReportParameter("pStore", f.lblSname.Text);
                ReportParameter pAddress = new ReportParameter("pAddress", f.lblAddress.Text);
                ReportParameter pTransaction = new ReportParameter("pTransaction", "Invoice #: " + f.lblTransno.Text);
                ReportParameter pCashier = new ReportParameter("pCashier", f.LblUser.Text);

                // reportViewer1.LocalReport.SetParameters(pItems);
                reportViewer1.LocalReport.SetParameters(pDiscount);
                reportViewer1.LocalReport.SetParameters(pPhone);
                reportViewer1.LocalReport.SetParameters(pTotal);
                reportViewer1.LocalReport.SetParameters(pCash);
                reportViewer1.LocalReport.SetParameters(pChange);
                reportViewer1.LocalReport.SetParameters(pStore);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(pTransaction);
                reportViewer1.LocalReport.SetParameters(pCashier);


                rtpDataSource = new ReportDataSource("DataSet1", ds.Tables["dtSold"]);
                reportViewer1.LocalReport.DataSources.Add(rtpDataSource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 100;
               // report.printToPrinter();



            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void reportViewer1_Load_1(object sender, EventArgs e)
        {
           
        }

       
    }
    }


