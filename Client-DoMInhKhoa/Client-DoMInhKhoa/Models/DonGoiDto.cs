using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    public class DonGoiDto
    {
        public int Id { get; set; }
        public int NhanVienId { get; set; }
        public int BanId { get; set; }
        public DateTime MoLuc { get; set; }
        public DateTime? DongLuc { get; set; }
        // 0: Chưa thanh toán, 1: Đã thanh toán
        public int TrangThai { get; set; }
        public string? GhiChu { get; set; }
    }

}
