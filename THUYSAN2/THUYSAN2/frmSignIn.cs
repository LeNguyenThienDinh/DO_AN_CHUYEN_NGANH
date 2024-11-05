using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;


namespace THUYSAN2
{
    public partial class frmSignIn : Form
    {
        private OracleConnect db;
        private MaHoaMatKhau mhmk;
        public frmSignIn()
        {
            InitializeComponent();
            db = new OracleConnect();
            mhmk = new MaHoaMatKhau();
            checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged_1);
        }
        private void frmSignIn_Load(object sender, EventArgs e)
        {

        }
        private bool IsUserLoggedIn()
        {
            if (UserSession.CurrentUser != null && UserSession.CurrentSessionID != null)
            {
                string sessionId = UserSession.CurrentSessionID;
                string username = UserSession.CurrentUser;

                OracleConnect db = new OracleConnect();

                    using (OracleCommand command = new OracleCommand("KiemtraSession", db.Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add("p_username", OracleDbType.Varchar2).Value = username;
                        var sessionIDParam = new OracleParameter("p_sessionID", OracleDbType.Varchar2, 255);
                        sessionIDParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(sessionIDParam);

                        command.ExecuteNonQuery();

                        string dbSessionID = sessionIDParam.Value.ToString();

                        // Kiểm tra nếu session ID từ DB khớp với session ID hiện tại
                        if (dbSessionID == sessionId)
                        {
                            return true; // Đã đăng nhập
                        }
                        else
                        {
                            ClearUserSession(); // Xóa thông tin phiên người dùng
                            return false; // Không còn đăng nhập
                        }
                    }   
            }
            return false; // Chưa đăng nhập
        }

        // Phương thức để xóa thông tin phiên người dùng
        private void ClearUserSession()
        {
            UserSession.CurrentUser = null;
            UserSession.CurrentRole = null;
            UserSession.CurrentSessionID = null;
        }
        private string ValidateUser(string username, string password)
        {
            OracleConnect db = new OracleConnect(); // Tạo đối tượng OracleConnect
            string result = null;
            string role = null;

            try
            {
                db.Connection.Open();
                using (OracleCommand command = new OracleCommand("Kiem_TraDangNhap", db.Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Sử dụng tham số cho thủ tục
                    command.Parameters.Add(new OracleParameter("p_username", OracleDbType.Varchar2) { Value = username });
                    command.Parameters.Add(new OracleParameter("p_password", OracleDbType.Varchar2) { Value = password });

                    // Khai báo output
                    command.Parameters.Add(new OracleParameter("p_result", OracleDbType.Varchar2, 100) { Direction = ParameterDirection.Output });
                    command.Parameters.Add(new OracleParameter("p_role", OracleDbType.Varchar2, 100) { Direction = ParameterDirection.Output });

                    command.ExecuteNonQuery();

                    result = command.Parameters["p_result"].Value.ToString();
                    role = command.Parameters["p_role"].Value.ToString();
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                db.Connection.Close(); 
            }

            if (result == "SUCCESS")
            {
                return role; 
            }
            else
            {
                return null; 
            }
        }
        private bool KiemTraUser(string username)
        {
            bool usernameExists = false;

            OracleConnect db = new OracleConnect();

            try
            {               
                    using (OracleCommand command = new OracleCommand("Kiem_TraDangNhap", db.Connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(new OracleParameter("p_username", OracleDbType.Varchar2)).Value = username;
                        command.Parameters.Add(new OracleParameter("p_exists", OracleDbType.Int32)).Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery();

                        int exists = int.Parse(command.Parameters["p_exists"].Value.ToString());
                        usernameExists = exists == 1;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return usernameExists; 
        }      
        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            frmSignUp signUpForm = new frmSignUp();
            signUpForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string taiKhoan = textBox1.Text;
            string matKhau = textBox2.Text;

            if (IsUserLoggedIn())
            {
                MessageBox.Show("Bạn đã đăng nhập rồi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string hashedPassword = mhmk.EncryptPassword(matKhau);
            string userRole = ValidateUser(taiKhoan, hashedPassword);

            if (userRole != null)
            {
                string sessionId = Guid.NewGuid().ToString();

                OracleConnect db = new OracleConnect();
                try
                {
                    db.Connection.Open(); // Mở kết nối trước khi thực hiện lệnh

                    using (OracleCommand updateCommand = new OracleCommand("Capnhat_SessionDangNhap", db.Connection))
                    {
                        updateCommand.CommandType = CommandType.StoredProcedure;
                        updateCommand.Parameters.Add("p_username", OracleDbType.Varchar2).Value = taiKhoan;
                        updateCommand.Parameters.Add("p_sessionID", OracleDbType.Varchar2).Value = sessionId;

                        updateCommand.ExecuteNonQuery();
                    }

                    UserSession.CurrentUser = taiKhoan;
                    UserSession.UserRole = userRole;
                    UserSession.SessionID = sessionId;

                    if (userRole == "ADMIN")
                    {
                        var frmXacThuc = new frmXacThucAdmin();
                        frmXacThuc.Show();
                    }
                    else
                    {
                        var homeForm = new Main();
                        homeForm.UpdateUserName(taiKhoan);
                        homeForm.Show();
                        homeForm.ShowHome();
                    }

                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    db.Connection.Close(); // Đảm bảo đóng kết nối sau khi hoàn thành
                }
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
            Application.Exit();
        }
    }
}
