using System;
using System.Collections.Generic;

namespace API.Models
{
    public class DonGoi
    {
        public int Id { get; set; }

        public int NhanVienId { get; set; }
        public int BanId { get; set; }

        // 0 = Open, 1 = Closed, 2 = Cancelled
        // Đổi byte -> int cho khớp với cột int trong DB
        public int TrangThai { get; set; }

        public DateTime MoLuc { get; set; }
        public DateTime? DongLuc { get; set; }

        // Ghi chú cho đơn
        public string? GhiChu { get; set; }

        // Navigation
        public NhanVien? NhanVien { get; set; }
        public Ban? Ban { get; set; }
        public ICollection<ChiTietDonGoi>? ChiTietDonGois { get; set; }
        public ICollection<ThanhToan>? ThanhToans { get; set; }
    }
}
