namespace API.DTOs
{
    public class ThongKeDoanhThuNhanVienDto
    {
        public int NhanVienId { get; set; }         
        public string MaNhanVien { get; set; } = null!;
        public string HoTen { get; set; } = null!; 

        public int SoHoaDon { get; set; }

        public decimal TongTien { get; set; }        
        public decimal TongGiamGia { get; set; }     
        public decimal TongThue { get; set; }       
        public decimal DoanhThuThucTe { get; set; } 
    }
}
