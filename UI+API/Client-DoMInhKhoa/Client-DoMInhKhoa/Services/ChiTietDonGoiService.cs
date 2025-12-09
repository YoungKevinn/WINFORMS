using Client_DoMInhKhoa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class ChiTietDonGoiService
    {
        // Lấy tất cả (nếu cần)
        public Task<List<ChiTietDonGoiDto>> GetAllAsync()
        {
            return ApiClient.GetAsync<List<ChiTietDonGoiDto>>("/api/ChiTietDonGoi");
        }

        public Task<ChiTietDonGoiDto> GetByIdAsync(int id)
        {
            return ApiClient.GetAsync<ChiTietDonGoiDto>($"/api/ChiTietDonGoi/{id}");
        }

        // Tuỳ vào controller của bạn filter theo donGoiId kiểu gì:
        // ví dụ: GET /api/ChiTietDonGoi?donGoiId=5
        public async Task<List<ChiTietDonGoiDto>> GetByDonGoiIdAsync(int donGoiId)
        {
            // Gọi API lấy toàn bộ rồi lọc phía client
            var all = await ApiClient.GetAsync<List<ChiTietDonGoiDto>>("/api/ChiTietDonGoi");
            return all.Where(x => x.DonGoiId == donGoiId).ToList();
        }

        public Task<ChiTietDonGoiDto> CreateAsync(object request)
        {
            return ApiClient.PostAsync<ChiTietDonGoiDto>("/api/ChiTietDonGoi", request, includeAuth: true);
        }

        public Task<ChiTietDonGoiDto> UpdateAsync(int id, object request)
        {
            return ApiClient.PutAsync<ChiTietDonGoiDto>($"/api/ChiTietDonGoi/{id}", request, includeAuth: true);
        }

        public Task DeleteAsync(int id)
        {
            return ApiClient.DeleteAsync($"/api/ChiTietDonGoi/{id}", includeAuth: true);
        }
    }
}
