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
    public partial class frmAdjustment : Form
    {
        SqlCommand cm;
        SqlConnection cn;
        DBConnection db = new DBConnection();
        SqlDataReader dr;
        Form1 f;
        int _qty = 0;
        public frmAdjustment(Form1 f)
        {
            InitializeComponent();
           
            cn = new SqlConnection();
            cn.ConnectionString = db.MyConnection();
            this.f = f;
        }

        private void frmAdjustment_Load(object sender, EventArgs e)
        {

        }
        public void referenceNo()
        {
            Random rnd = new Random();
            txtRef.Text = rnd.Next().ToString();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadRecords()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select p.pcode,p.barcode,p.pdesc,b.brand,c.category,p.price,p.qty from TblProduct1 as p inner join BrandTbl as b on b.id = p.bid inner join TblCatecory as c on c.id = p.cid where p.pdesc like '" + txtSearch.Text + "%'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                LoadRecords();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if(colName =="Select")
            {
                txtPcode.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
               txtdesc.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString() +" " + dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                _qty = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(txtQty.Text)> _qty)
                {
                    MessageBox.Show("Stock on hand Quantity should be greater than from adjustment qty", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(cbCommands.Text == "Remove from Inventory")
                {
                    sqlStatement("update TblProduct1 set qty = (qty - " + int.Parse(txtQty.Text) + ") where pcode like '" + txtPcode.Text + "'");
                } 
                {
                    sqlStatement("update TblProduct1 set qty = (qty + " + int.Parse(txtQty.Text) + ") where pcode like '" + txtPcode.Text + "'");
                }

                sqlStatement("insert into tblAdjustment (referenceno, pcode, qty, action, remarks, sdate ,[user])values('" + txtRef.Text + "', '" + txtPcode.Text + "','" + int.Parse(txtQty.Text) + "','" + cbCommands.Text + "','" + txtRemarks.Text + "','" + DateTime.Now.ToShortDateString()+"','"+txtUser.Text+"')");
                MessageBox.Show("Stock has been successfully Adjusted","Process completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadRecords();
                Clear();
            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void sqlStatement(String _sql)
        {
            cn.Open();
            cm = new SqlCommand(_sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
        }
        public void Clear()
        {
            txtdesc.Clear();
            txtPcode.Clear();
            txtQty.Clear();
            txtRemarks.Clear();
            cbCommands.Text = "";
            referenceNo();


        }
    }

}
