using System;

namespace API.DTOs
{
    public class DonGoiCreateUpdateDto
    {
        public int NhanVienId { get; set; }
        public int BanId { get; set; }

        // 0 = Mở, 1 = Đóng, 2 = Hủy
        public int TrangThai { get; set; }

        // Nếu client không gửi, server sẽ tự dùng DateTime.Now
        public DateTime MoLuc { get; set; }

        public DateTime? DongLuc { get; set; }

        // Ghi chú cho đơn
        public string? GhiChu { get; set; }
    }
}
