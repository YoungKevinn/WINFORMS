using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Models
{
    public class DangNhapResponse
    {
        public string Token { get; set; }

        // Tùy API của bạn trả về gì thì đặt tên tương ứng
        public string TenDangNhap { get; set; }
        public string VaiTro { get; set; }

        // Nếu API không trả về ngày hết hạn thì có thể bỏ
        public DateTime? HetHan { get; set; }
    }
}
