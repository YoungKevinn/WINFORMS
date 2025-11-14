namespace API.DTOs
{
    public class ChiTietDonGoiCreateUpdateDto
    {
        public int DonGoiId { get; set; }
        public int? ThucUongId { get; set; }
        public int? ThucAnId { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ChietKhau { get; set; }
        public string? GhiChu { get; set; }
    }
}
