namespace API.DTOs
{
    public class AdminRegisterDto
    {
        public string TenDangNhap { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public string HoTen { get; set; } = null!;
        public string? MaNhanVien { get; set; }
    }

    public class AdminLoginDto
    {
        public string TenDangNhap { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string HoTen { get; set; } = null!;
        public string VaiTro { get; set; } = null!;

        public bool MustChangePassword { get; set; } = false;
    }
}
