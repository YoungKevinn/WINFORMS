namespace API.DTOs
{
    public class NhanVienCreateUpdateDto
    {
        public string? MaNhanVien { get; set; }
        public string HoTen { get; set; } = null!;
        public string VaiTro { get; set; } = null!;
    }
}
