using Client_DoMInhKhoa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class ThucUongService
    {
        public Task<List<ThucUongDto>> GetAllAsync()
        {
            return ApiClient.GetAsync<List<ThucUongDto>>("/api/ThucUong");
        }

        public Task<ThucUongDto> GetByIdAsync(int id)
        {
            return ApiClient.GetAsync<ThucUongDto>($"/api/ThucUong/{id}");
        }

        public Task<ThucUongDto> CreateAsync(ThucUongDto dto)
        {
            return ApiClient.PostAsync<ThucUongDto>("/api/ThucUong", dto, includeAuth: true);
        }

        public Task<ThucUongDto> UpdateAsync(int id, ThucUongDto dto)
        {
            return ApiClient.PutAsync<ThucUongDto>($"/api/ThucUong/{id}", dto, includeAuth: true);
        }

        public Task DeleteAsync(int id)
        {
            return ApiClient.DeleteAsync($"/api/ThucUong/{id}", includeAuth: true);
        }
    }
}
