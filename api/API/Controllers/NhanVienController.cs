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
    public class NhanVienController : ControllerBase
    {
        private readonly CafeDbContext _context;

        public NhanVienController(CafeDbContext context)
        {
            _context = context;
        }

        // ✅ Ai cũng có thể xem danh sách (hoặc sau này muốn chỉ admin thì thêm [Authorize] cũng được)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetAll()
        {
            return await _context.NhanViens.ToListAsync();
        }

        // ✅ Ai cũng có thể xem chi tiết
        [HttpGet("{id:int}")]
        public async Task<ActionResult<NhanVien>> GetById(int id)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();
            return nv;
        }

        // 🔒 CHỈ ADMIN được thêm nhân viên
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<NhanVien>> Create(
            NhanVienCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var nv = new NhanVien
            {
                MaNhanVien = dto.MaNhanVien,
                HoTen = dto.HoTen,
                VaiTro = dto.VaiTro,
                // TenDangNhap / MatKhauHash: nếu bạn muốn admin tạo luôn account cho NV
            };

            _context.NhanViens.Add(nv);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(NhanVien),
                IdBanGhi = nv.Id,
                HanhDong = "Tao",
                GiaTriCu = null,
                GiaTriMoi = JsonSerializer.Serialize(nv),
                NguoiThucHien = nguoiThucHien,
                ThoiGian = DateTime.Now
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = nv.Id }, nv);
        }

        // 🔒 CHỈ ADMIN được sửa nhân viên
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            NhanVienCreateUpdateDto dto,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(nv);

            nv.MaNhanVien = dto.MaNhanVien;
            nv.HoTen = dto.HoTen;
            nv.VaiTro = dto.VaiTro;

            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var giaTriMoi = JsonSerializer.Serialize(nv);

            var log = new AuditLog
            {
                TenBang = nameof(NhanVien),
                IdBanGhi = nv.Id,
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

        // 🔒 CHỈ ADMIN được xóa nhân viên
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(
            int id,
            [FromHeader(Name = "X-NguoiThucHien")] string? nguoiThucHien)
        {
            var nv = await _context.NhanViens.FindAsync(id);
            if (nv == null) return NotFound();

            var giaTriCu = JsonSerializer.Serialize(nv);

            _context.NhanViens.Remove(nv);
            await _context.SaveChangesAsync();

            nguoiThucHien ??= "Unknown";

            var log = new AuditLog
            {
                TenBang = nameof(NhanVien),
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
