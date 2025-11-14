namespace API.Models
{
    public class DonGoi
    {
        public int Id { get; set; }
        public int NhanVienId { get; set; }
        public int BanId { get; set; }
        public byte TrangThai { get; set; } // 0=Open,1=Closed,2=Cancelled
        public DateTime MoLuc { get; set; }
        public DateTime? DongLuc { get; set; }
        public string? GhiChu { get; set; }

        public NhanVien? NhanVien { get; set; }
        public Ban? Ban { get; set; }
        public ICollection<ChiTietDonGoi>? ChiTietDonGois { get; set; }
        public ICollection<ThanhToan>? ThanhToans { get; set; }
    }
}
