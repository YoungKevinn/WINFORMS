    namespace API.Models
{
    public class ThanhToan
    {
        public int Id { get; set; }
        public int DonGoiId { get; set; }
        public decimal SoTien { get; set; }
        public string PhuongThuc { get; set; } = null!;
        public DateTime ThanhToanLuc { get; set; }
        public string? MaThamChieu { get; set; }

        public DonGoi? DonGoi { get; set; }
    }
}
