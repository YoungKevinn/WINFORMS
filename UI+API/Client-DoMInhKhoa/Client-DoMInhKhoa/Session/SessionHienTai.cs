using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Session
{
    public static class SessionHienTai
    {
        public static string JwtToken { get; private set; }
        public static string TenDangNhap { get; private set; }
        public static string VaiTro { get; private set; }
        public static DateTime? HetHan { get; private set; }

        public static bool DaDangNhap =>
            !string.IsNullOrEmpty(JwtToken) &&
            (HetHan == null || HetHan > DateTime.UtcNow);

        public static void SetSession(string token, string tenDangNhap, string vaiTro, DateTime? hetHan = null)
        {
            JwtToken = token;
            TenDangNhap = tenDangNhap;
            VaiTro = vaiTro;
            HetHan = hetHan;
        }

        public static void Clear()
        {
            JwtToken = null;
            TenDangNhap = null;
            VaiTro = null;
            HetHan = null;
        }
    }
}
