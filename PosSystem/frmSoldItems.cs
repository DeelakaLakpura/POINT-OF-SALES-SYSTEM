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
    public partial class frmSoldItems : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        // frmPOS fp;
        public string suser;

        public frmSoldItems()
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;
            LocalRecord();
            LoadCashier();
            // fp = frm;
           


        }

        private void frmSoldItems_Load(object sender, EventArgs e)
        {
            LocalRecord();
         
        }
        public void LocalRecord()
        {
            int i = 0;
            double _total = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            if (cbCashier.Text == "All Cashier")  { cm = new SqlCommand("select c.id ,c.transno,c.pcode, p.pdesc, c.price ,c.qty, c.disc ,total from tblCart1 as c inner join TblProduct1 as p on c.pcode = p.pcode where status like 'sold' and sdate between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "'", cn); }
            else { cm = new SqlCommand("select c.id ,c.transno,c.pcode, p.pdesc, c.price ,c.qty, c.disc ,total from tblCart1 as c inner join TblProduct1 as p on c.pcode = p.pcode where status like 'sold' and sdate between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' and  Cashier like '" + cbCashier.Text+ "'", cn); }
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                _total += double.Parse(dr["total"].ToString());
                dataGridView1.Rows.Add(i, dr["id"].ToString(), dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString());
            }
            dr.Close();
            cn.Close();
            lblTotal1.Text =_total.ToString("#,##0.00");
 
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
      

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
         
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "ColCancel")
            {
                frmCancelDetails f = new frmCancelDetails(this);
                f.txtID.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                f.txtTransno.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                f.txtPcode.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                f.txtDesc.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                f.txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                f.txtQty.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                f.txtDiscount.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                f.txtTotal.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                f.txtCancelled.Text = suser;
                f.ShowDialog();


            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            frmReportSold frm = new frmReportSold(this);
            frm.LoadReport();
            frm.ShowDialog();
        }

        private void cbCashier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

            public void LoadCashier()
            {
                cbCashier.Items.Clear();
                cbCashier.Items.Add("All Cashier");
                cn.Open();
                cm = new SqlCommand("Select * from tblUser where role like 'Cashier'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbCashier.Items.Add(dr["name"].ToString());
                }
                dr.Close();
                cn.Close();
            }
        
        private void cbCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cbCashier_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            LocalRecord();
        }
    }
    

}
