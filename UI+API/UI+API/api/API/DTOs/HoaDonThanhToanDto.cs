namespace API.DTOs
{
    public class HoaDonThanhToanDto
    {
        public decimal TongTien { get; set; }
        public decimal GiamGia { get; set; }
        public decimal Thue { get; set; }
        public string? GhiChu { get; set; }

        // nếu bạn muốn lưu thêm phương thức thanh toán, mã tham chiếu...
        public string? PhuongThuc { get; set; }
        public string? MaThamChieu { get; set; }
    }
}
