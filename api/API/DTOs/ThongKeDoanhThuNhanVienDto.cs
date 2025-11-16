namespace API.DTOs
{
    public class ThongKeDoanhThuNhanVienDto
    {
        public string MaNhanVien { get; set; } = null!;
        public string TenNhanVien { get; set; } = null!;

        public int SoHoaDon { get; set; }

        public decimal TongTien { get; set; }       // tổng tiền gốc
        public decimal TongGiamGia { get; set; }    // tổng giảm giá
        public decimal TongThue { get; set; }       // tổng thuế
        public decimal DoanhThuThucTe { get; set; } // = TongTien - TongGiamGia + TongThue
    }
}
