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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Oracle.ManagedDataAccess.Types;

namespace THUYSAN2
{
    public partial class frmTaoMoiUser : Form
    {
        private OracleConnect db;
        public frmTaoMoiUser()
        {
            InitializeComponent();
            db = new OracleConnect();
            check1.CheckedChanged += new EventHandler(check1_CheckedChanged);
            check2.CheckedChanged += new EventHandler(check2_CheckedChanged);
        }
        private string GenerateEmployeeId(int employeeCount)
        {
            int newEmployeeNumber = employeeCount + 1; 
                return "NV" + newEmployeeNumber.ToString("D3"); 
        }

        private void btn_Back_Click_1(object sender, EventArgs e)
        {
            this.Hide();           
            var homeForm = new Main();
            homeForm.Show();
            homeForm.ShowHome();
        }
        private int GetEmployeeCount()
        {
            int employeeCount = 0;
            OracleConnect db = new OracleConnect();

            using (OracleCommand countCmd = new OracleCommand("Dem_SoNhanVien", db.Connection))
            {
                countCmd.CommandType = CommandType.StoredProcedure;
                OracleParameter p_Count = new OracleParameter("p_Count", OracleDbType.Int32, ParameterDirection.Output);
                countCmd.Parameters.Add(p_Count);

                try
                {
                    db.Connection.Open();
                    countCmd.ExecuteNonQuery();

                    if (p_Count.Value != DBNull.Value && p_Count.Value != null)
                    {
                        employeeCount = Convert.ToInt32(p_Count.Value);
                    }

                }
                catch
                {

                    employeeCount = 0;
                }
                finally
                {
                    db.Connection.Close();
                }
            }

            return employeeCount;
        }



        private void btn_Create_Click_1(object sender, EventArgs e)
        {
            if (txt_Pass.Text != txt_PassCfm.Text)
            {
                MessageBox.Show("Mật khẩu và Xác nhận mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int employeeCount = GetEmployeeCount();
            txt_ID.Text = GenerateEmployeeId(employeeCount);

            OracleConnect db = new OracleConnect();
            DateTime ngaySinh;

            if (!DateTime.TryParse(msk_DoB.Text, out ngaySinh))
            {
                MessageBox.Show("Ngày sinh không hợp lệ. Vui lòng nhập lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            OracleParameter[] parameters = new OracleParameter[]
            {
        new OracleParameter("p_NhanVienID", OracleDbType.Varchar2) { Value = txt_ID.Text },
        new OracleParameter("p_TenNhanVien", OracleDbType.Varchar2) { Value = txt_Ten.Text },
        new OracleParameter("p_ChucVu", OracleDbType.Varchar2) { Value = cbo_PQ.SelectedItem?.ToString() },
        new OracleParameter("p_SoDienThoai", OracleDbType.Varchar2) { Value = txt_DT.Text },
        new OracleParameter("p_NgaySinh", OracleDbType.Date) { Value = ngaySinh },
        new OracleParameter("p_DiaChi", OracleDbType.Varchar2) { Value = txt_DC.Text },
        new OracleParameter("p_NoiSinh", OracleDbType.Varchar2) { Value = cbo_NS.SelectedItem?.ToString() },
        new OracleParameter("p_Password", OracleDbType.Varchar2) { Value = txt_Pass.Text }
            };

            db.ExecuteProcedureWithOutput("ThemNhanVien", parameters);

            MessageBox.Show("Tạo mới nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }



        private void check1_CheckedChanged(object sender, EventArgs e)
        {
            if (check1.Checked)
            {
                txt_Pass.UseSystemPasswordChar = false;
            }
            else
            {
                txt_Pass.UseSystemPasswordChar = true;
            }
        }

        private void check2_CheckedChanged(object sender, EventArgs e)
        {
            if (check2.Checked)
            {
                txt_PassCfm.UseSystemPasswordChar = false;
            }
            else
            {
                txt_PassCfm.UseSystemPasswordChar = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmTaoMoiUser_Load(object sender, EventArgs e)
        {
            int employeeCount = GetEmployeeCount();
            txt_ID.Text = GenerateEmployeeId(employeeCount);
        }
    }
}
