using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    public class ChiTietDonGoiDto
    {
        public int Id { get; set; }
        public int DonGoiId { get; set; }
        public int? ThucUongId { get; set; }
        public int? ThucAnId { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ChietKhau { get; set; }
        public string? GhiChu { get; set; }
    }
}
