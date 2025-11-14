namespace API.DTOs
{
    public class HoaDonCreateUpdateDto
    {
        public string? MaHoaDon { get; set; }
        public int NhanVienId { get; set; }
        public int? BanId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public byte TrangThai { get; set; }       // 0=Open,1=Paid,2=Cancel
        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }
        public string? GhiChu { get; set; }
    }
}
