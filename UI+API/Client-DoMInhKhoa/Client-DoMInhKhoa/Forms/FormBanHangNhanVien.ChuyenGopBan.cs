using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien
    {
        // ================== CHUYỂN BÀN ==================
        private async void btnChuyenBan_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_donGoiHienTai == null || _banDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn đang có đơn để chuyển.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (_donGoiHienTai.TrangThai != 0)
                {
                    MessageBox.Show("Đơn gọi không còn ở trạng thái đang mở.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int fromId = _banDangChon.Id;

                if (cboBanTo.SelectedItem is not BanDto banTo)
                {
                    MessageBox.Show("Vui lòng chọn bàn đích.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int toId = banTo.Id;

                if (toId == fromId)
                {
                    MessageBox.Show("Bàn đích phải khác bàn hiện tại.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Bàn đích phải trống
                if (banTo.TrangThai != STATUS_TRONG)
                {
                    MessageBox.Show("Bàn đích đang được sử dụng, không thể chuyển.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show($"Chuyển đơn từ {_banDangChon.TenBan} sang {banTo.TenBan} ?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                // ✅ Cập nhật BanId của đơn gọi hiện tại sang bàn mới
                var reqUpdateDon = new
                {
                    NhanVienId = _donGoiHienTai.NhanVienId,
                    BanId = toId,
                    MoLuc = _donGoiHienTai.MoLuc,
                    DongLuc = _donGoiHienTai.DongLuc,
                    TrangThai = _donGoiHienTai.TrangThai,
                    GhiChu = _donGoiHienTai.GhiChu
                };

                await ApiClient.PutAsync<string>(
                    $"api/DonGoi/{_donGoiHienTai.Id}",
                    reqUpdateDon,
                    includeAuth: true
                );

                // ✅ Trả bàn cũ về trống
                _banDangChon.TrangThai = STATUS_TRONG;
                await ApiClient.PutAsync<string>(
                    $"api/Ban/{_banDangChon.Id}",
                    _banDangChon,
                    includeAuth: true
                );

                // ✅ Bàn mới chuyển sang đang dùng
                banTo.TrangThai = STATUS_DANG_DUNG;
                await ApiClient.PutAsync<string>(
                    $"api/Ban/{banTo.Id}",
                    banTo,
                    includeAuth: true
                );

                // reload UI
                await LoadDanhSachBanAsync();

                // chọn bàn mới luôn
                _banDangChon = _dsBan.FirstOrDefault(x => x.Id == toId) ?? banTo;
                cboBanFrom.SelectedValue = toId;

                await LoadDonGoiChoBanAsync(toId);

                MessageBox.Show("Chuyển bàn thành công!", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chuyển bàn:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== GỘP BÀN ==================
        private async void btnGopBan_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_banDangChon == null)
                {
                    MessageBox.Show("Vui lòng chọn bàn nguồn (bàn đang có đơn).", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // bàn nguồn phải có đơn mở
                if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
                {
                    MessageBox.Show("Bàn nguồn chưa có đơn đang mở để gộp.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int fromId = _banDangChon.Id;

                if (cboBanTo.SelectedItem is not BanDto banTo)
                {
                    MessageBox.Show("Vui lòng chọn bàn đích để gộp vào.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int toId = banTo.Id;

                if (toId == fromId)
                {
                    MessageBox.Show("Bàn đích phải khác bàn nguồn.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // bàn đích phải đang dùng (có đơn)
                if (banTo.TrangThai != STATUS_DANG_DUNG)
                {
                    MessageBox.Show("Bàn đích phải đang phục vụ (đang dùng) để gộp.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // tìm đơn mở của bàn đích
                var tatCaDon = await _donGoiService.GetAllAsync();
                var donDich = tatCaDon
                    .Where(x => x.BanId == toId && x.TrangThai == 0)
                    .OrderByDescending(x => x.MoLuc)
                    .FirstOrDefault();

                if (donDich == null)
                {
                    MessageBox.Show("Bàn đích không có đơn đang mở để gộp.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show(
                    $"Gộp món từ {_banDangChon.TenBan} vào {banTo.TenBan} ?\n" +
                    $"(Món sẽ chuyển sang đơn của bàn đích)",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                // lấy chi tiết của bàn nguồn
                var dsNguon = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);

                // chuyển từng chi tiết sang DonGoi của bàn đích
                foreach (var ct in dsNguon)
                {
                    var reqUpdateCt = new
                    {
                        DonGoiId = donDich.Id,      // ✅ chuyển sang đơn đích
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

                // đóng đơn nguồn (vì đã chuyển hết món)
                var reqCloseNguon = new
                {
                    NhanVienId = _donGoiHienTai.NhanVienId,
                    BanId = _donGoiHienTai.BanId,
                    MoLuc = _donGoiHienTai.MoLuc,
                    DongLuc = DateTime.Now,
                    TrangThai = 2, // ✅ giống logic mày đang dùng khi đơn rỗng
                    GhiChu = _donGoiHienTai.GhiChu
                };

                await ApiClient.PutAsync<string>(
                    $"api/DonGoi/{_donGoiHienTai.Id}",
                    reqCloseNguon,
                    includeAuth: true
                );

                // trả bàn nguồn về trống
                _banDangChon.TrangThai = STATUS_TRONG;
                await ApiClient.PutAsync<string>(
                    $"api/Ban/{_banDangChon.Id}",
                    _banDangChon,
                    includeAuth: true
                );

                // reload UI
                await LoadDanhSachBanAsync();

                // chuyển focus sang bàn đích
                _banDangChon = _dsBan.FirstOrDefault(x => x.Id == toId) ?? banTo;
                cboBanFrom.SelectedValue = toId;

                await LoadDonGoiChoBanAsync(toId);

                MessageBox.Show("Gộp bàn thành công!", "OK",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gộp bàn:\n" + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
