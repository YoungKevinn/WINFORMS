namespace API.DTOs
{
    public class HoaDonDto
    {
        public int Id { get; set; }
        public string MaHoaDon { get; set; } = string.Empty;

        // Thời gian tạo / thời gian hiển thị trên lưới
        public DateTime ThoiGian { get; set; }

        // Các số tiền (nếu sau này muốn dùng)
        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }

        // = TongTien - GiamGia + Thue
        public decimal ThanhTien { get; set; }

        public string? TenNhanVien { get; set; }
        public string? TenBan { get; set; }
    }
}
