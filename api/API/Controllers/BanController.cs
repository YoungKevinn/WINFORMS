using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BanController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public BanController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ban>>> GetAll()
        {
            return await _context.Bans.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Ban>> GetById(int id)
        {
            var ban = await _context.Bans.FindAsync(id);
            if (ban == null) return NotFound();
            return ban;
        }

        [HttpPost]
        public async Task<ActionResult<Ban>> Create(
            BanCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var ban = new Ban
            {
                TenBan = dto.TenBan,
                TrangThai = dto.TrangThai
            };

            _context.Bans.Add(ban);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(Ban),
                IdBanGhi = ban.Id,
                HanhDong = "Tao",
                GiaTriMoi = JsonSerializer.Serialize(ban),
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = ban.Id }, ban);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            BanCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var ban = await _context.Bans.FindAsync(id);
            if (ban == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(ban);

            ban.TenBan = dto.TenBan;
            ban.TrangThai = dto.TrangThai;

            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";
            var giaTriMoi = JsonSerializer.Serialize(ban);

            var log = new AuditLog
            {
                TenBang = nameof(Ban),
                IdBanGhi = ban.Id,
                HanhDong = "CapNhat",
                GiaTriCu = giaTriCu,
                GiaTriMoi = giaTriMoi,
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var ban = await _context.Bans.FindAsync(id);
            if (ban == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(ban);

            _context.Bans.Remove(ban);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(Ban),
                IdBanGhi = id,
                HanhDong = "Xoa",
                GiaTriCu = giaTriCu,
                GiaTriMoi = null,
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
