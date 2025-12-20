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
        // ====== VẼ DANH SÁCH CHI TIẾT THEO KIỂU THẺ ======
        private void RenderChiTietList(List<ChiTietDonGoiDto> dsChiTiet)
        {
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

                var thanhTien = ct.DonGia * ct.SoLuong;
                tong += thanhTien;

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
                Width = flpChiTiet.ClientSize.Width - 25,
                Margin = new Padding(0, 0, 0, 6),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Tag = ct
            };

            flpChiTiet.SizeChanged += (s, e) =>
            {
                panel.Width = flpChiTiet.ClientSize.Width - 25;
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

            txtGhiChu.Width = panel.Width - 70 - 260 - 10;

            panel.Resize += (s, e) =>
            {
                txtGhiChu.Width = panel.Width - 70 - 260 - 10;
            };

            // Enter để lưu ghi chú
            txtGhiChu.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    await CapNhatGhiChuChiTietAsync(ctLocal, txtGhiChu.Text.Trim());
                }
            };

            var panelRight = new Panel
            {
                Dock = DockStyle.Right,
                Width = 260
            };

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
                await CapNhatSoLuongChiTietAsync(ctLocal, ctLocal.SoLuong + 1);
            };

            btnMinus.Click += async (s, e) =>
            {
                if (ctLocal.SoLuong <= 1)
                {
                    if (MessageBox.Show("Giảm về 0 sẽ xóa món này. Bạn chắc chắn?",
                        "Xác nhận", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.No)
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

            void AttachSelectHandler(Control c)
            {
                c.Click += ChiTietPanel_Click;
                foreach (Control child in c.Controls)
                {
                    AttachSelectHandler(child);
                }
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
            {
                p.BackColor = Color.White;
            }
            panel.BackColor = Color.FromArgb(230, 240, 255);
        }

        // ================== CẬP NHẬT GHI CHÚ CHI TIẾT ==================
        private async Task CapNhatGhiChuChiTietAsync(ChiTietDonGoiDto ct, string newNote)
        {
            if (_donGoiHienTai == null) return;

            var reqUpdate = new
            {
                DonGoiId = ct.DonGoiId,
                ThucAnId = ct.ThucAnId,
                ThucUongId = ct.ThucUongId,
                SoLuong = ct.SoLuong,
                DonGia = ct.DonGia,
                ChietKhau = ct.ChietKhau,
                GhiChu = string.IsNullOrWhiteSpace(newNote) ? (string?)null : newNote
            };

            await ApiClient.PutAsync<string>(
                $"api/ChiTietDonGoi/{ct.Id}",
                reqUpdate,
                includeAuth: true
            );

            await LoadDonGoiChoBanAsync(_banDangChon!.Id);
        }

        // ================== CẬP NHẬT / XOÁ CHI TIẾT ==================
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

            await ApiClient.PutAsync<string>(
                $"api/ChiTietDonGoi/{ct.Id}",
                reqUpdate,
                includeAuth: true
            );

            await LoadDonGoiChoBanAsync(_banDangChon!.Id);
        }

        private async Task XoaChiTietAsync(ChiTietDonGoiDto ct)
        {
            if (_donGoiHienTai == null) return;

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
                    TrangThai = 2, // đơn trống -> đóng
                    GhiChu = _donGoiHienTai.GhiChu
                };

                await ApiClient.PutAsync<string>(
                    $"api/DonGoi/{_donGoiHienTai.Id}",
                    reqCloseDon,
                    includeAuth: true
                );

                if (_banDangChon != null)
                {
                    _banDangChon.TrangThai = STATUS_TRONG;
                    await ApiClient.PutAsync<string>(
                        $"api/Ban/{_banDangChon.Id}",
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

        // ================== THÊM MÓN ==================
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

            int soLuong = (int)nudSoLuong.Value;
            if (soLuong <= 0)
            {
                MessageBox.Show("Số lượng phải > 0.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                    $"api/Ban/{_banDangChon.Id}",
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
                        $"api/Ban/{_banDangChon.Id}",
                        _banDangChon,
                        includeAuth: true
                    );
                    await LoadDanhSachBanAsync();
                }
            }

            var dsChiTietHienTai = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai!.Id);

            int? thucAnId = monVm.Loai == "ThucAn" ? monVm.Id : (int?)null;
            int? thucUongId = monVm.Loai == "ThucUong" ? monVm.Id : (int?)null;

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

        // ================== THANH TOÁN ==================
        private async void btnThanhToan_Click(object? sender, EventArgs e)
        {// 1. Kiểm tra điều kiện (Giữ nguyên code của bạn)
            if (_donGoiHienTai == null)
            {
                MessageBox.Show("Bàn này chưa có đơn gọi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_donGoiHienTai.TrangThai != 0)
            {
                MessageBox.Show("Đơn gọi đã thanh toán rồi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 2. Tính tổng tiền (Giữ nguyên cách bạn lấy dữ liệu từ Service)
            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            decimal tongTien = dsChiTiet.Sum(ct => ct.DonGia * ct.SoLuong);

            if (tongTien == 0)
            {
                MessageBox.Show("Đơn hàng 0 đồng, không cần thanh toán.", "Thông báo");
                return;
            }

            // 3. Mở Form Thanh Toán (Thay thế MessageBox cũ bằng Form mới)
            using (var formThanhToan = new FormThanhToan(_donGoiHienTai.Id, tongTien))
            {
                if (formThanhToan.ShowDialog() == DialogResult.OK)
                {
                    // Kiểm tra: Khách trả hết hay trả một phần?
                    if (formThanhToan.IsThanhToanHet)
                    {
                        // ====================================================
                        // TRƯỜNG HỢP 1: TRẢ HẾT -> THỰC HIỆN ĐÓNG ĐƠN & BÀN
                        // (Đoạn này lấy từ code cũ của bạn đưa vào)
                        // ====================================================

                        // A. Cập nhật Đơn hàng -> Đã thanh toán (TrangThai = 1)
                        var reqCloseDon = new
                        {
                            NhanVienId = _donGoiHienTai.NhanVienId,
                            BanId = _donGoiHienTai.BanId,
                            MoLuc = _donGoiHienTai.MoLuc,
                            DongLuc = DateTime.Now,
                            TrangThai = 1, // Đã thanh toán
                            GhiChu = _donGoiHienTai.GhiChu
                        };

                        await ApiClient.PutAsync<string>(
                            $"api/DonGoi/{_donGoiHienTai.Id}",
                            reqCloseDon,
                            includeAuth: true
                        );

                        // B. Cập nhật Bàn -> Trống (STATUS_TRONG)
                        if (_banDangChon != null)
                        {
                            _banDangChon.TrangThai = STATUS_TRONG;
                            await ApiClient.PutAsync<string>(
                                $"api/Ban/{_banDangChon.Id}",
                                _banDangChon,
                                includeAuth: true
                            );
                        }

                        // C. In Hóa Đơn (Mở form hóa đơn lên)
                        var formHoaDon = new FormHoaDon(_donGoiHienTai.Id);
                        formHoaDon.ShowDialog();

                        // D. Reset biến cục bộ
                        _donGoiHienTai = null;
                        _chiTietDangChonId = null;

                        // E. Tải lại giao diện
                        await LoadDanhSachBanAsync();

                        // Vì bàn đã đóng nên không load đơn nữa, hoặc load đơn rỗng
                        if (_banDangChon != null)
                            await LoadDonGoiChoBanAsync(_banDangChon.Id);
                    }
                    else
                    {
                        // ====================================================
                        // TRƯỜNG HỢP 2: TRẢ 1 PHẦN -> GIỮ NGUYÊN TRẠNG THÁI
                        // ====================================================
                        MessageBox.Show("Đã ghi nhận thanh toán một phần.\nBàn vẫn đang phục vụ (Màu đỏ).",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Chỉ load lại để cập nhật nếu cần, nhưng không đóng bàn
                        await LoadDanhSachBanAsync();
                    }
                }
            }

        }

        // ================== HÀM PHỤ DÙNG CHO CHUYỂN / GỘP BÀN ==================
        // CHỈ TRẢ VỀ ĐƠN ĐANG MỞ NẾU CÒN ÍT NHẤT 1 MÓN
        private async Task<DonGoiDto?> GetDonGoiDangMoTheoBanAsync(int banId)
        {
            var tatCaDonGoi = await _donGoiService.GetAllAsync();

            var donMo = tatCaDonGoi
                .Where(d => d.BanId == banId && d.TrangThai == 0)
                .OrderByDescending(d => d.MoLuc)
                .FirstOrDefault();

            if (donMo == null) return null;

            var chiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(donMo.Id);
            if (chiTiet == null || chiTiet.Count == 0)
            {
                // đơn mở nhưng không có món -> coi như không phục vụ
                return null;
            }

            return donMo;
        }

        // ================== CHUYỂN BÀN ==================
        private async void btnChuyenBan_Click(object? sender, EventArgs e)
        {
            if (cboBanFrom.SelectedItem is not BanDto banNguon ||
                cboBanTo.SelectedItem is not BanDto banDich)
            {
                MessageBox.Show("Vui lòng chọn bàn nguồn và bàn đích.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (banNguon.Id == banDich.Id)
            {
                MessageBox.Show("Bàn nguồn và bàn đích phải khác nhau.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var donNguon = await GetDonGoiDangMoTheoBanAsync(banNguon.Id);
            if (donNguon == null)
            {
                MessageBox.Show("Bàn nguồn hiện không có đơn đang phục vụ.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var donDich = await GetDonGoiDangMoTheoBanAsync(banDich.Id);
            if (donDich != null)
            {
                MessageBox.Show("Bàn đích đã có đơn đang phục vụ. Nếu muốn gộp, hãy dùng chức năng Gộp bàn.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var reqUpdateDonGoi = new
            {
                NhanVienId = donNguon.NhanVienId,
                BanId = banDich.Id,
                MoLuc = donNguon.MoLuc,
                DongLuc = donNguon.DongLuc,
                TrangThai = donNguon.TrangThai,
                GhiChu = donNguon.GhiChu
            };

            await ApiClient.PutAsync<string>(
                $"api/DonGoi/{donNguon.Id}",
                reqUpdateDonGoi,
                includeAuth: true
            );

            banNguon.TrangThai = STATUS_TRONG;
            banDich.TrangThai = STATUS_DANG_DUNG;

            await ApiClient.PutAsync<string>($"api/Ban/{banNguon.Id}", banNguon, includeAuth: true);
            await ApiClient.PutAsync<string>($"api/Ban/{banDich.Id}", banDich, includeAuth: true);

            await LoadDanhSachBanAsync();

            _banDangChon = _dsBan.FirstOrDefault(b => b.Id == banDich.Id);
            if (_banDangChon != null)
            {
                HighlightSelectedBan(_banDangChon);
                await LoadDonGoiChoBanAsync(_banDangChon.Id);
            }
        }

        // ================== GỘP BÀN ==================
        private async void btnGopBan_Click(object? sender, EventArgs e)
        {
            if (cboBanFrom.SelectedItem is not BanDto banNguon ||
                cboBanTo.SelectedItem is not BanDto banDich)
            {
                MessageBox.Show("Vui lòng chọn bàn nguồn và bàn đích.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (banNguon.Id == banDich.Id)
            {
                MessageBox.Show("Bàn nguồn và bàn đích phải khác nhau.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var donNguon = await GetDonGoiDangMoTheoBanAsync(banNguon.Id);
            var donDich = await GetDonGoiDangMoTheoBanAsync(banDich.Id);

            if (donNguon == null || donDich == null)
            {
                MessageBox.Show("Cả bàn nguồn và bàn đích đều phải đang phục vụ (có đơn mở).",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show(
                    $"Gộp tất cả món từ {banNguon.TenBan} sang {banDich.TenBan}?",
                    "Xác nhận gộp bàn",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.No)
                return;

            var chiTietNguon = await _ctDonGoiService.GetByDonGoiIdAsync(donNguon.Id);

            foreach (var ct in chiTietNguon)
            {
                var reqUpdateCt = new
                {
                    DonGoiId = donDich.Id,
                    ThucAnId = ct.ThucAnId,
                    ThucUongId = ct.ThucUongId,
                    SoLuong = ct.SoLuong,
                    DonGia = ct.DonGia,
                    ChietKhau = ct.ChietKhau,
                    GhiChu = ct.GhiChu
                };

                await ApiClient.PutAsync<string>(
                    $"api/ChiTietDonGoi/{ct.Id}",
                    reqUpdateCt,
                    includeAuth: true
                );
            }

            var reqCloseDonNguon = new
            {
                NhanVienId = donNguon.NhanVienId,
                BanId = donNguon.BanId,
                MoLuc = donNguon.MoLuc,
                DongLuc = DateTime.Now,
                TrangThai = 1,
                GhiChu = donNguon.GhiChu
            };

            await ApiClient.PutAsync<string>(
                $"api/DonGoi/{donNguon.Id}",
                reqCloseDonNguon,
                includeAuth: true
            );

            banNguon.TrangThai = STATUS_TRONG;
            await ApiClient.PutAsync<string>($"api/Ban/{banNguon.Id}", banNguon, includeAuth: true);

            await LoadDanhSachBanAsync();

            _banDangChon = _dsBan.FirstOrDefault(b => b.Id == banDich.Id);
            if (_banDangChon != null)
            {
                HighlightSelectedBan(_banDangChon);
                await LoadDonGoiChoBanAsync(_banDangChon.Id);
            }
        }
    }
}
