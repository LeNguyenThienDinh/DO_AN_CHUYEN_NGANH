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
    public partial class frmQLLoSanPham : Form
    {
        private OracleConnect db;
        public frmQLLoSanPham()
        {
            InitializeComponent();
            db = new OracleConnect();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                DataTable dataTable = db.GetDataFromProcedure("GetEmployeeData");

                dataGridView1.DataSource = dataTable;

                if (dataTable.Rows.Count > 0)
                {
                    textBox1.Text = dataTable.Rows[0]["NameColumn"].ToString(); 
                    textBox2.Text = dataTable.Rows[0]["AgeColumn"].ToString();   
                                                                                  
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        private void frmQLLoSanPham_Load(object sender, EventArgs e)
        {

        }
    }
}
