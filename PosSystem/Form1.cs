using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;
using System.Data.SqlClient;

namespace PosSystem
{
    public partial class Form1 : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        public string _pass, _user;
        
            
        public Form1()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            NotifyCriticalItems();
            
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void NotifyCriticalItems()
        {
            string critical = "";
            cn.Open();
            cm = new SqlCommand("select count(*) from vwCriticalItems", cn);
            string count = cm.ExecuteScalar().ToString();
            cn.Close();


            int i = 0;
            cn.Open();
            cm = new SqlCommand("select * from vwCriticalItems", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                critical += i +". "  +  dr["pdesc"].ToString() + Environment.NewLine;
            }
            dr.Close();
            cn.Close();

            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.close_window_16;
            popup.TitleText = count + " Critical Item(s)";
            popup.ContentText = critical;
            popup.Popup();
                


        }
        private void btnBrand_Click(object sender, EventArgs e)
        {
            frmBrandList frm = new frmBrandList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmCategoryList frm = new frmCategoryList();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.LoadCategory();
            frm.Show();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmProduct_List frm = new frmProduct_List();
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.LoadRecords();
            frm.Show();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            frmStockin frm = new frmStockin();     
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

      

        private void button8_Click(object sender, EventArgs e)
        {
            frmUserAccount frm = new frmUserAccount (this);
            frm.TopLevel = false;
            panel3.Controls.Add(frm);
            frm.txtU.Text = _user;
            frm.BringToFront();
            frm.Show();

        }

        private void btnSalesHistory_Click(object sender, EventArgs e)
        {
            frmSoldItems frm = new frmSoldItems();
            frm.ShowDialog();
         
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmRecords frm = new frmRecords();
            frm.TopLevel = false;
            frm.LoadCriticalItems();
            frm.LoadInventory();
            frm.LoadStockHistory();
            frm.CancelledOrder();
            panel3.Controls.Add(frm);
            frm.BringToFront();
            frm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmStoreSetting frm = new frmStoreSetting();
            frm.LoadRecord();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyDashbord();
        }

        public void MyDashbord()
            {
                 frmDashbord f = new frmDashbord();
                 f.TopLevel = false;
                 panel3.Controls.Add(f);
                 f.lblDailySales.Text = dbcon.DailySales().ToString("#,##0.00");
                 f.lblProduct.Text = dbcon.ProductLine().ToString();
                 f.lblStock.Text = dbcon.StockOnHand().ToString();
                 f.lblCriticalItems.Text = dbcon.CraticalItems().ToString();
                 f.BringToFront();
                 f.Show();
            }

        private void btnVendor_Click(object sender, EventArgs e)
        {
           frm_VendorList f = new frm_VendorList();
            f.TopLevel = false;
            panel3.Controls.Add(f);
            f.LoadRecords();
            f.BringToFront();
            
            f.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            frmAdjustment f = new frmAdjustment(this);
            f.LoadRecords();
           f.txtUser.Text = lblUser.Text;
            f.referenceNo();
            f.ShowDialog();

            
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
          
            if (MessageBox.Show("Logout Application?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                frmUserLogin frm = new frmUserLogin();
                frm.ShowDialog();
            }
        }
    }
}
