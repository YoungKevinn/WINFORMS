using Client_DoMInhKhoa.Models;   // <--- DÒNG QUAN TRỌNG
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class DanhMucService
    {
        public Task<List<DanhMucDto>> GetAllAsync()
        {
            return ApiClient.GetAsync<List<DanhMucDto>>("/api/DanhMuc");
        }

        public Task<DanhMucDto> GetByIdAsync(int id)
        {
            return ApiClient.GetAsync<DanhMucDto>($"/api/DanhMuc/{id}");
        }

        public Task<DanhMucDto> CreateAsync(DanhMucDto dto)
        {
            return ApiClient.PostAsync<DanhMucDto>("/api/DanhMuc", dto, includeAuth: true);
        }

        public Task<DanhMucDto> UpdateAsync(int id, DanhMucDto dto)
        {
            return ApiClient.PutAsync<DanhMucDto>($"/api/DanhMuc/{id}", dto, includeAuth: true);
        }

        public Task DeleteAsync(int id)
        {
            return ApiClient.DeleteAsync($"/api/DanhMuc/{id}", includeAuth: true);
        }
    }
}
