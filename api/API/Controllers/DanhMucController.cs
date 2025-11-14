using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class DanhMucController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public DanhMucController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DanhMuc>>> GetAll()
        {
            return await _context.DanhMucs.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DanhMuc>> GetById(int id)
        {
            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return NotFound();
            return dm;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<DanhMuc>> Create(
            DanhMucCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var dm = new DanhMuc
            {
                Ten = dto.Ten,
                DangHoatDong = dto.DangHoatDong
            };

            _context.DanhMucs.Add(dm);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(DanhMuc),
                IdBanGhi = dm.Id,
                HanhDong = "Tao",
                GiaTriMoi = JsonSerializer.Serialize(dm),
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = dm.Id }, dm);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            DanhMucCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(dm);

            dm.Ten = dto.Ten;
            dm.DangHoatDong = dto.DangHoatDong;

            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";
            var giaTriMoi = JsonSerializer.Serialize(dm);

            var log = new AuditLog
            {
                TenBang = nameof(DanhMuc),
                IdBanGhi = dm.Id,
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(
            int id,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var dm = await _context.DanhMucs.FindAsync(id);
            if (dm == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(dm);

            _context.DanhMucs.Remove(dm);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(DanhMuc),
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
