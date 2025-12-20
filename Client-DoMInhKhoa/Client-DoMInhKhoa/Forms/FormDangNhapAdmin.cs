using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormDangNhapAdmin : Form
    {
        private readonly DangNhapService _dangNhapService;
        private bool _isPasswordVisible = false;
        public FormDangNhapAdmin()
        {
            InitializeComponent();
            _dangNhapService = new DangNhapService();
            panelLogo.BackgroundImage = Image.FromFile("Images\\p2.png");
            panelLogo.BackgroundImageLayout = ImageLayout.Zoom;

            txtMatKhau.KeyDown += TxtMatKhau_KeyDown;
        }
        private void btnToggleMatKhau_Click(object sender, EventArgs e)
        {
            _isPasswordVisible = !_isPasswordVisible;

            if (_isPasswordVisible)
            {
                // Hiện mật khẩu
                txtMatKhau.UseSystemPasswordChar = false;
                txtMatKhau.PasswordChar = '\0';

                btnToggleMatKhau.Text = "Ẩn";
            }
            else
            {
                // Ẩn mật khẩu
                txtMatKhau.UseSystemPasswordChar = false;
                txtMatKhau.PasswordChar = '●';

                btnToggleMatKhau.Text = "Hiện";
            }
        }

        private void TxtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnDangNhap.PerformClick();
            }
        }

        private async void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tenDangNhap = txtTenDangNhap.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.",
                    "Thiếu thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnDangNhap.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                var result = await _dangNhapService.DangNhapAsync(tenDangNhap, matKhau);

                // TODO: lưu session nếu cần

                var formTrangChu = new FormTrangChuAdmin();
                formTrangChu.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                string message;

                // Nếu message của service có nói rõ 401 / sai mật khẩu thì che lại
                if (ex.Message.Contains("401") || ex.Message.Contains("Sai mật khẩu", StringComparison.OrdinalIgnoreCase))
                {
                    message = "Sai tên đăng nhập hoặc mật khẩu.";
                }
                else
                {
                    // Lỗi khác: network, server chết, v.v.
                    message = "Không thể đăng nhập. Vui lòng kiểm tra lại kết nối hoặc thử lại sau.";
                }

                MessageBox.Show(message,
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnDangNhap.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            var formChon = new FormChonVaiTro();
            formChon.Show();
            this.Close();
        }

        private void panelLogo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtTenDangNhap_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblSlogan_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void FormDangNhap_Load(object sender, EventArgs e)
        {

        }
    }
}
