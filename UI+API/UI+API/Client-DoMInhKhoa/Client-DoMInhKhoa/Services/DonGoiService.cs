using Client_DoMInhKhoa.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class DonGoiService
    {
        public Task<List<DonGoiDto>> GetAllAsync()
        {
            return ApiClient.GetAsync<List<DonGoiDto>>("/api/DonGoi");
        }

        public Task<DonGoiDto> GetByIdAsync(int id)
        {
            return ApiClient.GetAsync<DonGoiDto>($"/api/DonGoi/{id}");
        }

        public Task<DonGoiDto> CreateAsync(object request)
        {
            // request có thể là DonGoiDto hoặc 1 request model riêng
            return ApiClient.PostAsync<DonGoiDto>("/api/DonGoi", request, includeAuth: true);
        }

        public Task<DonGoiDto> UpdateAsync(int id, object request)
        {
            return ApiClient.PutAsync<DonGoiDto>($"/api/DonGoi/{id}", request, includeAuth: true);
        }

        public Task DeleteAsync(int id)
        {
            return ApiClient.DeleteAsync($"/api/DonGoi/{id}", includeAuth: true);
        }
    }
}
