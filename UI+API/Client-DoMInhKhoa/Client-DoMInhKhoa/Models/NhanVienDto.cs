namespace Client_DoMInhKhoa.Models
{
    public class NhanVienDto
    {
        public int Id { get; set; }
        public string MaNhanVien { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string TenDangNhap { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;   

        // 0=Đang làm, 1=Nghỉ, 2=Khóa
        public int TrangThai { get; set; }
        public bool DangHoatDong { get; set; } = true;
        public string TrangThaiText => TrangThai switch
        {
            0 => "Đang làm",
            1 => "Nghỉ",
            2 => "Khóa",
            _ => "Không rõ"
        };
    }
}
