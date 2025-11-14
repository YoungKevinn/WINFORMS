using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThanhToanController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public ThanhToanController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThanhToan>>> GetAll()
        {
            return await _context.ThanhToans.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ThanhToan>> GetById(int id)
        {
            var item = await _context.ThanhToans.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ThanhToan>> Create(
            ThanhToanCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = new ThanhToan
            {
                DonGoiId = dto.DonGoiId,
                SoTien = dto.SoTien,
                PhuongThuc = dto.PhuongThuc,
                ThanhToanLuc = dto.ThanhToanLuc == default ? DateTime.Now : dto.ThanhToanLuc,
                MaThamChieu = dto.MaThamChieu
            };

            _context.ThanhToans.Add(item);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(ThanhToan),
                IdBanGhi = item.Id,
                HanhDong = "Tao",
                GiaTriMoi = JsonSerializer.Serialize(item),
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            ThanhToanCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = await _context.ThanhToans.FindAsync(id);
            if (item == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(item);

            item.DonGoiId = dto.DonGoiId;
            item.SoTien = dto.SoTien;
            item.PhuongThuc = dto.PhuongThuc;
            item.ThanhToanLuc = dto.ThanhToanLuc;
            item.MaThamChieu = dto.MaThamChieu;

            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";
            var giaTriMoi = JsonSerializer.Serialize(item);

            var log = new AuditLog
            {
                TenBang = nameof(ThanhToan),
                IdBanGhi = item.Id,
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
            var item = await _context.ThanhToans.FindAsync(id);
            if (item == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(item);

            _context.ThanhToans.Remove(item);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(ThanhToan),
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
