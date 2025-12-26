using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien

    {
        // ================== STYLE BUTTON BÀN ==================
        private void StyleButtonBan(Button btn, int trangThai, bool isSelected = false)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = isSelected ? 3 : 2;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.TextAlign = ContentAlignment.MiddleCenter;

            if (trangThai == STATUS_DANG_DUNG)
            {
                btn.BackColor = Color.FromArgb(0, 120, 215);
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.FromArgb(0, 120, 215);
            }
            else
            {
                btn.BackColor = Color.White;
                btn.ForeColor = Color.RoyalBlue;
                btn.FlatAppearance.BorderColor = Color.RoyalBlue;
            }
        }

        private void HighlightSelectedBan(BanDto? selected)
        {
            foreach (var control in flpBan.Controls.OfType<Button>())
            {
                if (control.Tag is not BanDto ban) continue;
                bool isSelected = (selected != null && ban.Id == selected.Id);
                StyleButtonBan(control, ban.TrangThai, isSelected);
            }
        }

        // ================== LOAD DANH SÁCH BÀN ==================
        private async Task LoadDanhSachBanAsync()
        {
            _dsBan = await BanService.GetAllAsync(); // giữ nguyên như bạn đang dùng

            flpBan.Controls.Clear();

            foreach (var ban in _dsBan.OrderBy(b => b.Id))
            {
                var btn = new Button
                {
                    Width = 120,
                    Height = 70,
                    Tag = ban,
                    Margin = new Padding(8),
                };

                string trangThaiText = ban.TrangThai == STATUS_DANG_DUNG
                    ? "ĐANG DÙNG"
                    : "CÒN TRỐNG";

                btn.Text = $"{ban.TenBan}\n{trangThaiText}";
                StyleButtonBan(btn, ban.TrangThai, false);
                btn.Click += BanButton_Click;

                flpBan.Controls.Add(btn);
            }

            // Bind combo chuyển/gộp
            cboBanFrom.DataSource = _dsBan.ToList();
            cboBanFrom.DisplayMember = "TenBan";
            cboBanFrom.ValueMember = "Id";

            cboBanTo.DataSource = _dsBan.ToList();
            cboBanTo.DisplayMember = "TenBan";
            cboBanTo.ValueMember = "Id";

            HighlightSelectedBan(_banDangChon);
        }

        private async void BanButton_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not BanDto ban) return;

            _banDangChon = ban;
            cboBanFrom.SelectedValue = ban.Id;

            HighlightSelectedBan(_banDangChon);
            await LoadDonGoiChoBanAsync(ban.Id);
        }

        // ================== CLEAR BILL UI ==================
        private void ClearBillUi(int banId)
        {
            _donGoiHienTai = null;
            _chiTietDangChonId = null;

            flpChiTiet.Controls.Clear();
            lblTongTien.Text = "0 đ";

            string tenBan = _banDangChon?.TenBan
                            ?? _dsBan.FirstOrDefault(b => b.Id == banId)?.TenBan
                            ?? $"Bàn {banId}";

            lblBanHienTai.Text = $"Bàn hiện tại: {tenBan} (chưa có đơn gọi)";
        }

        // ================== LOAD ĐƠN GỌI + CHI TIẾT ==================
        private async Task LoadDonGoiChoBanAsync(int banId)
        {
            var tatCaDonGoi = await _donGoiService.GetAllAsync();

            var donDangMo = tatCaDonGoi
                .Where(x => x.BanId == banId && x.TrangThai == 0)
                .OrderByDescending(x => x.MoLuc)
                .FirstOrDefault();

            if (donDangMo == null)
            {
                ClearBillUi(banId);
                return;
            }

            _donGoiHienTai = donDangMo;
            _chiTietDangChonId = null;

            string tenBan = _banDangChon?.TenBan
                            ?? _dsBan.FirstOrDefault(b => b.Id == banId)?.TenBan
                            ?? $"Bàn {banId}";

            lblBanHienTai.Text = $"Bàn hiện tại: {tenBan} (chưa thanh toán)";

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(donDangMo.Id);

            if (dsChiTiet == null || dsChiTiet.Count == 0)
            {
                ClearBillUi(banId);
                return;
            }

            RenderChiTietList(dsChiTiet);
        }
    }
}
