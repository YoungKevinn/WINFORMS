using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client_DoMInhKhoa.Models;

namespace Client_DoMInhKhoa.Services
{
    public class BaoCaoService
    {
        // ❌ BỎ dòng này đi:
        // private readonly ApiClient _apiClient = new ApiClient();

        /// <summary>
        /// Gọi GET /api/BaoCao/doanh-thu?from=yyyy-MM-dd&to=yyyy-MM-dd&kieu=ngay|thang|nam
        /// </summary>
        public Task<List<ThongKeDoanhThuDto>> GetThongKeDoanhThuAsync(
            DateTime from,
            DateTime to,
            string kieuThongKe)
        {
            var query =
                $"from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}&kieu={kieuThongKe}";

            // 🟢 Gọi trực tiếp static ApiClient
            return ApiClient.GetAsync<List<ThongKeDoanhThuDto>>(
                $"api/BaoCao/doanh-thu?{query}");
        }

        /// <summary>
        /// Gọi GET /api/BaoCao/doanh-thu-nhan-vien?from=yyyy-MM-dd&to=yyyy-MM-dd
        /// </summary>
        public Task<List<ThongKeDoanhThuNhanVienDto>> GetThongKeDoanhThuNhanVienAsync(
            DateTime from,
            DateTime to)
        {
            var query = $"from={from:yyyy-MM-dd}&to={to:yyyy-MM-dd}";

            return ApiClient.GetAsync<List<ThongKeDoanhThuNhanVienDto>>(
                $"api/BaoCao/doanh-thu-nhan-vien?{query}");
        }
    }
}
