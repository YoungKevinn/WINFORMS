namespace API.Models
{
    public class DanhMuc
    {
        public int Id { get; set; }
        public string Ten { get; set; } = null!;
        public bool DangHoatDong { get; set; }

        public ICollection<ThucUong>? ThucUongs { get; set; }
        public ICollection<ThucAn>? ThucAns { get; set; }
    }
}
