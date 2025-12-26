namespace API.DTOs
{
    public class ChiTietHoaDonDto
    {
        public int Id { get; set; }

        public string TenMon { get; set; } = null!;   // Tên món hiển thị trên hoá đơn
        public string LoaiMon { get; set; } = "";     // "Thức uống" / "Thức ăn"

        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ChietKhau { get; set; }        // giảm giá dòng này
        public decimal ThanhTien { get; set; }        // tính ra để in

        public string? GhiChu { get; set; }           // ghi chú từng món
    }
}
