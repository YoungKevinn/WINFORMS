using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class NhanVienViewDto
    {
        public int Id { get; set; }
        public string? MaNhanVien { get; set; }
        public string HoTen { get; set; } = null!;
        public string VaiTro { get; set; } = null!;
        public string? TenDangNhap { get; set; }

        // ✅ thêm trạng thái để hiển thị trên UI
        public int TrangThai { get; set; }
    }

    public class NhanVienCreateUpdateDto
    {
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Mã NV phải 3-20 ký tự.")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "Mã NV chỉ gồm chữ và số (không khoảng trắng/ký tự đặc biệt).")]
        public string? MaNhanVien { get; set; }

        [Required(ErrorMessage = "Họ tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự.")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Vai trò là bắt buộc.")]
        [RegularExpression(@"^(Admin|NhanVien)$", ErrorMessage = "Vai trò chỉ được: Admin hoặc NhanVien.")]
        public string VaiTro { get; set; } = null!;
    }

    public class NhanVienStatusDto
    {
        [Range(0, 2, ErrorMessage = "Trạng thái chỉ được: 0=Đang làm, 1=Nghỉ, 2=Khóa.")]
        public int TrangThai { get; set; }
    }
}
