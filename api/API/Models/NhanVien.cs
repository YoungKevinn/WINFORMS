namespace API.Models
{

    public class NhanVien
    {
        public int Id { get; set; }
        public string? MaNhanVien { get; set; }
        public string HoTen { get; set; } = null!;
        public string VaiTro { get; set; } = null!;

        public ICollection<DonGoi>? DonGois { get; set; }
        public ICollection<HoaDon>? HoaDons { get; set; }
    }
}
