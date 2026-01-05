using System;

namespace Client_DoMInhKhoa.Models
{
    /// <summary>
    /// Dùng cho báo cáo doanh thu theo nhân viên.
    /// </summary>
    public class ThongKeDoanhThuNhanVienDto
    {
        public int NhanVienId { get; set; }
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TongTien { get; set; }
    }
}
