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
    public partial class frmUserAccount : Form

    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        DBConnection dbcon = new DBConnection();
       Form1 f;

        public frmUserAccount(Form1 f )

        {
            InitializeComponent();
            cn = new SqlConnection(dbcon.MyConnection());
            this.f = f;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void frmUserAccount_Resize(object sender, EventArgs e)
        {
         
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text != txtRePassword.Text)
                {
                    MessageBox.Show("Passowrd did not Math!","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                cn.Open();
                cm = new SqlCommand("insert into tbluser (username,password,role,name) Values (@username,@password,@role,@name)",cn);
                cm.Parameters.AddWithValue("@username", txtUser.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                cm.Parameters.AddWithValue("@role", cbRole.Text);
                cm.Parameters.AddWithValue("@name", txtName.Text);
                cm.ExecuteNonQuery();
                cn.Close();MessageBox.Show("New Account has saved!");
                Clear();



            }catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message);

            }
        }
        private void Clear()
        {
            txtName.Clear();
            txtPassword.Clear();
            txtRePassword.Clear();
            txtUser.Clear();
            cbRole.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void frmUserAccount_Load(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage2_Click_1(object sender, EventArgs e)
        {

        }

        private void btnSav_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtOld.Text != f._pass)
                {
                    MessageBox.Show("old password did not mached!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtNew.Text != txtRePass.Text)
                {
                    MessageBox.Show("Confirm new password did not mached!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                cn.Open();
                cm = new SqlCommand("update tblUser set password = @password where username like @username", cn);
                cm.Parameters.AddWithValue("@password", txtNew.Text);
                cm.Parameters.AddWithValue("@username", txtU.Text);
               
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Password has been successfully changed!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
              
                txtRePass.Clear();
                txtOld.Clear();
                txtNew.Clear();
            }
            catch(Exception ex)
            {
                
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtuser2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("select * from tblUser where username =@username",cn);
                cm.Parameters.AddWithValue("@username", txtuser2.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    checkBox1.Checked = bool.Parse(dr["isactive"].ToString());
                }else
                {
                    checkBox1.Checked = false;
                }
                dr.Close();
                cn.Close();

            }catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                bool found = true;
                cn.Open();
                cm = new SqlCommand("select * from tblUser where username =@username", cn);
                cm.Parameters.AddWithValue("@username", txtuser2.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows) { found = true;  }else { found = false; }
                dr.Close();
                cn.Close();

                if (found == true)
                {
                    cn.Open();
                    cm = new SqlCommand("update tblUser set isactive =@isactive where username =@username", cn);
                    cm.Parameters.AddWithValue("@isactive", checkBox1.Checked.ToString());
                    cm.Parameters.AddWithValue("@username", txtuser2.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Account status has been successfully updated", "Account", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtuser2.Clear();
                    checkBox1.Checked = false;
                }
                else
                {
                    MessageBox.Show("Account not exists!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txtOld_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
