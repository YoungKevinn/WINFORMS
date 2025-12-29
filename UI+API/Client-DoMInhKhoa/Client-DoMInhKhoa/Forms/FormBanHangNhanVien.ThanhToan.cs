using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien : Form
    {
        // ================== THANH TOÁN FULL ==================
        private async void btnThanhToan_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
            {
                MessageBox.Show("Bàn này chưa có đơn gọi đang mở.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            if (dsChiTiet == null || dsChiTiet.Count == 0)
            {
                MessageBox.Show("Đơn gọi rỗng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            decimal tong = dsChiTiet.Sum(x => x.DonGia * x.SoLuong);

            // 1) chọn phương thức
            using var fChon = new FormChonThanhToan(tong);
            if (fChon.ShowDialog(this) != DialogResult.OK) return;

            string method = fChon.PhuongThuc;
            decimal khachDua = fChon.KhachDua;
            decimal tienThoi = fChon.TienThoi;

            // 2) nếu chuyển khoản -> lấy QR + xác nhận
            System.Drawing.Image? qrImg = null;
            if (method == "Chuyển khoản")
            {
                using var fqr = new FormThanhToanQR(tong, _donGoiHienTai.Id, _banDangChon.Id);
                var dr = fqr.ShowDialog(this);
                if (dr != DialogResult.OK || !fqr.DaNhanTien)
                {
                    MessageBox.Show("Chưa xác nhận nhận tiền. Hủy thanh toán.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                qrImg = fqr.QrImage;
            }

            // 3) show bill (bill có QR nếu chuyển khoản)
            using (var bill = new FormHoaDon(
                _donGoiHienTai.Id,
                overrideLines: null,
                qrImage: qrImg,
                phuongThuc: method,
                khachDua: khachDua,
                tienThoi: tienThoi,
                tongOverride: tong))
            {
                bill.ShowDialog(this);
            }

            // 4) đóng đơn + trả bàn
            await CloseDonGoiSauThanhToanAsync();
            await LoadDanhSachBanAsync();
            await LoadDonGoiChoBanAsync(_banDangChon.Id);
        }

        // ================== THANH TOÁN TỪNG PHẦN ==================
        private async void btnThanhToanTungPhan_Click(object? sender, EventArgs e)
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
            {
                MessageBox.Show("Bàn này chưa có đơn gọi đang mở.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            if (dsChiTiet == null || dsChiTiet.Count == 0)
            {
                MessageBox.Show("Đơn gọi rỗng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // build list (ct + tenMon)
            var items = new List<(ChiTietDonGoiDto ct, string tenMon)>();
            foreach (var ct in dsChiTiet)
            {
                string tenMon = "";
                if (ct.ThucAnId.HasValue)
                    tenMon = _dsThucAn.FirstOrDefault(x => x.Id == ct.ThucAnId.Value)?.Ten ?? "Món ăn";
                else if (ct.ThucUongId.HasValue)
                    tenMon = _dsThucUong.FirstOrDefault(x => x.Id == ct.ThucUongId.Value)?.Ten ?? "Thức uống";
                items.Add((ct, tenMon));
            }

            // mở form chọn số lượng + phương thức (có QR nếu chuyển khoản)
            using var f = new FormThanhToanTungPhan(items, _donGoiHienTai.Id, _banDangChon.Id);
            if (f.ShowDialog(this) != DialogResult.OK) return;

            var lines = f.ResultLines;
            if (lines == null || lines.Count == 0) return;

            decimal totalPay = lines.Sum(x => x.DonGia * x.PayQty);
            var method = f.PhuongThuc;
            var khachDua = f.KhachDua;
            var tienThoi = f.TienThoi;
            var qrImg = f.QrImage; // ✅ ảnh QR (nếu chuyển khoản)

            // tạo bill lines cho phần thanh toán
            var billLines = lines.Select(x => new ChiTietHoaDonDto
            {
                TenMon = x.TenMon,
                SoLuong = x.PayQty,
                DonGia = x.DonGia,
                ThanhTien = x.DonGia * x.PayQty
            }).ToList();

            using (var bill = new FormHoaDon(
                _donGoiHienTai.Id,
                overrideLines: billLines,
                qrImage: qrImg,
                phuongThuc: method,
                khachDua: khachDua,
                tienThoi: tienThoi,
                tongOverride: totalPay))
            {
                bill.ShowDialog(this);
            }

            // apply thanh toán từng phần: trừ số lượng / xóa chi tiết
            await ApplyPartialPaymentAsync(lines);

            // reload UI
            await LoadDanhSachBanAsync();
            await LoadDonGoiChoBanAsync(_banDangChon.Id);
        }

        // ================== APPLY PARTIAL PAYMENT ==================
        private async Task ApplyPartialPaymentAsync(List<FormThanhToanTungPhan.PayLine> lines)
        {
            if (_donGoiHienTai == null || _banDangChon == null) return;

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);

            foreach (var line in lines)
            {
                var ct = dsChiTiet.FirstOrDefault(x => x.Id == line.ChiTietId);
                if (ct == null) continue;

                int conLai = ct.SoLuong - line.PayQty;
                if (conLai <= 0)
                {
                    await _ctDonGoiService.DeleteAsync(ct.Id);
                }
                else
                {
                    var reqUpdate = new
                    {
                        DonGoiId = ct.DonGoiId,
                        ThucAnId = ct.ThucAnId,
                        ThucUongId = ct.ThucUongId,
                        SoLuong = conLai,
                        DonGia = ct.DonGia,
                        ChietKhau = ct.ChietKhau,
                        GhiChu = ct.GhiChu
                    };

                    await ApiClient.PutAsync<string>(
                        $"api/ChiTietDonGoi/{ct.Id}",
                        reqUpdate,
                        includeAuth: true
                    );
                }
            }

            // nếu hết món -> đóng đơn + trả bàn
            var con = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            if (con.Count == 0)
            {
                await CloseDonGoiSauThanhToanAsync();
            }
        }

        // ================== CLOSE ORDER & FREE TABLE ==================
        private async Task CloseDonGoiSauThanhToanAsync()
        {
            if (_donGoiHienTai == null) return;

            // NOTE: mình dùng TrangThai=1 là "đã thanh toán" (nếu API mày khác thì đổi lại)
            var reqCloseDon = new
            {
                NhanVienId = _donGoiHienTai.NhanVienId,
                BanId = _donGoiHienTai.BanId,
                MoLuc = _donGoiHienTai.MoLuc,
                DongLuc = DateTime.Now,
                TrangThai = 1,
                GhiChu = _donGoiHienTai.GhiChu
            };

            await ApiClient.PutAsync<string>(
                $"api/DonGoi/{_donGoiHienTai.Id}",
                reqCloseDon,
                includeAuth: true
            );

            // trả bàn
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
        }
    }
}
