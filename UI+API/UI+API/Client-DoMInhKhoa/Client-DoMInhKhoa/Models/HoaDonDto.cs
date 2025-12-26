public class HoaDonDto
{
    public int Id { get; set; }
    public string MaHoaDon { get; set; } = string.Empty;
    public DateTime ThoiGian { get; set; }
    public decimal TongTien { get; set; }
    public decimal GiamGia { get; set; }
    public decimal Thue { get; set; }
    public decimal ThanhTien { get; set; }
    public string? TenNhanVien { get; set; }
    public string? TenBan { get; set; }
}
