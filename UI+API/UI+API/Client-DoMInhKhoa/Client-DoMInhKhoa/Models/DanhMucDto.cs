using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    public class DanhMucDto
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public bool DangHoatDong { get; set; }
    }
}
