using System;
using System.Net.Http;
using System.Threading.Tasks;
using Client_DoMInhKhoa.Models;

namespace Client_DoMInhKhoa.Services
{
    public class NhanVienService
    {
        /// <summary>
        /// Lấy thông tin nhân viên theo mã.
        /// Trả về null nếu không tồn tại hoặc lỗi HTTP.
        /// </summary>
        public async Task<NhanVienDto?> LayTheoMaAsync(string maNhanVien)
        {
            if (string.IsNullOrWhiteSpace(maNhanVien))
                return null;

            // TODO: CHỈNH LẠI URL CHO ĐÚNG VỚI API CỦA BẠN
            // Ví dụ: GET /api/nhanvien/by-code/{maNhanVien}
            var url = $"/api/NhanVien/by-code/{maNhanVien}";

            try
            {
                // ApiClient là static class => gọi trực tiếp qua tên class
                // Nếu hàm của bạn có thêm tham số (ví dụ bool includeAuth) thì truyền cho đúng
                var nv = await ApiClient.GetAsync<NhanVienDto>(url);

                return nv;
            }
            catch (HttpRequestException)
            {
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
