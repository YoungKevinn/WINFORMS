using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Client_DoMInhKhoa.Services;
namespace Client_DoMInhKhoa.Forms
{
    public partial class FormNhanVienDangNhap : Form
    {
        private readonly NhanVienService _nhanVienService;

        public FormNhanVienDangNhap()
        {
            InitializeComponent();
            _nhanVienService = new NhanVienService();
        }

        private async void btnBatDauCa_Click(object sender, EventArgs e)
        {
            string maNv = txtMaNhanVien.Text.Trim();

            if (string.IsNullOrEmpty(maNv))
            {
                MessageBox.Show("Vui lòng nhập mã nhân viên.",
                    "Thiếu thông tin",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnBatDauCa.Enabled = false;
            Cursor = Cursors.WaitCursor;

            try
            {
                // Gọi API kiểm tra mã nhân viên
                var nv = await _nhanVienService.LayTheoMaAsync(maNv);
                if (nv == null)
                {
                    MessageBox.Show("Mã nhân viên không tồn tại hoặc đã ngừng hoạt động.",
                        "Không hợp lệ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                // TODO: lưu thông tin nhân viên vào Session nếu có
                // SessionHienTai.MaNhanVien = nv.MaNhanVien;
                // SessionHienTai.TenNhanVien = nv.HoTen;

                // Mở form bán hàng (dashboard cho nhân viên)
                var formBanHang = new FormBanHangNhanVien(nv);
                formBanHang.Show();
                this.Hide();
            }
            catch (Exception)
            {
                MessageBox.Show("Không thể kiểm tra mã nhân viên. Vui lòng thử lại sau.",
                    "Lỗi hệ thống",
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
