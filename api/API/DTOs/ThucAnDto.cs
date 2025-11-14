namespace API.DTOs
{
    public class ThucAnCreateUpdateDto
    {
        public int DanhMucId { get; set; }
        public string Ten { get; set; } = null!;
        public decimal DonGia { get; set; }
        public bool DangHoatDong { get; set; }
    }
}
