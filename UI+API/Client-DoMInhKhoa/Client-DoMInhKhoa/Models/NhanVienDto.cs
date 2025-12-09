using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    public class NhanVienDto
    {
        public int Id { get; set; }
        public string MaNhanVien { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public string VaiTro { get; set; } = string.Empty;   // "Admin" / "NhanVien"
        public bool DangHoatDong { get; set; } = true;
    }
}
