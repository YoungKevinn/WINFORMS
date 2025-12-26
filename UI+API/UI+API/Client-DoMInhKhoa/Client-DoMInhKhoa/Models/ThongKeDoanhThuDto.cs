using System;

namespace Client_DoMInhKhoa.Models
{
    /// <summary>
    /// Dùng cho báo cáo doanh thu tổng hợp (theo ngày/tháng/năm).
    /// </summary>
    public class ThongKeDoanhThuDto
    {
        public DateTime Ngay { get; set; }        // Thời điểm đại diện (ngày / đầu tháng / đầu năm)
        public int SoHoaDon { get; set; }         // Số lượng hóa đơn
        public decimal TongTien { get; set; }     // Tổng doanh thu
    }
}
