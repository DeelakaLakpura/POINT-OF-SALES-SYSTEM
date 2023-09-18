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



    public partial class frmProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
        frmProduct_List flist;


        public frmProduct(frmProduct_List frm)
        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            flist = frm;

        }
        public void LocalCategory()
        {
            comboBox2.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select category  from TblCatecory", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBox2.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void LocalBrand()
        {
            comboBox1.Items.Clear();
            cn.Open();
            cm = new SqlCommand("select brand from BrandTbl ", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBox1.Items.Add(dr[0].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmProduct_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this product?", "Save product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string bid = ""; string cid = "";
                    cn.Open();
                    cm = new SqlCommand("select id from BrandTbl where brand like '" + comboBox1.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows) { bid = dr[0].ToString(); }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("select id from TblCatecory where category like '" + comboBox2.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows) { cid = dr[0].ToString(); }
                    dr.Close();
                    cn.Close();



                    cn.Open();
                    cm = new SqlCommand("INSERT INTO TblProduct1 (pcode,barcode,pdesc,bid,cid,price,reorder) VALUES(@pcode,@barcode,@pdesc,@bid,@cid,@price,@reorder)", cn);
                    cm.Parameters.AddWithValue("@pcode", TxtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@bid", bid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price",double.Parse( txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder",int.Parse( txtReOrder.Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product has been successfully saved");
                    Clear();
                    flist.LoadRecords();

                }
            } catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }
        public void Clear()
        {
            txtPrice.Clear();
            txtBarcode.Clear();
            txtPdesc.Clear();
            TxtPcode.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            TxtPcode.Focus();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;

        }

        public void Clear1()
        {
            txtPrice.Clear();
            txtBarcode.Clear();
            txtPdesc.Clear();
            TxtPcode.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            TxtPcode.Focus();
            btnSave.Enabled = false;
            btnUpdate.Enabled = true; ;

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to update this product?", "Save product", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string bid = ""; string cid = "";
                    cn.Open();
                    cm = new SqlCommand("select id from BrandTbl where brand like '" + comboBox1.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        bid = dr[0].ToString();
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("select id from TblCatecory where category like '" + comboBox2.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows) { cid = dr[0].ToString(); }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("UPDATE TblProduct1 SET barcode = @barcode,pdesc=@pdesc,bid=@bid,cid=@cid,price=@price,reorder=@reorder where pcode like @pcode", cn);
                    cm.Parameters.AddWithValue("@pcode", TxtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@bid", bid);
                    cm.Parameters.AddWithValue("@cid", cid);
                    cm.Parameters.AddWithValue("@price",double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder",int.Parse(txtReOrder .Text));
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Product has been successfully updated.");
                    Clear1();
                    flist.LoadRecords();
                    this.Dispose();

                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46)
            {

            }
            else if (e.KeyChar == 8)
            {

            }
            else if ((e.KeyChar < 48 || (e.KeyChar > 57)))
            {
                e.Handled = true;
            }
            }

        private void button1_Click(object sender, EventArgs e)
        {
            frmscanBarcode frm = new frmscanBarcode(this);
            frm.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmCreatebarcode FRM = new frmCreatebarcode();
            FRM.ShowDialog();
        }
    }
    
}
