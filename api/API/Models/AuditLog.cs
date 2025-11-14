namespace API.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        public string TenBang { get; set; } = null!;

        public int? IdBanGhi { get; set; }

        public string HanhDong { get; set; } = null!;

        public string? GiaTriCu { get; set; }

        public string? GiaTriMoi { get; set; }

        public string? NguoiThucHien { get; set; }

        public DateTime ThoiGian { get; set; }
    }
}
