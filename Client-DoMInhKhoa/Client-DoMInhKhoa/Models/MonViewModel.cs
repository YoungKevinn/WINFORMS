using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    //View-model nhỏ để ghép chung ThucAn + ThucUong:
    public class MonViewModel
    {
        public int Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public decimal DonGia { get; set; }

        // "ThucAn" hoặc "ThucUong"
        public string Loai { get; set; } = string.Empty;

        // Để combobox hiển thị đúng tên món
        public override string ToString() => Ten;
    }
}
