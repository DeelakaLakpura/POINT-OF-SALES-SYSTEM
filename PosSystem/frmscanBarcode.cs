using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Presentation;

namespace PosSystem
{
    public partial class frmscanBarcode : Form
    {
        frmProduct f;
        public frmscanBarcode(frmProduct frm)
        {
            InitializeComponent();
            f = frm;
        }

        FilterInfoCollection FilterInfoCollection;
        VideoCaptureDevice videoCaptureDevice;

        private void frmscanBarcode_Load(object sender, EventArgs e)
        {
            FilterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in FilterInfoCollection)
                cbCamara.Items.Add(device.Name);
            cbCamara.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            videoCaptureDevice = new VideoCaptureDevice(FilterInfoCollection[cbCamara.SelectedIndex].MonikerString);
            videoCaptureDevice.NewFrame += VideoCaptureDevice_NewFrame;
            videoCaptureDevice.Start();
        }

        private void VideoCaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            ZXing.BarcodeReader reader = new ZXing.BarcodeReader();
            var result = reader.Decode(bitmap);
            if(result != null)
            {
              f.txtBarcode.Invoke(new MethodInvoker(delegate()
              {
                  f.txtBarcode.Text = result.ToString();
              }));

            }
            pictureBox.Image = bitmap;
        }

        private void frmscanBarcode_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice != null)
            {
                if (videoCaptureDevice.IsRunning)
                    videoCaptureDevice.Stop();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
