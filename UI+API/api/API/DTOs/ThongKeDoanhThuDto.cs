namespace API.DTOs
{
    public class ThongKeDoanhThuDto
    {
      
        public DateTime Ngay { get; set; }

        public int SoHoaDon { get; set; }

        public decimal TongTien { get; set; }      // Tổng tiền gốc
        public decimal TongGiamGia { get; set; }   // Tổng giảm
        public decimal TongThue { get; set; }      // Tổng thuế

        public decimal DoanhThuThucTe { get; set; } // = TongTien - TongGiamGia + TongThue
    }
}
