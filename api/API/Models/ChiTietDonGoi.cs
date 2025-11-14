namespace API.Models
{
    public class ChiTietDonGoi
    {
        public int Id { get; set; }
        public int DonGoiId { get; set; }
        public int? ThucUongId { get; set; }
        public int? ThucAnId { get; set; } // map với cột DoAnId trong DB
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ChietKhau { get; set; }
        public string? GhiChu { get; set; }

        public DonGoi? DonGoi { get; set; }
        public ThucUong? ThucUong { get; set; }
        public ThucAn? ThucAn { get; set; }
    }
}
