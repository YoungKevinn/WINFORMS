using Client_DoMInhKhoa.Forms;
using Client_DoMInhKhoa.Models; // Hoặc DTOs
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class ThanhToanService
    {
        // Lấy lịch sử để biết đã trả bao nhiêu
        public async Task<List<ThanhToanDto>> GetLichSuAsync(int donGoiId)
        {
            return await ApiClient.GetAsync<List<ThanhToanDto>>($"api/ThanhToan?donGoiId={donGoiId}");
        }

        // Gửi thanh toán mới
        public async Task CreateAsync(ThanhToanCreateUpdateDto dto)
        {
            // includeAuth = true nếu API yêu cầu đăng nhập
            await ApiClient.PostAsync<object>("api/ThanhToan", dto, includeAuth: true);
        }
    }
}