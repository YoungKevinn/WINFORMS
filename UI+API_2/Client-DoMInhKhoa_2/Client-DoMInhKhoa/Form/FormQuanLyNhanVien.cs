using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormQuanLyNhanVien : Form
    {
        private readonly NhanVienService _service = new NhanVienService();

        private List<NhanVienDto> _all = new List<NhanVienDto>();

        public FormQuanLyNhanVien()
        {
            InitializeComponent();
        }

        private async void FormQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            await ReloadAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await ReloadAsync();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private async void btnThem_Click(object sender, EventArgs e)
        {
            await ThemAsync();
        }

        private async void btnSua_Click(object sender, EventArgs e)
        {
            await SuaAsync();
        }

        private async void btnKhoaMo_Click(object sender, EventArgs e)
        {
            await KhoaMoAsync();
        }

        private async void btnResetPass_Click(object sender, EventArgs e)
        {
            await ResetPassAsync();
        }

        private async void dgvNhanVien_DoubleClick(object sender, EventArgs e)
        {
            await SuaAsync();
        }

        private NhanVienDto? Current()
        {
            return bsNhanVien.Current as NhanVienDto;
        }

        private async Task ReloadAsync()
        {
            try
            {
                _all = await _service.LayTatCaAsync();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không tải được danh sách nhân viên.\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyFilter()
        {
            var q = (txtSearch.Text ?? "").Trim().ToLowerInvariant();

            var filtered = string.IsNullOrEmpty(q)
                ? _all
                : _all.Where(x =>
                    (x.MaNhanVien ?? "").ToLowerInvariant().Contains(q) ||
                    (x.HoTen ?? "").ToLowerInvariant().Contains(q) ||
                    (x.TenDangNhap ?? "").ToLowerInvariant().Contains(q)
                ).ToList();

            bsNhanVien.DataSource = filtered;
        }

        private async Task ThemAsync()
        {
            using var f = new FormNhanVienEdit("Thêm nhân viên");
            if (f.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                await _service.TaoAsync(f.MaNhanVien, f.HoTen, f.VaiTro);
                await ReloadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Thêm thất bại.\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SuaAsync()
        {
            var cur = Current();
            if (cur == null)
            {
                MessageBox.Show("Vui lòng chọn 1 nhân viên.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var f = new FormNhanVienEdit("Sửa nhân viên", cur.MaNhanVien, cur.HoTen, cur.VaiTro);
            if (f.ShowDialog(this) != DialogResult.OK) return;

            try
            {
                await _service.SuaAsync(cur.Id, f.MaNhanVien, f.HoTen, f.VaiTro);
                await ReloadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Sửa thất bại.\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task KhoaMoAsync()
        {
            var cur = Current();
            if (cur == null)
            {
                MessageBox.Show("Vui lòng chọn 1 nhân viên.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.Equals(cur.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Không cho phép khóa tài khoản Admin.",
                    "Chặn thao tác", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Toggle: Khóa(2) <-> Đang làm(0)
            var newStatus = cur.TrangThai == 2 ? 0 : 2;
            var confirmMsg = newStatus == 2 ? "Bạn muốn KHÓA nhân viên này?" : "Bạn muốn MỞ KHÓA nhân viên này?";

            if (MessageBox.Show(confirmMsg, "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                await _service.DatTrangThaiAsync(cur.Id, newStatus);
                await ReloadAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Cập nhật trạng thái thất bại.\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ResetPassAsync()
        {
            var cur = Current();
            if (cur == null)
            {
                MessageBox.Show("Vui lòng chọn 1 nhân viên.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.Equals(cur.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Không reset mật khẩu Admin bằng tab này.",
                    "Chặn thao tác", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var pass = PromptPassword("Reset mật khẩu", "Nhập mật khẩu mới (để trống = tự sinh):");
            if (pass == null) return;

            if (string.IsNullOrWhiteSpace(pass))
                pass = GenerateTempPassword(10);

            try
            {
                await _service.ResetMatKhauAsync(cur.Id, pass);
                MessageBox.Show($"Reset mật khẩu thành công.\nMật khẩu mới: {pass}",
                    "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Reset thất bại.\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string GenerateTempPassword(int length)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#";
            var bytes = RandomNumberGenerator.GetBytes(length);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(chars[bytes[i] % chars.Length]);
            return sb.ToString();
        }

        private static string? PromptPassword(string title, string label)
        {
            using var form = new Form
            {
                Text = title,
                Width = 520,
                Height = 190,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lbl = new Label { Left = 16, Top = 18, Text = label, AutoSize = true };
            var txt = new TextBox { Left = 16, Top = 45, Width = 470 };

            var ok = new Button { Text = "OK", Left = 316, Width = 80, Top = 85, DialogResult = DialogResult.OK };
            var cancel = new Button { Text = "Hủy", Left = 406, Width = 80, Top = 85, DialogResult = DialogResult.Cancel };

            form.Controls.AddRange(new Control[] { lbl, txt, ok, cancel });
            form.AcceptButton = ok;
            form.CancelButton = cancel;

            return form.ShowDialog() == DialogResult.OK ? txt.Text : null;
        }
    }
}
