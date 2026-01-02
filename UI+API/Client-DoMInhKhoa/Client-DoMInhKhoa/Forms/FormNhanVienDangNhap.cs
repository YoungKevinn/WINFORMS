using System;
using System.Windows.Forms;
using Client_DoMInhKhoa.Services;
using Client_DoMInhKhoa.Models;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormNhanVienDangNhap : Form
    {
        private readonly DangNhapService _dangNhapService;

        public FormNhanVienDangNhap()
        {
            InitializeComponent();
            _dangNhapService = new DangNhapService();

            txtMatKhau.KeyDown += TxtMatKhau_KeyDown;
        }

        private void TxtMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBatDauCa.PerformClick();
            }
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
                // Login nhân viên
                var result = await _dangNhapService.DangNhapNhanVienAsync(taiKhoan, matKhau);

                // FormBanHangNhanVien yêu cầu NhanVienDto trong constructor
                var nv = new NhanVienDto
                {
                    MaNhanVien = taiKhoan,
                    HoTen = result.HoTen ?? taiKhoan,
                    VaiTro = string.IsNullOrWhiteSpace(result.VaiTro) ? "NhanVien" : result.VaiTro,
                    DangHoatDong = true
                };

                var formBanHang = new FormBanHangNhanVien(nv);
                formBanHang.Show();
                this.Hide();
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
            var f = new FormChonVaiTro();
            f.Show();
            this.Hide();
        }
    }
}
