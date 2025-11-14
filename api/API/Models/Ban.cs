namespace API.Models
{
    public class Ban
    {
        public int Id { get; set; }
        public string TenBan { get; set; } = null!;
        public bool TrangThai { get; set; }  // 0 = Free, 1 = Occupied

        public ICollection<DonGoi>? DonGois { get; set; }
        public ICollection<HoaDon>? HoaDons { get; set; }
    }
}
