namespace API.Models
{
    public class Ban
    {
        public int Id { get; set; }

        public string TenBan { get; set; } = string.Empty;

        // 0 = trống, 1 = đang dùng, 2 = đặt bàn
        public int TrangThai { get; set; }

        // nếu có navigation:
        public ICollection<DonGoi> DonGois { get; set; } = new List<DonGoi>();
    }
}
