namespace API.DTOs
{
    public class ThongKeDoanhThuDto
    {
        public string Nhan { get; set; } = null!;

        public int SoHoaDon { get; set; }

        public decimal TongTien { get; set; }      // tổng tiền gốc
        public decimal TongGiamGia { get; set; }   // tổng giảm
        public decimal TongThue { get; set; }      // tổng thuế

        public decimal DoanhThuThucTe { get; set; }
    }
}
