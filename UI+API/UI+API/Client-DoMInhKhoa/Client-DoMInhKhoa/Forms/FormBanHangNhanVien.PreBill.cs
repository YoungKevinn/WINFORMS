using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_DoMInhKhoa.Forms
{
    public partial class FormBanHangNhanVien : Form
    {
        // ✅ DÙNG CHUNG: MỞ HÓA ĐƠN TẠM (PRE-BILL)
        private async Task<bool> ShowPreBillAsync()
        {
            if (_banDangChon == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (_donGoiHienTai == null || _donGoiHienTai.TrangThai != 0)
            {
                MessageBox.Show("Bàn này chưa có đơn đang mở để in tạm.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            var dsChiTiet = await _ctDonGoiService.GetByDonGoiIdAsync(_donGoiHienTai.Id);
            if (dsChiTiet == null || dsChiTiet.Count == 0)
            {
                MessageBox.Show("Đơn hiện tại chưa có món nào.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            using (var f = new FormHoaDonTam(_donGoiHienTai.Id))
            {
                f.ShowDialog(this);
            }

            return true;
        }

        // ✅ CLICK NÚT “IN TẠM”
        private async void btnInTam_Click(object? sender, EventArgs e)
        {
            try
            {
                await ShowPreBillAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi in tạm: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
