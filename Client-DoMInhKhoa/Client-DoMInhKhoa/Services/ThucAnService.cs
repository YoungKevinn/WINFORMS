using Client_DoMInhKhoa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class ThucAnService
    {
        public Task<List<ThucAnDto>> GetAllAsync()
        {
            return ApiClient.GetAsync<List<ThucAnDto>>("/api/ThucAn");
        }

        public Task<ThucAnDto> GetByIdAsync(int id)
        {
            return ApiClient.GetAsync<ThucAnDto>($"/api/ThucAn/{id}");
        }

        public Task<ThucAnDto> CreateAsync(ThucAnDto dto)
        {
            return ApiClient.PostAsync<ThucAnDto>("/api/ThucAn", dto, includeAuth: true);
        }

        public Task<ThucAnDto> UpdateAsync(int id, ThucAnDto dto)
        {
            return ApiClient.PutAsync<ThucAnDto>($"/api/ThucAn/{id}", dto, includeAuth: true);
        }

        public Task DeleteAsync(int id)
        {
            return ApiClient.DeleteAsync($"/api/ThucAn/{id}", includeAuth: true);
        }
    }
}
