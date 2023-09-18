using Microsoft.Reporting.WinForms;
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

namespace PosSystem
{
    public partial class frmPrint : Form
    {

        DataSet1.dtBarcodeDataTable _barcode;
        public frmPrint(DataSet1.dtBarcodeDataTable barcode)
        {
            InitializeComponent();
            this._barcode = barcode;

        }

        private void frmPrint_Load(object sender, EventArgs e)
        {
            this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Bill\ReportBarcode.rdlc";
            this.reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            reportDataSource.Value = _barcode;
            reportViewer1.LocalReport.EnableExternalImages = true;
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            this.reportViewer1.RefreshReport();

        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
           
            


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
    }

