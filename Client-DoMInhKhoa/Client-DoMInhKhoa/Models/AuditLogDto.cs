using System;

namespace Client_DoMInhKhoa.Models
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string TenBang { get; set; }
        public int? IdBanGhi { get; set; }
        public string HanhDong { get; set; }
        public string GiaTriCu { get; set; }
        public string GiaTriMoi { get; set; }
        public string NguoiThucHien { get; set; }
        public DateTime ThoiGian { get; set; }
    }
}
