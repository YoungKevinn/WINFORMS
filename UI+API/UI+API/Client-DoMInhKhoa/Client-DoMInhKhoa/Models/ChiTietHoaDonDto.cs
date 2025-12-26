using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    public class ChiTietHoaDonDto    // BẮT BUỘC phải có public
    {
        public int Id { get; set; }
        public int DonGoiId { get; set; }
        public string TenMon { get; set; } = "";
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ChietKhau { get; set; }
        public decimal ThanhTien { get; set; }
       // public int GiamGia { get; set; }

        public string? GhiChu { get; set; }
    }
}
