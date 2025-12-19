using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien : Form
    {
        // ================== TÌM MÓN (SEARCH) - ĐẶT NGAY DƯỚI COMBO MÓN ==================
        private void BuildMenuSearch()
        {
            // parent chính là panel top của cột menu
            var parent = _panelMenuTop ?? _panelMenu;

            var lblSearch = new Label
            {
                AutoSize = true,
                Text = "Tìm món:",
                Font = new Font("Segoe UI", 9),
                Location = new Point(5, 75)
            };

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

            _txtTimMon.TextChanged += TxtTimMon_TextChanged;

            parent.Controls.Add(lblSearch);
            parent.Controls.Add(_txtTimMon);

            lblSearch.BringToFront();
            _txtTimMon.BringToFront();
        }

        private void TxtTimMon_TextChanged(object? sender, EventArgs e)
        {
            if (cboDanhMuc.SelectedItem is DanhMucDto dm)
            {
                LoadMonTheoDanhMuc(dm, _txtTimMon?.Text);
            }

            RenderMonButtons();
        }

        // ================== LOAD DANH MỤC + MÓN ==================
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

        // ====== XÂY DS MÓN THEO DANH MỤC (DÙNG CHUNG) ======
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

        // ================== FILL MÓN THEO DANH MỤC + TÌM KIẾM ==================
        private void LoadMonTheoDanhMuc(DanhMucDto dm, string? keyword = null)
        {
            var dsMon = BuildDsMonTheoDanhMuc(dm);

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var key = keyword.Trim().ToLowerInvariant();
                dsMon = dsMon
                    .Where(m => m.Ten.ToLowerInvariant().Contains(key))
                    .ToList();
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

        // ================== BUTTON MÓN NHANH (LIST BÊN DƯỚI) ==================
        private void RenderMonButtons(DanhMucDto? dm = null)
        {
            if (flpMonNhanh == null) return;

            flpMonNhanh.Controls.Clear();

            var dsMon = BuildDsMonTheoDanhMuc(dm);

            string keyword = _txtTimMon?.Text.Trim().ToLowerInvariant() ?? "";
            if (!string.IsNullOrEmpty(keyword))
            {
                dsMon = dsMon
                    .Where(m => m.Ten.ToLowerInvariant().Contains(keyword))
                    .ToList();
            }

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
        }

        private void RenderMonButtons()
        {
            DanhMucDto? dm = cboDanhMuc.SelectedItem as DanhMucDto;
            RenderMonButtons(dm);
        }

        private void BtnMonNhanh_Click(object? sender, EventArgs e)
        {
            if (sender is not Button btn || btn.Tag is not MonViewModel monVm) return;

            int? dmId =
                _dsThucAn.FirstOrDefault(x => x.Id == monVm.Id)?.DanhMucId ??
                _dsThucUong.FirstOrDefault(x => x.Id == monVm.Id)?.DanhMucId;

            if (dmId.HasValue && cboDanhMuc.DataSource != null)
                cboDanhMuc.SelectedValue = dmId.Value;

            if (cboMon.DataSource is IEnumerable<MonViewModel> list)
            {
                var item = list.FirstOrDefault(x => x.Id == monVm.Id && x.Loai == monVm.Loai);
                if (item != null)
                    cboMon.SelectedItem = item;
            }

            if (nudSoLuong.Value <= 0) nudSoLuong.Value = 1;
            nudSoLuong.Value = 1;

            btnThemMon_Click(null, EventArgs.Empty);
        }
    }
}
