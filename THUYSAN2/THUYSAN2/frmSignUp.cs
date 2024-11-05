using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THUYSAN2
{
    public partial class frmSignUp : Form
    {
        private OracleConnect oracleConnect;
        private MaHoaMatKhau mhmk;
        public frmSignUp()
        {
            InitializeComponent();
            oracleConnect = new OracleConnect();
            mhmk = new MaHoaMatKhau();
            checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged_1);
            checkBox2.CheckedChanged += new EventHandler(checkBox2_CheckedChanged_1);
        }
        private void frmSignUp_Load(object sender, EventArgs e)
        {

        }            
        private void button1_Click_1(object sender, EventArgs e)
        {
            string username = txt_Username.Text;
            string password = txt_Password.Text;
            string tenKhachHang = txt_HoTen.Text;
            string email = txt_Email.Text;
            string soDienThoai = txt_DienThoai.Text;
            string diachi = txt_DiaChi.Text;

            string hashedPassword = mhmk.EncryptPassword(password);

            try
            {
                OracleConnect db = new OracleConnect();

                using (OracleCommand checkCommand = new OracleCommand("KiemTraUsernameTonTai", db.Connection))
                    {
                        checkCommand.CommandType = CommandType.StoredProcedure;

                        var existsParameter = new OracleParameter("p_exists", OracleDbType.Int32) { Direction = ParameterDirection.Output };
                        checkCommand.Parameters.Add(new OracleParameter("p_username", username));
                        checkCommand.Parameters.Add(existsParameter);

                        checkCommand.ExecuteNonQuery();

                        // Chuyển đổi giá trị từ OracleDecimal sang int
                        int v_exists = ((OracleDecimal)existsParameter.Value).ToInt32();
                        if (v_exists > 0)
                        {
                            MessageBox.Show("Tài khoản đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    using (OracleCommand command = new OracleCommand("TAO_NGUOIDUNG", db.Connection))
                    { 
                        command.CommandType = CommandType.StoredProcedure;

                        string oracleUsername = username.Length > 27 ? username.Substring(0, 27) : username;

                        command.Parameters.Add(new OracleParameter("p_username", oracleUsername));
                        command.Parameters.Add(new OracleParameter("p_password", hashedPassword));
                        command.Parameters.Add(new OracleParameter("p_tenkh", tenKhachHang));
                        command.Parameters.Add(new OracleParameter("p_diachi", diachi));
                        command.Parameters.Add(new OracleParameter("p_sodienthoai", soDienThoai));
                        command.Parameters.Add(new OracleParameter("p_email", email));

                    command.ExecuteNonQuery();
                    }      

                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide(); new frmSignIn().Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình đăng ký: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                txt_Password.UseSystemPasswordChar = false;
            }
            else
            {
                txt_Password.UseSystemPasswordChar = true;
            }
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                txt_cfrPassword.UseSystemPasswordChar = false;
            }
            else
            {
                txt_cfrPassword.UseSystemPasswordChar = true;
            }
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new frmSignIn().Show();
        }
    }
}
