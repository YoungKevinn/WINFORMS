namespace API.Models
{
    public class NhanVien
    {
        public int Id { get; set; }

        public string MaNhanVien { get; set; } = null!;   
        public string TenDangNhap { get; set; } = null!;  
        public string HoTen { get; set; } = null!;
        public string VaiTro { get; set; } = null!;      
        public string MatKhauHash { get; set; } = null!;

        public int TrangThai { get; set; } = 0;

        public int FailedLoginCount { get; set; } = 0;

        public DateTime? LockoutEndUtc { get; set; }

        public ICollection<DonGoi>? DonGois { get; set; }
        public ICollection<HoaDon>? HoaDons { get; set; }
    }
}
