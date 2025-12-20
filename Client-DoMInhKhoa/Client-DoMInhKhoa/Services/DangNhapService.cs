using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class DangNhapService
    {
        // TODO: sửa endpoint này cho đúng với API của bạn
        // Ví dụ: "api/auth/login" hoặc "api/admin-login"
        private const string DangNhapEndpoint = "api/Auth/admin-login";

        public async Task<DangNhapResponse> DangNhapAsync(string tenDangNhap, string matKhau)
        {
            var request = new DangNhapRequest
            {
                TenDangNhap = tenDangNhap,
                MatKhau = matKhau
            };

            // Vì login chưa có token nên includeAuth = false
            var response = await ApiClient.PostAsync<DangNhapResponse>(
                DangNhapEndpoint,
                request,
                includeAuth: false
            );

            if (response == null || string.IsNullOrEmpty(response.Token))
            {
                throw new Exception("Đăng nhập thất bại: không nhận được token.");
            }

            // Lưu session hiện tại
            SessionHienTai.SetSession(
                response.Token,
                response.TenDangNhap ?? tenDangNhap,
                response.VaiTro,
                response.HetHan
            );

            return response;
        }
    }
}
