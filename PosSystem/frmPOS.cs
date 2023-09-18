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
    public partial class frmPOS : Form
    {
        string id;
        string price;
        
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnection dbcon = new DBConnection();
        SqlDataReader dr;
        string stitle = "Pos System";
        int qty;
        frmUserLogin f;
       

        public frmPOS(frmUserLogin frm)
        {

            InitializeComponent();
            lblDate.Text = DateTime.Now.ToLongDateString();
            this.KeyPreview = true;
            cn = new SqlConnection(dbcon.MyConnection());
            f = frm;
            NotifyCriticalItems();


        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            timer1.Start();
            cn.Open();
            cm = new SqlCommand("select * from tblStore", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
               
                lblAddress.Text = dr["address"].ToString();
                lblSname.Text = dr["store"].ToString();
                lblPhone.Text = dr["phone"].ToString();

            }
            dr.Close();
            cn.Close();
        }
        public void GetCartTotal()
        {
            double discount = Double.Parse(lblDiscount.Text);
            double subTot = Double.Parse(lblTotal.Text);
            double sales = Double.Parse(lblTotal.Text) - discount; 
            double vat = sales * dbcon.GetVal();
            double vatble = sales - vat;
            lblDisplayTotal.Text = vatble.ToString("#,##0.00");
          //  lblTotal.Text = sales.ToString("#,000.00");
            lblVat.Text = vat.ToString("#,##0.00");
            lblVatable.Text = vatble.ToString("#,##0.00");


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
                critical += i + ". " + dr["pdesc"].ToString() + Environment.NewLine;
            }
            dr.Close();
            cn.Close();

            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.close_window_16;
            popup.TitleText = count + " Critical Item(s)";
            popup.ContentText = critical;
            popup.Popup();



        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dataGridView1.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Remove this item ?", stitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tblCart1 where id like '" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Item Remove Successfully", stitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
            else if (colName == "ColAdd")
            {
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select sum(qty) as qty from TblProduct1 where  pcode like'" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' group by pcode", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();

                if (int.Parse(dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString()) < i)
                {

                    cn.Open(); cm = new SqlCommand("update tblCart1 set qty = qty +" + int.Parse(txtQty.Text) + " where transno like '" + lblTransno.Text + "' and pcode like '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();


                }
                else
                {
                    MessageBox.Show("Remaning qty on hand is" + i + " !", "Out of Stock!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }
            else if (colName == "ColRemove")
            {
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select sum(qty) as qty from tblCart1 where  pcode like'" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' and transno like '" + lblTransno.Text + "' group by transno, pcode", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();

                if (i > 1)
                {

                    cn.Open(); cm = new SqlCommand("update tblCart1 set qty = qty  - " + int.Parse(txtQty.Text) + " where transno like '" + lblTransno.Text + "' and pcode like '" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    LoadCart();

                }
                else
                    MessageBox.Show("Remaning qty on cart is" + i + " !", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;

            }
        }
                public void GetTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                string transno;
                int count;
                cn.Open();
                cm = new SqlCommand("select top 1 transno from tblCart1 where transno like '" + sdate + "%' order by id desc", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    transno = dr[0].ToString();
                    count = int.Parse(transno.Substring(8, 4));
                    lblTransno.Text = sdate + (count + 1);
                }
                else
                {
                    transno = sdate + "1001"; lblTransno.Text = transno;
                }
                dr.Close();
                cn.Close();


            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void btnTrans_Click(object sender, EventArgs e)
        {
            if(dataGridView1.Rows.Count > 0)
            {
                return;
            }
            GetTransNo();
            txtSearch.Enabled = true;
            txtSearch.Focus();
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text == string.Empty) { return; }
                else
                {
                    String _pcode;
                      double _price;
                      int _qty;
                       cn.Open();
                    cm = new SqlCommand("select * from TblProduct1 where barcode like '" + txtSearch.Text + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        qty = int.Parse(dr["qty"].ToString());
                        _pcode = dr["pcode"].ToString();
                        _price = double.Parse(dr["price"].ToString());
                        _qty = int.Parse(txtQty.Text);

                        dr.Close();
                        cn.Close();
                        AddToCart(_pcode, _price, _qty);
                      
                    }
                    else
                    {
                        dr.Close();
                        cn.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }

        }
        public void LoadCart()
        {
            try
            {
                Boolean hasrecord = false;
                dataGridView1.Rows.Clear();
                int i = 0;
                double total = 0;
                double discount = 0;
                cn.Open();
                cm= new SqlCommand("select c.id, c.pcode,p.pdesc,c.price,c.qty,c.disc,c.total from tblCart1 as c inner join  TblProduct1 as p on c.pcode = p.pcode where transno like '" + lblTransno.Text + "'and status like 'pending'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    total += Double.Parse(dr["total"].ToString());
                    discount += Double.Parse(dr["disc"].ToString());
                    dataGridView1.Rows.Add(i,dr["id"].ToString(),dr["pcode"].ToString(),dr["pdesc"].ToString(),dr["price"].ToString(),dr["qty"].ToString(),dr["disc"].ToString(),Double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                    hasrecord = true;
                }
                dr.Close();
                cn.Close();
                lblDiscount .Text = discount.ToString("#,000.00");
                lblTotal.Text = total.ToString("#,##0.00");
                GetCartTotal();
                if(hasrecord == true) { btnSattle.Enabled = true; btnDiscount.Enabled = true; btnCancel.Enabled = true; } else { btnSattle.Enabled = false; btnDiscount.Enabled = false; btnCancel.Enabled = false; }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, stitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cn.Close();
            }
        }
        private void AddToCart( string _pcode, double _price , int _qty)
        {
            bool found = false;
            int cart_qty = 0;
            cn.Open();
            cm = new SqlCommand("select * from tblCart1 where transno = @transno and pcode = @pcode", cn);
            cm.Parameters.AddWithValue("@transno",lblTransno.Text);
            cm.Parameters.AddWithValue("@pcode", _pcode);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                found = true;
                id = dr["id"].ToString();
                cart_qty = int.Parse(dr["qty"].ToString());
            }
            else { found = false; }
            dr.Close();
            cn.Close();

            if (found == true)
            {
                if (qty < (int.Parse(txtQty.Text) + cart_qty))
                {
                    MessageBox.Show("Unable to proceed. Remaning qty on hand is " + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.Open();
                cm = new SqlCommand("update tblCart1  set qty =  (qty+ " +_qty+ ")  where id= '" + id + "'", cn);

                cm.ExecuteNonQuery();
                cn.Close();

                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = txtSearch.Text.Length;
               LoadCart();
                this.Dispose();
            }
            else
            {
                if (qty < int.Parse(txtQty.Text))
                {
                    MessageBox.Show("Unable to proceed. Remaning qty on hand is " + qty, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.Open();
                cm = new SqlCommand("insert into tblCart1 (transno,pcode,price,qty,sdate,cashier) values (@transno ,@pcode, @price ,@qty, @sdate, @cashier)", cn);
                cm.Parameters.AddWithValue("@transno", lblTransno.Text);
                cm.Parameters.AddWithValue("@pcode", _pcode);
                cm.Parameters.AddWithValue("@price", _price);
                cm.Parameters.AddWithValue("@qty", _qty);
                cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                cm.Parameters.AddWithValue("@cashier",LblUser.Text);
                cm.ExecuteNonQuery();
                cn.Close();

                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = txtSearch.Text.Length;
               LoadCart();
                //this.Dispose();
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (lblTransno.Text == "00000000000000") { return; }
            frmLookUp frm = new frmLookUp(this);
            frm.LoadRecords();
            frm.ShowDialog();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            frmDiscount frm = new frmDiscount(this);
            frm.lblID.Text = id;
            frm.txtPrice.Text = price;
            frm.ShowDialog();
           
                
        }
        

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            int i = dataGridView1.CurrentRow.Index;
            id = dataGridView1[1, i].Value.ToString();
            price = dataGridView1[7, i].Value.ToString();
          
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = DateTime.Now.ToLongTimeString();
            label6.Text = DateTime.Now.ToLongDateString();
        }

        private void btnSattle_Click(object sender, EventArgs e)
        {
            frmSettel frm = new frmSettel(this);
            frm.txtSale.Text = lblDisplayTotal.Text;
            frm.ShowDialog();
        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            frmSoldItems frm = new frmSoldItems();
            frm.dateTimePicker1.Enabled = false;
            frm.dateTimePicker2.Enabled = false;
            frm.suser = LblUser.Text;
            frm.cbCashier.Enabled = false;
            frm.cbCashier.Text = LblUser.Text;
            frm.ShowDialog();
          
          

        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count >0 )
            {
                MessageBox.Show("Unable to Logout.Please cancel the transaction.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Logout Application?","Logout",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                this.Hide();
                frmUserLogin frm = new frmUserLogin();
                frm.ShowDialog();
            }
        }

        private void LblUser_Click(object sender, EventArgs e)
        {

        }

        private void frmPOS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                btnTrans_Click(sender, e);
            }

            if (e.KeyCode == Keys.F2)
            {
                btnSearch_Click(sender, e);
            }
            if (e.KeyCode == Keys.F5)
            {
                btnCancel_Click(sender, e);
            }

            if (e.KeyCode == Keys.F3)
            {
                btnDiscount_Click(sender, e);
            }
            if (e.KeyCode == Keys.F7)
            {
                btnChange_Click(sender, e);
            }
            if(e.KeyCode == Keys.F6)
            {
                btnSales_Click(sender, e);
            }
            if(e.KeyCode == Keys.F10)
            {
                btnClose_Click(sender, e);
            }

            if (e.KeyCode == Keys.F4)
            {
                btnSattle_Click(sender, e);
            }
            else if(e.KeyCode == Keys.F8 )
            {
                txtSearch.SelectionStart = 0;
                txtSearch.SelectionLength = txtSearch.Text.Length;
            }
           
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            frmChangePaasword frm = new frmChangePaasword(this);
           frm. ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Remove items from card?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) ==DialogResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("delete from tblCart1 where transno like '" + lblTransno.Text + "'", cn);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("All items has been successfully remove!", "Remove", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
