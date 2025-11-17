using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    public class ThucUongDto
    {
        public int Id { get; set; }
        public int DanhMucId { get; set; }
        public string Ten { get; set; } = string.Empty;
        public decimal DonGia { get; set; }
        public bool DangHoatDong { get; set; }
    }

}
