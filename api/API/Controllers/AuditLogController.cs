using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuditLogController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public AuditLogController(CafeDbContext context)
        {
            _context = context;
        }

        // GET /api/AuditLog  -> lấy top mới nhất (có thể filter thêm bên WinForms)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> GetAll()
        {
            var data = await _context.AuditLogs
                .OrderByDescending(x => x.ThoiGian)
                .ToListAsync();

            var result = data.Select(x => new AuditLogDto
            {
                Id = x.Id,
                TenBang = x.TenBang,
                IdBanGhi = x.IdBanGhi,
                HanhDong = x.HanhDong,
                GiaTriCu = x.GiaTriCu,
                GiaTriMoi = x.GiaTriMoi,
                NguoiThucHien = x.NguoiThucHien,
                ThoiGian = x.ThoiGian
            }).ToList();

            return Ok(result);
        }

        // GET /api/AuditLog/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuditLogDto>> GetById(int id)
        {
            var log = await _context.AuditLogs.FindAsync(id);
            if (log == null) return NotFound();

            var dto = new AuditLogDto
            {
                Id = log.Id,
                TenBang = log.TenBang,
                IdBanGhi = log.IdBanGhi,
                HanhDong = log.HanhDong,
                GiaTriCu = log.GiaTriCu,
                GiaTriMoi = log.GiaTriMoi,
                NguoiThucHien = log.NguoiThucHien,
                ThoiGian = log.ThoiGian
            };

            return Ok(dto);
        }

        // GET /api/AuditLog/search?keyword=...&from=yyyy-MM-dd&to=yyyy-MM-dd
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AuditLogDto>>> Search(
            [FromQuery] AuditLogSearchRequest request)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var kw = request.Keyword.Trim();
                query = query.Where(x =>
                    x.TenBang.Contains(kw) ||
                    x.HanhDong.Contains(kw) ||
                    (x.NguoiThucHien != null && x.NguoiThucHien.Contains(kw)));
            }

            if (request.From.HasValue)
            {
                var fromDate = request.From.Value.Date;
                query = query.Where(x => x.ThoiGian >= fromDate);
            }

            if (request.To.HasValue)
            {
                // +1 ngày để lấy hết trong ngày To
                var toDate = request.To.Value.Date.AddDays(1);
                query = query.Where(x => x.ThoiGian < toDate);
            }

            query = query.OrderByDescending(x => x.ThoiGian);

            var data = await query.ToListAsync();

            var result = data.Select(x => new AuditLogDto
            {
                Id = x.Id,
                TenBang = x.TenBang,
                IdBanGhi = x.IdBanGhi,
                HanhDong = x.HanhDong,
                GiaTriCu = x.GiaTriCu,
                GiaTriMoi = x.GiaTriMoi,
                NguoiThucHien = x.NguoiThucHien,
                ThoiGian = x.ThoiGian
            }).ToList();

            return Ok(result);
        }
    }
}
