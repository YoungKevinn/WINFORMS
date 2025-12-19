using Client_DoMInhKhoa.Models; // Namespace chứa DTO
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    // Tạo nhanh DTO để hứng dữ liệu báo cáo (có thể tách ra file riêng nếu muốn)
    public class BaoCaoNhanVienDto
    {
        public string MaNhanVien { get; set; }
        public string HoTen { get; set; }
        public int SoHoaDon { get; set; }
        public decimal TongTien { get; set; }
    }

    public class BaoCaoService
    {
        public async Task<List<BaoCaoNhanVienDto>> GetDoanhThuNhanVienAsync(DateTime from, DateTime to)
        {
            // Format yyyy-MM-dd để gửi lên URL an toàn
            string f = from.ToString("yyyy-MM-dd");
            string t = to.ToString("yyyy-MM-dd");

            return await ApiClient.GetAsync<List<BaoCaoNhanVienDto>>(
                $"api/BaoCao/doanh-thu-nhan-vien?from={f}&to={t}"
            );
        }
    }
}