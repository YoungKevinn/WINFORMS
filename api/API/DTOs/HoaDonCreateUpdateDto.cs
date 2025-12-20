namespace API.DTOs
{
    public class HoaDonCreateUpdateDto
    {
        public string? MaHoaDon { get; set; }

        // dùng Id để lưu DB
        public int NhanVienId { get; set; }
        public int? BanId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // tinyint trong DB -> thường là byte (0 = chưa thanh toán, 1 = đã thanh toán...)
        public byte TrangThai { get; set; }

        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }

        public string? GhiChu { get; set; }
    }
}
