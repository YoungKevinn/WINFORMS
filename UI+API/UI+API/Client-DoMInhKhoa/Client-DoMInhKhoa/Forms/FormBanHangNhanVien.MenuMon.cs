using Client_DoMInhKhoa.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

// ✅ Alias để khỏi bị ambiguous với System.Threading.Timer
using WinTimer = System.Windows.Forms.Timer;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien
    {
        // ❌ KHÔNG KHAI BÁO _txtTimMon Ở ĐÂY NỮA (tránh trùng field)
        // _txtTimMon phải được khai báo DUY NHẤT 1 nơi trong project.

        private readonly WinTimer _searchTimer = new WinTimer { Interval = 250 };
        private bool _hookedSearchTimer = false;

        private void BuildMenuSearch()
        {
            var parent = _panelMenuTop ?? _panelMenu;
            if (parent == null) return;

            // ✅ tránh add trùng nếu lỡ gọi lại
            if (parent.Controls.OfType<TextBox>().Any(t => t.Name == "txtTimMon"))
                return;

            var lblSearch = new Label
            {
                AutoSize = true,
                Text = "Tìm món:",
                Font = new Font("Segoe UI", 9),
                Location = new Point(5, 75)
            };

            // ✅ tạo textbox và gán vào field _txtTimMon (field đã có sẵn ở file khác)
            _txtTimMon = new TextBox
            {
                Name = "txtTimMon",
                Height = cboMon.Height,
                BorderStyle = BorderStyle.FixedSingle,
                Location = new Point(90, 71)
            };

            void UpdateSearchWidth()
            {
                int rightPadding = 15;
                int newWidth = parent.Width - 90 - rightPadding;
                if (newWidth < 120) newWidth = 120;
                _txtTimMon.Width = newWidth;
            }

            parent.Resize += (s, e) => UpdateSearchWidth();
            UpdateSearchWidth();

            // ✅ debounce search (gõ mượt, không giật)
            if (!_hookedSearchTimer)
            {
                _hookedSearchTimer = true;
                _searchTimer.Tick += (s, e) =>
                {
                    _searchTimer.Stop();

                    if (cboDanhMuc.SelectedItem is DanhMucDto dm)
                        LoadMonTheoDanhMuc(dm, _txtTimMon?.Text);

                    RenderMonButtons();
                };
            }

            _txtTimMon.TextChanged += TxtTimMon_TextChanged;

            parent.Controls.Add(lblSearch);
            parent.Controls.Add(_txtTimMon);

            lblSearch.BringToFront();
            _txtTimMon.BringToFront();
        }

        private void TxtTimMon_TextChanged(object? sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private async Task LoadDanhMucVaMonAsync()
        {
            try
            {
                _dsDanhMuc = await _danhMucService.GetAllAsync();
                _dsThucAn = await _thucAnService.GetAllAsync();
                _dsThucUong = await _thucUongService.GetAllAsync();

                var dsDmActive = _dsDanhMuc
                    .Where(x => x.DangHoatDong)
                    .OrderBy(x => x.Ten)
                    .ToList();

                cboDanhMuc.SelectedIndexChanged -= cboDanhMuc_SelectedIndexChanged;

                cboDanhMuc.DataSource = dsDmActive;
                cboDanhMuc.DisplayMember = "Ten";
                cboDanhMuc.ValueMember = "Id";

                cboDanhMuc.SelectedIndexChanged += cboDanhMuc_SelectedIndexChanged;

                if (dsDmActive.Any())
                {
                    cboDanhMuc.SelectedIndex = 0;
                    var dm = (DanhMucDto)cboDanhMuc.SelectedItem;
                    LoadMonTheoDanhMuc(dm, _txtTimMon?.Text);
                }
                else
                {
                    cboMon.DataSource = null;
                    cboMon.Text = string.Empty;
                }

                RenderMonButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh mục / món: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboDanhMuc_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboDanhMuc.SelectedItem is DanhMucDto dm)
            {
                LoadMonTheoDanhMuc(dm, _txtTimMon?.Text);
                RenderMonButtons(dm);
            }
            else
            {
                cboMon.DataSource = null;
                cboMon.Text = string.Empty;
            }
        }

        private List<MonViewModel> BuildDsMonTheoDanhMuc(DanhMucDto? dm)
        {
            var list = new List<MonViewModel>();

            var thucAn = _dsThucAn.AsEnumerable();
            var thucUong = _dsThucUong.AsEnumerable();

            if (dm != null)
            {
                thucAn = thucAn.Where(m => m.DanhMucId == dm.Id && m.DangHoatDong);
                thucUong = thucUong.Where(m => m.DanhMucId == dm.Id && m.DangHoatDong);
            }
            else
            {
                thucAn = thucAn.Where(m => m.DangHoatDong);
                thucUong = thucUong.Where(m => m.DangHoatDong);
            }

            list.AddRange(thucAn.Select(m => new MonViewModel
            {
                Id = m.Id,
                Ten = m.Ten,
                DonGia = m.DonGia,
                Loai = "ThucAn"
            }));

            list.AddRange(thucUong.Select(m => new MonViewModel
            {
                Id = m.Id,
                Ten = m.Ten,
                DonGia = m.DonGia,
                Loai = "ThucUong"
            }));

            return list.OrderBy(x => x.Ten).ToList();
        }

        private void LoadMonTheoDanhMuc(DanhMucDto dm, string? keyword = null)
        {
            var dsMon = BuildDsMonTheoDanhMuc(dm);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var key = keyword.Trim().ToLowerInvariant();
                dsMon = dsMon.Where(m => m.Ten.ToLowerInvariant().Contains(key)).ToList();
            }

            if (dsMon.Count == 0)
            {
                cboMon.DataSource = null;
                cboMon.Text = string.Empty;
                return;
            }

            cboMon.DataSource = dsMon;
            cboMon.DisplayMember = "Ten";
            cboMon.ValueMember = "Id";
            cboMon.SelectedIndex = 0;
        }

        private void RenderMonButtons(DanhMucDto? dm = null)
        {
            if (flpMonNhanh == null) return;

            flpMonNhanh.SuspendLayout();
            flpMonNhanh.Controls.Clear();

            var dsMon = BuildDsMonTheoDanhMuc(dm);

            string keyword = _txtTimMon?.Text.Trim().ToLowerInvariant() ?? "";
            if (!string.IsNullOrEmpty(keyword))
                dsMon = dsMon.Where(m => m.Ten.ToLowerInvariant().Contains(keyword)).ToList();

            foreach (var mon in dsMon)
            {
                var btn = new Button
                {
                    Width = 120,
                    Height = 70,
                    Margin = new Padding(6),
                    Tag = mon,
                    Text = $"{mon.Ten}\n{mon.DonGia:N0}",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    BackColor = Color.FromArgb(240, 248, 255),
                    FlatStyle = FlatStyle.Flat
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.RoyalBlue;

                btn.Click += BtnMonNhanh_Click;
                flpMonNhanh.Controls.Add(btn);
            }

            flpMonNhanh.ResumeLayout();
        }

        private void RenderMonButtons()
        {
            DanhMucDto? dm = cboDanhMuc.SelectedItem as DanhMucDto;
            RenderMonButtons(dm);
        }

        // ✅ FIX TRÙNG ID: lấy DanhMucId theo Loai
        private void BtnMonNhanh_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not MonViewModel monVm) return;

            int? dmId = null;

            if (string.Equals(monVm.Loai, "ThucAn", StringComparison.OrdinalIgnoreCase))
                dmId = _dsThucAn.FirstOrDefault(x => x.Id == monVm.Id)?.DanhMucId;
            else if (string.Equals(monVm.Loai, "ThucUong", StringComparison.OrdinalIgnoreCase))
                dmId = _dsThucUong.FirstOrDefault(x => x.Id == monVm.Id)?.DanhMucId;

            if (dmId.HasValue && cboDanhMuc.DataSource != null)
                cboDanhMuc.SelectedValue = dmId.Value;

            if (cboMon.DataSource is IEnumerable<MonViewModel> list)
            {
                var item = list.FirstOrDefault(x =>
                    x.Id == monVm.Id &&
                    string.Equals(x.Loai, monVm.Loai, StringComparison.OrdinalIgnoreCase));

                if (item != null)
                    cboMon.SelectedItem = item;
            }

            btnThemMon_Click(null, EventArgs.Empty);
        }
    }
}
