namespace API.Models
{
    public class NhanVien
    {
        public int Id { get; set; }

        public string MaNhanVien { get; set; } = null!;   // NOT NULL + UNIQUE

        public string TenDangNhap { get; set; } = null!;  // NOT NULL + UNIQUE, dùng cho login

        public string HoTen { get; set; } = null!;

        public string VaiTro { get; set; } = null!;       // "Admin" / "NhanVien" / ...

        public string MatKhauHash { get; set; } = null!;

        public ICollection<DonGoi>? DonGois { get; set; }
        public ICollection<HoaDon>? HoaDons { get; set; }
    }
}
