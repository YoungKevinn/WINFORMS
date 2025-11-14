namespace API.Models
{
    public class HoaDon
    {
        public int Id { get; set; }
        public string? MaHoaDon { get; set; }
        public int NhanVienId { get; set; }
        public int? BanId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public byte TrangThai { get; set; } // 0=open,1=paid,2=cancel
        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }
        public string? GhiChu { get; set; }

        public NhanVien? NhanVien { get; set; }
        public Ban? Ban { get; set; }
    }
}
