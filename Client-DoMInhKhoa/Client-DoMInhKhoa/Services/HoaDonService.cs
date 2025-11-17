using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Client_DoMInhKhoa.Models;   // chỉnh lại namespace models của bạn cho đúng

namespace Client_DoMInhKhoa.Services
{
    public class HoaDonService
    {
        /// <summary>
        /// Lấy danh sách hóa đơn để tra cứu (lọc theo ngày)
        /// Gọi API: GET /api/hoadon/filter?from=&to=
        /// </summary>
        public Task<List<HoaDonDto>?> GetHoaDonsAsync(DateTime? from, DateTime? to)
        {
            string url = "api/hoadon";

            var qs = new List<string>();

            if (from.HasValue)
                qs.Add("from=" + Uri.EscapeDataString(from.Value.ToString("yyyy-MM-dd")));
            if (to.HasValue)
                qs.Add("to=" + Uri.EscapeDataString(to.Value.ToString("yyyy-MM-dd")));

            if (qs.Count > 0)
                url += "?" + string.Join("&", qs);

            return ApiClient.GetAsync<List<HoaDonDto>>(url, true);
        }


        /// <summary>
        /// Lấy chi tiết 1 hóa đơn để in
        /// Gọi API: GET /api/hoadon/{donGoiId}/chi-tiet
        /// (id ở đây đang là DonGoiId tương ứng với hóa đơn đó)
        /// </summary>
        public Task<List<ChiTietHoaDonDto>> GetChiTietAsync(int donGoiId)
        {
            string url = $"api/hoadon/{donGoiId}/chi-tiet";
            return ApiClient.GetAsync<List<ChiTietHoaDonDto>>(url, true);
        }
    }
}
