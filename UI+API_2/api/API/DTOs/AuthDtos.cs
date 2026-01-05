using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AdminRegisterDto
    {
        [Required, StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9._-]+$", ErrorMessage = "Tên đăng nhập chỉ gồm chữ/số và . _ -")]
        public string TenDangNhap { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 6)]
        public string MatKhau { get; set; } = null!;

        [Required, StringLength(100)]
        public string HoTen { get; set; } = null!;

        public string? MaNhanVien { get; set; }
    }

    public class AdminLoginDto
    {
        [Required, StringLength(50)]
        public string TenDangNhap { get; set; } = null!;

        [Required, StringLength(100, MinimumLength = 1)]
        public string MatKhau { get; set; } = null!;
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public string HoTen { get; set; } = null!;
        public string VaiTro { get; set; } = null!;
    }
}
