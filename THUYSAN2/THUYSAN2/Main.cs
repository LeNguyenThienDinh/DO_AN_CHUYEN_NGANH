using System;
using System.Windows.Forms;

namespace THUYSAN2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(UserSession.CurrentUser))
            {
                UpdateUserName(UserSession.CurrentUser);
            }
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }
        private void ShowChildForm(Form childForm)
        {
            panel1.Controls.Clear();

            childForm.TopLevel = false;

            childForm.Dock = DockStyle.Fill;

            panel1.Controls.Add(childForm);

            childForm.Show();
        }
        public void ShowHome()
        {
            trangChínhToolStripMenuItem.PerformClick();
        }
        private void thủySảnTươiSốngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmThuySanTuoiSong = new frmThuySanTuoiSong();
            ShowChildForm(frmThuySanTuoiSong);
        }

        private void quảnLýToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmQLVungNuoi = new frmQLVungNuoi();
            ShowChildForm(frmQLVungNuoi);
        }

        private void quảnLýToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var frmQLHoNuoi = new frmQLHoNuoi();
            ShowChildForm(frmQLHoNuoi);
        }

        private void đồĐóngHộpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmThuySanDongHop = new frmThuySanDongHop();
            ShowChildForm(frmThuySanDongHop);
        }

        private void quảnLýLôToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmQLLoSanPham = new frmQLLoSanPham();
            ShowChildForm(frmQLLoSanPham);
        }

        private void đơnVịVậnChuyểnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmDonViVanChuyen = new frmDonViVanChuyen();
            ShowChildForm(frmDonViVanChuyen);
        }

        private void trangChínhToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmDashboard = new frmDashboard();
            ShowChildForm(frmDashboard);
        }
        private void đăngXuấtToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmSignIn = new frmSignIn();
            frmSignIn.Owner = this;
            frmSignIn.ShowDialog();
        }
        public void UpdateUserName(string userName)
        {
            linkLabel1.Text = $"Hello, {userName}";
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("HH:mm:ss tt");
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void tạoTàiKhoảnMớiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            var frmTaoMoiUser = new frmTaoMoiUser();
            frmTaoMoiUser.Owner = this;
            frmTaoMoiUser.ShowDialog();
        }

        private void chỉnhSửaThôngTinToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frmChinhSuaNhanVien = new frmChinhSuaNhanVien();
            ShowChildForm(frmChinhSuaNhanVien);
        }
    }
}
