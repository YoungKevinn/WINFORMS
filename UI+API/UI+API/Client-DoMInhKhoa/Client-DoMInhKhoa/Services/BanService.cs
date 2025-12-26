using Client_DoMInhKhoa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public static class BanService
    {
        // Lấy danh sách bàn
        public static Task<List<BanDto>> GetAllAsync()
        {
            return ApiClient.GetAsync<List<BanDto>>("api/Ban");
        }

        // Cập nhật trạng thái (true = đang dùng, false = trống)
        public static Task UpdateTrangThaiAsync(int banId, bool trangThai)
        {
            var dto = new
            {
                TenBan = "",      // server không dùng cũng được
                TrangThai = trangThai
            };

            // dùng PUT cùng endpoint Update của BanController
            return ApiClient.PutAsync<string>($"api/Ban/{banId}", dto, includeAuth: true);
        }
    }
}
