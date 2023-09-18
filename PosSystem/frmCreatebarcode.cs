using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PosSystem
{
    public partial class frmCreatebarcode : Form
    {
        public frmCreatebarcode()
        {
            InitializeComponent();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            BarcodeLib.Barcode barcode = new BarcodeLib.Barcode();
            Image img = barcode.Encode(BarcodeLib.TYPE.UPCA, txtBarcod.Text, Color.Black, Color.White, 100, 30);
            pictureBox.Image = img;
            this.dataSet11.Clear();

            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                for (int i = 0; i < number.Value; i++)
               this.dataSet11.dtBarcode.AdddtBarcodeRow(txtBarcod.Text, ms.ToArray());


            }
            using(frmPrint FRM = new frmPrint(this.dataSet11.dtBarcode))
            {
                FRM.ShowDialog();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void frmCreatebarcode_Load(object sender, EventArgs e)
        {

        }
    }
}
