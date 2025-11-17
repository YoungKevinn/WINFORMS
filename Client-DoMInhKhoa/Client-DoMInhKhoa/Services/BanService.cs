using Client_DoMInhKhoa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class BanService
    {
        public Task<List<BanDto>> GetAllAsync()
        {
            return ApiClient.GetAsync<List<BanDto>>("/api/Ban");
        }

        public Task<BanDto> GetByIdAsync(int id)
        {
            return ApiClient.GetAsync<BanDto>($"/api/Ban/{id}");
        }

        public Task<BanDto> CreateAsync(BanDto dto)
        {
            // includeAuth = true vì endpoint có khóa 🔒
            return ApiClient.PostAsync<BanDto>("/api/Ban", dto, includeAuth: true);
        }

        public Task<BanDto> UpdateAsync(int id, BanDto dto)
        {
            return ApiClient.PutAsync<BanDto>($"/api/Ban/{id}", dto, includeAuth: true);
        }

        public Task DeleteAsync(int id)
        {
            return ApiClient.DeleteAsync($"/api/Ban/{id}", includeAuth: true);
        }
    }
}
