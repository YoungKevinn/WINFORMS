using API.DTOs;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietDonGoiController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public ChiTietDonGoiController(CafeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChiTietDonGoi>>> GetAll()
        {
            return await _context.ChiTietDonGois.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ChiTietDonGoi>> GetById(int id)
        {
            var item = await _context.ChiTietDonGois.FindAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpPost]
        public async Task<ActionResult<ChiTietDonGoi>> Create(
            ChiTietDonGoiCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = new ChiTietDonGoi
            {
                DonGoiId = dto.DonGoiId,
                ThucUongId = dto.ThucUongId,
                ThucAnId = dto.ThucAnId,
                SoLuong = dto.SoLuong,
                DonGia = dto.DonGia,
                ChietKhau = dto.ChietKhau,

                // ✅ map ghi chú món
                GhiChu = dto.GhiChu
            };

            _context.ChiTietDonGois.Add(item);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(ChiTietDonGoi),
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
            ChiTietDonGoiCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var item = await _context.ChiTietDonGois.FindAsync(id);
            if (item == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(item);

            item.DonGoiId = dto.DonGoiId;
            item.ThucUongId = dto.ThucUongId;
            item.ThucAnId = dto.ThucAnId;
            item.SoLuong = dto.SoLuong;
            item.DonGia = dto.DonGia;
            item.ChietKhau = dto.ChietKhau;

            // ✅ cập nhật ghi chú món
            item.GhiChu = dto.GhiChu;

            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";
            var giaTriMoi = JsonSerializer.Serialize(item);

            var log = new AuditLog
            {
                TenBang = nameof(ChiTietDonGoi),
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
            var item = await _context.ChiTietDonGois.FindAsync(id);
            if (item == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(item);

            _context.ChiTietDonGois.Remove(item);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(ChiTietDonGoi),
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
