using Client_DoMInhKhoa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_DoMInhKhoa.Services
{
    public class AuditLogService
    {
        // GET /api/AuditLog
        public Task<List<AuditLogDto>> GetAllAsync()
        {
            return ApiClient.GetAsync<List<AuditLogDto>>("api/AuditLog");
        }

        // GET /api/AuditLog/search?keyword=&from=&to=
        public Task<List<AuditLogDto>> SearchAsync(string keyword, DateTime? from, DateTime? to)
        {
            var parameters = new List<string>();

            if (!string.IsNullOrWhiteSpace(keyword))
                parameters.Add("keyword=" + Uri.EscapeDataString(keyword.Trim()));

            if (from.HasValue)
                parameters.Add("from=" + from.Value.ToString("yyyy-MM-dd"));

            if (to.HasValue)
                parameters.Add("to=" + to.Value.ToString("yyyy-MM-dd"));

            var url = "api/AuditLog/search";
            if (parameters.Any())
            {
                url += "?" + string.Join("&", parameters);
            }

            return ApiClient.GetAsync<List<AuditLogDto>>(url);
        }
    }
}
    