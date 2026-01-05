using System;
using System.Linq;
using System.Windows.Forms;
using Client_DoMInhKhoa.Services;
using Client_DoMInhKhoa.Models;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormNhanVienDangNhap : Form
    {
        private readonly DangNhapService _dangNhapService;
        private bool _loginSucceeded = false;

        public FormNhanVienDangNhap()
        {
            InitializeComponent();
            _dangNhapService = new DangNhapService();

            txtMatKhau.KeyDown += TxtMatKhau_KeyDown;

            // ✅ Nếu đóng form login NV mà CHƯA login thành công -> hiện lại FormChonVaiTro
            this.FormClosed += (_, __) =>
            {
                if (!_loginSucceeded) ShowChonVaiTro();
            };
        }

        private void ShowChonVaiTro()
        {
            var f = Application.OpenForms.OfType<FormChonVaiTro>().FirstOrDefault();
            if (f == null || f.IsDisposed)
                f = new FormChonVaiTro();

            f.Show();
            if (f.WindowState == FormWindowState.Minimized)
                f.WindowState = FormWindowState.Normal;

            f.BringToFront();
            f.Activate();
        }

        private void TxtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnBatDauCa.PerformClick();
        }

        private async void btnBatDauCa_Click(object sender, EventArgs e)
        {
            string taiKhoan = txtMaNhanVien.Text.Trim();
            string matKhau = txtMatKhau.Text;

            if (string.IsNullOrEmpty(taiKhoan) || string.IsNullOrEmpty(matKhau))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu.",
                    "Thiếu thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnBatDauCa.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                var result = await _dangNhapService.DangNhapNhanVienAsync(taiKhoan, matKhau);

                var nv = new NhanVienDto
                {
                    MaNhanVien = taiKhoan,
                    HoTen = result.HoTen ?? taiKhoan,
                    VaiTro = string.IsNullOrWhiteSpace(result.VaiTro) ? "NhanVien" : result.VaiTro,
                    DangHoatDong = true
                };

                _loginSucceeded = true;

                var formBanHang = new FormBanHangNhanVien(nv);
                formBanHang.Show();

                // ✅ đóng form login NV để không nằm ẩn trong background
                this.Hide();
                BeginInvoke(new Action(() => this.Close()));
            }
            catch (Exception ex)
            {
                string message;

                if (ex.Message.Contains("401") || ex.Message.Contains("Sai", StringComparison.OrdinalIgnoreCase))
                    message = "Sai tài khoản hoặc mật khẩu.";
                else
                    message = "Không thể đăng nhập. Vui lòng kiểm tra kết nối hoặc thử lại sau.";

                MessageBox.Show(message,
                    "Đăng nhập thất bại",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnBatDauCa.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void btnQuayLai_Click(object sender, EventArgs e)
        {
            ShowChonVaiTro();
            this.Close();
        }
    }
}
