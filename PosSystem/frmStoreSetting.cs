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
    public partial class frmStoreSetting : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        DBConnection db = new DBConnection();
        public frmStoreSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(db.MyConnection());
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public void LoadRecord()
        {
            cn.Open();
            cm = new SqlCommand("select * from tblStore", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                txtAddress.Text = dr["address"].ToString();
                txtStore.Text = dr["store"].ToString();
                txtPhone.Text = dr["phone"].ToString();

            }
            else
            {
                txtStore.Clear();
                txtAddress.Clear();
            }
            dr.Close();
            cn.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Save Store Deatils?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult .Yes)
                {
                    {
                        int count;
                        cn.Open(); cm = new SqlCommand("select count (*) from tblStore", cn);
                        count = int.Parse(cm.ExecuteScalar().ToString());
                        cn.Close();
                       if(count > 0)
                        {
                            cn.Open();
                            cm = new SqlCommand("update tblStore set address =@address, phone =@phone, store =@store ", cn);
                            cm.Parameters.AddWithValue("@store", txtStore.Text);
                            cm.Parameters.AddWithValue("@address", txtAddress.Text);
                            cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                            cm.ExecuteNonQuery();

                        }
                        else
                        {
                            cn.Open();
                            cm = new SqlCommand("insert into tblStore (store , address, phone) values (@store,@address,@phone)", cn);
                            cm.Parameters.AddWithValue("@store", txtStore.Text);
                            cm.Parameters.AddWithValue("@address", txtAddress.Text);
                            cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                            cm.ExecuteNonQuery();
                        }
                        MessageBox.Show("Store Details has been successfully saved!", "Save Store", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    cn.Close();
                   
                }

            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmStoreSetting_Load(object sender, EventArgs e)
        {

        }
    }
}
