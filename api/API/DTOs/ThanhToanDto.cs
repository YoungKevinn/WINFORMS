namespace API.DTOs
{
    public class ThanhToanCreateUpdateDto
    {
        public int DonGoiId { get; set; }
        public decimal SoTien { get; set; }
        public string PhuongThuc { get; set; } = null!;
        public DateTime ThanhToanLuc { get; set; }
        public string? MaThamChieu { get; set; }
    }
}
