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
    public partial class FormBanHangNhanVien
    {
        // ✅ chống leak event resize
        private bool _hookedFlpChiTietResize = false;

        // ✅ chỉ hook 1 lần: resize flpChiTiet -> update width panel
        private void HookFlpChiTietResizeOnce()
        {
            if (flpChiTiet == null) return;
            if (_hookedFlpChiTietResize) return;

            _hookedFlpChiTietResize = true;
            flpChiTiet.SizeChanged += (s, e) =>
            {
                int w = flpChiTiet.ClientSize.Width - 25;
                if (w < 200) w = 200;

                foreach (var p in flpChiTiet.Controls.OfType<Panel>())
                {
                    p.Width = w;

                    // textbox ghi chú nếu có
                    var txt = p.Controls.OfType<TextBox>().FirstOrDefault();
                    if (txt != null)
                    {
                        int newW = p.Width - 70 - 260 - 10;
                        if (newW < 80) newW = 80;
                        txt.Width = newW;
                    }
                }
            };
        }

        // ====== VẼ DANH SÁCH CHI TIẾT ======
        private void RenderChiTietList(List<ChiTietDonGoiDto> dsChiTiet)
        {
            HookFlpChiTietResizeOnce();

            flpChiTiet.SuspendLayout();
            flpChiTiet.Controls.Clear();

            decimal tong = 0;
            var vi = new CultureInfo("vi-VN");

            int index = 1;
            foreach (var ct in dsChiTiet)
            {
                string tenMon = "";
                if (ct.ThucAnId.HasValue)
                    tenMon = _dsThucAn.FirstOrDefault(x => x.Id == ct.ThucAnId.Value)?.Ten ?? "Món ăn";
                else if (ct.ThucUongId.HasValue)
                    tenMon = _dsThucUong.FirstOrDefault(x => x.Id == ct.ThucUongId.Value)?.Ten ?? "Thức uống";

                tong += ct.DonGia * ct.SoLuong;

                var itemPanel = CreateChiTietItemPanel(index, tenMon, ct, vi);
                flpChiTiet.Controls.Add(itemPanel);
                index++;
            }

            flpChiTiet.ResumeLayout();
            lblTongTien.Text = string.Format(vi, "{0:N0} đ", tong);
        }

        private Panel CreateChiTietItemPanel(int index, string tenMon, ChiTietDonGoiDto ct, CultureInfo vi)
        {
            var panel = new Panel
            {
                Height = 75,
                Width = Math.Max(200, flpChiTiet.ClientSize.Width - 25),
                Margin = new Padding(0, 0, 0, 6),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Tag = ct
            };

            var lblTen = new Label
            {
                Text = $"{index}. {tenMon}",
                AutoSize = true,
                Location = new Point(8, 6),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblGhiChuCaption = new Label
            {
                Text = "Ghi chú:",
                AutoSize = true,
                Location = new Point(18, 30),
                Font = new Font("Segoe UI", 8, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            var ctLocal = ct;

            var txtGhiChu = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Location = new Point(70, 30),
                Height = 16,
                Text = ct.GhiChu ?? "",
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                ForeColor = Color.Black
            };

            int txtW = panel.Width - 70 - 260 - 10;
            if (txtW < 80) txtW = 80;
            txtGhiChu.Width = txtW;

            // ===== lưu ghi chú =====
            string _lastSavedNote = (ct.GhiChu ?? "").Trim();
            static string NormalizeNote(string s) => (s ?? "").Trim();

            async Task SaveNoteIfChangedAsync()
            {
                var current = NormalizeNote(txtGhiChu.Text);
                ctLocal.GhiChu = string.IsNullOrWhiteSpace(current) ? null : current;

                if (current == _lastSavedNote) return;

                await CapNhatGhiChuChiTietAsync(ctLocal, current);
                _lastSavedNote = current;
            }

            txtGhiChu.TextChanged += (s, e) =>
            {
                var current = NormalizeNote(txtGhiChu.Text);
                ctLocal.GhiChu = string.IsNullOrWhiteSpace(current) ? null : current;
            };

            txtGhiChu.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    await SaveNoteIfChangedAsync();
                }
            };

            txtGhiChu.Leave += async (s, e) => await SaveNoteIfChangedAsync();

            // ===== panel phải (giá, qty, thành tiền) =====
            var panelRight = new Panel { Dock = DockStyle.Right, Width = 260 };

            var lblDonGia = new Label
            {
                Text = ct.DonGia.ToString("N0", vi),
                AutoSize = true,
                Location = new Point(5, 8),
                Font = new Font("Segoe UI", 9)
            };

            var panelQty = new Panel
            {
                Location = new Point(70, 4),
                Size = new Size(110, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnMinus = new Button
            {
                Text = "-",
                Width = 30,
                Height = 26,
                Location = new Point(0, 1),
                FlatStyle = FlatStyle.Flat
            };
            btnMinus.FlatAppearance.BorderSize = 0;

            var lblQty = new Label
            {
                Text = ct.SoLuong.ToString(),
                Width = 40,
                Height = 26,
                Location = new Point(35, 2),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnPlus = new Button
            {
                Text = "+",
                Width = 30,
                Height = 26,
                Location = new Point(74, 1),
                FlatStyle = FlatStyle.Flat
            };
            btnPlus.FlatAppearance.BorderSize = 0;

            btnPlus.Click += async (s, e) =>
            {
                var current = NormalizeNote(txtGhiChu.Text);
                ctLocal.GhiChu = string.IsNullOrWhiteSpace(current) ? null : current;

                await CapNhatSoLuongChiTietAsync(ctLocal, ctLocal.SoLuong + 1);
            };

            btnMinus.Click += async (s, e) =>
            {
                var current = NormalizeNote(txtGhiChu.Text);
                ctLocal.GhiChu = string.IsNullOrWhiteSpace(current) ? null : current;

                if (ctLocal.SoLuong <= 1)
                {
                    if (MessageBox.Show("Giảm về 0 sẽ xóa món này. Bạn chắc chắn?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        return;

                    await XoaChiTietAsync(ctLocal);
                }
                else
                {
                    await CapNhatSoLuongChiTietAsync(ctLocal, ctLocal.SoLuong - 1);
                }
            };

            panelQty.Controls.Add(btnMinus);
            panelQty.Controls.Add(lblQty);
            panelQty.Controls.Add(btnPlus);

            var lblThanhTien = new Label
            {
                Text = (ct.DonGia * ct.SoLuong).ToString("N0", vi),
                AutoSize = true,
                Location = new Point(190, 8),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            panelRight.Controls.Add(lblDonGia);
            panelRight.Controls.Add(panelQty);
            panelRight.Controls.Add(lblThanhTien);

            // chọn item
            void AttachSelectHandler(Control c)
            {
                c.Click += ChiTietPanel_Click;
                foreach (Control child in c.Controls)
                    AttachSelectHandler(child);
            }
            AttachSelectHandler(panel);

            panel.Controls.Add(panelRight);
            panel.Controls.Add(lblTen);
            panel.Controls.Add(lblGhiChuCaption);
            panel.Controls.Add(txtGhiChu);

            return panel;
        }

        private void ChiTietPanel_Click(object? sender, EventArgs e)
        {
            Panel? panel = null;

            if (sender is Panel p1 && p1.Tag is ChiTietDonGoiDto)
                panel = p1;
            else if (sender is Control c && c.Parent is Panel p2 && p2.Tag is ChiTietDonGoiDto)
                panel = p2;

            if (panel == null || panel.Tag is not ChiTietDonGoiDto ct) return;

            _chiTietDangChonId = ct.Id;

            foreach (Panel p in flpChiTiet.Controls.OfType<Panel>())
                p.BackColor = Color.White;

            panel.BackColor = Color.FromArgb(230, 240, 255);
        }

        // ================== UPDATE GHI CHÚ ==================
        private async Task CapNhatGhiChuChiTietAsync(ChiTietDonGoiDto ct, string newNote)
        {
            if (_donGoiHienTai == null) return;

            var note = (newNote ?? "").Trim();
            ct.GhiChu = string.IsNullOrWhiteSpace(note) ? null : note;

            var reqUpdate = new
            {
                DonGoiId = ct.DonGoiId,
                ThucAnId = ct.ThucAnId,
                ThucUongId = ct.ThucUongId,
                SoLuong = ct.SoLuong,
                DonGia = ct.DonGia,
                ChietKhau = ct.ChietKhau,
                GhiChu = ct.GhiChu
            };

            try
            {
                await _ctDonGoiService.UpdateAsync(ct.Id, reqUpdate);
                await LoadDonGoiChoBanAsync(_banDangChon!.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật ghi chú: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== UPDATE SỐ LƯỢNG ==================
        private async Task CapNhatSoLuongChiTietAsync(ChiTietDonGoiDto ct, int newSoLuong)
        {
            if (_donGoiHienTai == null) return;

            if (newSoLuong <= 0)
            {
                await XoaChiTietAsync(ct);
                return;
            }

            var reqUpdate = new
            {
                DonGoiId = ct.DonGoiId,
                ThucAnId = ct.ThucAnId,
                ThucUongId = ct.ThucUongId,
                SoLuong = newSoLuong,
                DonGia = ct.DonGia,
                ChietKhau = ct.ChietKhau,
                GhiChu = ct.GhiChu
            };

            try
            {
                await _ctDonGoiService.UpdateAsync(ct.Id, reqUpdate);
                await LoadDonGoiChoBanAsync(_banDangChon!.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật số lượng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task XoaChiTietAsync(ChiTietDonGoiDto ct)
        {
            if (_donGoiHienTai == null) return;

            try
            {
                await _ctDonGoiService.DeleteAsync(ct.Id);

                var dsConLai = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);

                if (dsConLai.Count == 0)
                {
                    var reqCloseDon = new
                    {
                        NhanVienId = _donGoiHienTai.NhanVienId,
                        BanId = _donGoiHienTai.BanId,
                        MoLuc = _donGoiHienTai.MoLuc,
                        DongLuc = DateTime.Now,
                        TrangThai = 2,
                        GhiChu = _donGoiHienTai.GhiChu
                    };

                    await ApiClient.PutAsync<string>(
                        $"/api/DonGoi/{_donGoiHienTai.Id}",
                        reqCloseDon,
                        includeAuth: true
                    );

                    if (_banDangChon != null)
                    {
                        _banDangChon.TrangThai = STATUS_TRONG;
                        await ApiClient.PutAsync<string>(
                            $"/api/Ban/{_banDangChon.Id}",
                            _banDangChon,
                            includeAuth: true
                        );
                    }

                    _donGoiHienTai = null;
                    _chiTietDangChonId = null;

                    await LoadDanhSachBanAsync();
                    flpChiTiet.Controls.Clear();
                    lblTongTien.Text = "0 đ";

                    string tenBan = _banDangChon?.TenBan ?? "";
                    lblBanHienTai.Text = $"Bàn hiện tại: {tenBan} (chưa có đơn gọi)";
                }
                else
                {
                    await LoadDonGoiChoBanAsync(_banDangChon!.Id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa món: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== THÊM MÓN (ONLINE-ONLY) ==================
        private async void btnThemMon_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trong danh sách bên trái.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (cboMon.SelectedItem is not MonViewModel monVm)
            {
                MessageBox.Show("Vui lòng chọn món.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int soLuong = 1;

            try
            {
                // mở đơn nếu chưa có
                if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
                {
                    var reqDonGoi = new
                    {
                        NhanVienId = _nhanVienDangNhap.Id,
                        BanId = _banDangChon.Id,
                        MoLuc = DateTime.Now,
                        TrangThai = 0,
                        GhiChu = (string?)null
                    };

                    _donGoiHienTai = await _donGoiService.CreateAsync(reqDonGoi);

                    _banDangChon.TrangThai = STATUS_DANG_DUNG;
                    await ApiClient.PutAsync<string>(
                        $"/api/Ban/{_banDangChon.Id}",
                        _banDangChon,
                        includeAuth: true
                    );

                    await LoadDanhSachBanAsync();
                }
                else
                {
                    if (_banDangChon.TrangThai != STATUS_DANG_DUNG)
                    {
                        _banDangChon.TrangThai = STATUS_DANG_DUNG;
                        await ApiClient.PutAsync<string>(
                            $"/api/Ban/{_banDangChon.Id}",
                            _banDangChon,
                            includeAuth: true
                        );
                        await LoadDanhSachBanAsync();
                    }
                }

                var dsChiTietHienTai = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai!.Id);

                int? thucAnId = string.Equals(monVm.Loai, "ThucAn", StringComparison.OrdinalIgnoreCase) ? monVm.Id : (int?)null;
                int? thucUongId = string.Equals(monVm.Loai, "ThucUong", StringComparison.OrdinalIgnoreCase) ? monVm.Id : (int?)null;

                var existing = dsChiTietHienTai.FirstOrDefault(x =>
                    x.ThucAnId == thucAnId &&
                    x.ThucUongId == thucUongId &&
                    x.DonGia == monVm.DonGia &&
                    x.ChietKhau == 0m
                );

                if (existing != null)
                {
                    await CapNhatSoLuongChiTietAsync(existing, existing.SoLuong + soLuong);
                }
                else
                {
                    var reqChiTiet = new
                    {
                        DonGoiId = _donGoiHienTai.Id,
                        ThucAnId = thucAnId,
                        ThucUongId = thucUongId,
                        SoLuong = soLuong,
                        DonGia = monVm.DonGia,
                        ChietKhau = 0m,
                        GhiChu = (string?)null
                    };

                    await _ctDonGoiService.CreateAsync(reqChiTiet);
                    await LoadDonGoiChoBanAsync(_banDangChon!.Id);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm món: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== XÓA MÓN (THEO ITEM ĐANG CHỌN) ==================
        private async void btnXoaMon_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null || _donGoiHienTai == null)
                return;

            if (!_chiTietDangChonId.HasValue)
            {
                MessageBox.Show("Vui lòng click chọn món trong danh sách bên phải.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Xóa món đang chọn?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            ChiTietDonGoiDto? ct = null;
            foreach (Panel p in flpChiTiet.Controls.OfType<Panel>())
            {
                if (p.Tag is ChiTietDonGoiDto ctTmp && ctTmp.Id == _chiTietDangChonId.Value)
                {
                    ct = ctTmp;
                    break;
                }
            }

            if (ct == null) return;

            await XoaChiTietAsync(ct);
        }
    }
}
