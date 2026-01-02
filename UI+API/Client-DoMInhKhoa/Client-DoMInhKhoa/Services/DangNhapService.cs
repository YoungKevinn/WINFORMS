using Client_DoMInhKhoa.Models;
using Client_DoMInhKhoa.Session;
using System;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class DangNhapService
    {
        private const string AdminLoginEndpoint = "api/Auth/admin-login";
        private const string EmployeeLoginEndpoint = "api/Auth/employee-login";

        // ✅ Giữ lại để FormDangNhapAdmin dùng (tránh CS1061)
        public Task<DangNhapResponse> DangNhapAsync(string tenDangNhap, string matKhau)
            => DangNhapCoreAsync(AdminLoginEndpoint, tenDangNhap, matKhau);

        // ✅ Login nhân viên
        public Task<DangNhapResponse> DangNhapNhanVienAsync(string tenDangNhap, string matKhau)
            => DangNhapCoreAsync(EmployeeLoginEndpoint, tenDangNhap, matKhau);

        private async Task<DangNhapResponse> DangNhapCoreAsync(string endpoint, string tenDangNhap, string matKhau)
        {
            var request = new DangNhapRequest
            {
                TenDangNhap = tenDangNhap,
                MatKhau = matKhau
            };

            var response = await ApiClient.PostAsync<DangNhapResponse>(
                endpoint,
                request,
                includeAuth: false
            );

            if (response == null || string.IsNullOrEmpty(response.Token))
                throw new Exception("Đăng nhập thất bại: không nhận được token.");

            var usernameForSession = !string.IsNullOrWhiteSpace(response.TenDangNhap)
                ? response.TenDangNhap
                : tenDangNhap;

            SessionHienTai.SetSession(
                response.Token,
                usernameForSession,
                response.VaiTro,
                response.HetHanHopLe
            );

            return response;
        }
    }
}
