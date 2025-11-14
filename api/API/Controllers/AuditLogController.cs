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

        // GET: api/AuditLog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuditLog>>> GetAll()
        {
            return await _context.AuditLogs
                .OrderByDescending(l => l.ThoiGian)
                .ToListAsync();
        }

        // GET: api/AuditLog/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AuditLog>> GetById(int id)
        {
            var log = await _context.AuditLogs.FindAsync(id);
            if (log == null) return NotFound();
            return log;
        }

        // GET: api/AuditLog/search?tenBang=HoaDon&idBanGhi=1
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AuditLog>>> Search(
            [FromQuery] string? tenBang,
            [FromQuery] int? idBanGhi,
            [FromQuery] DateTime? tuNgay,
            [FromQuery] DateTime? denNgay)
        {
            var query = _context.AuditLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tenBang))
                query = query.Where(l => l.TenBang == tenBang);

            if (idBanGhi.HasValue)
                query = query.Where(l => l.IdBanGhi == idBanGhi.Value);

            if (tuNgay.HasValue)
                query = query.Where(l => l.ThoiGian >= tuNgay.Value);

            if (denNgay.HasValue)
                query = query.Where(l => l.ThoiGian <= denNgay.Value);

            return await query
                .OrderByDescending(l => l.ThoiGian)
                .ToListAsync();
        }
    }
}
