using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    public class BanDto
    {
        public int Id { get; set; }
        public string TenBan { get; set; } = string.Empty;

        // API trả true/false nên để bool
        public bool TrangThai { get; set; }    // true = có khách, false = trống
    }
}
