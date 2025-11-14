using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonGoiController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public DonGoiController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonGoi>>> GetAll()
        {
            return await _context.DonGois.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DonGoi>> GetById(int id)
        {
            var item = await _context.DonGois.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<DonGoi>> Create(
            DonGoiCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = new DonGoi
            {
                NhanVienId = dto.NhanVienId,
                BanId = dto.BanId,
                TrangThai = dto.TrangThai,
                MoLuc = dto.MoLuc == default ? DateTime.Now : dto.MoLuc,
                DongLuc = dto.DongLuc,
                GhiChu = dto.GhiChu
            };

            _context.DonGois.Add(item);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(DonGoi),
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
            DonGoiCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = await _context.DonGois.FindAsync(id);
            if (item == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(item);

            item.NhanVienId = dto.NhanVienId;
            item.BanId = dto.BanId;
            item.TrangThai = dto.TrangThai;
            item.MoLuc = dto.MoLuc;
            item.DongLuc = dto.DongLuc;
            item.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";
            var giaTriMoi = JsonSerializer.Serialize(item);

            var log = new AuditLog
            {
                TenBang = nameof(DonGoi),
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
            var item = await _context.DonGois.FindAsync(id);
            if (item == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(item);

            _context.DonGois.Remove(item);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(DonGoi),
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
