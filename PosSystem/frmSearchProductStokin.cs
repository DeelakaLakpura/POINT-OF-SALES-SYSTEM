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

    public partial class frmSearchProductStokin : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        string stitle = "PosSystem";
        frmStockin slist;

        public frmSearchProductStokin(frmStockin flist)
        {
            InitializeComponent();
            slist = flist;
            cn = new SqlConnection(dbcon.MyConnection());
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                string ColName = dataGridView1.Columns[e.ColumnIndex].Name;
                if (ColName == "colSelect")
                    if (slist.txtRefNo.Text == string.Empty) { MessageBox.Show("Please enter reference no", stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning); slist.txtRefNo.Focus(); return; }
                if (slist.txtBy.Text == string.Empty) { MessageBox.Show("Please enter Stock in by", stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning); slist.txtBy.Focus(); return; }
                {
                    if (MessageBox.Show("Add this item?", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        cn.Open();
                        cm = new SqlCommand("insert into tblStockin (refno,pcode,sdate,stockinby,vendorid) values (@refno,@pcode,@sdate,@stockinby,@vendorid)", cn);
                        cm.Parameters.AddWithValue("@refno", slist.txtRefNo.Text);
                        cm.Parameters.AddWithValue("@pcode", dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                        cm.Parameters.AddWithValue("@stockinby", slist.txtBy.Text);
                        cm.Parameters.AddWithValue("@sdate", slist.dt1.Value);
                        cm.Parameters.AddWithValue("@vendorid", slist.lblVendorID.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();

                        MessageBox.Show("Successfully Add!", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        slist.LoadStockIn();
                    }

                }
            }
        }
        public void LoadProduct()
        {
            int i = 0;
            dataGridView1.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select pcode,pdesc,qty from TblProduct1 where pdesc like '%" + txtSearch.Text + "%' order by pdesc", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dataGridView1.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}

