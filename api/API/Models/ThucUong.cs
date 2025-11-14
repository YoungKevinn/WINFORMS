    namespace API.Models
{
    public class ThucUong
    {
        public int Id { get; set; }
        public int DanhMucId { get; set; }
        public string Ten { get; set; } = null!;
        public decimal DonGia { get; set; }
        public bool DangHoatDong { get; set; }

        public DanhMuc? DanhMuc { get; set; }
        public ICollection<ChiTietDonGoi>? ChiTietDonGois { get; set; }
    }
}
