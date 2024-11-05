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

namespace THUYSAN2
{
    public partial class frmChinhSuaNhanVien : Form
    {
        private OracleConnect db;
        public frmChinhSuaNhanVien()
        {
            InitializeComponent();
            db = new OracleConnect();
            LoadEmployeeData();
            LoadResourceNames();
        }

        private void frmChinhSuaNhanVien_Load(object sender, EventArgs e)
        {

        }
        private void LoadEmployeeData()
        {
            OracleConnect db = new OracleConnect();

            using (OracleCommand cmd = new OracleCommand("LayThongTinNhanVien", db.Connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                OracleParameter cursorParam = new OracleParameter("p_Cursor", OracleDbType.RefCursor);
                cursorParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(cursorParam);

                try
                {
                    db.Connection.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(cmd))
                    {
                        DataTable employeeData = new DataTable();
                        adapter.Fill(employeeData);

                        // In ra danh sách cột
                        foreach (DataColumn column in employeeData.Columns)
                        {
                            Console.WriteLine(column.ColumnName);
                        }

                        tbl_NhanVien.DataSource = employeeData;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải dữ liệu nhân viên: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    db.Connection.Close();
                }
            }
        }


        private void tbl_NhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = tbl_NhanVien.Rows[e.RowIndex];

                txt_id.Text = row.Cells["NHANVIENID"].Value?.ToString();
                txt_ten.Text = row.Cells["TENNHANVIEN"].Value?.ToString();
                cbo_PQ.SelectedItem = row.Cells["CHUCVU"].Value?.ToString();
                txt_DT.Text = row.Cells["SODIENTHOAI"].Value?.ToString();
                msk_dob.Text = Convert.ToDateTime(row.Cells["NGAYSINH"].Value).ToString("dd/MM/yyyy");
                txt_DC.Text = row.Cells["DIACHI"].Value?.ToString();
                cbo_noisinh.SelectedItem = row.Cells["NOISINH"].Value?.ToString();
            }
        }
        private void LoadResourceNames()
        {
            try
            {
                OracleConnect db = new OracleConnect();
                db.Connection.Open();

                using (OracleCommand command = new OracleCommand("GetResourceNames", db.Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    OracleParameter cursorParam = new OracleParameter("p_Cursor", OracleDbType.RefCursor);
                    cursorParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(cursorParam);

                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        // Xóa tất cả các mục hiện có trong ComboBox
                        cbo_PQ.Items.Clear();

                        // Đọc từng dòng và thêm vào ComboBox
                        while (reader.Read())
                        {
                            string resourceName = reader["RESOURCE_NAME"].ToString();
                            cbo_PQ.Items.Add(resourceName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.CloseConnection(); // Đóng kết nối
            }
        }
        private void SaveCurrentEmployeeData()
        {

            string id = txt_id.Text;
            string ten = txt_ten.Text;
            string soDienThoai = txt_DT.Text;
            DateTime dob = DateTime.ParseExact(msk_dob.Text, "dd/MM/yyyy", null);
            string diaChi = txt_DC.Text;
            string noisinh = cbo_noisinh.SelectedItem?.ToString();

        }

        private void btn_Enter_Click(object sender, EventArgs e)
        {
            if (cbo_PQ.SelectedItem != null && !string.IsNullOrWhiteSpace(txt_SL.Text))
            {
                string resourceName = cbo_PQ.SelectedItem.ToString();
                string quantity = txt_SL.Text;

                // Kết hợp giá trị thành một chuỗi
                string combinedValue = $"{resourceName} {quantity}";

                txt_Result.Text = combinedValue;

                SaveCurrentEmployeeData();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn resource và nhập số lượng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_Result.Text))
            {
                string[] parts = txt_Result.Text.Split(' '); // Tách chuỗi thành hai phần
                if (parts.Length == 2)
                {
                    string resourceName = parts[0];
                    string quantity = parts[1];

                    // Cập nhật thông tin nhân viên
                    UpdateEmployeeData(txt_id.Text, txt_ten.Text, txt_DT.Text, msk_dob.Text, txt_pass.Text, txt_DC.Text, cbo_noisinh.SelectedItem?.ToString(), resourceName, quantity);
                }
                else
                {
                    MessageBox.Show("Kết quả không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập giá trị cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void UpdateEmployeeData(string userId, string ten, string soDienThoai, string dob, string pass, string diaChi, string noisinh, string resourceName, string quantity)
        {
            try
            {
                db.Connection.Open();

                using (OracleCommand command = new OracleCommand("CapNhatThongTinNguoiDung", db.Connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số cho thủ tục
                    command.Parameters.Add(new OracleParameter("p_userId", OracleDbType.Varchar2) { Value = userId });
                    command.Parameters.Add(new OracleParameter("p_ten", OracleDbType.Varchar2) { Value = ten });
                    command.Parameters.Add(new OracleParameter("p_soDienThoai", OracleDbType.Varchar2) { Value = soDienThoai });
                    command.Parameters.Add(new OracleParameter("p_ngaySinh", OracleDbType.Date) { Value = DateTime.ParseExact(dob, "dd/MM/yyyy", null) });
                    command.Parameters.Add(new OracleParameter("p_matKhau", OracleDbType.Varchar2) { Value = pass });
                    command.Parameters.Add(new OracleParameter("p_diaChi", OracleDbType.Varchar2) { Value = diaChi });
                    command.Parameters.Add(new OracleParameter("p_noiSinh", OracleDbType.Varchar2) { Value = noisinh });
                    command.Parameters.Add(new OracleParameter("p_resourceName", OracleDbType.Varchar2) { Value = resourceName });
                    command.Parameters.Add(new OracleParameter("p_quantity", OracleDbType.Int32) { Value = Convert.ToInt32(quantity) });

                    command.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.Connection.Close();
            }
        }
        //private void UpdateUserResource(string userId, string resourceName, string quantity)
        //{
        //    try
        //    {

        //        db.Connection.Open();

        //        using (OracleCommand command = new OracleCommand("CapNhatThongTinNguoiDung", db.Connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            // Thêm tham số cho thủ tục
        //            command.Parameters.Add(new OracleParameter("p_userId", OracleDbType.Varchar2) { Value = userId });
        //            command.Parameters.Add(new OracleParameter("p_resourceName", OracleDbType.Varchar2) { Value = resourceName });
        //            command.Parameters.Add(new OracleParameter("p_quantity", OracleDbType.Int32) { Value = Convert.ToInt32(quantity) });

        //            command.ExecuteNonQuery();
        //            MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi khi cập nhật: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        db.Connection.Close();
        //    }
        //}
    }
}
