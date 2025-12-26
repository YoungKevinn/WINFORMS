using System;

namespace API.DTOs
{
    public class DonGoiDto
    {
        public int Id { get; set; }
        public int NhanVienId { get; set; }
        public int BanId { get; set; }

        public int TrangThai { get; set; }

        public DateTime MoLuc { get; set; }
        public DateTime? DongLuc { get; set; }

        public string? GhiChu { get; set; }

        // Nếu bạn có thêm mấy field hiển thị (tên bàn, tên nhân viên)
        // thì để thêm ở đây, ví dụ:
        // public string? TenNhanVien { get; set; }
        // public string? TenBan { get; set; }
    }
}
