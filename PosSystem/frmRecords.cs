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
using System.Windows.Forms.DataVisualization.Charting;

namespace PosSystem
{
    public partial class frmRecords : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();

        public frmRecords()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmRecords_Load(object sender, EventArgs e)
        {

        }
        public void LoadRecords()
        {
            int i = 0;

            cn.Open();
            dataGridView1.Rows.Clear();
            if(cdTopSelling.Text =="Short by Qty")
            {
                cm = new SqlCommand("select top 10 pcode, pdesc,isnull(sum(qty),0) as qty, isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'sold' group by pcode,pdesc order by qty desc", cn);

            }
            else if(cdTopSelling.Text =="Short by Total Amount")
            {
                cm = new SqlCommand("select top 10 pcode, pdesc,isnull(sum(qty),0) as qty, isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'sold' group by pcode,pdesc order by total desc", cn);
            }
          
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["qty"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00")); 
            }
            dr.Close();
            cn.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadRecords();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                dataGridView2.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select c.pcode, p.pdesc, c.price, sum (c.qty) as tot_qty, sum (c.disc) as tot_disc, sum (c.total) as total  from tblCart1 as c inner join TblProduct1 as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "'  group by c.pcode, p.pdesc, c.price", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView2.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), Double.Parse(dr["price"].ToString()).ToString("#,##0.00"), dr["tot_qty"].ToString(), dr["tot_disc"].ToString(),Double.Parse( dr["total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select isnull(sum (total),0) from tblCart1  where status like 'Sold' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "'", cn);
                lblTotal.Text = Double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void LoadCriticalItems()
        {
            try
            {
                dataGridView3.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select * from vwCriticalItems",cn);
                dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    i++;
                    dataGridView3.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(),dr[6].ToString(), dr[7].ToString());
                }
                dr.Close();
                cn.Close();

            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }
        public void LoadInventory()
        {
            int i = 0;
            dataGridView4.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select p.pcode , p.barcode, p.pdesc,b.brand,c.category , p.price, p.qty, p.reorder from TblProduct1 as p inner join BrandTbl as b on p.bid=b.id inner join TblCatecory as c on p.cid = c.id", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView4.Rows.Add(i, dr["pcode"].ToString(), dr["barcode"].ToString(), dr["pdesc"].ToString(), dr["brand"].ToString(), dr["category"].ToString(), dr["price"].ToString(), dr["reorder"].ToString(), dr["qty"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            frm.LoadReport();
            frm.ShowDialog();
        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView5_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void CancelledOrder()
        {
            int i = 0;
            dataGridView5.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwCancelledOrder where sdate between '" + dateTimePicker5.Value.ToString() + "' and '" + dateTimePicker6.Value.ToString() + "'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView5.Rows.Add(i, dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["total"].ToString(), dr["sdate"].ToString(), dr["voidby"].ToString(), dr["cancelledby"].ToString(),dr["reason"].ToString(), dr["action"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
       public void LoadStockHistory()
        {
            int i = 0;
            dataGridView6.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwStockin where cast(sdate as date) between '" + dateTimePicker8.Value.ToString() + "' and '" + dateTimePicker7.Value.ToString()+ "' and status like 'Done'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView6.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();

        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport f = new frmInventoryReport();
            if (cdTopSelling.Text == "Short by Qty")
            {
                f.LoadTopSelling("select top 10 pcode, pdesc, sum(qty) as qty, isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'sold' group by pcode,pdesc order by qty desc" , "From : " + dateTimePicker1.Value.ToString() + " To : " + dateTimePicker2.Value.ToString(),"TOP SELLING ITEMS SHORT BY QTY");

            }
            else if (cdTopSelling.Text == "Short by Total Amount")
            {
               // cm = new SqlCommand("select top 10 pcode, pdesc,isnull(sum(qty),0) as qty, isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'sold' group by pcode,pdesc order by total desc", cn);
                f.LoadTopSelling("select top 10 pcode, pdesc,isnull(sum(qty),0) as qty ,isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'Sold' group by pcode ,pdesc order by total desc", "From : " + dateTimePicker1.Value.ToString() + " To : " + dateTimePicker2.Value.ToString(), "TOP SELLING ITEMS SHORT BY TOTAL AMOUNT" );
            }

            f.ShowDialog();
           
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport f = new frmInventoryReport();
            f.LoadSoldItems("select c.pcode, p.pdesc, c.price, sum (c.qty) as tot_qty, sum (c.disc) as tot_disc, sum (c.total) as total  from tblCart1 as c inner join TblProduct1 as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "'  group by c.pcode, p.pdesc, c.price", " From : " + dateTimePicker4.Value.ToString() + " To : " + dateTimePicker3.Value.ToString());
            f.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(cdTopSelling.Text == String.Empty)
            {
                MessageBox.Show("Please select from the dropdown list.","Warning",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            LoadRecords();
            LoadChart();
        }

        private void cdTopSelling_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        public void LoadChart()
        {
            SqlDataAdapter da = new SqlDataAdapter();
            cn.Open();
            if (cdTopSelling.Text == "Short by Qty")
            {
               da = new SqlDataAdapter("select top 10 pcode,isnull(sum(qty),0) as qty from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'sold' group by pcode order by qty desc", cn);

            }
            else if (cdTopSelling.Text == "Short by Total Amount")
            {
             da= new SqlDataAdapter("select top 10 pcode, isnull(sum(total),0) as total from vwSoldItems where sdate between '" + dateTimePicker1.Value.ToString() + "' and '" + dateTimePicker2.Value.ToString() + "' and status like 'sold' group by pcode order by total desc", cn);
            }
            DataSet ds = new DataSet();
             da.Fill(ds, "TOPSELLING");
            chart1.DataSource = ds.Tables["TOPSELLING"];
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Doughnut;


            series.Name = "TOP SELLING";
            var chart = chart1;
            chart.Series[0].XValueMember = "pcode";
            if (cdTopSelling.Text == "Short by Qty") { chart.Series[0].YValueMembers ="qty"; }
            if (cdTopSelling.Text == "Short by Total Amount") { chart.Series[0].YValueMembers = "total"; }
            chart.Series[0].IsValueShownAsLabel = true;
             if (cdTopSelling.Text == "Short by Total Amount") { chart.Series[0].LabelFormat = ("#,##0.00"); }
            if (cdTopSelling.Text == "Short by Qty") { chart.Series[0].LabelFormat = ("#,##0.00"); }
            cn.Close();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {

                dataGridView2.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select c.pcode, p.pdesc, c.price, sum (c.qty) as tot_qty, sum (c.disc) as tot_disc, sum (c.total) as total  from tblCart1 as c inner join TblProduct1 as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "'  group by c.pcode, p.pdesc, c.price", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dataGridView2.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), Double.Parse(dr["price"].ToString()).ToString("#,##0.00"), dr["tot_qty"].ToString(), dr["tot_disc"].ToString(), Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select isnull(sum (total),0) from tblCart1  where status like 'Sold' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "'", cn);
                lblTotal.Text = Double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                cn.Close();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmCharts f = new frmCharts();
            f.lblTitel.Text = "Sold Items [" + dateTimePicker1.Value.ToShortDateString() + " - " + dateTimePicker4.Value.ToShortDateString() + "]";
            f.LoadCardSold("select p.pdesc, sum (c.total) as total  from tblCart1 as c inner join TblProduct1 as p on c.pcode = p.pcode where status like 'Sold' and sdate between '" + dateTimePicker4.Value.ToString() + "' and '" + dateTimePicker3.Value.ToString() + "'  group by p.pdesc order by total desc");
                f.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport frm = new frmInventoryReport();
            string param = "Date Convered: " + dateTimePicker8.Value.ToShortDateString() + " - " + dateTimePicker7.Value.ToShortDateString();
            frm.LoadStockReport("select * from vwStockin where cast(sdate as date) between '" + dateTimePicker8.Value.ToShortDateString() + "' and '" + dateTimePicker7.Value.ToShortDateString() + "' and status like 'Done'",param);
            frm.ShowDialog();
           
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadStockHistory();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CancelledOrder();
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmInventoryReport f = new frmInventoryReport();
            string param = "Date coverd: "+ dateTimePicker5.Value.ToString() + " - " + dateTimePicker6.Value.ToString()  ;
            f.LoadCancelReport("select * from vwCancelledOrder where sdate between '" + dateTimePicker5.Value.ToString() + "' and '" + dateTimePicker6.Value.ToString() + "'", param);
            f.ShowDialog();
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
