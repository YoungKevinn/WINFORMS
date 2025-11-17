namespace API.DTOs
{
    public class DonGoiCreateUpdateDto
    {
        public int NhanVienId { get; set; }
        public int BanId { get; set; }
        public byte TrangThai { get; set; }   // 0 = Mo, 1 = Dong, 2 = Huy
        public DateTime MoLuc { get; set; }   // có thể để DateTime.Now ở server nếu default
        public DateTime? DongLuc { get; set; }
        public string? GhiChu { get; set; }

    }
}
